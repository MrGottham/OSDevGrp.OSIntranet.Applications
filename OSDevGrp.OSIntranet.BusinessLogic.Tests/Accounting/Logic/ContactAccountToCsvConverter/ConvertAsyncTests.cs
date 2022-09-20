using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.ContactAccountToCsvConverter
{
    [TestFixture]
    public class ConvertAsyncTests
    {
        #region Private variables

        private Mock<IStatusDateProvider> _statusDateProviderMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _statusDateProviderMock = new Mock<IStatusDateProvider>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void ConvertAsync_WhenContactAccountIsNull_ThrowsArgumentNullException()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("contactAccount"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertAccountNumberWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertAccountNameWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.AccountName, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertMailAddressWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.MailAddress, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertPrimaryPhoneWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.PrimaryPhone, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertSecondaryPhoneWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.SecondaryPhone, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertDescriptionWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.Description, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertNoteWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.Note, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertPaymentTermWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.PaymentTerm, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertNumberWasCalledOnPaymentTermFromContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IPaymentTerm> paymentTermMock = _fixture.BuildPaymentTermMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(paymentTerm: paymentTermMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            paymentTermMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertNameWasCalledOnPaymentTermFromContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IPaymentTerm> paymentTermMock = _fixture.BuildPaymentTermMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(paymentTerm: paymentTermMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            paymentTermMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertValuesAtStatusDateWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertBalanceWasCalledOnValuesAtStatusDateFromContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactInfoValues> valuesAtStatusDateMock = _fixture.BuildContactInfoValuesMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(valuesAtStatusDate: valuesAtStatusDateMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            valuesAtStatusDateMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertValuesAtEndOfLastMonthFromStatusDateWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.ValuesAtEndOfLastMonthFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertBalanceWasCalledOnValuesAtEndOfLastMonthFromStatusDateFromContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactInfoValues> valuesAtEndOfLastMonthFromStatusDateMock = _fixture.BuildContactInfoValuesMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(valuesAtEndOfLastMonthFromStatusDate: valuesAtEndOfLastMonthFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            valuesAtEndOfLastMonthFromStatusDateMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertValuesAtEndOfLastYearFromStatusDateWasCalledOnContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactAccount> contactAccountMock = _fixture.BuildContactAccountMock();
            await sut.ConvertAsync(contactAccountMock.Object);

            contactAccountMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_AssertBalanceWasCalledOnValuesAtEndOfLastYearFromStatusDateFromContactAccount()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            Mock<IContactInfoValues> valuesAtEndOfLastYearFromStatusDateMock = _fixture.BuildContactInfoValuesMock();
            IContactAccount contactAccount = _fixture.BuildContactAccountMock(valuesAtEndOfLastYearFromStatusDate: valuesAtEndOfLastYearFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(contactAccount);

            valuesAtEndOfLastYearFromStatusDateMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_ReturnsNotNull()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_ReturnsNonEmptyCollection()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildContactAccountMock().Object);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenContactAccountIsNotNull_ReturnsNonEmptyCollectionWhichMatchNumberOfColumnsFromGetColumnNamesAsync()
        {
            IContactAccountToCsvConverter sut = CreateSut();

            string[] result = (await sut.ConvertAsync(_fixture.BuildContactAccountMock().Object)).ToArray();

            Assert.That(result.Count, Is.EqualTo((await sut.GetColumnNamesAsync()).Count()));
        }

        private IContactAccountToCsvConverter CreateSut()
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            return new BusinessLogic.Accounting.Logic.ContactAccountToCsvConverter(_statusDateProviderMock.Object);
        }
    }
}