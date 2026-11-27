using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ContactsAsyncTests : ContactControllerTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async Task ContactsAsync_WhenMicrosoftTokenClaimIsMissing_ReturnsUnauthorized()
        {
            Controller sut = CreateSut(CreatePrincipalWithoutMicrosoftToken());

            ActionResult<IEnumerable<ContactModel>> result = await sut.ContactsAsync();

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactsAsync_WhenMicrosoftTokenIsMalformed_ReturnsUnauthorized()
        {
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken("!!!"));

            ActionResult<IEnumerable<ContactModel>> result = await sut.ContactsAsync();

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactsAsync_WhenMicrosoftTokenHasExpired_ReturnsUnauthorized()
        {
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildExpiredMicrosoftToken("access-token")));

            ActionResult<IEnumerable<ContactModel>> result = await sut.ContactsAsync();

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactsAsync_WhenMicrosoftTokenHasExpired_DoesNotQueryContacts()
        {
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildExpiredMicrosoftToken("access-token")));

            await sut.ContactsAsync();

            QueryBusMock.Verify(mock => mock.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactCollectionQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactsAsync_WhenMicrosoftTokenIsValid_QueriesWithAccessTokenFromClaim()
        {
            IGetContactCollectionQuery query = null;
            QueryBusMock.Setup(mock => mock.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactCollectionQuery>()))
                .Callback<IGetContactCollectionQuery>(capturedQuery => query = capturedQuery)
                .Returns(Task.FromResult<IEnumerable<IContact>>(Enumerable.Empty<IContact>()));
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildValidMicrosoftToken("access-token")));

            await sut.ContactsAsync();

            Assert.That(query?.AccessToken, Is.EqualTo("access-token"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactsAsync_WhenContactsHaveDifferentDisplayNames_OrdersByDisplayNameIgnoringCase()
        {
            Mock<IContact> betaContact = Fixture.BuildContactMock(externalIdentifier: "1");
            betaContact.Setup(mock => mock.Name).Returns(Fixture.BuildNameMock("beta").Object);
            Mock<IContact> alphaContact = Fixture.BuildContactMock(externalIdentifier: "2");
            alphaContact.Setup(mock => mock.Name).Returns(Fixture.BuildNameMock("Alpha").Object);
            Mock<IContact> gammaContact = Fixture.BuildContactMock(externalIdentifier: "3");
            gammaContact.Setup(mock => mock.Name).Returns(Fixture.BuildNameMock("gamma").Object);
            QueryBusMock.Setup(mock => mock.QueryAsync<IGetContactCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactCollectionQuery>()))
                .Returns(Task.FromResult<IEnumerable<IContact>>(new[] { betaContact.Object, gammaContact.Object, alphaContact.Object }));
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildValidMicrosoftToken("access-token")));

            ActionResult<IEnumerable<ContactModel>> result = await sut.ContactsAsync();

            Assert.That(((IEnumerable<ContactModel>) ((OkObjectResult) result.Result).Value).Select(model => model.ExternalIdentifier), Is.EqualTo(new[] { "2", "1", "3" }));
        }
    }
}