using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetInfoCollection
{
    [TestFixture]
    public class ValuesForLastMonthOfStatusDateTests
    {
        [Test]
        [Category("UnitTest")]
        public void ValuesForLastMonthOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForLastMonthOfStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForLastMonthOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsBudgetInfoValuesWhereBudgetIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForLastMonthOfStatusDate;

            Assert.That(result.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForLastMonthOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsBudgetInfoValuesWherePostedIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForLastMonthOfStatusDate;

            Assert.That(result.Posted, Is.EqualTo(0M));
        }

        private IBudgetInfoCollection CreateSut()
        {
            return new Domain.Accounting.BudgetInfoCollection();
        }
    }
}