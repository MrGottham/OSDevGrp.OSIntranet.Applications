using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.Token
{
    [TestFixture]
    public class HasExpiredTests
    {
        #region Private variables

        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public void HasExpired_WhenExpiresIsFutureUniversalTime_ReturnsFalse()
        {
            DateTime expires = DateTime.UtcNow.AddMinutes(_random.Next(5, 60));
            IToken sut = CreateSut(expires);

            bool result = sut.HasExpired;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void HasExpired_WhenExpiresIsPastUniversalTime_ReturnsTrue()
        {
            DateTime expires = DateTime.UtcNow.AddMinutes(_random.Next(5, 60) * -1);
            IToken sut = CreateSut(expires);

            bool result = sut.HasExpired;

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void HasExpired_WhenExpiresIsFutureLocalTime_ReturnsFalse()
        {
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60));
            IToken sut = CreateSut(expires);

            bool result = sut.HasExpired;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void HasExpired_WhenExpiresIsPastLocalTime_ReturnsTrue()
        {
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60) * -1);
            IToken sut = CreateSut(expires);

            bool result = sut.HasExpired;

            Assert.That(result, Is.True);
        }

        private IToken CreateSut(DateTime? expires = null)
        {
            return new Domain.Security.Token(_fixture.Create<string>(), _fixture.Create<string>(), expires ?? DateTime.UtcNow.AddMinutes(_random.Next(5, 60)));
        }
    }
}