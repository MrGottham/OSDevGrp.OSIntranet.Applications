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
    public class UpdateContactAccountTests
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
        public void UpdateContactAccount_WhenAccountingNumberIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactAccount(_fixture.Create<int>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateContactAccount_WhenAccountingNumberIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactAccount(_fixture.Create<int>(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateContactAccount_WhenAccountingNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactAccount(_fixture.Create<int>(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumber_AssertQueryAsyncWasCalledOnQueryBusWithGetContactAccountQuery()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            string accountNumber = _fixture.Create<string>();
            await sut.UpdateContactAccount(accountingNumber, accountNumber);

            _queryBusMock.Verify(m => m.QueryAsync<IGetContactAccountQuery, IContactAccount>(It.Is<IGetContactAccountQuery>(query => query != null && query.AccountingNumber == accountingNumber && string.CompareOrdinal(query.AccountNumber, accountNumber.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumber_AssertQueryAsyncWasCalledOnQueryBusWithEmptyQueryForPaymentTermCollection()
        {
            Controller sut = CreateSut();

            await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            _queryBusMock.Verify(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsNotNull<EmptyQuery>()), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForNonExistingContactAccount_ReturnsNotNull()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForNonExistingContactAccount_ReturnsBadRequestResult()
        {
            Controller sut = CreateSut(false);

            IActionResult result = await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<BadRequestResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereViewNameIsEqualToEditContactAccountPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_EditContactAccountPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<ContactAccountViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithEditModeEqualToEdit()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.EditMode, Is.EqualTo(EditMode.Edit));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithAccountingNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithAccountingMatchingAccountingOnContactAccountFromQueryBus()
        {
            int accountingNumber = _fixture.Create<int>();
            IAccounting accounting = _fixture.BuildAccountingMock(accountingNumber).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accounting).Object;
            Controller sut = CreateSut(contactAccount: contactAccount);

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithAccountNumberNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.AccountNumber, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithAccountNumberMatchingAccountNumberOnContactAccountFromQueryBus()
        {
            string accountNumber = _fixture.Create<string>();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(accountNumber: accountNumber).Object;
            Controller sut = CreateSut(contactAccount: contactAccount);

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.AccountNumber, Is.EqualTo(accountNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithPaymentTermNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.PaymentTerm, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithPaymentTermMatchingPaymentTermOnContactAccountFromQueryBus()
        {
            int paymentTermNumber = _fixture.Create<int>();
            IPaymentTerm paymentTerm = _fixture.BuildPaymentTermMock(paymentTermNumber).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(paymentTerm: paymentTerm).Object;
            Controller sut = CreateSut(contactAccount: contactAccount);

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.PaymentTerm.Number, Is.EqualTo(paymentTermNumber));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithValuesAtStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.ValuesAtStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithValuesAtEndOfLastMonthFromStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.ValuesAtEndOfLastMonthFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithValuesAtEndOfLastYearFromStatusDateNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.ValuesAtEndOfLastYearFromStatusDate, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithBalanceInfosNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.BalanceInfos, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithBalanceInfosContainingDataFromContactInfoCollectionOnContactAccountFromQueryBus()
        {
            IContactInfo[] contactInfos = _fixture.BuildContactInfoCollectionMock().Object.ToArray();
            IContactInfoCollection contactInfoCollection = _fixture.BuildContactInfoCollectionMock(contactInfoCollection: contactInfos).Object;
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(contactInfoCollection: contactInfoCollection).Object;
            Controller sut = CreateSut(contactAccount: contactAccount);

            PartialViewResult result = (PartialViewResult)await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel)result.Model;

            foreach (IGrouping<short, IContactInfo> group in contactInfos.GroupBy(contactInfo => contactInfo.Year))
            {
                Assert.That(contactAccountViewModel.BalanceInfos.ContainsKey(group.Key), Is.True);

                BalanceInfoCollectionViewModel balanceInfoCollectionViewModel = contactAccountViewModel.BalanceInfos[group.Key];
                Assert.That(balanceInfoCollectionViewModel, Is.Not.Null);
                Assert.That(balanceInfoCollectionViewModel.All(balanceInfoViewModel => group.SingleOrDefault(m => m.Month == balanceInfoViewModel.Month) != null), Is.True);
            }
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithPostingLinesNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.PostingLines, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithPaymentTermsNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(contactAccountViewModel.PaymentTerms, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenCalledWithAccountingNumberAndAccountNumberForExistingContactAccount_ReturnsPartialViewResultWhereModelIsContactAccountViewModelWithPaymentTermsMatchingPaymentTermCollectionFromQueryBus()
        {
            IEnumerable<IPaymentTerm> paymentTermCollection = _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToArray();
            Controller sut = CreateSut(paymentTermCollection: paymentTermCollection);

            PartialViewResult result = (PartialViewResult) await sut.UpdateContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            ContactAccountViewModel contactAccountViewModel = (ContactAccountViewModel) result.Model;

            Assert.That(paymentTermCollection.All(paymentTerm => contactAccountViewModel.PaymentTerms.SingleOrDefault(m => m.Number == paymentTerm.Number) != null), Is.True);
        }

        private Controller CreateSut(bool hasContactAccount = true, IContactAccount contactAccount = null, IEnumerable<IPaymentTerm> paymentTermCollection = null)
        {
            _queryBusMock.Setup(m => m.QueryAsync<IGetContactAccountQuery, IContactAccount>(It.IsAny<IGetContactAccountQuery>()))
                .Returns(Task.FromResult(hasContactAccount ? contactAccount ?? _fixture.BuildContactAccountMock().Object : null));
            _queryBusMock.Setup(m => m.QueryAsync<EmptyQuery, IEnumerable<IPaymentTerm>>(It.IsAny<EmptyQuery>()))
                .Returns(Task.FromResult(paymentTermCollection ?? _fixture.CreateMany<IPaymentTerm>(_random.Next(5, 10)).ToArray()));

            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}