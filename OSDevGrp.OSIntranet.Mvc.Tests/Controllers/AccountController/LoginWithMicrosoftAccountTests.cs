using System;
using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Mvc.Tests.Helpers;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class LoginWithMicrosoftAccountTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IUrlHelper> _urlHelperMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _urlHelperMock = new Mock<IUrlHelper>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNull_AssertActionWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount();

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNull_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount();

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsNull_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithMicrosoftAccount();

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmpty_AssertActionWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount(string.Empty);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmpty_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount(string.Empty);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsEmpty_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithMicrosoftAccount(string.Empty);

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpace_AssertActionWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount(" ");

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpace_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.LoginWithMicrosoftAccount(" ");

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlIsWhiteSpace_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            ChallengeResult result = (ChallengeResult) sut.LoginWithMicrosoftAccount(" ");

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValue_AssertActionWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            Uri returnUrl = new Uri($"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}"); 
            IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl.AbsolutePath);

            _urlHelperMock.Verify(m => m.Action(It.IsAny<UrlActionContext>()), Times.Exactly(1));
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValue_ReturnsChallengeResult()
        {
            Controller sut = CreateSut();

            Uri returnUrl = new Uri($"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}"); 
            IActionResult result = sut.LoginWithMicrosoftAccount(returnUrl.AbsolutePath);

            Assert.That(result, Is.TypeOf<ChallengeResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void LoginWithMicrosoftAccount_WhenReturnUrlHasValue_ReturnsChallengeResultWhereAuthenticationSchemesContainsSchemeForMicrosoftAccount()
        {
            Controller sut = CreateSut();

            Uri returnUrl = new Uri($"http://localhost/{_fixture.Create<string>()}/{_fixture.Create<string>()}"); 
            ChallengeResult result = (ChallengeResult) sut.LoginWithMicrosoftAccount(" ");

            Assert.That(result.AuthenticationSchemes.Contains(MicrosoftAccountDefaults.AuthenticationScheme), Is.True);
        }

        private Controller CreateSut()
        {
            _urlHelperMock.Setup(_fixture);

            Controller sut = new Controller(_commandBusMock.Object);
            sut.Url = _urlHelperMock.Object;
            return sut;
        }
    }
}