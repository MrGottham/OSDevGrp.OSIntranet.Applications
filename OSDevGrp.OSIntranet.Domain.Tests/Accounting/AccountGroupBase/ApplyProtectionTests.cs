using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Tests.Accounting.AccountGroupBase
{
    [TestFixture]
    public class ApplyProtectionTests
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
        public void ApplyProtection_WhenCalled_AssertIsProtectedIsTrue()
        {
            IProtectable sut = CreateSut();

            Assert.That(sut.IsProtected, Is.False);

            sut.ApplyProtection();

            Assert.That(sut.IsProtected, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void ApplyProtection_WhenCalled_AssertDeletableIsFalse()
        {
            IProtectable sut = CreateSut();

            Assert.That(sut.Deletable, Is.True);

            sut.ApplyProtection();

            Assert.That(sut.Deletable, Is.False);
        }

        private IProtectable CreateSut()
        {
            return new Sut(_fixture.Create<int>(), _fixture.Create<string>(), true);
        }

        private class Sut : Domain.Accounting.AccountGroupBase
        {
            #region Constructor

            public Sut(int number, string name, bool deletable)
                : base(number, name, deletable)
            {
            }

            #endregion
        }
    }
}