using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.ContactInfoCollection
{
    [TestFixture]
    public class ValuesAtStatusDateTests
    {
        [Test]
        [Category("UnitTest")]
        public void ValuesAtStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsNotNull()
        {
            IContactInfoCollection sut = CreateSut();

            IContactInfoValues result = sut.ValuesAtStatusDate;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ValuesAtStatusDate_WhenCalculateAsyncHasNotBeenCalled_ReturnsContactInfoValuesWhereBalanceIsEqualToZero()
        {
            IContactInfoCollection sut = CreateSut();

            IContactInfoValues result = sut.ValuesAtStatusDate;

            Assert.That(result.Balance, Is.EqualTo(0M));
        }

        private IContactInfoCollection CreateSut()
        {
            return new Domain.Accounting.ContactInfoCollection();
        }
    }
}