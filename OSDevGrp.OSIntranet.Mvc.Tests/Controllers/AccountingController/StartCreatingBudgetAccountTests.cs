using AutoFixture;
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
    public class StartCreatingBudgetAccountTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingBudgetAccount_WhenCalled_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.StartCreatingBudgetAccount(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingBudgetAccount_WhenCalled_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.StartCreatingBudgetAccount(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingBudgetAccount_WhenCalled_ReturnsPartialViewResultWhereViewNameIsEqualToCreatingBudgetAccountPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartCreatingBudgetAccount(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("_CreatingBudgetAccountPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingBudgetAccount_WhenCalled_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartCreatingBudgetAccount(_fixture.Create<int>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingBudgetAccount_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingIdentificationViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartCreatingBudgetAccount(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<AccountingIdentificationViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public void StartCreatingBudgetAccount_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingIdentificationViewModelWithAccountingNumberEqualToArgument()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult) sut.StartCreatingBudgetAccount(accountingNumber);

            AccountingIdentificationViewModel accountingIdentificationViewModel = (AccountingIdentificationViewModel) result.Model;

            Assert.That(accountingIdentificationViewModel.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut()
        {
            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}