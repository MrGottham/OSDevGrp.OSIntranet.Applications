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
    public class ContactAsyncTests : ContactControllerTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async Task ContactAsync_WhenMicrosoftTokenClaimIsMissing_ReturnsUnauthorized()
        {
            Controller sut = CreateSut(CreatePrincipalWithoutMicrosoftToken());

            ActionResult<ContactModel> result = await sut.ContactAsync("external-identifier");

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAsync_WhenMicrosoftTokenHasExpired_ReturnsUnauthorized()
        {
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildExpiredMicrosoftToken("access-token")));

            ActionResult<ContactModel> result = await sut.ContactAsync("external-identifier");

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAsync_WhenMicrosoftTokenHasExpired_DoesNotQueryContact()
        {
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildExpiredMicrosoftToken("access-token")));

            await sut.ContactAsync("external-identifier");

            QueryBusMock.Verify(mock => mock.QueryAsync<IGetContactQuery, IContact>(It.IsAny<IGetContactQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAsync_WhenContactIsNotFound_ReturnsNotFound()
        {
            QueryBusMock.Setup(mock => mock.QueryAsync<IGetContactQuery, IContact>(It.IsAny<IGetContactQuery>()))
                .Returns(Task.FromResult<IContact>(null));
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildValidMicrosoftToken("access-token")));

            ActionResult<ContactModel> result = await sut.ContactAsync("external-identifier");

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAsync_WhenContactIsFound_QueriesWithExternalIdentifier()
        {
            IGetContactQuery query = null;
            QueryBusMock.Setup(mock => mock.QueryAsync<IGetContactQuery, IContact>(It.IsAny<IGetContactQuery>()))
                .Callback<IGetContactQuery>(capturedQuery => query = capturedQuery)
                .Returns(Task.FromResult<IContact>(Fixture.BuildContactMock(externalIdentifier: "external-identifier").Object));
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildValidMicrosoftToken("access-token")));

            await sut.ContactAsync("external-identifier");

            Assert.That(query?.ExternalIdentifier, Is.EqualTo("external-identifier"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactAsync_WhenContactIsFound_ReturnsContactWithExternalIdentifier()
        {
            QueryBusMock.Setup(mock => mock.QueryAsync<IGetContactQuery, IContact>(It.IsAny<IGetContactQuery>()))
                .Returns(Task.FromResult<IContact>(Fixture.BuildContactMock(externalIdentifier: "external-identifier").Object));
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildValidMicrosoftToken("access-token")));

            ActionResult<ContactModel> result = await sut.ContactAsync("external-identifier");

            Assert.That(((ContactModel) ((OkObjectResult) result.Result).Value).ExternalIdentifier, Is.EqualTo("external-identifier"));
        }
    }
}