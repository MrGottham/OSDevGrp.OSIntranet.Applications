using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.TestHelpers;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.Accounting
{
    [TestFixture]
    public class ApplyDefaultForPrincipalTests
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
        public void ApplyDefaultForPrincipal_WhenDefaultAccountingNumberIsNull_AssertDefaultForPrincipalEqualToFalse()
        {
            IAccounting sut = CreateSut();

            sut.ApplyDefaultForPrincipal(null);

            Assert.That(sut.DefaultForPrincipal, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyDefaultForPrincipal_WhenDefaultAccountingNumberDoesNotMatchNumber_AssertDefaultForPrincipalEqualToFalse()
        {
            int number = _fixture.Create<int>();
            IAccounting sut = CreateSut(number);

            sut.ApplyDefaultForPrincipal(number + _fixture.Create<int>());

            Assert.That(sut.DefaultForPrincipal, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyDefaultForPrincipal_WhenDefaultAccountingNumberMatchNumber_AssertDefaultForPrincipalEqualToTrue()
        {
            int number = _fixture.Create<int>();
            IAccounting sut = CreateSut(number);

            sut.ApplyDefaultForPrincipal(number);

            Assert.That(sut.DefaultForPrincipal, Is.True);
        }

        private IAccounting CreateSut(int? number = null)
        {
            return new Domain.Accounting.Accounting(number ?? _fixture.Create<int>(), _fixture.Create<string>(), _fixture.BuildLetterHeadMock().Object);
        }
    }
}
