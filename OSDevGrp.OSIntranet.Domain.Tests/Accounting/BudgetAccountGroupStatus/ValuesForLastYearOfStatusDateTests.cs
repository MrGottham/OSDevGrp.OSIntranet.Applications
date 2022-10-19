using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccountGroupStatus
{
    [TestFixture]
    public class ValuesForLastYearOfStatusDateTests
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
        public void ValuesForLastYearOfStatusDate_WhenCalled_AssertValuesForLastYearOfStatusDateWasCalledOnBudgetAccountCollection()
        {
            Mock<IBudgetAccountCollection> budgetAccountCollectionMock = _fixture.BuildBudgetAccountCollectionMock();
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollectionMock.Object);

            // ReSharper disable UnusedVariable
            IBudgetInfoValues result = sut.ValuesForLastYearOfStatusDate;
            // ReSharper restore UnusedVariable

            budgetAccountCollectionMock.Verify(m => m.ValuesForLastYearOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForLastYearOfStatusDate_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccountGroupStatus sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForLastYearOfStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForLastYearOfStatusDate_WhenCalled_ReturnsBudgetInfoValuesEqualToValuesForLastYearOfStatusDateFromBudgetAccountCollection()
        {
            IBudgetInfoValues valuesForLastYearOfStatusDate = _fixture.BuildBudgetInfoValuesMock().Object;
            IBudgetAccountCollection budgetAccountCollection = _fixture.BuildBudgetAccountCollectionMock(valuesForLastYearOfStatusDate: valuesForLastYearOfStatusDate).Object;
            IBudgetAccountGroupStatus sut = CreateSut(budgetAccountCollection);

            IBudgetInfoValues result = sut.ValuesForLastYearOfStatusDate;

            Assert.That(result, Is.EqualTo(valuesForLastYearOfStatusDate));
        }

        private IBudgetAccountGroupStatus CreateSut(IBudgetAccountCollection budgetAccountCollection = null)
        {
            return new Domain.Accounting.BudgetAccountGroupStatus(_fixture.BuildBudgetAccountGroupMock().Object, budgetAccountCollection ?? _fixture.BuildBudgetAccountCollectionMock().Object);
        }
    }
}