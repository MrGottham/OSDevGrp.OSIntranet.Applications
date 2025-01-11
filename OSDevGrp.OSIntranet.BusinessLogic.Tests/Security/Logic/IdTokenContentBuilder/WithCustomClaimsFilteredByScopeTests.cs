using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentBuilder
{
    [TestFixture]
    public class WithCustomClaimsFilteredByScopesFilteredByScopeTests : IdTokenContentBuilderTestBase
    {
        #region Private variables

        private Mock<IClaimsSelector> _claimSelectorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _claimSelectorMock = new Mock<IClaimsSelector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        protected override Mock<IClaimsSelector> ClaimsSelectorMock => _claimSelectorMock;

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaimsFilteredByScope(null, _fixture.CreateClaims(_random)));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("scope"));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenCustomClaimsIsNull_ThrowsArgumentNullException()
        {
            IIdTokenContentBuilder sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.WithCustomClaimsFilteredByScope(CreateWebApiScope(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("customClaims"));
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithCustomClaimsFilteredByScope_WhenScopeNotInSupportedScopesGivenByConstructor_AssertNameWasCalledOnScope(bool withWebApiScope)
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: withWebApiScope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            Mock<IScope> scopeMock = CreateWebApiScopeMock();
            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, _fixture.CreateClaims(_random));

            scopeMock.Verify(m => m.Name, Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithCustomClaimsFilteredByScope_WhenScopeNotInSupportedScopesGivenByConstructor_AssertFilterWasNotCalledOnScope(bool withWebApiScope)
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: withWebApiScope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            Mock<IScope> scopeMock = CreateWebApiScopeMock();
            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, _fixture.CreateClaims(_random));

            scopeMock.Verify(m => m.Filter(It.IsAny<IEnumerable<Claim>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithCustomClaimsFilteredByScope_WhenScopeNotInSupportedScopesGivenByConstructor_ReturnsNotNull(bool withWebApiScope)
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: withWebApiScope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IScope scope = CreateWebApiScope();
            IIdTokenContentBuilder result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(true)]
        [TestCase(false)]
        public void WithCustomClaimsFilteredByScope_WhenScopeNotInSupportedScopesGivenByConstructor_ReturnsSameIdTokenContentBuilder(bool withWebApiScope)
        {
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope());
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: withWebApiScope);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IScope scope = CreateWebApiScope();
            IIdTokenContentBuilder result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeNotInScopesGivenByConstructor_AssertNameWasCalledOnScope()
        {
            Mock<IScope> scopeMock = CreateWebApiScopeMock();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scopeMock.Object);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: false);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, _fixture.CreateClaims(_random));

            scopeMock.Verify(m => m.Name, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeNotInScopesGivenByConstructor_AssertFilterWasNotCalledOnScope()
        {
            Mock<IScope> scopeMock = CreateWebApiScopeMock();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scopeMock.Object);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: false);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, _fixture.CreateClaims(_random));

            scopeMock.Verify(m => m.Filter(It.IsAny<IEnumerable<Claim>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeNotInScopesGivenByConstructor_ReturnsNotNull()
        {
            IScope scope = CreateWebApiScope();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: false);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IIdTokenContentBuilder result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeNotInScopesGivenByConstructor_ReturnsSameIdTokenContentBuilder()
        {
            IScope scope = CreateWebApiScope();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: false);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IIdTokenContentBuilder result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random));

            Assert.That(result, Is.SameAs(sut));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeInBothSupportedScopesAndScopesGivenByConstructor_AssertNameWasCalledOnScope()
        {
            Mock<IScope> scopeMock = CreateWebApiScopeMock();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scopeMock.Object);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, _fixture.CreateClaims(_random));

            scopeMock.Verify(m => m.Name, Times.Exactly(2));
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeInBothSupportedScopesAndScopesGivenByConstructor_AssertFilterWasCalledOnScope()
        {
            Mock<IScope> scopeMock = CreateWebApiScopeMock();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scopeMock.Object);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IEnumerable<Claim> claims = _fixture.CreateClaims(_random);
            sut.WithCustomClaimsFilteredByScope(scopeMock.Object, claims);

            scopeMock.Verify(m => m.Filter(It.Is<IEnumerable<Claim>>(value => value != null && value == claims)), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeInBothSupportedScopesAndScopesGivenByConstructor_ReturnsNotNull()
        {
            IScope scope = CreateWebApiScope();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IIdTokenContentBuilder result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random));

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void WithCustomClaimsFilteredByScope_WhenScopeInBothSupportedScopesAndScopesGivenByConstructor_ReturnsSameIdTokenContentBuilder()
        {
            IScope scope = CreateWebApiScope();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), scope);
            IReadOnlyCollection<string> scopes = CreateScopes(withWebApiScope: true);
            IIdTokenContentBuilder sut = CreateSut(supportedScopes: supportedScopes, scopes: scopes);

            IIdTokenContentBuilder result = sut.WithCustomClaimsFilteredByScope(scope, _fixture.CreateClaims(_random));

            Assert.That(result, Is.SameAs(sut));
        }
    }
}