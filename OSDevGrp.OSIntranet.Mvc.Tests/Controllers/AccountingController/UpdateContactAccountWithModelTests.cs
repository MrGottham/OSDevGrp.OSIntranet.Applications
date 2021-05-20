using System;
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
    public class UpdateContactAccountWithModelTests
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
        public void UpdateContactAccount_WhenContactAccountViewModelIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.UpdateContactAccount(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("contactAccountViewModel"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateContactAccount_WhenContactAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereErrorCodeIsEqualToInternalError()
        {
            Controller sut = CreateSut(false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.UpdateContactAccount(CreateContactAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ErrorCode, Is.EqualTo(ErrorCode.InternalError));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateContactAccount_WhenContactAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereMessageEndsWithErrorMessagesFromModelState()
        {
            string errorMessage = _fixture.Create<string>();
            Controller sut = CreateSut(false, errorMessage: errorMessage);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.UpdateContactAccount(CreateContactAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.Message.EndsWith(errorMessage), Is.True);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void UpdateContactAccount_WhenContactAccountViewModelIsInvalid_ThrowsIntranetSystemExceptionWhereInnerExceptionIsNull()
        {
            Controller sut = CreateSut(false);

            IntranetSystemException result = Assert.ThrowsAsync<IntranetSystemException>(async () => await sut.UpdateContactAccount(CreateContactAccountViewModel()));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.InnerException, Is.Null);
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingAccountingNumberFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            AccountingIdentificationViewModel accountingIdentificationViewModel = CreateAccountingIdentificationViewModel(accountingNumber);
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(accountingIdentificationViewModel);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && command.AccountingNumber == accountingNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingAccountNumberFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            string accountNumber = _fixture.Create<string>();
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(accountNumber: accountNumber);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && string.CompareOrdinal(command.AccountNumber, accountNumber.ToUpper()) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingAccountNameFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            string accountName = _fixture.Create<string>();
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(accountName: accountName);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && string.CompareOrdinal(command.AccountName, accountName) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithoutDescription_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandWhereDescriptionIsEqualToNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(hasDescription: false);

            await sut.UpdateContactAccount(contactAccountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && command.Description == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithDescription_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingDescriptionFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            string description = _fixture.Create<string>();
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(description: description);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && string.CompareOrdinal(command.Description, description) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithoutNote_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandWhereNoteIsEqualToNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(hasNote: false);

            await sut.UpdateContactAccount(contactAccountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && command.Note == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithNote_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingNoteFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            string note = _fixture.Create<string>();
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(note: note);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && string.CompareOrdinal(command.Note, note) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithoutMailAddress_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandWhereMailAddressIsEqualToNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(hasMailAddress: false);

            await sut.UpdateContactAccount(contactAccountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && command.MailAddress == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithMailAddress_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingMailAddressFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            string mailAddress = _fixture.Create<string>();
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(mailAddress: mailAddress);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && string.CompareOrdinal(command.MailAddress, mailAddress) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithoutPrimaryPhone_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandWherePrimaryPhoneIsEqualToNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(hasPrimaryPhone: false);

            await sut.UpdateContactAccount(contactAccountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && command.PrimaryPhone == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithPrimaryPhone_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingPrimaryPhoneFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            string primaryPhone = _fixture.Create<string>();
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(primaryPhone: primaryPhone);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && string.CompareOrdinal(command.PrimaryPhone, primaryPhone) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithoutSecondaryPhone_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandWhereSecondaryPhoneIsEqualToNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(hasSecondaryPhone: false);

            await sut.UpdateContactAccount(contactAccountViewModel);

            // ReSharper disable MergeIntoPattern
            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && command.SecondaryPhone == null)), Times.Once);
            // ReSharper restore MergeIntoPattern
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValidWithSecondaryPhone_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingSecondaryPhoneFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            string secondaryPhone = _fixture.Create<string>();
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(secondaryPhone: secondaryPhone);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && string.CompareOrdinal(command.SecondaryPhone, secondaryPhone) == 0)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_AssertPublishAsyncWasCalledOnCommandBusWithUpdateContactAccountCommandContainingPaymentTermNumberFromContactAccountViewModel()
        {
            Controller sut = CreateSut();

            int paymentTermNumber = _fixture.Create<int>();
            PaymentTermViewModel paymentTermViewModel = CreatePaymentTermViewModel(paymentTermNumber);
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(paymentTermViewModel: paymentTermViewModel);

            await sut.UpdateContactAccount(contactAccountViewModel);

            _commandBusMock.Verify(m => m.PublishAsync(It.Is<IUpdateContactAccountCommand>(command => command != null && command.PaymentTermNumber == paymentTermNumber)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel();

            IActionResult result = await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsRedirectToActionResult()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel();

            IActionResult result = await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result, Is.TypeOf<RedirectToActionResult>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsNotNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result.ControllerName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsRedirectToActionResultWhereControllerNameIsEqualToAccounting()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result.ControllerName, Is.EqualTo("Accounting"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsNotNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result.ActionName, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsRedirectToActionResultWhereActionNameIsEqualToAccountings()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result.ActionName, Is.EqualTo("Accountings"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesIsNotNull()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result.RouteValues, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumber()
        {
            Controller sut = CreateSut();

            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel();

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result.RouteValues.ContainsKey("accountingNumber"), Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public async Task UpdateContactAccount_WhenContactAccountViewModelIsValid_ReturnsRedirectToActionResultWhereRouteValuesContainsAccountingNumberWithAccountingNumberFromAccountViewModel()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            AccountingIdentificationViewModel accountingIdentificationViewModel = CreateAccountingIdentificationViewModel(accountingNumber);
            ContactAccountViewModel contactAccountViewModel = CreateContactAccountViewModel(accountingIdentificationViewModel);

            RedirectToActionResult result = (RedirectToActionResult) await sut.UpdateContactAccount(contactAccountViewModel);

            Assert.That(result.RouteValues["accountingNumber"], Is.EqualTo(accountingNumber));
        }

        private Controller CreateSut(bool modelIsValid = true, string errorKey = null, string errorMessage = null)
        {
            _commandBusMock.Setup(m => m.PublishAsync(It.IsAny<IUpdateContactAccountCommand>()))
                .Returns(Task.CompletedTask);

            Controller controller = new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
            if (modelIsValid == false)
            {
                controller.ModelState.AddModelError(errorKey ?? _fixture.Create<string>(), errorMessage ?? _fixture.Create<string>());
            }
            return controller;
        }

        private ContactAccountViewModel CreateContactAccountViewModel(AccountingIdentificationViewModel accountingIdentificationViewModel = null, string accountNumber = null, string accountName = null, bool hasDescription = true, string description = null, bool hasNote = true, string note = null, bool hasMailAddress = true, string mailAddress = null, bool hasPrimaryPhone = true, string primaryPhone = null, bool hasSecondaryPhone = true, string secondaryPhone = null, PaymentTermViewModel paymentTermViewModel = null)
        {
            return _fixture.Build<ContactAccountViewModel>()
                .With(m => m.Accounting, accountingIdentificationViewModel ?? CreateAccountingIdentificationViewModel())
                .With(m => m.AccountNumber, accountNumber ?? _fixture.Create<string>())
                .With(m => m.AccountName, accountName ?? _fixture.Create<string>())
                .With(m => m.Description, hasDescription ? description ?? _fixture.Create<string>() : null)
                .With(m => m.Note, hasNote ? note ?? _fixture.Create<string>() : null)
                .With(m => m.MailAddress, hasMailAddress ? mailAddress ?? _fixture.Create<string>() : null)
                .With(m => m.PrimaryPhone, hasPrimaryPhone ? primaryPhone ?? _fixture.Create<string>() : null)
                .With(m => m.SecondaryPhone, hasSecondaryPhone ? secondaryPhone ?? _fixture.Create<string>() : null)
                .With(m => m.PaymentTerm, paymentTermViewModel ?? CreatePaymentTermViewModel())
                .With(m => m.BalanceInfos, CreateBalanceInfoDictionaryViewModel())
                .Create();
        }

        private AccountingIdentificationViewModel CreateAccountingIdentificationViewModel(int? accountingNumber = null)
        {
            return _fixture.Build<AccountingIdentificationViewModel>()
                .With(m => m.AccountingNumber, accountingNumber ?? _fixture.Create<int>())
                .Create();
        }

        private PaymentTermViewModel CreatePaymentTermViewModel(int? paymentTermNumber = null)
        {
            return _fixture.Build<PaymentTermViewModel>()
                .With(m => m.Number, paymentTermNumber ?? _fixture.Create<int>())
                .Create();
        }

        private BalanceInfoDictionaryViewModel CreateBalanceInfoDictionaryViewModel()
        {
            return _fixture.Create<BalanceInfoDictionaryViewModel>();
        }
    }
}