using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Security.Logic.IdTokenContentBuilder
{
    public abstract class IdTokenContentBuilderTestBase
    {
        #region Properties

        protected abstract Mock<IClaimsSelector> ClaimsSelectorMock { get; }

        protected abstract Fixture Fixture { get; }

        protected abstract Random Random { get; }

        #endregion

        #region Methods

        protected IIdTokenContentBuilder CreateSut(string nameIdentifier = null, IUserInfo userInfo = null, DateTimeOffset? authenticationTime = null, IReadOnlyDictionary<string, IScope> supportedScopes = null, IReadOnlyCollection<string> scopes = null)
        {
            ClaimsSelectorMock.Setup(m => m.Select(It.IsAny<IReadOnlyDictionary<string, IScope>>(), It.IsAny<IEnumerable<string>>(), It.IsAny<IEnumerable<Claim>>()))
                .Returns<IReadOnlyDictionary<string, IScope>, IEnumerable<string>, IEnumerable<Claim>>((internalSupportedScopes, internalScopes, internalClaims) =>internalScopes.SelectMany(withScope => internalSupportedScopes[withScope].Filter(internalClaims)).ToArray());

            return new BusinessLogic.Security.Logic.IdTokenContentBuilder(nameIdentifier ?? Fixture.Create<string>(), userInfo ?? Fixture.BuildUserInfoMock().Object, authenticationTime ?? CreateAuthenticationTime(), supportedScopes ?? CreateSupportedScopes(CreateOpenIdScope(), CreateProfileScope(), CreateEmailScope(), CreateWebApiScope()), scopes ?? CreateScopes(), ClaimsSelectorMock.Object);
        }

        private DateTimeOffset CreateAuthenticationTime()
        {
            return DateTimeOffset.UtcNow.AddSeconds(Random.Next(300) * -1);
        }

        protected IReadOnlyDictionary<string, IScope> CreateSupportedScopes(params IScope[] supportedScopes)
        {
            NullGuard.NotNull(supportedScopes, nameof(supportedScopes));

            return supportedScopes.ToDictionary(supportedScope => supportedScope.Name, supportedScope => supportedScope).AsReadOnly();
        }

        protected IScope CreateOpenIdScope(IEnumerable<Claim> filteredClaims = null)
        {
            return CreateOpenIdScopeMock(filteredClaims).Object;
        }

        protected IScope CreateProfileScope(IEnumerable<Claim> filteredClaims = null)
        {
            return CreateProfileScopeMock(filteredClaims).Object;
        }

        protected IScope CreateEmailScope(IEnumerable<Claim> filteredClaims = null)
        {
            return CreateEmailScopeMock(filteredClaims).Object;
        }

        protected IScope CreateWebApiScope(IEnumerable<Claim> filteredClaims = null)
        {
            return CreateWebApiScopeMock(filteredClaims).Object;
        }

        protected Mock<IScope> CreateWebApiScopeMock(IEnumerable<Claim> filteredClaims = null)
        {
            return Fixture.BuildScopeMock(name: ScopeHelper.WebApiScope, filteredClaims: filteredClaims ?? Fixture.CreateClaims(Random));
        }

        private Mock<IScope> CreateOpenIdScopeMock(IEnumerable<Claim> filteredClaims = null)
        {
            return Fixture.BuildScopeMock(name: ScopeHelper.OpenIdScope, filteredClaims: filteredClaims ?? Fixture.CreateClaims(Random));
        }

        private Mock<IScope> CreateProfileScopeMock(IEnumerable<Claim> filteredClaims = null)
        {
            return Fixture.BuildScopeMock(name: ScopeHelper.ProfileScope, filteredClaims: filteredClaims ?? Fixture.CreateClaims(Random));
        }

        private Mock<IScope> CreateEmailScopeMock(IEnumerable<Claim> filteredClaims = null)
        {
            return Fixture.BuildScopeMock(name: ScopeHelper.EmailScope, filteredClaims: filteredClaims ?? Fixture.CreateClaims(Random));
        }

        protected static IReadOnlyCollection<string> CreateScopes(bool withOpenIdScope = true, bool withProfileScope = true, bool withEmailScope = true, bool withWebApiScope = true)
        {
            IList<string> scopes = new List<string>();
            if (withOpenIdScope)
            {
                scopes.Add(ScopeHelper.OpenIdScope);
            }
            if (withProfileScope)
            {
                scopes.Add(ScopeHelper.ProfileScope);
            }
            if (withEmailScope)
            {
                scopes.Add(ScopeHelper.EmailScope);
            }
            if (withWebApiScope)
            {
                scopes.Add(ScopeHelper.WebApiScope);
            }
            return scopes.AsReadOnly();
        }

        #endregion
    }
}