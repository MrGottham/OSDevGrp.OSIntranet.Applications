using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountController
{
    [TestFixture]
    public class ManageTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
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
            return new Controller(_commandBusMock.Object, _queryBusMock.Object);
        }
     }
}