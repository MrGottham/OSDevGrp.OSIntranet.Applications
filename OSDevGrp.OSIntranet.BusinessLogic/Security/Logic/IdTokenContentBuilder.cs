using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
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

        private readonly string _subjectIdentifier;
        private readonly IUserInfo _userInfo;
        private readonly DateTimeOffset _authenticationTime;
        private readonly IList<KeyValuePair<string, string>> _customClaims = new List<KeyValuePair<string, string>>();
        private string _nonce;
        private string _authenticationContextClassReference;
        private string[] _authenticationMethodsReferences;
        private string _authorizedParty;

        private static readonly string[] HandledClaimType =
        [
            SubjectIdentifierClaimType,
            AuthenticationTimeClaimType,
            NonceClaimType,
            AuthenticationContextClassReferenceClaimType,
            AuthenticationMethodsReferencesClaimType,
            AuthorizedPartyClaimType
        ];

        #endregion

        #region Constructor

        public IdTokenContentBuilder(string subjectIdentifier, IUserInfo userInfo, DateTimeOffset authenticationTime)
        {
            NullGuard.NotNullOrWhiteSpace(subjectIdentifier, nameof(subjectIdentifier))
                .NotNull(userInfo, nameof(userInfo));

            _subjectIdentifier = subjectIdentifier;
            _userInfo = userInfo;
            _authenticationTime = authenticationTime;
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

        public IIdTokenContentBuilder WithCustomClaim(string claimType, string value)
        {
            NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType));

            if (HandledClaimType.Contains(claimType))
            {
                throw new ArgumentException($"Cannot add a custom claim with the type: {claimType}", nameof(claimType));
            }

            _customClaims.Add(new KeyValuePair<string, string>(claimType, value));

            return this;
        }

        public IEnumerable<Claim> Build()
        {
            List<Claim> claims =
            [
                new(SubjectIdentifierClaimType, _subjectIdentifier),
                new(AuthenticationTimeClaimType, _authenticationTime.ToUniversalTime().ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture))
            ];

            claims.AddRange(ResolveUserInfoClaims());

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

            claims.AddRange(_customClaims.Select(customClaim => new Claim(customClaim.Key, string.IsNullOrWhiteSpace(customClaim.Value) == false ? customClaim.Value : string.Empty)));

            return claims;
        }

        private IEnumerable<Claim> ResolveUserInfoClaims()
        {
            return _userInfo.ToClaims()
                .Where(claim => string.IsNullOrWhiteSpace(claim.Type) == false && HandledClaimType.Contains(claim.Type) == false)
                .Where(claim => string.IsNullOrWhiteSpace(claim.Value) == false)
                .ToArray();
        }

        #endregion
    }
}