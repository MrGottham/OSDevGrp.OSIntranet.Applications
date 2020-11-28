using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.CreditInfoCollection
{
    [TestFixture]
    public class ValuesAtEndOfLastYearFromStatusDateTests
    {
        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsNotNull()
        {
            ICreditInfoCollection sut = CreateSut();

            ICreditInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsCreditInfoValuesWhereCreditIsEqualToZero()
        {
            ICreditInfoCollection sut = CreateSut();

            ICreditInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result.Credit, Is.EqualTo(0M));
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtEndOfLastYearFromStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsCreditInfoValuesWhereBalanceIsEqualToZero()
        {
            ICreditInfoCollection sut = CreateSut();

            ICreditInfoValues result = sut.ValuesAtEndOfLastYearFromStatusDate;

            Assert.That(result.Balance, Is.EqualTo(0M));
        }

        private ICreditInfoCollection CreateSut()
        {
            return new Domain.Accounting.CreditInfoCollection();
        }
    }
}