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
    public class CreateBudgetAccountWithModelTests
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
        public void CreateBudgetAccount_WhenBudgetAccountViewModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.CreateBudgetAccount(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("budgetAccountViewModel"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void CreateBudgetAccount_WhenBudgetAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToInternalError()
        {
            Controller sut = CreateSut(false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.CreateBudgetAccount(CreateBudgetAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.InternalError));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void CreateBudgetAccount_WhenBudgetAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereMessageEndsWithErrorMessagesFromModelState()
        {
            string errorMessage = _fixture.Create<string>();
            Controller sut = CreateSut(false, errorMessage: errorMessage);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.CreateBudgetAccount(CreateBudgetAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.EndsWith(errorMessage), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void CreateBudgetAccount_WhenBudgetAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            Controller sut = CreateSut(false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.CreateBudgetAccount(CreateBudgetAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingAccountingNumberFromBudgetAccountViewModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            AccountingIdentificationViewModel accountingIdentificationViewModel = CreateAccountingIdentificationViewModel(accountingNumber);
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(accountingIdentificationViewModel);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingAccountNumberFromBudgetAccountViewModel()
        {
            Controller sut = CreateSut();

            string accountNumber = _fixture.Create<string>();
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(accountNumber: accountNumber);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && string.CompareOrdinal(command.AccountNumber, accountNumber.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingAccountNameFromBudgetAccountViewModel()
        {
            Controller sut = CreateSut();

            string accountName = _fixture.Create<string>();
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(accountName: accountName);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && string.CompareOrdinal(command.AccountName, accountName) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValidWithoutDescription_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandWhereDescriptionIsEqualToNull()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(hasDescription: false);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.Description == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValidWithDescription_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingDescriptionFromBudgetAccountViewModel()
        {
            Controller sut = CreateSut();

            string description = _fixture.Create<string>();
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(description: description);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && string.CompareOrdinal(command.Description, description) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValidWithoutNote_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandWhereNoteIsEqualToNull()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(hasNote: false);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.Note == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValidWithNote_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingNoteFromBudgetAccountViewModel()
        {
            Controller sut = CreateSut();

            string note = _fixture.Create<string>();
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(note: note);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && string.CompareOrdinal(command.Note, note) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingBudgetAccountGroupNumberFromBudgetAccountViewModel()
        {
            Controller sut = CreateSut();

            int budgetAccountGroupNumber = _fixture.Create<int>();
            BudgetAccountGroupViewModel budgetAccountGroupViewModel = CreateBudgetAccountGroupViewModel(budgetAccountGroupNumber);
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(budgetAccountGroupViewModel: budgetAccountGroupViewModel);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.BudgetAccountGroupNumber == budgetAccountGroupNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingBudgetInfoCollectionNotEqualToNull()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.BudgetInfoCollection != null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValidWithBudgetInfosOnlyContainingDataForLastYear_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingEmptyBudgetInfoCollection()
        {
            Controller sut = CreateSut();

            BudgetInfoDictionaryViewModel budgetInfoDictionaryViewModel = CreateBudgetInfoDictionaryViewModel(CreateBudgetInfoViewModelCollectionForLastYear());
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(budgetInfoDictionaryViewModel: budgetInfoDictionaryViewModel);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.BudgetInfoCollection.Any() == false)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValidWithBudgetInfosOnlyContainingDataForYearToDate_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingBudgetInfoCollectionWithOneBudgetInfo()
        {
            Controller sut = CreateSut();

            BudgetInfoDictionaryViewModel budgetInfoDictionaryViewModel = CreateBudgetInfoDictionaryViewModel(CreateBudgetInfoViewModelCollectionForYearToDate());
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(budgetInfoDictionaryViewModel: budgetInfoDictionaryViewModel);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.BudgetInfoCollection.Count() == 1)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValidWithBudgetInfosOnlyContainingDataForYearToDate_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingBudgetInfoCollectionWithBudgetInfoForCurrentMonth()
        {
            Controller sut = CreateSut();

            BudgetInfoDictionaryViewModel budgetInfoDictionaryViewModel = CreateBudgetInfoDictionaryViewModel(CreateBudgetInfoViewModelCollectionForYearToDate());
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(budgetInfoDictionaryViewModel: budgetInfoDictionaryViewModel);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.BudgetInfoCollection.All(budgetInfo => budgetInfo.Year == (short) DateTime.Today.Year && budgetInfo.Month == (short) DateTime.Today.Month))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValidWithBudgetInfosOnlyContainingDataForNext11Months_AssertPublishAsyncWasCalledOnCommandBusWithCreateBudgetAccountCommandContainingBudgetInfoCollectionWithBudgetInfoForNext11Months()
        {
            Controller sut = CreateSut();

            BudgetInfoViewModel[] budgetInfoViewModelCollection = CreateBudgetInfoViewModelCollectionForNext11Months().ToArray();
            BudgetInfoDictionaryViewModel budgetInfoDictionaryViewModel = CreateBudgetInfoDictionaryViewModel(budgetInfoViewModelCollection);
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(budgetInfoDictionaryViewModel: budgetInfoDictionaryViewModel);

            await sut.CreateBudgetAccount(budgetAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<ICreateBudgetAccountCommand>(command => command != null && command.BudgetInfoCollection.All(budgetInfo => budgetInfoViewModelCollection.SingleOrDefault(budgetInfoViewModel => budgetInfoViewModel.Year == budgetInfo.Year && budgetInfoViewModel.Month == budgetInfo.Month) != null))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            IActionResult result = await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            IActionResult result = await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountings()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result.ActionName, Is.EqualTo("Accountings"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumber()
        {
            Controller sut = CreateSut();

            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result.RouteValues.ContainsKey("accountingNumber"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task CreateBudgetAccount_WhenBudgetAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumberWithAccountingNumberFromAccountViewModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            AccountingIdentificationViewModel accountingIdentificationViewModel = CreateAccountingIdentificationViewModel(accountingNumber);
            BudgetAccountViewModel budgetAccountViewModel = CreateBudgetAccountViewModel(accountingIdentificationViewModel);

            RedirectToActionResult result = (RedirectToActionResult) await sut.CreateBudgetAccount(budgetAccountViewModel);

            Assert.That(result.RouteValues["accountingNumber"], Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut(bool modelIsValid = true, string errorKey = null, string errorMessage = null)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<ICreateBudgetAccountCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(errorKey ?? _fixture.Create<string>(), errorMessage ?? _fixture.Create<string>());
            }
            return controller;
        }

        private BudgetAccountViewModel CreateBudgetAccountViewModel(AccountingIdentificationViewModel accountingIdentificationViewModel = null, string accountNumber = null, string accountName = null, bool hasDescription = true, string description = null, bool hasNote = true, string note = null, BudgetAccountGroupViewModel budgetAccountGroupViewModel = null, BudgetInfoDictionaryViewModel budgetInfoDictionaryViewModel = null)
        {
            return _fixture.Build<BudgetAccountViewModel>()
                .With(m => m.Accounting, accountingIdentificationViewModel ?? CreateAccountingIdentificationViewModel())
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>())
                .With(m => m.AccountName, accountName ?? _fixture.Create<string>())
                .With(m => m.Description, hasDescription ? description ?? _fixture.Create<string>() : null)
                .With(m => m.Note, hasNote ? note ?? _fixture.Create<string>() : null)
                .With(m => m.BudgetAccountGroup, budgetAccountGroupViewModel ?? CreateBudgetAccountGroupViewModel())
                .With(m => m.BudgetInfos, budgetInfoDictionaryViewModel ?? CreateBudgetInfoDictionaryViewModel())
                .Create();
        }

        private AccountingIdentificationViewModel CreateAccountingIdentificationViewModel(int? accountingNumber = null)
        {
            return _fixture.Build<AccountingIdentificationViewModel>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .Create();
        }

        private BudgetAccountGroupViewModel CreateBudgetAccountGroupViewModel(int? budgetAccountGroupNumber = null)
        {
            return _fixture.Build<BudgetAccountGroupViewModel>()
                .With(m => m.Number, budgetAccountGroupNumber ?? _fixture.Create<int>())
                .Create();
        }

        private BudgetInfoDictionaryViewModel CreateBudgetInfoDictionaryViewModel(IEnumerable<BudgetInfoViewModel> budgetInfoViewModelCollection = null)
        {
            return new()
            {
                Items = (budgetInfoViewModelCollection ?? CreateBudgetInfoViewModelCollection())
                    .GroupBy(budgetInfoViewModel => budgetInfoViewModel.Year)
                    .ToDictionary(item => item.Key, item => new BudgetInfoCollectionViewModel {Items = item.ToArray()})
            };
        }

        private IEnumerable<BudgetInfoViewModel> CreateBudgetInfoViewModelCollection()
        {
            List<BudgetInfoViewModel> budgetInfoViewModelCollection = new List<BudgetInfoViewModel>();
            budgetInfoViewModelCollection.AddRange(CreateBudgetInfoViewModelCollectionForLastYear());
            budgetInfoViewModelCollection.AddRange(CreateBudgetInfoViewModelCollectionForYearToDate());
            budgetInfoViewModelCollection.AddRange(CreateBudgetInfoViewModelCollectionForNext11Months());
            return budgetInfoViewModelCollection;
        }

        private IEnumerable<BudgetInfoViewModel> CreateBudgetInfoViewModelCollectionForLastYear()
        {
            DateTime today = DateTime.Today;
            DateTime date = new DateTime(today.Year - 1, 1, 1);

            IList<BudgetInfoViewModel> budgetInfoViewModelCollection = new List<BudgetInfoViewModel>();
            while (date.Year <= today.Year - 1)
            {
                budgetInfoViewModelCollection.Add(CreateBudgetInfoViewModel((short) date.Year, (short) date.Month));
                date = date.AddMonths(1);
            }
            return budgetInfoViewModelCollection;
        }

        private IEnumerable<BudgetInfoViewModel> CreateBudgetInfoViewModelCollectionForYearToDate()
        {
            DateTime today = DateTime.Today;
            DateTime date = new DateTime(today.Year, 1, 1);

            IList<BudgetInfoViewModel> budgetInfoViewModelCollection = new List<BudgetInfoViewModel>();
            while (date.Year == today.Year && date.Month <= today.Month)
            {
                budgetInfoViewModelCollection.Add(CreateBudgetInfoViewModel((short) date.Year, (short) date.Month));
                date = date.AddMonths(1);
            }
            return budgetInfoViewModelCollection;
        }

        private IEnumerable<BudgetInfoViewModel> CreateBudgetInfoViewModelCollectionForNext11Months()
        {
            DateTime today = DateTime.Today;
            DateTime date = new DateTime(today.Year, today.Month, 1).AddMonths(1);

            IList<BudgetInfoViewModel> budgetInfoViewModelCollection = new List<BudgetInfoViewModel>();
            while (date.Year == today.Year || date.Year == today.Year + 1 && date.Month < today.Month)
            {
                budgetInfoViewModelCollection.Add(CreateBudgetInfoViewModel((short) date.Year, (short) date.Month));
                date = date.AddMonths(1);
            }
            return budgetInfoViewModelCollection;
        }

        private BudgetInfoViewModel CreateBudgetInfoViewModel(short year, short month)
        {
            return _fixture.Build<BudgetInfoViewModel>()
                .With(m => m.Year, year)
                .With(m => m.Month, month)
                .Create();
        }
    }
}