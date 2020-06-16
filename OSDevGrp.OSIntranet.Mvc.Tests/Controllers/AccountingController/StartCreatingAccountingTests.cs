using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class StartCreatingAccountingTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingAccounting_WhenCalled_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.StartCreatingAccounting();

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingAccounting_WhenCalled_ReturnsPartialViewResultWhereViewNameIsEqualToCreatingAccountingPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartCreatingAccounting();

            Assert.That(result.ViewName, Is.EqualTo("_CreatingAccountingPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingAccounting_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingOptionsViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartCreatingAccounting();

            Assert.That(result.Model, Is.TypeOf<AccountingOptionsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingAccounting_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingOptionsViewModelWithoutDefaultAccountingNumber()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartCreatingAccounting();

            AccountingOptionsViewModel accountingOptionsViewModel = (AccountingOptionsViewModel) result.Model;

            Assert.That(accountingOptionsViewModel.DefaultAccountingNumber, Is.Null);
        }

        private Controller CreateSut()
        {
            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}