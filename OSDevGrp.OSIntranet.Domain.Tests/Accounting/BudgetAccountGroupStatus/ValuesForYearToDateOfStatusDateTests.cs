using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroupStatus
{
    [TestFixture]
    public class ValuesForYearToDateOfStatusDateTests
    {
        #region Properties

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForYearToDateOfStatusDate_WhenCalled_AssertValuesForYearToDateOfStatusDateWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollectionMock.Object);

            // ReSharper disable UnusedVariable
            IBudgetInfoValues result = sut.ValuesForYearToDateOfStatusDate;
            // ReSharper restore UnusedVariable

            budgetAccountCollectionMock.Verify(m => m.ValuesForYearToDateOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForYearToDateOfStatusDate_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForYearToDateOfStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForYearToDateOfStatusDate_WhenCalled_ReturnsBudgetInfoValuesEqualToValuesForYearToDateOfStatusDateFromBudgetAccountCollection()
        {
            IBudgetInfoValues valuesForYearToDateOfStatusDate = _fixture.BuildBudgetInfoValuesMock().Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(valuesForYearToDateOfStatusDate: valuesForYearToDateOfStatusDate).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection);

            IBudgetInfoValues result = sut.ValuesForYearToDateOfStatusDate;

            Assert.That(result, Is.EqualTo(valuesForYearToDateOfStatusDate));
        }

        private IBudgetAccountGroupStatus CreateSut(IBudgetAccountCollection budgetAccountCollection = null)
        {
            return new Domain.Accounting.BudgetAccountGroupStatus(_fixture.BuildBudgetAccountGroupMock().Object, budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object);
        }
    }
}