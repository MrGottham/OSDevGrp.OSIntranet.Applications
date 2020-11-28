using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetAccount
{
    [TestFixture]
    public class ValuesForMonthOfStatusDateTests
    {
        #region Private variables

        private Fixture _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForMonthOfStatusDate_WhenCalled_AssertValuesForMonthOfStatusDateWasCalledOnBudgetInfoCollection()
        {
            Mock<IBudgetInfoCollection> budgetInfoCollectionMock = _fixture.BuildBudgetInfoCollectionMock();
            IBudgetAccount sut = CreateSut(budgetInfoCollectionMock.Object);

            IBudgetInfoValues result = sut.ValuesForMonthOfStatusDate;
            Assert.That(result, Is.Not.Null);

            budgetInfoCollectionMock.Verify(m => m.ValuesForMonthOfStatusDate, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForMonthOfStatusDate_WhenCalled_ReturnsNotNull()
        {
            IBudgetAccount sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForMonthOfStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForMonthOfStatusDate_WhenCalled_ReturnsSameBudgetInfoValuesAsValuesForMonthOfStatusDateOnBudgetInfoCollection()
        {
            IBudgetInfoCollection budgetInfoCollection = _fixture.BuildBudgetInfoCollectionMock().Object;
            IBudgetAccount sut = CreateSut(budgetInfoCollection);

            IBudgetInfoValues result = sut.ValuesForMonthOfStatusDate;

            Assert.That(result, Is.SameAs(budgetInfoCollection.ValuesForMonthOfStatusDate));
        }

        private IBudgetAccount CreateSut(IBudgetInfoCollection budgetInfoCollection = null)
        {
            return new Domain.Accounting.BudgetAccount(_fixture.BuildAccountingMock().Object, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.BuildBudgetAccountGroupMock().Object, budgetInfoCollection ?? _fixture.BuildBudgetInfoCollectionMock().Object, _fixture.BuildPostingLineCollectionMock().Object);
        }
    }
}