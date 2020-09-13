using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;
using OSDevGrp.OSIntranet.Mvc.Models.Home;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.HomeController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.HomeController
{
    [TestFixture]
    public class IndexTests
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
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_AssertGetTokenAsyncWasNotCalledOnTokenHelperFactoryForMicrosoftGraphToken()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            await sut.Index();

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_AssertGetAccountingNumberWasNotCalledOnClaimResolver()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            await sut.Index();

            _claimResolverMock.Verify(m => m.GetAccountingNumber(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_ReturnsViewResult()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            IActionResult result = await sut.Index();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_ReturnsViewResultWhereViewNameIsEqualToIndex()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_ReturnsViewResultWhereModelIsHomeOperationsViewModel()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            Assert.That(result.Model, Is.TypeOf<HomeOperationsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithCanAccessContactsEqualToFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.CanAccessContacts, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithHasAcquiredMicrosoftGraphTokenEqualToFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.HasAcquiredMicrosoftGraphToken, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithUpcomingBirthdaysWithinDaysEqualToDefaultValue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.UpcomingBirthdaysWithinDays, Is.EqualTo(default(int)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithCanAccessAccountingsEqualToFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.CanAccessAccountings, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsNotAuthenticated_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithAccountingNumberEqualToNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.AccountingNumber, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContacts_AssertGetTokenAsyncWasCalledOnTokenHelperFactoryForMicrosoftGraphToken()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal);

            await sut.Index();

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsNotNull<HttpContext>()),
                Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContactsAndMicrosoftGraphTokenWasReturnedFromTokenHelperFactory_AssertWillExpireWithinWasCalledOnMicrosoftGraphTokenFromTokenHelperFactory()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal, true, refreshableTokenMock.Object);

            await sut.Index();

            refreshableTokenMock.Verify(m => m.WillExpireWithin(It.Is<TimeSpan>(value => value.Days == 0 && value.Hours == 0 && value.Minutes == 1 && value.Seconds == 0 && value.Milliseconds == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithoutAccessToContacts_AssertGetTokenAsyncWasNotCalledOnTokenHelperFactoryForMicrosoftGraphToken()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: false);
            Controller sut = CreateSut(claimsPrincipal);

            await sut.Index();

            _tokenHelperFactoryMock.Verify(m => m.GetTokenAsync<IRefreshableToken>(
                    It.Is<TokenType>(value => value == TokenType.MicrosoftGraphToken),
                    It.IsAny<HttpContext>()),
                Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToAccountings_AssertGetAccountingNumberWasCalledOnClaimResolver()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasAccountingClaimType: true);
            Controller sut = CreateSut(claimsPrincipal);

            await sut.Index();

            _claimResolverMock.Verify(m => m.GetAccountingNumber(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithoutAccessToAccountings_AssertGetAccountingNumberWasNotCalledOnClaimResolver()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasAccountingClaimType: false);
            Controller sut = CreateSut(claimsPrincipal);

            await sut.Index();

            _claimResolverMock.Verify(m => m.GetAccountingNumber(), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticated_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.Index();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticated_ReturnsViewResultWhereViewNameIsEqualToIndex()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Index();

            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticated_ReturnsViewResultWhereModelIsHomeOperationsViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) await sut.Index();

            Assert.That(result.Model, Is.TypeOf<HomeOperationsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContacts_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithCanAccessContactsEqualToTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.CanAccessContacts, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContactsAndMicrosoftGraphTokenWhichWillNotExpireWasReturnedFromTokenHelperFactory_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithHasAcquiredMicrosoftGraphTokenEqualToTrue()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal, true, refreshableTokenMock.Object);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.HasAcquiredMicrosoftGraphToken, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContactsAndMicrosoftGraphTokenWhichWillNotExpireWasReturnedFromTokenHelperFactory_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithUpcomingBirthdaysWithinDaysEqualTo14()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock();
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal, true, refreshableTokenMock.Object);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.UpcomingBirthdaysWithinDays, Is.EqualTo(14));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContactsAndMicrosoftGraphTokenWhichWillExpireWasReturnedFromTokenHelperFactory_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithHasAcquiredMicrosoftGraphTokenEqualToFalse()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock(willExpireWithin: true);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal, true, refreshableTokenMock.Object);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.HasAcquiredMicrosoftGraphToken, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContactsAndMicrosoftGraphTokenWhichWillExpireWasReturnedFromTokenHelperFactory_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithUpcomingBirthdaysWithinDaysEqualToDefaultValue()
        {
            Mock<IRefreshableToken> refreshableTokenMock = _fixture.BuildRefreshableTokenMock(willExpireWithin: true);
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal, true, refreshableTokenMock.Object);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.UpcomingBirthdaysWithinDays, Is.EqualTo(default(int)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContactsAndNoMicrosoftGraphTokenWasReturnedFromTokenHelperFactory_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithHasAcquiredMicrosoftGraphTokenEqualToFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal, false);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.HasAcquiredMicrosoftGraphToken, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToContactsAndNoMicrosoftGraphTokenWasReturnedFromTokenHelperFactory_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithinDaysEqualToDefaultValue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: true);
            Controller sut = CreateSut(claimsPrincipal, false);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.UpcomingBirthdaysWithinDays, Is.EqualTo(default(int)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithoutAccessToContacts_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithCanAccessContactsEqualToFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.CanAccessContacts, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithoutAccessToContacts_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithHasAcquiredMicrosoftGraphTokenEqualToFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.HasAcquiredMicrosoftGraphToken, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithoutAccessToContacts_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithUpcomingBirthdaysWithinDaysEqualToDefaultValue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasContactsClaimType: false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.UpcomingBirthdaysWithinDays, Is.EqualTo(default(int)));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToAccountings_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithCanAccessAccountingsEqualToTrue()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasAccountingClaimType: true);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.CanAccessAccountings, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToAccountingsAndAccountingNumberWasReturnedFromClaimResolver_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithAccountingNumberEqualToAccountingNumberFromClaimResolver()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasAccountingClaimType: true);
            int accountingNumber = _fixture.Create<int>();
            Controller sut = CreateSut(claimsPrincipal, hasAccountingNumber: true, accountingNumber: accountingNumber);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithAccessToAccountingsAndNoAccountingNumberWasReturnedFromClaimResolver_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithAccountingNumberEqualToNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasAccountingClaimType: true);
            Controller sut = CreateSut(claimsPrincipal, hasAccountingNumber: false);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.AccountingNumber, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithoutAccessToAccountings_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithCanAccessAccountingsEqualToFalse()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasAccountingClaimType: false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.CanAccessAccountings, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public async Task Index_WhenUserIsAuthenticatedWithoutAccessToAccountings_ReturnsViewResultWhereModelIsHomeOperationsViewModelWithAccountingNumberEqualToNull()
        {
            ClaimsPrincipal claimsPrincipal = CreateClaimsPrincipal(hasAccountingClaimType: false);
            Controller sut = CreateSut(claimsPrincipal);

            ViewResult result = (ViewResult) await sut.Index();

            HomeOperationsViewModel homeOperationsViewModel = (HomeOperationsViewModel) result.Model;

            Assert.That(homeOperationsViewModel.AccountingNumber, Is.Null);
        }

        private Controller CreateSut(ClaimsPrincipal claimsPrincipal = null, bool? hasRefreshableToken = null, IRefreshableToken refreshableToken = null, bool? hasAccountingNumber = null, int? accountingNumber = null)
        {
            _tokenHelperFactoryMock.Setup(m => m.GetTokenAsync<IRefreshableToken>(It.IsAny<TokenType>(), It.IsAny<HttpContext>()))
                .Returns(Task.FromResult(hasRefreshableToken ?? _random.Next(100) > 50 ? refreshableToken ?? _fixture.BuildRefreshableTokenMock().Object : null));

            _claimResolverMock.Setup(m => m.GetAccountingNumber())
                .Returns(hasAccountingNumber ?? _random.Next(100) > 50 ? accountingNumber ?? (int?) _fixture.Create<int>() : null);

            Controller controller = new Controller(_queryBusMock.Object, _claimResolverMock.Object, _tokenHelperFactoryMock.Object, _acmeChallengeResolverMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = CreateHttpContext(claimsPrincipal)
                }
            };
            return controller;
        }

        private HttpContext CreateHttpContext(ClaimsPrincipal claimsPrincipal = null)
        {
            return new DefaultHttpContext
            {
                User = claimsPrincipal ?? CreateClaimsPrincipal()
            };
        }

        private ClaimsPrincipal CreateClaimsPrincipal(bool isAuthenticated = true, bool? hasContactsClaimType = null, bool? hasAccountingClaimType = null)
        {
            return new ClaimsPrincipal(CreateClaimsIdentity(isAuthenticated, hasContactsClaimType, hasAccountingClaimType));
        }

        private ClaimsIdentity CreateClaimsIdentity(bool isAuthenticated = true, bool? hasContactsClaimType = null, bool? hasAccountingClaimType = null)
        {
            IList<Claim> claimCollection = new List<Claim>();
            if (isAuthenticated && (hasContactsClaimType ?? _random.Next(100) > 50))
            {
                claimCollection.Add(ClaimHelper.CreateContactsClaim());
            }

            if (isAuthenticated && (hasAccountingClaimType ?? _random.Next(100) > 50))
            {
                claimCollection.Add(ClaimHelper.CreateAccountingClaim());
            }

            return isAuthenticated
                ? new ClaimsIdentity(claimCollection, _fixture.Create<string>())
                : new ClaimsIdentity(claimCollection);
        }
    }
}