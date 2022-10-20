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
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Accounting.Logic.BudgetAccountGroupStatusToCsvConverter
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
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            await sut.GetColumnNamesAsync();

            _statusDateProviderMock.Verify(m => m.GetStatusDate(), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.GetColumnNamesAsync();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollection()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            IEnumerable<string> result = await sut.GetColumnNamesAsync();

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollectionWithColumnNames()
        {
            IFormatProvider formatProvider = DefaultFormatProvider.Create();

            DateTime statusDate = DateTime.Today.AddDays(_random.Next(0, 7) * -1);
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut(statusDate);

            string[] result = (await sut.GetColumnNamesAsync()).ToArray();

            Assert.That(result.Length, Is.EqualTo(10));
            Assert.That(result[0], Is.EqualTo("Nummer"));
            Assert.That(result[1], Is.EqualTo("Navn"));
            Assert.That(result[2], Is.EqualTo($"Budget pr. {statusDate.ToDateText(formatProvider)}"));
            Assert.That(result[3], Is.EqualTo($"Bogført pr. {statusDate.ToDateText(formatProvider)}"));
            Assert.That(result[4], Is.EqualTo($"Budget {statusDate.GetEndDateOfLastMonth().ToMonthYearText(formatProvider)}"));
            Assert.That(result[5], Is.EqualTo($"Bogført {statusDate.GetEndDateOfLastMonth().ToMonthYearText(formatProvider)}"));
            Assert.That(result[6], Is.EqualTo($"Budget {statusDate.GetFirstDateOfYear().ToDateText(formatProvider)} til {statusDate.ToDateText(formatProvider)}"));
            Assert.That(result[7], Is.EqualTo($"Bogført {statusDate.GetFirstDateOfYear().ToDateText(formatProvider)} til {statusDate.ToDateText(formatProvider)}"));
            Assert.That(result[8], Is.EqualTo($"Budget {statusDate.GetEndDateOfLastYear().ToYearText(formatProvider)}"));
            Assert.That(result[9], Is.EqualTo($"Bogført {statusDate.GetEndDateOfLastYear().ToYearText(formatProvider)}"));
        }

        [Test]
        [Category("UnitTest")]
        public async Task GetColumnNamesAsync_WhenCalled_ReturnsNonEmptyCollectionWhichMatchNumberOfColumnsFromConvertAsync()
        {
            IBudgetAccountGroupStatusToCsvConverter sut = CreateSut();

            string[] result = (await sut.GetColumnNamesAsync()).ToArray();

            Assert.That(result.Count, Is.EqualTo((await sut.ConvertAsync(_fixture.BuildBudgetAccountGroupStatusMock().Object)).Count()));
        }

        private IBudgetAccountGroupStatusToCsvConverter CreateSut(DateTime? statusDate = null)
        {
            _statusDateProviderMock.Setup(m => m.GetStatusDate())
                .Returns(statusDate ?? DateTime.Today.AddDays(_random.Next(0, 7) * -1));

            return new BusinessLogic.Accounting.Logic.BudgetAccountGroupStatusToCsvConverter(_statusDateProviderMock.Object);
        }
    }
}