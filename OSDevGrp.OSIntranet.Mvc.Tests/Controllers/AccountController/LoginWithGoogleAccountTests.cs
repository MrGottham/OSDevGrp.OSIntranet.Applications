using System;
using AutoFixture;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Tests.Helpers;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class LoginWithGoogleAccountTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IUrlHelper> _urlHelperMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNull_AssertActionWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount();

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNull_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithGoogleAccount();

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsNull_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount();

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmpty_AssertActionWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmpty_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithGoogleAccount(string.Empty);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsEmpty_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount(string.Empty);

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpace_AssertActionWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.LoginWithGoogleAccount(" ");

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpace_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithGoogleAccount(" ");

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlIsWhiteSpace_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount(" ");

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValue_AssertActionWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            Uri returnUrl = new Uri($"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}"); 
            sut.LoginWithGoogleAccount(returnUrl.AbsoluteUri);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Exactly(1));
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValue_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            Uri returnUrl = new Uri($"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}"); 
            IActionResult result = sut.LoginWithGoogleAccount(returnUrl.AbsoluteUri);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithGoogleAccount_WhenReturnUrlHasValue_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForGoogle()
        {
            Controller sut = CreateSut();

            Uri returnUrl = new Uri($"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}"); 
            ChallengeResult result = (ChallengeResult) sut.LoginWithGoogleAccount(returnUrl.AbsoluteUri);

            Assert.That(result.AuthenticationSchemes.Contains(GoogleDefaults.AuthenticationScheme), Is.True);
        }

        private Controller CreateSut()
        {
            _urlHelperMock.Setup(_fixture);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object)
            {
                Url = _urlHelperMock.Object
            };
        }
    }
}