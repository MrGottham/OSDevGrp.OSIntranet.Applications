using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class ClaimsSelector : IClaimsSelector
    {
        #region Private constanct

        private static readonly string[] ProtectedClaimTypes =
        [
            ClaimHelper.MicrosoftTokenClaimType,
            ClaimHelper.GoogleTokenClaimType
        ];

        #endregion

        #region Methods

        public IReadOnlyCollection<Claim> Select(IReadOnlyDictionary<string, IScope> supportedScopes, IEnumerable<string> scopes, IEnumerable<Claim> claims)
        {
            NullGuard.NotNull(supportedScopes, nameof(supportedScopes))
                .NotNull(scopes, nameof(scopes))
                .NotNull(claims, nameof(claims));

            return Select(supportedScopes, scopes.ToArray(), claims.ToArray());
        }

        private static IReadOnlyCollection<Claim> Select(IReadOnlyDictionary<string, IScope> supportedScopes, string[] scopes, Claim[] claims)
        {
            NullGuard.NotNull(supportedScopes, nameof(supportedScopes))
                .NotNull(scopes, nameof(scopes))
                .NotNull(claims, nameof(claims));

            if (scopes.Length == 0 || claims.Length == 0)
            {
                return Array.Empty<Claim>();
            }

            List<Claim> selectedClaims = new List<Claim>();
            foreach (var scope in scopes.Where(value => string.IsNullOrWhiteSpace(value) == false))
            {
                if (supportedScopes.TryGetValue(scope, out IScope s) == false)
                {
                    continue;
                }

                selectedClaims.AddRange(s.Filter(claims).ToArray());
            }

            selectedClaims.AddRange(claims.Where(claim => claim != null && string.IsNullOrWhiteSpace(claim.Type) == false && ProtectedClaimTypes.Contains(claim.Type)).ToArray());

            return selectedClaims.AsReadOnly();
        }

        #endregion
    }
}