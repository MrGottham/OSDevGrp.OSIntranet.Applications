using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationCodeBuilder
{
    [TestFixture]
    public class BuildTests
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
        public void Build_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationCodeBuilder sut = CreateSut();

            IAuthorizationCode result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationCode()
        {
            IAuthorizationCodeBuilder sut = CreateSut();

            IAuthorizationCode result = sut.Build();

            Assert.That(result, Is.TypeOf<Domain.Security.AuthorizationCode>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationCodeWhereValueIsNotNull()
        {
            IAuthorizationCodeBuilder sut = CreateSut();

            IAuthorizationCode result = sut.Build();

            Assert.That(result.Value, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationCodeWhereValueIsEmpty()
        {
            IAuthorizationCodeBuilder sut = CreateSut();

            IAuthorizationCode result = sut.Build();

            Assert.That(result.Value, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationCodeWhereValueIsEqualToValueFromConstructor()
        {
            string value = _fixture.Create<string>();
            IAuthorizationCodeBuilder sut = CreateSut(value: value);

            IAuthorizationCode result = sut.Build();

            Assert.That(result.Value, Is.EqualTo(value));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void Build_WhenCalled_ReturnsAuthorizationCodeWhereExpiresIsEqualToUtcDateTimeForExpiresFromConstructor(bool isUtc)
        {
            DateTimeOffset expires = (isUtc ? DateTimeOffset.UtcNow : DateTimeOffset.Now).AddSeconds(_random.Next(60, 600));
            IAuthorizationCodeBuilder sut = CreateSut(expires: expires);

            IAuthorizationCode result = sut.Build();

            Assert.That(result.Expires, Is.EqualTo(expires.UtcDateTime));
        }

        private IAuthorizationCodeBuilder CreateSut(string value = null, DateTimeOffset? expires = null)
        {
            return new Domain.Security.AuthorizationCodeBuilder(value ?? _fixture.Create<string>(), expires ?? DateTimeOffset.UtcNow.AddSeconds(_random.Next(60, 600)));
        }
    }
}