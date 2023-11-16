using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using Controller = OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
	[TestFixture]
    public class ManageTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<ITrustedDomainHelper> _trustedDomainHelperMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _trustedDomainHelperMock = new Mock<ITrustedDomainHelper>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
        }

        [Test]
        [Category("UnitTest")]
        public void Manage_WhenCalled_ReturnsNotNull()
        {
	        Controller sut = CreateSut();

	        IActionResult result = sut.Manage();

	        Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Manage_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Manage();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Manage_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToManage()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Manage();

            Assert.That(result.ViewName, Is.EqualTo("Manage"));
        }

        [Test]
        [Category("UnitTest")]
        public void Manage_WhenCalled_ReturnsViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Manage();

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Manage_WhenCalled_ReturnsViewResultWhereModelIsClaimsPrincipal()
        {
	        Controller sut = CreateSut();

	        ViewResult result = (ViewResult)sut.Manage();

	        Assert.That(result.Model, Is.TypeOf<ClaimsPrincipal>());
        }

        [Test]
        [Category("UnitTest")]
        public void Manage_WhenCalled_ReturnsViewResultWhereModelIsClaimsPrincipalFromHttpContext()
        {
	        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
	        Controller sut = CreateSut(claimsPrincipal);

	        ViewResult result = (ViewResult)sut.Manage();

	        Assert.That(result.Model, Is.EqualTo(claimsPrincipal));
        }

        private Controller CreateSut(ClaimsPrincipal claimsPrincipal = null)
        {
	        return new Controller(_commandBusMock.Object, _trustedDomainHelperMock.Object, _tokenHelperFactoryMock.Object, _dataProtectionProviderMock.Object)
	        {
		        ControllerContext = new ControllerContext
		        {
			        HttpContext = new DefaultHttpContext
			        {
				        User = claimsPrincipal ?? new ClaimsPrincipal()
			        }
		        }
	        };
        }
     }
}