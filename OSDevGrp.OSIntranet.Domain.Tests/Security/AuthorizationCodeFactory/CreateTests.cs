using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationCodeFactory
{
    [TestFixture]
    public class CreateTests
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
        public void Create_WhenValueIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationCodeFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(null, DateTimeOffset.UtcNow.AddSeconds(_random.Next(60, 600))));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenValueIsEmpty_ThrowsArgumentNullException()
        {
            IAuthorizationCodeFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(string.Empty, DateTimeOffset.UtcNow.AddSeconds(_random.Next(60, 600))));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenValueIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAuthorizationCodeFactory sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Create(" ", DateTimeOffset.UtcNow.AddSeconds(_random.Next(60, 600))));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("value"));
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationCodeFactory sut = CreateSut();

            IAuthorizationCodeBuilder result = sut.Create(_fixture.Create<string>(), DateTimeOffset.UtcNow.AddSeconds(_random.Next(60, 600)));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Create_WhenCalled_ReturnsAuthorizationCodeBuilder()
        {
            IAuthorizationCodeFactory sut = CreateSut();

            IAuthorizationCodeBuilder result = sut.Create(_fixture.Create<string>(), DateTimeOffset.UtcNow.AddSeconds(_random.Next(60, 600)));

            Assert.That(result, Is.TypeOf<Domain.Security.AuthorizationCodeBuilder>());
        }

        private IAuthorizationCodeFactory CreateSut()
        {
            return new Domain.Security.AuthorizationCodeFactory();
        }
    }
}