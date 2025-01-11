using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class AccessDeniedTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<ITrustedDomainResolver> _trustedDomainResolverMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.AccessDenied();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.AccessDenied();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereViewNameIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.AccessDenied();

            Assert.That(result.ViewName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereViewNameIsNotEmpty()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.AccessDenied();

            Assert.That(result.ViewName, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToError()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.AccessDenied();

            Assert.That(result.ViewName, Is.EqualTo("Error"));
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.AccessDenied();

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsErrorViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult)sut.AccessDenied();

            Assert.That(result.Model, Is.TypeOf<ErrorViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsErrorViewModelWithRequestIdNotNull()
        {
            Controller sut = CreateSut();

            ErrorViewModel result = (ErrorViewModel) ((ViewResult) sut.AccessDenied()).Model;

            Assert.That(result!.RequestId, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsErrorViewModelWithRequestIdNotEmpty()
        {
            Controller sut = CreateSut();

            ErrorViewModel result = (ErrorViewModel) ((ViewResult) sut.AccessDenied()).Model;

            Assert.That(result!.RequestId, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsErrorViewModelWithRequestIdEqualToTraceIdentifierOnHttpContext()
        {
            string traceIdentifier = _fixture.Create<string>();
            HttpContext httpContext = CreateHttpContext(traceIdentifier: traceIdentifier);
            Controller sut = CreateSut(httpContext: httpContext);

            ErrorViewModel result = (ErrorViewModel) ((ViewResult) sut.AccessDenied()).Model;

            Assert.That(result!.RequestId, Is.EqualTo(traceIdentifier));
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsErrorViewModelWithErrorCodeNull()
        {
            Controller sut = CreateSut();

            ErrorViewModel result = (ErrorViewModel) ((ViewResult) sut.AccessDenied()).Model;

            Assert.That(result!.ErrorCode, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsErrorViewModelWithErrorMessageNotNull()
        {
            Controller sut = CreateSut();

            ErrorViewModel result = (ErrorViewModel) ((ViewResult) sut.AccessDenied()).Model;

            Assert.That(result!.ErrorMessage, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsErrorViewModelWithErrorMessageNotEmpty()
        {
            Controller sut = CreateSut();

            ErrorViewModel result = (ErrorViewModel) ((ViewResult) sut.AccessDenied()).Model;

            Assert.That(result!.ErrorMessage, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void AccessDenied_WhenCalled_ReturnsViewResultWhereModelIsErrorViewModelWithErrorMessageEqualToExpectedErrorMessage()
        {
            Controller sut = CreateSut();

            ErrorViewModel result = (ErrorViewModel) ((ViewResult) sut.AccessDenied()).Model;

            Assert.That(result!.ErrorMessage, Is.EqualTo("Handlingen kan ikke udføres, fordi du ikke har de nødvendige tilladelser."));
        }

        private Controller CreateSut(HttpContext httpContext = null)
        {
            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _trustedDomainResolverMock.Object, _tokenHelperFactoryMock.Object, _dataProtectionProviderMock.Object)
            {
                ControllerContext =
                {
                    HttpContext = httpContext ?? CreateHttpContext()
                }
            };
        }

        private HttpContext CreateHttpContext(string traceIdentifier = null)
        {
            return new DefaultHttpContext
            {
                TraceIdentifier = traceIdentifier ?? _fixture.Create<string>()
            };
        }
    }
}
