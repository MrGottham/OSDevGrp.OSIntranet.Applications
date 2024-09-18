using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateBuilder
{
    [TestFixture]
    public class WithClientSecretTests : AuthorizationStateBuilderTestBase
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
        public void WithClientSecret_WhenClientSecretIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientSecret(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientSecret_WhenClientSecretIsEmpty_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientSecret(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientSecret_WhenClientSecretIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithClientSecret(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("clientSecret"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientSecret_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithClientSecret(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithClientSecret_WhenCalled_ReturnsSameAuthorizationStateBuilder()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithClientSecret(_fixture.Create<string>());

            Assert.That(result, Is.SameAs(sut));
        }

        private IAuthorizationStateBuilder CreateSut()
        {
            return CreateSut(_fixture, _random);
        }
    }
}