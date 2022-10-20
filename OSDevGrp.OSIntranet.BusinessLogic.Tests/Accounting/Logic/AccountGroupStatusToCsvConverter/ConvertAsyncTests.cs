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

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.AccountGroupStatusToCsvConverter
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
        public void ConvertAsync_WhenAccountGroupStatusIsNull_ThrowsArgumentNullException()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            ArgumentNullException result = Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.ConvertAsync(null));

            // ReSharper disable PossibleNullReferenceException
            Assert.That(result.ParamName, Is.EqualTo("accountGroupStatus"));
            // ReSharper restore PossibleNullReferenceException
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertNumberWasCalledOnAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountGroupStatus> accountGroupStatusMock = _fixture.BuildAccountGroupStatusMock();
            await sut.ConvertAsync(accountGroupStatusMock.Object);

            accountGroupStatusMock.Verify(m => m.Number, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertNameWasCalledOnAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountGroupStatus> accountGroupStatusMock = _fixture.BuildAccountGroupStatusMock();
            await sut.ConvertAsync(accountGroupStatusMock.Object);

            accountGroupStatusMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertAccountGroupTypeWasCalledOnAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountGroupStatus> accountGroupStatusMock = _fixture.BuildAccountGroupStatusMock();
            await sut.ConvertAsync(accountGroupStatusMock.Object);

            accountGroupStatusMock.Verify(m => m.AccountGroupType, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertValuesAtStatusDateWasCalledOnAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountGroupStatus> accountGroupStatusMock = _fixture.BuildAccountGroupStatusMock();
            await sut.ConvertAsync(accountGroupStatusMock.Object);

            accountGroupStatusMock.Verify(m => m.ValuesAtStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertAssetsWasCalledOnValuesAtStatusDateFromAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountCollectionValues> valuesAtStatusDateMock = _fixture.BuildAccountCollectionValuesMock();
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(valuesAtStatusDate: valuesAtStatusDateMock.Object).Object;
            await sut.ConvertAsync(accountGroupStatus);

            valuesAtStatusDateMock.Verify(m => m.Assets, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertLiabilitiesWasCalledOnValuesAtStatusDateFromAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountCollectionValues> valuesAtStatusDateMock = _fixture.BuildAccountCollectionValuesMock();
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(valuesAtStatusDate: valuesAtStatusDateMock.Object).Object;
            await sut.ConvertAsync(accountGroupStatus);

            valuesAtStatusDateMock.Verify(m => m.Liabilities, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertValuesAtEndOfLastMonthFromStatusDateWasCalledOnAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountGroupStatus> accountGroupStatusMock = _fixture.BuildAccountGroupStatusMock();
            await sut.ConvertAsync(accountGroupStatusMock.Object);

            accountGroupStatusMock.Verify(m => m.ValuesAtEndOfLastMonthFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertAssetsWasCalledOnValuesAtEndOfLastMonthFromStatusDateFromAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountCollectionValues> valuesAtEndOfLastMonthFromStatusDateMock = _fixture.BuildAccountCollectionValuesMock();
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(valuesAtEndOfLastMonthFromStatusDate: valuesAtEndOfLastMonthFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(accountGroupStatus);

            valuesAtEndOfLastMonthFromStatusDateMock.Verify(m => m.Assets, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertLiabilitiesWasCalledOnValuesAtEndOfLastMonthFromStatusDateFromAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountCollectionValues> valuesAtEndOfLastMonthFromStatusDateMock = _fixture.BuildAccountCollectionValuesMock();
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(valuesAtEndOfLastMonthFromStatusDate: valuesAtEndOfLastMonthFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(accountGroupStatus);

            valuesAtEndOfLastMonthFromStatusDateMock.Verify(m => m.Liabilities, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertValuesAtEndOfLastYearFromStatusDateWasCalledOnAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountGroupStatus> accountGroupStatusMock = _fixture.BuildAccountGroupStatusMock();
            await sut.ConvertAsync(accountGroupStatusMock.Object);

            accountGroupStatusMock.Verify(m => m.ValuesAtEndOfLastYearFromStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertAssetsWasCalledOnValuesAtEndOfLastYearFromStatusDateFromAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountCollectionValues> valuesAtEndOfLastYearFromStatusDateMock = _fixture.BuildAccountCollectionValuesMock();
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(valuesAtEndOfLastYearFromStatusDate: valuesAtEndOfLastYearFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(accountGroupStatus);

            valuesAtEndOfLastYearFromStatusDateMock.Verify(m => m.Assets, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_AssertLiabilitiesWasCalledOnValuesAtEndOfLastYearFromStatusDateFromAccountGroupStatus()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            Mock<IAccountCollectionValues> valuesAtEndOfLastYearFromStatusDateMock = _fixture.BuildAccountCollectionValuesMock();
            IAccountGroupStatus accountGroupStatus = _fixture.BuildAccountGroupStatusMock(valuesAtEndOfLastYearFromStatusDate: valuesAtEndOfLastYearFromStatusDateMock.Object).Object;
            await sut.ConvertAsync(accountGroupStatus);

            valuesAtEndOfLastYearFromStatusDateMock.Verify(m => m.Liabilities, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_ReturnsNotNull()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildAccountGroupStatusMock().Object);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_ReturnsNonEmptyCollection()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.ConvertAsync(_fixture.BuildAccountGroupStatusMock().Object);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task ConvertAsync_WhenAccountGroupStatusIsNotNull_ReturnsNonEmptyCollectionWhichMatchNumberOfColumnsFromGetColumnNamesAsync()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            string[] result = (await sut.ConvertAsync(_fixture.BuildAccountGroupStatusMock().Object)).ToArray();

            Assert.That(result.Count, Is.EqualTo((await sut.GetColumnNamesAsync()).Count()));
        }

        private IAccountGroupStatusToCsvConverter CreateSut()
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            return new BusinessLogic.Accounting.Logic.AccountGroupStatusToCsvConverter(_statusDateProviderMock.Object);
        }
    }
}