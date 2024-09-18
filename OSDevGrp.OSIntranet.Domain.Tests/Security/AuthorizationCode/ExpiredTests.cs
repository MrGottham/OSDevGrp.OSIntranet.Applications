using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationCode
{
    [TestFixture]
    public class ExpiredTests
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
        public void Expired_WhenAuthorizationCodeWasCreatedWithNonExpiredUtcTime_ReturnsFalse()
        {
            DateTimeOffset expires = DateTimeOffset.UtcNow.AddSeconds(_random.Next(5, 10));
            IAuthorizationCode sut = CreateSut(expires);

            bool result = sut.Expired;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Expired_WhenAuthorizationCodeWasCreatedWithNonExpiredLocalTime_ReturnsFalse()
        {
            DateTimeOffset expires = DateTimeOffset.Now.AddSeconds(_random.Next(5, 10));
            IAuthorizationCode sut = CreateSut(expires);

            bool result = sut.Expired;

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void Expired_WhenAuthorizationCodeWasCreatedWithExpiredUtcTime_ReturnsTrue()
        {
            DateTimeOffset expires = DateTimeOffset.UtcNow.AddSeconds(_random.Next(5, 10) * -1);
            IAuthorizationCode sut = CreateSut(expires);

            bool result = sut.Expired;

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void Expired_WhenAuthorizationCodeWasCreatedWithExpiredLocalTime_ReturnsTrue()
        {
            DateTimeOffset expires = DateTimeOffset.Now.AddSeconds(_random.Next(5, 10) * -1);
            IAuthorizationCode sut = CreateSut(expires);

            bool result = sut.Expired;

            Assert.That(result, Is.True);
        }

        private IAuthorizationCode CreateSut(DateTimeOffset? expires = null)
        {
            return new Domain.Security.AuthorizationCode(_fixture.Create<string>(), expires ?? DateTimeOffset.UtcNow.AddSeconds(_random.Next(5, 7)));
        }
    }
}