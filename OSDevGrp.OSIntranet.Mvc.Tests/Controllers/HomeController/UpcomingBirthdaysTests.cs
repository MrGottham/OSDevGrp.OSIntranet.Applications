using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Home;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.HomeController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.HomeController
{
    [TestFixture]
    public class UpcomingBirthdaysTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IAcmeChallengeResolver> _acmeChallengeResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _acmeChallengeResolverMock = new Mock<IAcmeChallengeResolver>();

            _fixture = new Fixture();
            _fixture.Customize<IContactGroup>(builder => builder.FromFactory(() => _fixture.BuildContactGroupMock().Object));
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));
            _fixture.Customize<ICompany>(builder => builder.FromFactory(() => _fixture.BuildCompanyMock().Object));
            _fixture.Customize<IContact>(builder => builder.FromFactory(() => _fixture.BuildContactMock(hasBirthday: true).Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpcomingBirthdays_WhenCalled_AssertGetTokenAsyncWasCalledOnTokenHelperFactory()
        {
            Controller sut = CreateSut();

            await sut.UpcomingBirthdays(_fixture.Create<int>());

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpcomingBirthdays_WhenNoTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasNotCalledOnQueryBus()
        {
            Controller sut = CreateSut(false);

            await sut.UpcomingBirthdays(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactWithBirthdayCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactWithBirthdayCollectionQuery>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpcomingBirthdays_WhenNoTokenWasReturnedFromTokenHelperFactory_ReturnsUnauthorizedResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpcomingBirthdays(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<UnauthorizedResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpcomingBirthdays_WhenTokenWasReturnedFromTokenHelperFactory_AssertQueryAsyncWasCalledOnQueryBus()
        {
            string tokenType = _fixture.Create<string>();
            string accessToken = _fixture.Create<string>();
            string refreshToken = _fixture.Create<string>();
            DateTime expires = DateTime.Now.AddSeconds(_random.Next(60, 300));
            IRefreshableToken refreshableToken = _fixture.BuildRefreshableTokenMock(tokenType, accessToken, refreshToken, expires).Object;
            Controller sut = CreateSut(refreshableToken: refreshableToken);

            int withinDays = _fixture.Create<int>();
            await sut.UpcomingBirthdays(withinDays);

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactWithBirthdayCollectionQuery, IEnumerable<IContact>>(It.Is<IGetContactWithBirthdayCollectionQuery>(query =>
                    query != null &&
                    query.BirthdayWithinDays == withinDays &&
                    string.CompareOrdinal(query.TokenType, tokenType) == 0 &&
                    string.CompareOrdinal(query.AccessToken, accessToken) == 0 &&
                    string.CompareOrdinal(query.RefreshToken, refreshToken) == 0 &&
                    query.Expires == expires)),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpcomingBirthdays_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpcomingBirthdays(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpcomingBirthdays_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereViewNameIsEqualToUpcomingBirthdayCollectionPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpcomingBirthdays(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("_UpcomingBirthdayCollectionPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpcomingBirthdays_WhenTokenWasReturnedFromTokenHelperFactory_ReturnsPartialViewResultWhereModelIsCollectionOfContactWithUpcomingBirthdayViewModel()
        {
            IEnumerable<IContact> contactCollection = _fixture.CreateMany<IContact>(_random.Next(10, 15)).ToList();
            Controller sut = CreateSut(contactCollection: contactCollection);

            PartialViewResult result = (PartialViewResult) await sut.UpcomingBirthdays(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<List<ContactWithUpcomingBirthdayViewModel>>());

            List<ContactWithUpcomingBirthdayViewModel> contactViewModelCollection = (List<ContactWithUpcomingBirthdayViewModel>) result.Model;
            Assert.That(contactViewModelCollection, Is.Not.Null);
            Assert.That(contactViewModelCollection, Is.Not.Empty);
            Assert.That(contactViewModelCollection.Count, Is.EqualTo(contactCollection.Count()));
            Assert.That(contactViewModelCollection.All(contactViewModel => contactCollection.Any(contact => string.CompareOrdinal(contact.ExternalIdentifier, contactViewModel.ExternalIdentifier) == 0)), Is.True);
        }

        private Controller CreateSut(bool hasRefreshableToken = true, IRefreshableToken refreshableToken = null, IEnumerable<IContact> contactCollection = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.FromResult(hasRefreshableToken ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));

            _queryBusMock.Setup(m => m.QueryAsync<IGetContactWithBirthdayCollectionQuery, IEnumerable<IContact>>(It.IsAny<IGetContactWithBirthdayCollectionQuery>()))
                .Returns(Task.FromResult(contactCollection ?? _fixture.CreateMany<IContact>(_random.Next(10, 15)).AsEnumerable()));

            return new Controller(_queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object, _acmeChallengeResolverMock.Object);
        }
    }
}