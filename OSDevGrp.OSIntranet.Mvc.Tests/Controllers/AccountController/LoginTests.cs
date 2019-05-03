using System;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Tests.Helpers;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class LoginTests
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
        public void Login_WhenReturnUrlIsNull_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.Login();

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNull_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNull_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login();

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsNull_ReturnsViewResultWhereModelIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login();

            Assert.That(result.Model, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.Login(string.Empty);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(string.Empty);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login(string.Empty);

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsEmpty_ReturnsViewResultWhereModelIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login(string.Empty);

            Assert.That(result.Model, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            sut.Login(" ");

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Login(" ");

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login(" ");

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsWhiteSpace_ReturnsViewResultWhereModelIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Login(" ");

            Assert.That(result.Model, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsAbsoluteUri_AssertContentWasNotCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            string absoluteUri = $"http://localhost//{_fixture.Create<string>()}";
            sut.Login(absoluteUri);

            _urlHelperMock.Verify(m => m.Content(It.IsAny<string>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsAbsoluteUri_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            string absoluteUri = $"http://localhost//{_fixture.Create<string>()}";
            IActionResult result = sut.Login(absoluteUri);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsAbsoluteUri_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            string absoluteUri = $"http://localhost//{_fixture.Create<string>()}";
            ViewResult result = (ViewResult) sut.Login(absoluteUri);

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsAbsoluteUri_ReturnsViewResultWhereModelIsUri()
        {
            Controller sut = CreateSut();

            string absoluteUri = $"http://localhost//{_fixture.Create<string>()}";
            ViewResult result = (ViewResult) sut.Login(absoluteUri);

            Assert.That(result.Model, Is.TypeOf<Uri>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsAbsoluteUri_ReturnsViewResultWhereModelIsUriWithAbsoluteUriEqualToInput()
        {
            Controller sut = CreateSut();

            string absoluteUri = $"http://localhost//{_fixture.Create<string>()}";
            Uri result = (Uri) ((ViewResult) sut.Login(absoluteUri)).Model;

            Assert.That(result.AbsoluteUri, Is.EqualTo(absoluteUri));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUriNotStartingWithTildeAndSlash_AssertContentWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            string relativeUri = $"{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            sut.Login(relativeUri);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.Compare(value, $"~/{relativeUri}", StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUriStartingWithSlash_AssertContentWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            string relativeUri = $"/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            sut.Login(relativeUri);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.Compare(value, $"~{relativeUri}", StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUriStartingWithTildeAndSlash_AssertContentWasCalledOnUrlHelper()
        {
            Controller sut = CreateSut();

            string relativeUri = $"~/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            sut.Login(relativeUri);

            _urlHelperMock.Verify(m => m.Content(It.Is<string>(value => string.Compare(value, $"{relativeUri}", StringComparison.Ordinal) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUri_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            string relativeUri = $"/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            IActionResult result = sut.Login(relativeUri);

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUri_ReturnsViewResultWhereViewNameIsEqualToLogin()
        {
            Controller sut = CreateSut();

            string relativeUri = $"/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            ViewResult result = (ViewResult) sut.Login(relativeUri);

            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUri_ReturnsViewResultWhereModelIsUri()
        {
            Controller sut = CreateSut();

            string relativeUri = $"/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            ViewResult result = (ViewResult) sut.Login(relativeUri);

            Assert.That(result.Model, Is.TypeOf<Uri>());
        }

        [Test]
        [Category("UnitTest")]
        public void Login_WhenReturnUrlIsRelativeUri_ReturnsViewResultWhereModelIsUriWithAbsoluteUriForInput()
        {
            string contentPath = $"/{_fixture.Create<string>()}";
            Controller sut = CreateSut(contentPath);

            string relativeUri = $"/{_fixture.Create<string>()}/{_fixture.Create<string>()}";
            Uri result = (Uri) ((ViewResult) sut.Login(relativeUri)).Model;

            Assert.That(result.AbsoluteUri.EndsWith(contentPath), Is.True);
        }

        private Controller CreateSut(string contentPath = null)
        {
            _urlHelperMock.Setup(_fixture, absolutePath: contentPath);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object)
            {
                Url = _urlHelperMock.Object
            };
        }
    }
}