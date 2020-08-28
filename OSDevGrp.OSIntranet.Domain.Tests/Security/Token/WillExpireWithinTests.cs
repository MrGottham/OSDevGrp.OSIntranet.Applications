using System;
using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.Token
{
    [TestFixture]
    public class WillExpireWithinTests
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
        public void WillExpireWithin_WhenExpiresIsFutureUniversalTimeAndGivenTimeSpanGivesTimeBeforeThis_ReturnsFalse()
        {
            int secondsToExpire = _random.Next(60, 300);
            DateTime expires = DateTime.UtcNow.AddSeconds(secondsToExpire);
            IToken sut = CreateSut(expires);

            bool result = sut.WillExpireWithin(new TimeSpan(0, 0, 0, secondsToExpire - 5));

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void WillExpireWithin_WhenExpiresIsFutureUniversalTimeAndGivenTimeSpanGivesTimeAfterThis_ReturnsTrue()
        {
            int secondsToExpire = _random.Next(60, 300);
            DateTime expires = DateTime.UtcNow.AddSeconds(secondsToExpire);
            IToken sut = CreateSut(expires);

            bool result = sut.WillExpireWithin(new TimeSpan(0, 0, 0, secondsToExpire + 5));

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void WillExpireWithin_WhenExpiresIsPastUniversalTime_ReturnsTrue()
        {
            DateTime expires = DateTime.UtcNow.AddMinutes(_random.Next(5, 60) * -1);
            IToken sut = CreateSut(expires);

            bool result = sut.WillExpireWithin(new TimeSpan(0, 0, 0, _random.Next(300)));

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void WillExpireWithin_WhenExpiresIsFutureLocalTimeAndGivenTimeSpanGivesTimeBeforeThis_ReturnsFalse()
        {
            int secondsToExpire = _random.Next(60, 300);
            DateTime expires = DateTime.Now.AddSeconds(secondsToExpire);
            IToken sut = CreateSut(expires);

            bool result = sut.WillExpireWithin(new TimeSpan(0, 0, 0, secondsToExpire - 5));

            Assert.That(result, Is.False);
        }

        [Test]
        [Category("UnitTest")]
        public void WillExpireWithin__WhenExpiresIsFutureLocalTimeAndGivenTimeSpanGivesTimeAfterThis_ReturnsTrue()
        {
            int secondsToExpire = _random.Next(60, 300);
            DateTime expires = DateTime.Now.AddSeconds(secondsToExpire);
            IToken sut = CreateSut(expires);

            bool result = sut.WillExpireWithin(new TimeSpan(0, 0, 0, secondsToExpire + 5));

            Assert.That(result, Is.True);
        }

        [Test]
        [Category("UnitTest")]
        public void WillExpireWithin_WhenExpiresIsPastLocalTime_ReturnsTrue()
        {
            DateTime expires = DateTime.Now.AddMinutes(_random.Next(5, 60) * -1);
            IToken sut = CreateSut(expires);

            bool result = sut.WillExpireWithin(new TimeSpan(0, 0, 0, _random.Next(300)));

            Assert.That(result, Is.True);
        }

        private IToken CreateSut(DateTime? expires = null)
        {
            return new Domain.Security.Token(_fixture.Create<string>(), _fixture.Create<string>(), expires ?? DateTime.UtcNow.AddMinutes(_random.Next(5, 60)));
        }
    }
}