using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class ManageTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<ITrustedDomainHelper> _trustedDomainHelperMock;
        private Mock<ITokenHelperFactory> _tokenHelperFactoryMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _trustedDomainHelperMock = new Mock<ITrustedDomainHelper>();
            _tokenHelperFactoryMock = new Mock<ITokenHelperFactory>();
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
        public void Manage_WhenCalled_ReturnsViewResultWhereModelIsNull()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Manage();

            Assert.That(result.Model, Is.Null);
        }

        private Controller CreateSut()
        {
            return new Controller(_commandBusMock.Object, _trustedDomainHelperMock.Object, _tokenHelperFactoryMock.Object);
        }
     }
}