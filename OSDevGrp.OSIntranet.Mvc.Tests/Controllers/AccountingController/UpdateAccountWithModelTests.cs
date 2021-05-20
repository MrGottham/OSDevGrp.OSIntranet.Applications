using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using Controller=OSDevGrp.OSIntranet.Mvc.Controllers.AccountingController;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Controllers.AccountingController
{
    [TestFixture]
    public class UpdateAccountWithModelTests
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
        public void UpdateAccount_WhenAccountViewModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateAccount(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountViewModel"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateAccount_WhenAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToInternalError()
        {
            Controller sut = CreateSut(false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.UpdateAccount(CreateAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.InternalError));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateAccount_WhenAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereMessageEndsWithErrorMessagesFromModelState()
        {
            string errorMessage = _fixture.Create<string>();
            Controller sut = CreateSut(false, errorMessage: errorMessage);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.UpdateAccount(CreateAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.EndsWith(errorMessage), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateAccount_WhenAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            Controller sut = CreateSut(false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.UpdateAccount(CreateAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingAccountingNumberFromAccountViewModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            AccountingIdentificationViewModel accountingIdentificationViewModel = CreateAccountingIdentificationViewModel(accountingNumber);
            AccountViewModel accountViewModel = CreateAccountViewModel(accountingIdentificationViewModel);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingAccountNumberFromAccountViewModel()
        {
            Controller sut = CreateSut();

            string accountNumber = _fixture.Create<string>();
            AccountViewModel accountViewModel = CreateAccountViewModel(accountNumber: accountNumber);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && string.CompareOrdinal(command.AccountNumber, accountNumber.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingAccountNameFromAccountViewModel()
        {
            Controller sut = CreateSut();

            string accountName = _fixture.Create<string>();
            AccountViewModel accountViewModel = CreateAccountViewModel(accountName: accountName);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && string.CompareOrdinal(command.AccountName, accountName) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValidWithoutDescription_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandWhereDescriptionIsEqualToNull()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel(hasDescription: false);

            await sut.UpdateAccount(accountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.Description == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValidWithDescription_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingDescriptionFromAccountViewModel()
        {
            Controller sut = CreateSut();

            string description = _fixture.Create<string>();
            AccountViewModel accountViewModel = CreateAccountViewModel(description: description);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && string.CompareOrdinal(command.Description, description) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValidWithoutNote_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandWhereNoteIsEqualToNull()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel(hasNote: false);

            await sut.UpdateAccount(accountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.Note == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValidWithNote_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingNoteFromAccountViewModel()
        {
            Controller sut = CreateSut();

            string note = _fixture.Create<string>();
            AccountViewModel accountViewModel = CreateAccountViewModel(note: note);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && string.CompareOrdinal(command.Note, note) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingAccountGroupNumberFromAccountViewModel()
        {
            Controller sut = CreateSut();

            int accountGroupNumber = _fixture.Create<int>();
            AccountGroupViewModel accountGroupViewModel = CreateAccountGroupViewModel(accountGroupNumber);
            AccountViewModel accountViewModel = CreateAccountViewModel(accountGroupViewModel: accountGroupViewModel);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.AccountGroupNumber == accountGroupNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingCreditInfoCollectionNotEqualToNull()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            await sut.UpdateAccount(accountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.CreditInfoCollection != null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValidWithCreditInfosOnlyContainingDataForLastYear_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingEmptyCreditInfoCollection()
        {
            Controller sut = CreateSut();

            CreditInfoDictionaryViewModel creditInfoDictionaryViewModel = CreateCreditInfoDictionaryViewModel(CreateCreditInfoViewModelCollectionForLastYear());
            AccountViewModel accountViewModel = CreateAccountViewModel(creditInfoDictionaryViewModel: creditInfoDictionaryViewModel);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.CreditInfoCollection.Any() == false)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValidWithCreditInfosOnlyContainingDataForYearToDate_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingCreditInfoCollectionWithOneCreditInfo()
        {
            Controller sut = CreateSut();

            CreditInfoDictionaryViewModel creditInfoDictionaryViewModel = CreateCreditInfoDictionaryViewModel(CreateCreditInfoViewModelCollectionForYearToDate());
            AccountViewModel accountViewModel = CreateAccountViewModel(creditInfoDictionaryViewModel: creditInfoDictionaryViewModel);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.CreditInfoCollection.Count() == 1)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValidWithCreditInfosOnlyContainingDataForYearToDate_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingCreditInfoCollectionWithCreditInfoForCurrentMonth()
        {
            Controller sut = CreateSut();

            CreditInfoDictionaryViewModel creditInfoDictionaryViewModel = CreateCreditInfoDictionaryViewModel(CreateCreditInfoViewModelCollectionForYearToDate());
            AccountViewModel accountViewModel = CreateAccountViewModel(creditInfoDictionaryViewModel: creditInfoDictionaryViewModel);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.CreditInfoCollection.All(creditInfo => creditInfo.Year == (short)DateTime.Today.Year && creditInfo.Month == (short)DateTime.Today.Month))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValidWithCreditInfosOnlyContainingDataForNext11Months_AssertPublishAsyncWasCalledOnCommandBusWithUpdateAccountCommandContainingCreditInfoCollectionWithCreditInfoForNext11Months()
        {
            Controller sut = CreateSut();

            CreditInfoViewModel[] creditInfoViewModelCollection = CreateCreditInfoViewModelCollectionForNext11Months().ToArray();
            CreditInfoDictionaryViewModel creditInfoDictionaryViewModel = CreateCreditInfoDictionaryViewModel(creditInfoViewModelCollection);
            AccountViewModel accountViewModel = CreateAccountViewModel(creditInfoDictionaryViewModel: creditInfoDictionaryViewModel);

            await sut.UpdateAccount(accountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateAccountCommand>(command => command != null && command.CreditInfoCollection.All(creditInfo => creditInfoViewModelCollection.SingleOrDefault(creditInfoViewModel => creditInfoViewModel.Year == creditInfo.Year && creditInfoViewModel.Month == creditInfo.Month) != null))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            IActionResult result = await sut.UpdateAccount(accountViewModel);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            IActionResult result = await sut.UpdateAccount(accountViewModel);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccount(accountViewModel);

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccount(accountViewModel);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccount(accountViewModel);

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountings()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccount(accountViewModel);

            Assert.That(result.ActionName, Is.EqualTo("Accountings"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccount(accountViewModel);

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumber()
        {
            Controller sut = CreateSut();

            AccountViewModel accountViewModel = CreateAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccount(accountViewModel);

            Assert.That(result.RouteValues.ContainsKey("accountingNumber"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateAccount_WhenAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumberWithAccountingNumberFromAccountViewModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            AccountingIdentificationViewModel accountingIdentificationViewModel = CreateAccountingIdentificationViewModel(accountingNumber);
            AccountViewModel accountViewModel = CreateAccountViewModel(accountingIdentificationViewModel);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateAccount(accountViewModel);

            Assert.That(result.RouteValues["accountingNumber"], Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut(bool modelIsValid = true, string errorKey = null, string errorMessage = null)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateAccountCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(errorKey ?? _fixture.Create<string>(), errorMessage ?? _fixture.Create<string>());
            }
            return controller;
        }

        private AccountViewModel CreateAccountViewModel(AccountingIdentificationViewModel accountingIdentificationViewModel = null, string accountNumber = null, string accountName = null, bool hasDescription = true, string description = null, bool hasNote = true, string note = null, AccountGroupViewModel accountGroupViewModel = null, CreditInfoDictionaryViewModel creditInfoDictionaryViewModel = null)
        {
            return _fixture.Build<AccountViewModel>()
                .With(m => m.Accounting, accountingIdentificationViewModel ?? CreateAccountingIdentificationViewModel())
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>())
                .With(m => m.AccountName, accountName ?? _fixture.Create<string>())
                .With(m => m.Description, hasDescription ? description ?? _fixture.Create<string>() : null)
                .With(m => m.Note, hasNote ? note ?? _fixture.Create<string>() : null)
                .With(m => m.AccountGroup, accountGroupViewModel ?? CreateAccountGroupViewModel())
                .With(m => m.CreditInfos, creditInfoDictionaryViewModel ?? CreateCreditInfoDictionaryViewModel())
                .Create();
        }

        private AccountingIdentificationViewModel CreateAccountingIdentificationViewModel(int? accountingNumber = null)
        {
            return _fixture.Build<AccountingIdentificationViewModel>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .Create();
        }

        private AccountGroupViewModel CreateAccountGroupViewModel(int? accountGroupNumber = null)
        {
            return _fixture.Build<AccountGroupViewModel>()
                .With(m => m.Number, accountGroupNumber ?? _fixture.Create<int>())
                .Create();
        }

        private CreditInfoDictionaryViewModel CreateCreditInfoDictionaryViewModel(IEnumerable<CreditInfoViewModel> creditInfoViewModelCollection = null)
        {
            return new()
            {
                Items = (creditInfoViewModelCollection ?? CreateCreditInfoViewModelCollection())
                    .GroupBy(creditInfoViewModel => creditInfoViewModel.Year)
                    .ToDictionary(item => item.Key, item => new CreditInfoCollectionViewModel {Items = item.ToArray()})
            };
        }

        private IEnumerable<CreditInfoViewModel> CreateCreditInfoViewModelCollection()
        {
            List<CreditInfoViewModel> creditInfoViewModelCollection = new List<CreditInfoViewModel>();
            creditInfoViewModelCollection.AddRange(CreateCreditInfoViewModelCollectionForLastYear());
            creditInfoViewModelCollection.AddRange(CreateCreditInfoViewModelCollectionForYearToDate());
            creditInfoViewModelCollection.AddRange(CreateCreditInfoViewModelCollectionForNext11Months());
            return creditInfoViewModelCollection;
        }

        private IEnumerable<CreditInfoViewModel> CreateCreditInfoViewModelCollectionForLastYear()
        {
            DateTime today = DateTime.Today;
            DateTime date = new DateTime(today.Year - 1, 1, 1);

            IList<CreditInfoViewModel> creditInfoViewModelCollection = new List<CreditInfoViewModel>();
            while (date.Year <= today.Year - 1)
            {
                creditInfoViewModelCollection.Add(CreateCreditInfoViewModel((short) date.Year, (short) date.Month));
                date = date.AddMonths(1);
            }
            return creditInfoViewModelCollection;
        }

        private IEnumerable<CreditInfoViewModel> CreateCreditInfoViewModelCollectionForYearToDate()
        {
            DateTime today = DateTime.Today;
            DateTime date = new DateTime(today.Year, 1, 1);

            IList<CreditInfoViewModel> creditInfoViewModelCollection = new List<CreditInfoViewModel>();
            while (date.Year == today.Year && date.Month <= today.Month)
            {
                creditInfoViewModelCollection.Add(CreateCreditInfoViewModel((short) date.Year, (short) date.Month));
                date = date.AddMonths(1);
            }
            return creditInfoViewModelCollection;
        }

        private IEnumerable<CreditInfoViewModel> CreateCreditInfoViewModelCollectionForNext11Months()
        {
            DateTime today = DateTime.Today;
            DateTime date = new DateTime(today.Year, today.Month, 1).AddMonths(1);

            IList<CreditInfoViewModel> creditInfoViewModelCollection = new List<CreditInfoViewModel>();
            while (date.Year == today.Year || date.Year == today.Year + 1 && date.Month < today.Month)
            {
                creditInfoViewModelCollection.Add(CreateCreditInfoViewModel((short) date.Year, (short) date.Month));
                date = date.AddMonths(1);
            }
            return creditInfoViewModelCollection;
        }

        private CreditInfoViewModel CreateCreditInfoViewModel(short year, short month)
        {
            return _fixture.Build<CreditInfoViewModel>()
                .With(m => m.Year, year)
                .With(m => m.Month, month)
                .Create();
        }
    }
}