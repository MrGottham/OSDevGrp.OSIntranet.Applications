using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.WebApi.Models.Contacts;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.ContactController
{
    [TestFixture]
    public class ContactsWithBirthdayAsyncTests : ContactControllerTestBase
    {
        [Test]
        [Category("UnitTest")]
        public async Task ContactsWithBirthdayAsync_WhenMicrosoftTokenClaimIsMissing_ReturnsUnauthorized()
        {
            Controller sut = CreateSut(CreatePrincipalWithoutMicrosoftToken());

            ActionResult<IEnumerable<ContactModel>> result = await sut.ContactsWithBirthdayAsync(7);

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactsWithBirthdayAsync_WhenMicrosoftTokenHasExpired_ReturnsUnauthorized()
        {
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildExpiredMicrosoftToken("access-token")));

            ActionResult<IEnumerable<ContactModel>> result = await sut.ContactsWithBirthdayAsync(7);

            Assert.That(result.Result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactsWithBirthdayAsync_WhenMicrosoftTokenHasExpired_DoesNotQueryContacts()
        {
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildExpiredMicrosoftToken("access-token")));

            await sut.ContactsWithBirthdayAsync(7);

            QueryBusMock.Verify(mock => mock.QueryAsync<IGetContactWithBirthdayCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactWithBirthdayCollectionQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ContactsWithBirthdayAsync_WhenCalled_QueriesWithBirthdayWithinDays()
        {
            IGetContactWithBirthdayCollectionQuery query = null;
            QueryBusMock.Setup(mock => mock.QueryAsync<IGetContactWithBirthdayCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactWithBirthdayCollectionQuery>()))
                .Callback<IGetContactWithBirthdayCollectionQuery>(capturedQuery => query = capturedQuery)
                .Returns(Task.FromResult<IEnumerable<IContact>>(Enumerable.Empty<IContact>()));
            Controller sut = CreateSut(CreatePrincipalWithMicrosoftToken(BuildValidMicrosoftToken("access-token")));

            await sut.ContactsWithBirthdayAsync(14);

            Assert.That(query?.BirthdayWithinDays, Is.EqualTo(14));
        }
    }
}