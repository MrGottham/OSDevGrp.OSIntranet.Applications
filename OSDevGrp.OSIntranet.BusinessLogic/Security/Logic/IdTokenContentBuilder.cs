using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class IdTokenContentBuilder : IIdTokenContentBuilder
    {
        #region Internal constants

        internal const string SubjectIdentifierClaimType = "sub";
        internal const string AuthenticationTimeClaimType = "auth_time";
        internal const string NonceClaimType = "nonce";
        internal const string AuthenticationContextClassReferenceClaimType = "acr";
        internal const string AuthenticationMethodsReferencesClaimType = "amr";
        internal const string AuthorizedPartyClaimType = "azp";

        #endregion

        #region Private variables

        private readonly string _nameIdentifier;
        private readonly IUserInfo _userInfo;
        private readonly DateTimeOffset _authenticationTime;
        private readonly IReadOnlyDictionary<string, IScope> _supportedScopes;
        private readonly IReadOnlyCollection<string> _scopes;
        private readonly IClaimsSelector _claimsSelector;
        private readonly IList<KeyValuePair<string, string>> _customClaims = new List<KeyValuePair<string, string>>();
        private string _nonce;
        private string _authenticationContextClassReference;
        private string[] _authenticationMethodsReferences;
        private string _authorizedParty;

        private static readonly IEnumerable<string> NoneSupportedClaimTypes =
        [
            ClaimTypes.NameIdentifier
        ];

        #endregion

        #region Constructor

        public IdTokenContentBuilder(string nameIdentifier, IUserInfo userInfo, DateTimeOffset authenticationTime, IReadOnlyDictionary<string, IScope> supportedScopes, IReadOnlyCollection<string> scopes, IClaimsSelector claimsSelector)
        {
            NullGuard.NotNullOrWhiteSpace(nameIdentifier, nameof(nameIdentifier))
                .NotNull(userInfo, nameof(userInfo))
                .NotNull(supportedScopes, nameof(supportedScopes))
                .NotNull(scopes, nameof(scopes))
                .NotNull(claimsSelector, nameof(claimsSelector));

            _nameIdentifier = nameIdentifier;
            _userInfo = userInfo;
            _authenticationTime = authenticationTime;
            _supportedScopes = supportedScopes;
            _scopes = scopes;
            _claimsSelector = claimsSelector;
        }

        #endregion

        #region Methods

        public IIdTokenContentBuilder WithNonce(string nonce)
        {
            NullGuard.NotNullOrWhiteSpace(nonce, nameof(nonce));

            _nonce = nonce;

            return this;
        }

        public IIdTokenContentBuilder WithAuthenticationContextClassReference(string authenticationContextClassReference)
        {
            NullGuard.NotNullOrWhiteSpace(authenticationContextClassReference, nameof(authenticationContextClassReference));

            _authenticationContextClassReference = authenticationContextClassReference;

            return this;
        }

        public IIdTokenContentBuilder WithAuthenticationMethodsReferences(IEnumerable<string> authenticationMethodsReferences)
        {
            NullGuard.NotNull(authenticationMethodsReferences, nameof(authenticationMethodsReferences));

            _authenticationMethodsReferences = authenticationMethodsReferences.Where(value => string.IsNullOrWhiteSpace(value) == false).ToArray();

            return this;
        }

        public IIdTokenContentBuilder WithAuthorizedParty(string authorizedParty)
        {
            NullGuard.NotNullOrWhiteSpace(authorizedParty, nameof(authorizedParty));

            _authorizedParty = authorizedParty;

            return this;
        }

        public IIdTokenContentBuilder WithCustomClaimsFilteredByScope(IScope scope, IEnumerable<Claim> customClaims)
        {
            NullGuard.NotNull(scope, nameof(scope))
                .NotNull(customClaims, nameof(customClaims));

            string scopeName = scope.Name;
            if (_supportedScopes.ContainsKey(scopeName) == false || _scopes.Contains(scopeName) == false)
            {
                return this;
            }

            foreach (Claim filteredClaim in scope.Filter(customClaims))
            {
                _customClaims.Add(new KeyValuePair<string, string>(filteredClaim.Type, string.IsNullOrWhiteSpace(filteredClaim.Value) ? string.Empty : filteredClaim.Value));
            }

            return this;
        }

        public IIdTokenContentBuilder WithCustomClaimsFilteredByClaimType(string claimType, IEnumerable<Claim> customClaims)
        {
            NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType))
                .NotNull(customClaims, nameof(customClaims));

            foreach (Claim customClaim in customClaims.Where(customClaim => string.IsNullOrWhiteSpace(customClaim.Type) == false && customClaim.Type == claimType))
            {
                _customClaims.Add(new KeyValuePair<string, string>(customClaim.Type, string.IsNullOrWhiteSpace(customClaim.Value) ? string.Empty : customClaim.Value));
            }

            return this;
        }

        public IEnumerable<Claim> Build()
        {
            if (_scopes.Contains(ScopeHelper.OpenIdScope) == false)
            {
                throw new IntranetExceptionBuilder(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser).Build();
            }

            List<Claim> claims = [];
            claims.AddRange(ResolveClaimsForOpenIdScope());
            claims.AddRange(ResolveClaimsForProfileScope());
            claims.AddRange(ResolveClaimsForEmailScope());
            claims.AddRange(ResolveCustomClaims());
            claims.AddRange(ResolveClaimsSpecifiedForIdToken());

            return RemoveNoneSupportedClaimTypes(claims);
        }

        private IReadOnlyCollection<Claim> ResolveClaimsForOpenIdScope()
        {
            List<Claim> claims =
            [
                new(SubjectIdentifierClaimType, _nameIdentifier)
            ];

            claims.AddRange(_claimsSelector.Select(_supportedScopes, [ScopeHelper.OpenIdScope], Array.Empty<Claim>()));

            return claims.AsReadOnly();
        }

        private IReadOnlyCollection<Claim> ResolveClaimsForProfileScope()
        {
            if (_scopes.Contains(ScopeHelper.ProfileScope) == false)
            {
                return Array.Empty<Claim>();
            }

            IList<Claim> claims = new List<Claim>();

            Claim nameClaim = GenerateClaim(ClaimTypes.Name, _userInfo, userInfo => userInfo.FullName);
            if (nameClaim != null)
            {
                claims.Add(nameClaim);
            }

            Claim givenNameClaim = GenerateClaim(ClaimTypes.GivenName, _userInfo, userInfo => userInfo.GivenName);
            if (givenNameClaim != null)
            {
                claims.Add(givenNameClaim);
            }

            Claim surnameClaim = GenerateClaim(ClaimTypes.Surname, _userInfo, userInfo => userInfo.Surname);
            if (surnameClaim != null)
            {
                claims.Add(surnameClaim);
            }

            return _claimsSelector.Select(_supportedScopes, [ScopeHelper.ProfileScope], claims);
        }

        private IReadOnlyCollection<Claim> ResolveClaimsForEmailScope()
        {
            if (_scopes.Contains(ScopeHelper.EmailScope) == false)
            {
                return Array.Empty<Claim>();
            }

            IList<Claim> claims = new List<Claim>();

            Claim emailClaim = GenerateClaim(ClaimTypes.Email, _userInfo, userInfo => userInfo.Email);
            if (emailClaim != null)
            {
                claims.Add(emailClaim);
            }

            return _claimsSelector.Select(_supportedScopes, [ScopeHelper.EmailScope], claims);
        }

        private IReadOnlyCollection<Claim> ResolveCustomClaims()
        {
            return _customClaims.Select(customClaim => new Claim(customClaim.Key, customClaim.Value)).ToArray();
        }

        private IReadOnlyCollection<Claim> ResolveClaimsSpecifiedForIdToken()
        {
            IList<Claim> claims =
            [
                new(AuthenticationTimeClaimType, _authenticationTime.ToUniversalTime().ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture))
            ];

            if (string.IsNullOrWhiteSpace(_nonce) == false)
            {
                claims.Add(new Claim(NonceClaimType, _nonce));
            }

            if (string.IsNullOrWhiteSpace(_authenticationContextClassReference) == false)
            {
                claims.Add(new Claim(AuthenticationContextClassReferenceClaimType, _authenticationContextClassReference));
            }

            if (_authenticationMethodsReferences?.Length > 0)
            {
                claims.Add(new Claim(AuthenticationMethodsReferencesClaimType, JsonSerializer.Serialize(_authenticationMethodsReferences)));
            }

            if (string.IsNullOrWhiteSpace(_authorizedParty) == false)
            {
                claims.Add(new Claim(AuthorizedPartyClaimType, _authorizedParty));
            }

            return claims.AsReadOnly();
        }

        private static IReadOnlyCollection<Claim> RemoveNoneSupportedClaimTypes(IEnumerable<Claim> claims)
        {
            NullGuard.NotNull(claims, nameof(claims));

            return claims.Where(claim => string.IsNullOrWhiteSpace(claim.Type) == false && NoneSupportedClaimTypes.Contains(claim.Type) == false).ToArray();
        }

        private static Claim GenerateClaim(string claimType, IUserInfo userInfo, Func<IUserInfo, string> valueGetter)
        {
            NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType))
                .NotNull(userInfo, nameof(userInfo))
                .NotNull(valueGetter, nameof(valueGetter));

            string value = valueGetter(userInfo);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return new Claim(claimType, value);
        }

        #endregion
    }
}