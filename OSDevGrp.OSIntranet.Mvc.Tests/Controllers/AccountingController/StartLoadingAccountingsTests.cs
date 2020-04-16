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
    public class StartLoadingAccountingsTests
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
        public void StartLoadingAccountings_WhenCalled_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.StartLoadingAccountings();

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingAccountings_WhenCalled_ReturnsPartialViewResultWhereViewNameIsEqualToLoadingAccountingsPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartLoadingAccountings();

            Assert.That(result.ViewName, Is.EqualTo("_LoadingAccountingsPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingAccountings_WhenCalled_ReturnsPartialViewResultWhereModelIsAccountingOptionsViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartLoadingAccountings();

            Assert.That(result.Model, Is.TypeOf<AccountingOptionsViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingAccountings_WhenCalledWithoutAccountingNumber_ReturnsPartialViewResultWhereModelIsAccountingOptionsViewModelWithoutDefaultAccountingNumber()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartLoadingAccountings();

            AccountingOptionsViewModel accountingOptionsViewModel = result.Model as AccountingOptionsViewModel;
            Assert.That(accountingOptionsViewModel, Is.Not.Null);
            Assert.That(accountingOptionsViewModel.DefaultAccountingNumber, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void StartLoadingAccountings_WhenCalledWithAccountingNumber_ReturnsPartialViewResultWhereModelIsAccountingOptionsViewModelWithDefaultAccountingNumberEqualToAccountingNumber()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult) sut.StartLoadingAccountings(accountingNumber);

            AccountingOptionsViewModel accountingOptionsViewModel = result.Model as AccountingOptionsViewModel;
            Assert.That(accountingOptionsViewModel, Is.Not.Null);
            Assert.That(accountingOptionsViewModel.DefaultAccountingNumber, Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut()
        {
            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}