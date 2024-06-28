using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.AuthorizationStateBuilder
{
    [TestFixture]
    public class BuildTests : AuthorizationStateBuilderTestBase
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
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationState()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result, Is.TypeOf<Domain.Security.AuthorizationState>());
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result.ResponseType, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsNotEmpty()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result.ResponseType, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereResponseTypeIsEqualToResponseTypeFromConstructor()
        {
            string responseType = _fixture.Create<string>();
            IAuthorizationStateBuilder sut = CreateSut(responseType: responseType);

            IAuthorizationState result = sut.Build();

            Assert.That(result.ResponseType, Is.EqualTo(responseType));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result.ClientId, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsNotEmpty()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result.ClientId, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereClientIdIsEqualToClientIdFromConstructor()
        {
            string clientId = _fixture.Create<string>();
            IAuthorizationStateBuilder sut = CreateSut(clientId: clientId);

            IAuthorizationState result = sut.Build();

            Assert.That(result.ClientId, Is.EqualTo(clientId));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereRedirectUriIsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result.RedirectUri, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereRedirectUriIsEqualToRedirectUriFromConstructor()
        {
            Uri redirectUri = CreateRedirectUri(_fixture);
            IAuthorizationStateBuilder sut = CreateSut(redirectUri: redirectUri);

            IAuthorizationState result = sut.Build();

            Assert.That(result.RedirectUri, Is.EqualTo(redirectUri));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereScopesIsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result.Scopes, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereScopesIsNotEmpty()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result.Scopes, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenCalled_ReturnsAuthorizationStateWhereScopesIsEqualToScopesFromConstructor()
        {
            string[] scopes = CreateScopes(_fixture, _random);
            IAuthorizationStateBuilder sut = CreateSut(scopes: scopes);

            IAuthorizationState result = sut.Build();

            Assert.That(result.Scopes, Is.EqualTo(scopes));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithExternalStateWasCalled_ReturnsAuthorizationStateWhereExternalStateIsNotNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.WithExternalState(_fixture.Create<string>()).Build();

            Assert.That(result.ExternalState, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithExternalStateWasCalled_ReturnsAuthorizationStateWhereExternalStateIsNotEmpty()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.WithExternalState(_fixture.Create<string>()).Build();

            Assert.That(result.ExternalState, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithExternalStateWasCalled_ReturnsAuthorizationStateWhereExternalStateIsEqualToExternalStateFromWithExternalState()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            string externalState = _fixture.Create<string>();
            IAuthorizationState result = sut.WithExternalState(externalState).Build();

            Assert.That(result.ExternalState, Is.EqualTo(externalState));
        }

        [Test]
        [Category("UnitTest")]
        public void Build_WhenWithExternalStateWasNotCalled_ReturnsAuthorizationStateWhereExternalStateIsNull()
        {
            IAuthorizationStateBuilder sut = CreateSut();

            IAuthorizationState result = sut.Build();

            Assert.That(result.ExternalState, Is.Null);
        }

        private IAuthorizationStateBuilder CreateSut(string responseType = null, string clientId = null, Uri redirectUri = null, string[] scopes = null)
        {
            return CreateSut(_fixture, _random, responseType, clientId, redirectUri, scopes);
        }
    }
}