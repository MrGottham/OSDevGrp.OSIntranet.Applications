using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.AccountGroupStatusToCsvConverter
{
    [TestFixture]
    public class GetColumnNamesAsyncTests
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
        public async Task GetColumnNamesAsync_WhenCalled_AssertGetStatusDateWasCalledOnStatusDateProvider()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            await sut.GetColumnNamesAsync();

            _statusDateProviderMock.Verify(m => m.GetStatusDate(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNotNull()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.GetColumnNamesAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollection()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.GetColumnNamesAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollectionWithColumnNames()
        {
            IFormatProvider formatProvider = DefaultFormatProvider.Create();

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IAccountGroupStatusToCsvConverter sut = CreateSut(statusDate);

            string[] result = (await sut.GetColumnNamesAsync()).ToArray();

            Assert.That(result.Length, Is.EqualTo(9));
            Assert.That(result[0], Is.EqualTo("Nummer"));
            Assert.That(result[1], Is.EqualTo("Navn"));
            Assert.That(result[2], Is.EqualTo("Type"));
            Assert.That(result[3], Is.EqualTo($"{AccountGroupType.Assets.Translate()} pr. {statusDate.ToDateText(formatProvider)}"));
            Assert.That(result[4], Is.EqualTo($"{AccountGroupType.Liabilities.Translate()} pr. {statusDate.ToDateText(formatProvider)}"));
            Assert.That(result[5], Is.EqualTo($"{AccountGroupType.Assets.Translate()} pr. {statusDate.GetEndDateOfLastMonth().ToDateText(formatProvider)}"));
            Assert.That(result[6], Is.EqualTo($"{AccountGroupType.Liabilities.Translate()} pr. {statusDate.GetEndDateOfLastMonth().ToDateText(formatProvider)}"));
            Assert.That(result[7], Is.EqualTo($"{AccountGroupType.Assets.Translate()} pr. {statusDate.GetEndDateOfLastYear().ToDateText(formatProvider)}"));
            Assert.That(result[8], Is.EqualTo($"{AccountGroupType.Liabilities.Translate()} pr. {statusDate.GetEndDateOfLastYear().ToDateText(formatProvider)}"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollectionWhichMatchNumberOfColumnsFromConvertAsync()
        {
            IAccountGroupStatusToCsvConverter sut = CreateSut();

            string[] result = (await sut.GetColumnNamesAsync()).ToArray();

            Assert.That(result.Count, Is.EqualTo((await sut.ConvertAsync(_fixture.BuildAccountGroupStatusMock().Object)).Count()));
        }

        private IAccountGroupStatusToCsvConverter CreateSut(DateTime? statusDate = null)
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            return new BusinessLogic.Accounting.Logic.AccountGroupStatusToCsvConverter(_statusDateProviderMock.Object);
        }
    }
}