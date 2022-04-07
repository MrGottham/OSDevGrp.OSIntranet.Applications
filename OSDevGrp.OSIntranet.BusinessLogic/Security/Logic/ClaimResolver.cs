using System;
using System.Security.Claims;
using System.Security.Principal;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class ClaimResolver : IClaimResolver
    {
        #region Private variables

        private readonly IPrincipalResolver _principalResolver;

        #endregion

        #region Constructor

        public ClaimResolver(IPrincipalResolver principalResolver)
        {
            NullGuard.NotNull(principalResolver, nameof(principalResolver));

            _principalResolver = principalResolver;
        }

        #endregion

        #region Methods

        public string GetCountryCode()
        {
            return GetClaimStingValue(currentPrincipal => currentPrincipal.GetClaim(ClaimHelper.CountryCodeClaimType));
        }

        public int? GetAccountingNumber()
        {
            return GetClaimIntegerValue(currentPrincipal => currentPrincipal.GetClaim(ClaimHelper.AccountingClaimType));
        }

        public string GetNameIdentifier()
        {
            return GetClaimStingValue(currentPrincipal => currentPrincipal.GetClaim(ClaimTypes.NameIdentifier));
        }

        public string GetMailAddress()
        {
            return GetClaimStingValue(currentPrincipal => currentPrincipal.GetClaim(ClaimTypes.Email));
        }

        public int? GetNumberOfNewsToCollect()
        {
            return GetClaimIntegerValue(currentPrincipal => currentPrincipal.GetClaim(ClaimHelper.CollectNewsClaimType));
        } 

        public TToken GetToken<TToken>(Func<string, string> unprotect) where TToken : class, IToken
        {
            NullGuard.NotNull(unprotect, nameof(unprotect));

            string tokenValue = GetClaimStingValue(currentPrincipal => currentPrincipal.GetClaim(ClaimHelper.TokenClaimType));
            if (string.IsNullOrWhiteSpace(tokenValue))
            {
                return null;
            }

            return Token.Create<TToken>(unprotect(tokenValue));
        }

        private string GetClaimStingValue(Func<IPrincipal, Claim> claimGetter)
        {
            NullGuard.NotNull(claimGetter, nameof(claimGetter));

            IPrincipal currentPrincipal = _principalResolver.GetCurrentPrincipal();

            Claim claim = claimGetter(currentPrincipal);
            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
            {
                return null;
            }

            return claim.Value;
        }

        private int? GetClaimIntegerValue(Func<IPrincipal, Claim> claimGetter)
        {
            NullGuard.NotNull(claimGetter, nameof(claimGetter));

            IPrincipal currentPrincipal = _principalResolver.GetCurrentPrincipal();

            Claim claim = claimGetter(currentPrincipal);
            if (claim == null || int.TryParse(claim.Value, out int value) == false)
            {
                return null;
            }

            return value;
        }

        #endregion
    }
}