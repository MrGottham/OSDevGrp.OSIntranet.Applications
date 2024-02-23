using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

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

        public bool IsAccountingAdministrator()
        {
            return HasClaim(principal => principal.GetClaim(ClaimHelper.AccountingAdministratorClaimType));
        }

        public bool IsAccountingCreator()
        {
            return HasClaim(principal => principal.GetClaim(ClaimHelper.AccountingCreatorClaimType));
        }

        public bool CanModifyAccounting(int accountingNumber)
        {
            return EvaluateValueAgainstClaimValue(principal => principal.GetClaim(ClaimHelper.AccountingModifierClaimType), accountingNumber);
        }

        public bool CanAccessAccounting(int accountingNumber)
        {
            return EvaluateValueAgainstClaimValue(principal => principal.GetClaim(ClaimHelper.AccountingViewerClaimType), accountingNumber);
        }

        public string GetNameIdentifier()
        {
            return GetClaimStingValue(currentPrincipal => currentPrincipal.GetClaim(ClaimTypes.NameIdentifier));
        }

        public string GetName()
        {
            return GetClaimStingValue(currentPrincipal => currentPrincipal.GetClaim(ClaimTypes.Name));
        }

        public string GetMailAddress()
        {
            return GetClaimStingValue(currentPrincipal => currentPrincipal.GetClaim(ClaimTypes.Email));
        }

        public int? GetNumberOfNewsToCollect()
        {
            return GetClaimIntegerValue(currentPrincipal => currentPrincipal.GetClaim(ClaimHelper.CollectNewsClaimType));
        }

        public bool IsMediaLibraryModifier()
        {
	        return HasClaim(principal => principal.GetClaim(ClaimHelper.MediaLibraryModifierClaimType));
        }

		public bool IsMediaLibraryLender()
        {
	        return HasClaim(principal => principal.GetClaim(ClaimHelper.MediaLibraryLenderClaimType));
        }

		public IRefreshableToken GetMicrosoftToken(Func<string, string> unprotect)
		{
			NullGuard.NotNull(unprotect, nameof(unprotect));

			return GetMicrosoftToken(_principalResolver.GetCurrentPrincipal(), unprotect);
		}

		public IRefreshableToken GetMicrosoftToken(IPrincipal principal, Func<string, string> unprotect)
		{
			NullGuard.NotNull(principal, nameof(principal))
				.NotNull(unprotect, nameof(unprotect));

			return GetExternalToken(() => principal.GetClaim(ClaimHelper.MicrosoftTokenClaimType), unprotect) as IRefreshableToken;
		}

		public IToken GetGoogleToken(Func<string, string> unprotect)
		{
			NullGuard.NotNull(unprotect, nameof(unprotect));

			return GetGoogleToken(_principalResolver.GetCurrentPrincipal(), unprotect);
		}

		public IToken GetGoogleToken(IPrincipal principal, Func<string, string> unprotect)
		{
			NullGuard.NotNull(principal, nameof(principal))
				.NotNull(unprotect, nameof(unprotect));

			return GetExternalToken(() => principal.GetClaim(ClaimHelper.GoogleTokenClaimType), unprotect);
		}

		private bool HasClaim(Func<IPrincipal, Claim> claimGetter)
        {
            NullGuard.NotNull(claimGetter, nameof(claimGetter));

            IPrincipal currentPrincipal = _principalResolver.GetCurrentPrincipal();

            return claimGetter(currentPrincipal) != null;
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

        private bool EvaluateValueAgainstClaimValue(Func<IPrincipal, Claim> claimGetter, int value, char separator = ',', char wildcard = '*', bool allowWildcard = true)
        {
            NullGuard.NotNull(claimGetter, nameof(claimGetter))
                .NotNull(separator, nameof(separator))
                .NotNull(wildcard, nameof(wildcard));

            string claimValue = GetClaimStingValue(claimGetter);
            if (string.IsNullOrWhiteSpace(claimValue))
            {
                return false;
            }

            string[] valueCollection = claimValue.Split(separator);
            if (valueCollection.Length == 0)
            {
                return false;
            }

            if (allowWildcard && valueCollection.Contains(wildcard.ToString(CultureInfo.InvariantCulture)))
            {
                return true;
            }

            return valueCollection.Contains(value.ToString(CultureInfo.InvariantCulture));
        }

        private static IToken GetExternalToken(Func<Claim> claimGetter, Func<string, string> unprotect)
        {
	        NullGuard.NotNull(claimGetter, nameof(claimGetter))
		        .NotNull(unprotect, nameof(unprotect));

	        Claim claim = claimGetter();
	        if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
	        {
		        return null;
	        }

	        if (claim.ValueType == typeof(IToken).FullName)
	        {
		        return TokenFactory.Create().FromBase64String(unprotect(claim.Value));
	        }

	        if (claim.ValueType == typeof(IRefreshableToken).FullName)
	        {
		        return RefreshableTokenFactory.Create().FromBase64String(unprotect(claim.Value));
	        }

	        throw new NotSupportedException($"Unsupported token type: {claim.ValueType}");
        }

        #endregion
    }
}