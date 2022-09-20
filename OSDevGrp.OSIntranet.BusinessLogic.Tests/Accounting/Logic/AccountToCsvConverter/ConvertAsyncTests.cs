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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.AccountToCsvConverter
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
        public void ConvertAsync_WhenAccountIsNull_ThrowsArgumentNullException()
        {
            IAccountToCsvConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("account"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertAccountNumberWasCalledOnAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.AccountNumber, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertAccountNameWasCalledOnAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.AccountName, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertDescriptionWasCalledOnAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.Description, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertNoteWasCalledOnAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.Note, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertAccountGroupWasCalledOnAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.AccountGroup, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertNumberWasCalledOnAccountGroupFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            IAccount account = _fixture.BuildAccountMock(accountGroup: accountGroupMock.Object).Object;
            await sut.ConvertAsync(account);

            accountGroupMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertNameWasCalledOnAccountGroupFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            IAccount account = _fixture.BuildAccountMock(accountGroup: accountGroupMock.Object).Object;
            await sut.ConvertAsync(account);

            accountGroupMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertAccountGroupTypeWasCalledOnAccountGroupFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccountGroup> accountGroupMock = _fixture.BuildAccountGroupMock();
            IAccount account = _fixture.BuildAccountMock(accountGroup: accountGroupMock.Object).Object;
            await sut.ConvertAsync(account);

            accountGroupMock.Verify(m => m.AccountGroupType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertValuesAtStatusDateWasCalledOnAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertBalanceWasCalledOnValuesAtStatusDateFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<ICreditInfoValues> valuesAtStatusDateMock = _fixture.BuildCreditInfoValuesMock();
            IAccount account = _fixture.BuildAccountMock(valuesAtStatusDate: valuesAtStatusDateMock.Object).Object;
            await sut.ConvertAsync(account);

            valuesAtStatusDateMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertCreditWasCalledOnValuesAtStatusDateFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<ICreditInfoValues> valuesAtStatusDateMock = _fixture.BuildCreditInfoValuesMock();
            IAccount account = _fixture.BuildAccountMock(valuesAtStatusDate: valuesAtStatusDateMock.Object).Object;
            await sut.ConvertAsync(account);

            valuesAtStatusDateMock.Verify(m => m.Credit, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertValuesAtEndOfLastMonthFromStatusDateWasCalledOnAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.ValuesAtEndOfLastMonthFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertBalanceWasCalledOnValuesAtEndOfLastMonthFromStatusDateFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<ICreditInfoValues> valuesAtEndOfLastMonthFromStatusDateMock = _fixture.BuildCreditInfoValuesMock();
            IAccount account = _fixture.BuildAccountMock(valuesAtEndOfLastMonthFromStatusDate: valuesAtEndOfLastMonthFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(account);

            valuesAtEndOfLastMonthFromStatusDateMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertCreditWasCalledOnValuesAtEndOfLastMonthFromStatusDateFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<ICreditInfoValues> valuesAtEndOfLastMonthFromStatusDateMock = _fixture.BuildCreditInfoValuesMock();
            IAccount account = _fixture.BuildAccountMock(valuesAtEndOfLastMonthFromStatusDate: valuesAtEndOfLastMonthFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(account);

            valuesAtEndOfLastMonthFromStatusDateMock.Verify(m => m.Credit, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertValuesAtValuesAtEndOfLastYearFromStatusDateWasCalledOnAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<IAccount> accountMock = _fixture.BuildAccountMock();
            await sut.ConvertAsync(accountMock.Object);

            accountMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertBalanceWasCalledOnValuesAtEndOfLastYearFromStatusDateFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<ICreditInfoValues> valuesAtEndOfLastYearFromStatusDateMock = _fixture.BuildCreditInfoValuesMock();
            IAccount account = _fixture.BuildAccountMock(valuesAtEndOfLastYearFromStatusDate: valuesAtEndOfLastYearFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(account);

            valuesAtEndOfLastYearFromStatusDateMock.Verify(m => m.Balance, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_AssertCreditWasCalledOnValuesAtEndOfLastYearFromStatusDateFromAccount()
        {
            IAccountToCsvConverter sut = CreateSut();

            Mock<ICreditInfoValues> valuesAtEndOfLastYearFromStatusDateMock = _fixture.BuildCreditInfoValuesMock();
            IAccount account = _fixture.BuildAccountMock(valuesAtEndOfLastYearFromStatusDate: valuesAtEndOfLastYearFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(account);

            valuesAtEndOfLastYearFromStatusDateMock.Verify(m => m.Credit, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_ReturnsNotNull()
        {
            IAccountToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_ReturnsNonEmptyCollection()
        {
            IAccountToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildAccountMock().Object);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountIsNotNull_ReturnsNonEmptyCollectionWhichMatchNumberOfColumnsFromGetColumnNamesAsync()
        {
            IAccountToCsvConverter sut = CreateSut();

            string[] result = (await sut.ConvertAsync(_fixture.BuildAccountMock().Object)).ToArray();

            Assert.That(result.Count, Is.EqualTo((await sut.GetColumnNamesAsync()).Count()));
        }

        private IAccountToCsvConverter CreateSut()
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            return new BusinessLogic.Accounting.Logic.AccountToCsvConverter(_statusDateProviderMock.Object);
        }
    }
}