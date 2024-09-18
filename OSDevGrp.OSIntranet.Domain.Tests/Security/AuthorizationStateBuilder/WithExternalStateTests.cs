using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateBuilder
{
    [TestFixture]
    public class WithExternalStateTests : AuthorizationStateBuilderTestBase
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
        public void WithExternalState_WhenExternalStateIsNull_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithExternalState(null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("externalState"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalState_WhenExternalStateIsEmpty_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithExternalState(string.Empty));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("externalState"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalState_WhenExternalStateIsWhiteSpace_ThrowsArgumentNullException()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithExternalState(" "));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("externalState"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalState_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithExternalState(_fixture.Create<string>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithExternalState_WhenCalled_ReturnsSameAuthorizationStateBuilder()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationStateBuilder result = sut.WithExternalState(_fixture.Create<string>());

            Assert.That(result, Is.SameAs(sut));
        }

        private IAuthorizationStateBuilder CreateSut()
        {
            return CreateSut(_fixture, _random);
        }
    }
}