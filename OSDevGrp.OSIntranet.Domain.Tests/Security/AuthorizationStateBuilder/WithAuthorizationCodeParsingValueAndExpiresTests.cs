using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateBuilder
{
    [TestFixture]
    public class WithAuthorizationCodeParsingValueAndExpiresTests : AuthorizationStateBuilderTestBase
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
        public void WithAuthorizationCode_WhenValueIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthorizationCode(null, DateTimeOffset.Now.AddSeconds(_random.Next(5, 10))));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizationCode_WhenValueIsEmpty_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthorizationCode(string.Empty, DateTimeOffset.Now.AddSeconds(_random.Next(5, 10))));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizationCode_WhenValueIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithAuthorizationCode(" ", DateTimeOffset.Now.AddSeconds(_random.Next(5, 10))));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizationCode_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithAuthorizationCode(_fixture.Create<string>(), DateTimeOffset.Now.AddSeconds(_random.Next(5, 10)));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithAuthorizationCode_WhenCalled_ReturnsSameAuthorizationStateBuilder()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithAuthorizationCode(_fixture.Create<string>(), DateTimeOffset.Now.AddSeconds(_random.Next(5, 10)));

            Assert.That(result, Is.SameAs(sut));
        }

        private IAuthorizationStateBuilder CreateSut()
        {
            return CreateSut(_fixture, _random);
        }
    }
}