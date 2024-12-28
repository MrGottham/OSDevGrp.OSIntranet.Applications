using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateBuilder
{
    [TestFixture]
    public class WithNonceTests : AuthorizationStateBuilderTestBase
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
        public void WithNonce_WhenNonceIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithNonce(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nonce"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenNonceIsEmpty_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithNonce(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nonce"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenNonceIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithNonce(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("nonce"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithNonce(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithNonce_WhenCalled_ReturnsSameAuthorizationStateBuilder()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithNonce(_fixture.Create<string>());

            Assert.That(result, Is.SameAs(sut));
        }

        private IAuthorizationStateBuilder CreateSut()
        {
            return CreateSut(_fixture, _random);
        }
    }
}