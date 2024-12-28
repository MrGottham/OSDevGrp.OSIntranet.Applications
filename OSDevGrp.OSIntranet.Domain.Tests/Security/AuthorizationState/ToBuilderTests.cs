using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationState
{
    [TestFixture]
    public class ToBuilderTests : AuthorizationStateTestBase
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
        public void ToBuilder_WhenCalled_ReturnsNotNull()
        {
            IAuthorizationState sut = CreateSut();

            IAuthorizationStateBuilder result = sut.ToBuilder();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void ToBuilder_WhenCalled_ReturnsAuthorizationStateBuilder()
        {
            IAuthorizationState sut = CreateSut();

            IAuthorizationStateBuilder result = sut.ToBuilder();

            Assert.That(result, Is.TypeOf<Domain.Security.AuthorizationStateBuilder>());
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true, true, true, true)]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, true)]
        [TestCase(true, true, false, false)]
        [TestCase(true, false, true, true)]
        [TestCase(true, false, true, false)]
        [TestCase(true, false, false, true)]
        [TestCase(true, false, false, false)]
        [TestCase(false, true, true, true)]
        [TestCase(false, true, true, false)]
        [TestCase(false, true, false, true)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, false, false, true)]
        [TestCase(false, false, false, false)]
        public void ToBuilder_WhenCalled_ReturnsAuthorizationStateBuilderWhichCanBuildClonedAuthorizationState(bool hasClientSecret, bool hasExternalState, bool hasNonce, bool hasAuthorizationCode)
        {
            IAuthorizationState sut = CreateSut(hasClientSecret: hasClientSecret, hasExternalState: hasExternalState, hasNonce: hasNonce, hasAuthorizationCode: hasAuthorizationCode);

            IAuthorizationStateBuilder result = sut.ToBuilder();

            Assert.That(result.Build(), Is.EqualTo(sut));
        }

        private IAuthorizationState CreateSut(bool hasClientSecret = false, bool hasExternalState = true, bool hasNonce = true, bool hasAuthorizationCode = false)
        {
            return CreateSut(_fixture, _random, hasClientSecret: hasClientSecret, hasExternalState: hasExternalState, hasNonce: hasNonce, hasAuthorizationCode: hasAuthorizationCode);
        }
    }
}