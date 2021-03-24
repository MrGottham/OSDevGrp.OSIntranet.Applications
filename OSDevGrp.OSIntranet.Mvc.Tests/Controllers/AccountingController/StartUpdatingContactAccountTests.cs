using System;
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
    public class StartUpdatingContactAccountTests
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
        public void StartUpdatingContactAccount_WhenAccountNumberIsNull_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StartUpdatingContactAccount(_fixture.Create<int>(), null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsEmpty_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StartUpdatingContactAccount(_fixture.Create<int>(), string.Empty));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsWhiteSpace_ThrowsArgumentNullException()
        {
            Controller sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.StartUpdatingContactAccount(_fixture.Create<int>(), " "));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountNumber"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_ReturnsNotNull()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.StartUpdatingContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResult()
        {
            Controller sut = CreateSut();

            IActionResult result = sut.StartUpdatingContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result, Is.TypeOf<PartialViewResult>());
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereViewNameIsEqualToUpdatingContactAccountPartial()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartUpdatingContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.ViewName, Is.EqualTo("_UpdatingContactAccountPartial"));
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereModelIsNotNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartUpdatingContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereModelIsAccountIdentificationViewModel()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartUpdatingContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            Assert.That(result.Model, Is.TypeOf<AccountIdentificationViewModel>());
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereModelIsAccountIdentificationViewModelWithAccountingNotEqualToNull()
        {
            Controller sut = CreateSut();

            PartialViewResult result = (PartialViewResult) sut.StartUpdatingContactAccount(_fixture.Create<int>(), _fixture.Create<string>());

            AccountIdentificationViewModel accountIdentificationViewModel = (AccountIdentificationViewModel) result.Model;

            Assert.That(accountIdentificationViewModel.Accounting, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereModelIsAccountIdentificationViewModelWithAccountingWhereAccountingNumberEqualToArgument()
        {
            Controller sut = CreateSut();

            int accountingNumber = _fixture.Create<int>();
            PartialViewResult result = (PartialViewResult) sut.StartUpdatingContactAccount(accountingNumber, _fixture.Create<string>());

            AccountIdentificationViewModel accountIdentificationViewModel = (AccountIdentificationViewModel) result.Model;

            Assert.That(accountIdentificationViewModel.Accounting.AccountingNumber, Is.EqualTo(accountingNumber));
        }

        [Test]
        [Category("UnitTest")]
        public void StartUpdatingContactAccount_WhenAccountNumberIsNotNullEmptyOrWhiteSpace_ReturnsPartialViewResultWhereModelIsAccountIdentificationViewModelWithAccountNumberEqualToArgument()
        {
            Controller sut = CreateSut();

            string accountNumber = _fixture.Create<string>();
            PartialViewResult result = (PartialViewResult) sut.StartUpdatingContactAccount(_fixture.Create<int>(), accountNumber);

            AccountIdentificationViewModel accountIdentificationViewModel = (AccountIdentificationViewModel)result.Model;

            Assert.That(accountIdentificationViewModel.AccountNumber, Is.EqualTo(accountNumber));
        }

        private Controller CreateSut()
        {
            return new Controller(_commandBusMock.Object, _queryBusMock.Object, _claimResolverMock.Object);
        }
    }
}