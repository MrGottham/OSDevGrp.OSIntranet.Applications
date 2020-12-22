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
    public class ClaimResolver : IClaimResolver
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
            IPrincipal currentPrincipal = _principalResolver.GetCurrentPrincipal();

            Claim countryCodeClaim = currentPrincipal.GetClaim(ClaimHelper.CountryCodeClaimType);
            if (countryCodeClaim == null || string.IsNullOrWhiteSpace(countryCodeClaim.Value))
            {
                return null;
            }

            return countryCodeClaim.Value;
        }

        public int? GetAccountingNumber()
        {
            IPrincipal currentPrincipal = _principalResolver.GetCurrentPrincipal();

            Claim accountingClaim = currentPrincipal.GetClaim(ClaimHelper.AccountingClaimType);
            if (accountingClaim == null || int.TryParse(accountingClaim.Value, out int accountingNumber) == false)
            {
                return null;
            }

            return accountingNumber;
        }

        public TToken GetToken<TToken>(Func<string, string> unprotect) where TToken : class, IToken
        {
            NullGuard.NotNull(unprotect, nameof(unprotect));

            IPrincipal currentPrincipal = _principalResolver.GetCurrentPrincipal();

            Claim tokenClaim = currentPrincipal.GetClaim(ClaimHelper.TokenClaimType);
            if (tokenClaim == null || string.IsNullOrWhiteSpace(tokenClaim.Value))
            {
                return null;
            }

            return Token.Create<TToken>(unprotect(tokenClaim.Value));
        }

        #endregion
    }
}