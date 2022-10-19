using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroupStatus
{
    [TestFixture]
    public class ValuesForLastMonthOfStatusDateTests
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
        public void ValuesForLastMonthOfStatusDate_WhenCalled_AssertValuesForLastMonthOfStatusDateWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollectionMock.Object);

            // ReSharper disable UnusedVariable
            IBudgetInfoValues result = sut.ValuesForLastMonthOfStatusDate;
            // ReSharper restore UnusedVariable

            budgetAccountCollectionMock.Verify(m => m.ValuesForLastMonthOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForLastMonthOfStatusDate_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForLastMonthOfStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForLastMonthOfStatusDate_WhenCalled_ReturnsBudgetInfoValuesEqualToValuesForLastMonthOfStatusDateFromBudgetAccountCollection()
        {
            IBudgetInfoValues valuesForLastMonthOfStatusDate = _fixture.BuildBudgetInfoValuesMock().Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(valuesForLastMonthOfStatusDate: valuesForLastMonthOfStatusDate).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection);

            IBudgetInfoValues result = sut.ValuesForLastMonthOfStatusDate;

            Assert.That(result, Is.EqualTo(valuesForLastMonthOfStatusDate));
        }

        private IBudgetAccountGroupStatus CreateSut(IBudgetAccountCollection budgetAccountCollection = null)
        {
            return new Domain.Accounting.BudgetAccountGroupStatus(_fixture.BuildBudgetAccountGroupMock().Object, budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object);
        }
    }
}