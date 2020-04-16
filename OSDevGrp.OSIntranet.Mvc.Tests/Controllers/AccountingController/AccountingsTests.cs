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
    public class AccountingsTests
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
        public void Accountings_WhenCalled_AssertGetAccountingNumberWasCalledOnClaimResolver()
        {
            Controller sut = CreateSut();

            sut.Accountings();

            _claimResolverMock.Verify(m => m.GetAccountingNumber());
        }

        [Test]
        [Category("UnitTest")]
        public void Accountings_WhenCalled_ReturnsViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.Accountings();

            Assert.That(result, Is.TypeOf<ViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void Accountings_WhenCalled_ReturnsViewResultWhereViewNameIsEqualToAccountings()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Accountings();

            Assert.That(result.ViewName, Is.EqualTo("Accountings"));
        }

        [Test]
        [Category("UnitTest")]
        public void Accountings_WhenCalled_ReturnsViewResultWhereModelIsAccountingOptionsViewModel()
        {
            Controller sut = CreateSut();

            ViewResult result = (ViewResult) sut.Accountings();

            Assert.That(result.Model, Is.TypeOf<AccountingOptionsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public void Accountings_WhenAccountingNumberWasNotReturnedFromClaimResolver_ReturnsViewResultWhereModelIsAccountingOptionsViewModelWhereDefaultAccountingNumberIsNull()
        {
            Controller sut = CreateSut(false);

            ViewResult result = (ViewResult) sut.Accountings();

            AccountingOptionsViewModel accountingOptionsViewModel = (AccountingOptionsViewModel) result.Model;
            
            Assert.That(accountingOptionsViewModel.DefaultAccountingNumber, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Accountings_WhenAccountingNumberWasReturnedFromClaimResolver_ReturnsViewResultWhereModelIsAccountingOptionsViewModelWhereDefaultAccountingNumberIsEqualToAccountingNumberFromClaimResolver()
        {
            int accountingNumber = _fixture.Create<int>();
            Controller sut = CreateSut(accountingNumber: accountingNumber);

            ViewResult result = (ViewResult) sut.Accountings();

            AccountingOptionsViewModel accountingOptionsViewModel = (AccountingOptionsViewModel) result.Model;

            Assert.That(accountingOptionsViewModel.DefaultAccountingNumber, Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut(bool hasAccountingNumber = true, int? accountingNumber = null)
        {
            _claimResolverMock.Setup(m => m.GetAccountingNumber())
                .Returns(() => hasAccountingNumber ? accountingNumber ?? _fixture.Create<int>() : (int?) null);

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}