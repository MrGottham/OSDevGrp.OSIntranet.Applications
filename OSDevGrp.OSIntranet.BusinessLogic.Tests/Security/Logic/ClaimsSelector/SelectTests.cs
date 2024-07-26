using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.ClaimsSelector
{
    [TestFixture]
    public class SelectTests
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
        public void Select_WhenSupportedScopesIsNull_ThrowsArgumentNullException()
        {
            IClaimsSelector sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Select(null, CreateScopes(), CreateClaims()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("supportedScopes"));
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenScopesIsNull_ThrowsArgumentNullException()
        {
            IClaimsSelector sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Select(CreateSupportedScopes(), null, CreateClaims()));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("scopes"));
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenClaimsIsNull_ThrowsArgumentNullException()
        {
            IClaimsSelector sut = CreateSut();

            ArgumentNullException result = Assert.Throws<ArgumentNullException>(() => sut.Select(CreateSupportedScopes(), CreateScopes(), null));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.ParamName, Is.EqualTo("claims"));
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenScopesIsEmpty_AssertFilterWasNotCalledOnAnyScopesInSupportedScopes()
        {
            IClaimsSelector sut = CreateSut();

            Mock<IScope>[] supportedScopeMocks = _fixture.CreateMany<string>(_random.Next(5, 10))
                .Select(scope => CreateScopeMock(scope))
                .ToArray();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(supportedScopeMocks.Select(supportedScopeMock => supportedScopeMock.Object).ToArray());
            IEnumerable<Claim> claims = CreateClaims();

            sut.Select(supportedScopes, Array.Empty<string>(), claims);

            foreach (Mock<IScope> supportedScopeMock in supportedScopeMocks)
            {
                supportedScopeMock.Verify(m => m.Filter(It.IsAny<IEnumerable<Claim>>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenScopesIsEmpty_ReturnsNotNull()
        {
            IClaimsSelector sut = CreateSut();

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes();
            IEnumerable<Claim> claims = CreateClaims();

            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, Array.Empty<string>(), claims);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenScopesIsEmpty_ReturnsEmptyCollectionOfClaims()
        {
            IClaimsSelector sut = CreateSut();

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes();
            IEnumerable<Claim> claims = CreateClaims();

            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, Array.Empty<string>(), claims);

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenClaimsIsEmpty_AssertFilterWasNotCalledOnAnyScopesInSupportedScopes()
        {
            IClaimsSelector sut = CreateSut();

            Mock<IScope>[] supportedScopeMocks = _fixture.CreateMany<string>(_random.Next(5, 10))
                .Select(scope => CreateScopeMock(scope))
                .ToArray();
            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(supportedScopeMocks.Select(supportedScopeMock => supportedScopeMock.Object).ToArray());
            IEnumerable<string> scopes = CreateScopes(supportedScopes.Select(supportedScope => supportedScope.Key).ToArray());

            sut.Select(supportedScopes, scopes, Array.Empty<Claim>());

            foreach (Mock<IScope> supportedScopeMock in supportedScopeMocks)
            {
                supportedScopeMock.Verify(m => m.Filter(It.IsAny<IEnumerable<Claim>>()), Times.Never);
            }
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenClaimsIsEmpty_ReturnsNotNull()
        {
            IClaimsSelector sut = CreateSut();

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes();
            IEnumerable<string> scopes = CreateScopes(supportedScopes.Select(supportedScope => supportedScope.Key).ToArray());

            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, scopes, Array.Empty<Claim>());

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenClaimsIsEmpty_ReturnsEmptyCollectionOfClaims()
        {
            IClaimsSelector sut = CreateSut();

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes();
            IEnumerable<string> scopes = CreateScopes(supportedScopes.Select(supportedScope => supportedScope.Key).ToArray());

            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, scopes, Array.Empty<Claim>());

            Assert.That(result, Is.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenCalledWithScopesAndClaims_AssertFilterWasCalledOnEachMatchingScopesInSupportedScopes()
        {
            IClaimsSelector sut = CreateSut();

            Mock<IScope> openIdScopeMock = CreateScopeMock(ScopeHelper.OpenIdScope);
            Mock<IScope> profileScopeMock = CreateScopeMock(ScopeHelper.ProfileScope);
            Mock<IScope> emailScopeMock = CreateScopeMock(ScopeHelper.EmailScope);
            Mock<IScope> webApiScopeMock = CreateScopeMock(ScopeHelper.WebApiScope);

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(openIdScopeMock.Object, profileScopeMock.Object, emailScopeMock.Object, webApiScopeMock.Object);
            IEnumerable<string> scopes = CreateScopes(ScopeHelper.OpenIdScope, ScopeHelper.ProfileScope);
            Claim[] claims = CreateClaims().ToArray();

            sut.Select(supportedScopes, scopes, claims);

            openIdScopeMock.Verify(m => m.Filter(It.Is<IEnumerable<Claim>>(value => value != null && claims.All(value.Contains))), Times.Once);
            profileScopeMock.Verify(m => m.Filter(It.Is<IEnumerable<Claim>>(value => value != null && claims.All(value.Contains))), Times.Once);
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenCalledWithScopesAndClaims_AssertFilterWasNotCalledOnNoneMatchingScopesInSupportedScopes()
        {
            IClaimsSelector sut = CreateSut();

            Mock<IScope> openIdScopeMock = CreateScopeMock(ScopeHelper.OpenIdScope);
            Mock<IScope> profileScopeMock = CreateScopeMock(ScopeHelper.ProfileScope);
            Mock<IScope> emailScopeMock = CreateScopeMock(ScopeHelper.EmailScope);
            Mock<IScope> webApiScopeMock = CreateScopeMock(ScopeHelper.WebApiScope);

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(openIdScopeMock.Object, profileScopeMock.Object, emailScopeMock.Object, webApiScopeMock.Object);
            IEnumerable<string> scopes = CreateScopes(ScopeHelper.OpenIdScope, ScopeHelper.ProfileScope);
            IEnumerable<Claim> claims = CreateClaims();

            sut.Select(supportedScopes, scopes, claims);

            emailScopeMock.Verify(m => m.Filter(It.IsAny<IEnumerable<Claim>>()), Times.Never);
            webApiScopeMock.Verify(m => m.Filter(It.IsAny<IEnumerable<Claim>>()), Times.Never);
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenCalledWithScopesAndClaims_ReturnsNotNull()
        {
            IClaimsSelector sut = CreateSut();

            IScope openIdScope = CreateScope(ScopeHelper.OpenIdScope, filteredClaims: CreateClaims());
            IScope profileScope = CreateScope(ScopeHelper.ProfileScope, filteredClaims: CreateClaims());
            IScope emailScope = CreateScope(ScopeHelper.EmailScope, filteredClaims: CreateClaims());
            IScope webApiScope = CreateScope(ScopeHelper.WebApiScope, filteredClaims: CreateClaims());

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(openIdScope, profileScope, emailScope, webApiScope);
            IEnumerable<string> scopes = CreateScopes(ScopeHelper.OpenIdScope, ScopeHelper.ProfileScope);
            IEnumerable<Claim> claims = CreateClaims();
            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, scopes, claims);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenCalledWithScopesAndClaims_ReturnsNoneEmptyCollectionOfClaims()
        {
            IClaimsSelector sut = CreateSut();

            IScope openIdScope = CreateScope(ScopeHelper.OpenIdScope, filteredClaims: CreateClaims());
            IScope profileScope = CreateScope(ScopeHelper.ProfileScope, filteredClaims: CreateClaims());
            IScope emailScope = CreateScope(ScopeHelper.EmailScope, filteredClaims: CreateClaims());
            IScope webApiScope = CreateScope(ScopeHelper.WebApiScope, filteredClaims: CreateClaims());

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(openIdScope, profileScope, emailScope, webApiScope);
            IEnumerable<string> scopes = CreateScopes(ScopeHelper.OpenIdScope, ScopeHelper.ProfileScope);
            IEnumerable<Claim> claims = CreateClaims();
            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, scopes, claims);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        public void Select_WhenCalledWithScopesAndClaims_ReturnsNoneEmptyCollectionOfFilteredClaimsFromOnlyMatchingScopesInSupportedScopes()
        {
            IClaimsSelector sut = CreateSut();

            Claim[] openIdClaims = CreateClaims().ToArray();
            Claim[] profileClaims = CreateClaims().ToArray();
            Claim[] emailClaims = CreateClaims().ToArray();
            Claim[] webApiClaims = CreateClaims().ToArray();

            IScope openIdScope = CreateScope(ScopeHelper.OpenIdScope, filteredClaims: openIdClaims);
            IScope profileScope = CreateScope(ScopeHelper.ProfileScope, filteredClaims: profileClaims);
            IScope emailScope = CreateScope(ScopeHelper.EmailScope, filteredClaims: emailClaims);
            IScope webApiScope = CreateScope(ScopeHelper.WebApiScope, filteredClaims: webApiClaims);

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes(openIdScope, profileScope, emailScope, webApiScope);
            IEnumerable<string> scopes = CreateScopes(ScopeHelper.OpenIdScope, ScopeHelper.ProfileScope);
            IEnumerable<Claim> claims = CreateClaims();
            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, scopes, claims);

            Assert.That(openIdClaims.All(claim => result.Contains(claim)), Is.True);
            Assert.That(profileClaims.All(claim => result.Contains(claim)), Is.True);
            Assert.That(emailClaims.Any(claim => result.Contains(claim)), Is.False);
            Assert.That(webApiClaims.Any(claim => result.Contains(claim)), Is.False);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ClaimHelper.MicrosoftTokenClaimType)]
        [TestCase(ClaimHelper.GoogleTokenClaimType)]
        public void Select_WhenCalledWithScopesAndClaimsContainingProtectedClaims_ReturnsNotNull(string protectedClaimType)
        {
            IClaimsSelector sut = CreateSut();

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes();
            IEnumerable<string> scopes = CreateScopes();
            IEnumerable<Claim> claims = CreateClaims(protectedClaimType, _fixture.Create<string>(), _fixture.Create<string>());
            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, scopes, claims);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ClaimHelper.MicrosoftTokenClaimType)]
        [TestCase(ClaimHelper.GoogleTokenClaimType)]
        public void Select_WhenCalledWithScopesAndClaimsContainingProtectedClaims_ReturnsNoneEmptyCollectionOfClaims(string protectedClaimType)
        {
            IClaimsSelector sut = CreateSut();

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes();
            IEnumerable<string> scopes = CreateScopes();
            IEnumerable<Claim> claims = CreateClaims(protectedClaimType, _fixture.Create<string>(), _fixture.Create<string>());
            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, scopes, claims);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        [Category("UnitTest")]
        [TestCase(ClaimHelper.MicrosoftTokenClaimType)]
        [TestCase(ClaimHelper.GoogleTokenClaimType)]
        public void Select_WhenCalledWithScopesAndClaimsContainingProtectedClaims_ReturnsNoneEmptyCollectionOfClaimsContainingProtectedClaims(string protectedClaimType)
        {
            IClaimsSelector sut = CreateSut();

            IReadOnlyDictionary<string, IScope> supportedScopes = CreateSupportedScopes();
            IEnumerable<string> scopes = CreateScopes();
            IEnumerable<Claim> claims = CreateClaims(protectedClaimType, _fixture.Create<string>(), _fixture.Create<string>());
            IReadOnlyCollection<Claim> result = sut.Select(supportedScopes, scopes, claims);

            Assert.That(result.Any(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && claim.Type == protectedClaimType), Is.True);
        }

        private IClaimsSelector CreateSut()
        {
            return new BusinessLogic.Security.Logic.ClaimsSelector();
        }

        private IReadOnlyDictionary<string, IScope> CreateSupportedScopes(params IScope[] supportedScopes)
        {
            if (supportedScopes == null || supportedScopes.Length == 0)
            {
                supportedScopes = _fixture.CreateMany<string>(_random.Next(5, 10)).Select(scope => CreateScope(name: scope)).ToArray();
            }

            return supportedScopes.ToDictionary(supportedScope => supportedScope.Name, supportedScope => supportedScope).AsReadOnly();
        }

        private IScope CreateScope(string name = null, IEnumerable<Claim> filteredClaims = null)
        {
            return CreateScopeMock(name, filteredClaims).Object;
        }

        private Mock<IScope> CreateScopeMock(string name = null, IEnumerable<Claim> filteredClaims = null)
        {
            return _fixture.BuildScopeMock(name: name, filteredClaims: filteredClaims);
        }

        private IEnumerable<string> CreateScopes(params string[] scopes)
        {
            if (scopes == null || scopes.Length == 0)
            {
                scopes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            }

            return scopes;
        }

        private IEnumerable<Claim> CreateClaims(params string[] claimTypes)
        {
            if (claimTypes == null || claimTypes.Length == 0)
            {
                claimTypes = _fixture.CreateMany<string>(_random.Next(5, 10)).ToArray();
            }

            return claimTypes.Select(claimType => new Claim(claimType, string.Empty)).ToArray();
        }
    }
}