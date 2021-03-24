using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class CreateContactAccountTests
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IClaimResolver> _claimResolverMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _claimResolverMock = new Mock<IClaimResolver>();

            _fixture = new Fixture();
            _fixture.Customize<IPaymentTerm>(builder => builder.FromFactory(() => _fixture.BuildPaymentTermMock().Object));

            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumber_AssertQueryAsyncWasCalledOnQueryBusWithGetAccountingQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            await sut.CreateContactAccount(accountingNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.Is<IGetAccountingQuery>(query => query != null && query.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumber_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForPaymentTermCollection()
        {
            Controller sut = CreateSut();

            await sut.CreateContactAccount(_fixture.Create<int>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForNonExistingAccounting_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.CreateContactAccount(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForNonExistingAccounting_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.CreateContactAccount(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateContactAccount(_fixture.Create<int>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.CreateContactAccount(_fixture.Create<int>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereViewNameIsEqualToEditContactAccountPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            Assert.That(result.ViewName, Is.EqualTo("_EditContactAccountPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            Assert.That(result.Model, Is.TypeOf<ContactAccountViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithEditModeEqualToCreate()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.EditMode, Is.EqualTo(EditMode.Create));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithAccountingNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithAccountingMatchingAccountingFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            Controller sut = CreateSut(accounting: accounting);

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithAccountNumberEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.AccountNumber, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithPaymentTermEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.PaymentTerm, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithValuesAtStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.ValuesAtStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithValuesAtEndOfLastMonthFromStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.ValuesAtEndOfLastMonthFromStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithValuesAtEndOfLastYearFromStatusDateEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.ValuesAtEndOfLastYearFromStatusDate, Is.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithBudgetInfosNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.BalanceInfos, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithBalanceInfosContainingDataForCurrentYear()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            short currentYear = (short) DateTime.Today.Year;
            short currentMonth = (short) DateTime.Today.Month;

            Assert.That(contactAccountViewModel.BalanceInfos.ContainsKey(currentYear), Is.True);

            BalanceInfoCollectionViewModel balanceInfoCollectionViewModel = contactAccountViewModel.BalanceInfos[currentYear];
            Assert.That(balanceInfoCollectionViewModel, Is.Not.Null);
            Assert.That(balanceInfoCollectionViewModel.Count, Is.EqualTo(12 - currentMonth + 1));
            for (short month = currentMonth; month <= 12; month++)
            {
                BalanceInfoViewModel balanceInfoViewModel = balanceInfoCollectionViewModel.Single(m => m.Year == currentYear && m.Month == month);
                Assert.That(balanceInfoViewModel.EditMode, Is.EqualTo(EditMode.Create));
                Assert.That(balanceInfoViewModel.Balance, Is.EqualTo(0M));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithBalanceInfosContainingDataForNextYear()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            short nextYear = (short) (DateTime.Today.Year + 1);

            Assert.That(contactAccountViewModel.BalanceInfos.ContainsKey(nextYear), Is.True);

            BalanceInfoCollectionViewModel balanceInfoCollectionViewModel = contactAccountViewModel.BalanceInfos[nextYear];
            Assert.That(balanceInfoCollectionViewModel, Is.Not.Null);
            Assert.That(balanceInfoCollectionViewModel.Count, Is.EqualTo(12));
            for (short month = 1; month <= 12; month++)
            {
                BalanceInfoViewModel balanceInfoViewModel = balanceInfoCollectionViewModel.Single(m => m.Year == nextYear && m.Month == month);
                Assert.That(balanceInfoViewModel.EditMode, Is.EqualTo(EditMode.Create));
                Assert.That(balanceInfoViewModel.Balance, Is.EqualTo(0M));
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithPaymentTermsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.PaymentTerms, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateContactAccount_WhenCalledWithAccountingNumberForExistingAccounting_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithPaymentTermsMatchingPaymentTermCollectionFromQueryBus()
        {
            IEnumerable<IPaymentTerm> paymentTermCollection = _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToArray();
            Controller sut = CreateSut(paymentTermCollection: paymentTermCollection);

            PartialViewResult result = (PartialViewResult) await sut.CreateContactAccount(_fixture.Create<int>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(paymentTermCollection.All(paymentTerm => contactAccountViewModel.PaymentTerms.SingleOrDefault(m => m.Number == paymentTerm.Number) != null), Is.True);
        }

        private Controller CreateSut(bool hasAccounting = true, IAccounting accounting = null, IEnumerable<IPaymentTerm> paymentTermCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetAccountingQuery, IAccounting>(It.IsAny<IGetAccountingQuery>()))
                .Returns(Task.FromResult(hasAccounting ? accounting ?? _fixture.BuildAccountingMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(paymentTermCollection ?? _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToArray()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}