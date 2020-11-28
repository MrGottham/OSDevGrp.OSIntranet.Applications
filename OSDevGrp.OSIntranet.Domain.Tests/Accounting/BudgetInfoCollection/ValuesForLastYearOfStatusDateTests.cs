using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.BudgetInfoCollection
{
    [TestFixture]
    public class ValuesForLastYearOfStatusDateTests
    {
        [Test]
        [Category("UnitTest")]
        public void ValuesForLastYearOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsNotNull()
        {
            IBudgetInfoCollection sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForLastYearOfStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForLastYearOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsBudgetInfoValuesWhereBudgetIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForLastYearOfStatusDate;

            Assert.That(result.Budget, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesForLastYearOfStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsBudgetInfoValuesWherePostedIsEqualToZero()
        {
            IBudgetInfoCollection sut = CreateSut();

            IBudgetInfoValues result = sut.ValuesForLastYearOfStatusDate;

            Assert.That(result.Posted, Is.EqualTo(0M));
        }

        private IBudgetInfoCollection CreateSut()
        {
            return new Domain.Accounting.BudgetInfoCollection();
        }
    }
}