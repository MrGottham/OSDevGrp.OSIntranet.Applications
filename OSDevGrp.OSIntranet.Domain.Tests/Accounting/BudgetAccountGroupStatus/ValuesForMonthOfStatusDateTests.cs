using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroupStatus
{
    [TestFixture]
    public class ValuesForMonthOfStatusDateTests
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
        public void ValuesForMonthOfStatusDate_WhenCalled_AssertValuesForMonthOfStatusDateWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollectionMock.Object);

            // ReSharper disable UnusedVariable
            IBudgetInfoValues result = sut.ValuesForMonthOfStatusDate;
            // ReSharper restore UnusedVariable

            budgetAccountCollectionMock.Verify(m => m.ValuesForMonthOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForMonthOfStatusDate_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForMonthOfStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForMonthOfStatusDate_WhenCalled_ReturnsBudgetInfoValuesEqualToValuesForMonthOfStatusDateFromBudgetAccountCollection()
        {
            IBudgetInfoValues valuesForMonthOfStatusDate = _fixture.BuildBudgetInfoValuesMock().Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(valuesForMonthOfStatusDate: valuesForMonthOfStatusDate).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection);

            IBudgetInfoValues result = sut.ValuesForMonthOfStatusDate;

            Assert.That(result, Is.EqualTo(valuesForMonthOfStatusDate));
        }

        private IBudgetAccountGroupStatus CreateSut(IBudgetAccountCollection budgetAccountCollection = null)
        {
            return new Domain.Accounting.BudgetAccountGroupStatus(_fixture.BuildBudgetAccountGroupMock().Object, budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object);
        }
    }
}