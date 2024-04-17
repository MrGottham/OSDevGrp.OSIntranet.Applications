using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Logic
{
    internal class SupportedScopesProvider : ISupportedScopesProvider
    {
        #region Properties

        public IReadOnlyDictionary<string, IScope> SupportedScopes { get; } = new ReadOnlyDictionary<string, IScope>(BuildSupportedScopes());

        #endregion

        #region Methods

        internal static IScope BuildOpenIdScope()
        {
            return ScopeFactory.Create(ScopeHelper.OpenIdScope, "this mandatory scope indicates that the application intends to use OpenID Connect to verify the user's identity.")
                .WithRelatedClaim(ClaimTypes.NameIdentifier)
                .Build();
        }

        internal static IScope BuildProfileScope()
        {
            return ScopeFactory.Create(ScopeHelper.ProfileScope, "this scope requests access to the End-User's profile.")
                .WithRelatedClaim(ClaimTypes.Name)
                .WithRelatedClaim(ClaimTypes.GivenName)
                .WithRelatedClaim(ClaimTypes.Surname)
                .Build();
        }

        internal static IScope BuildEmailScope()
        {
            return ScopeFactory.Create(ScopeHelper.EmailScope, "this scope requests access to the End-User's mail addresses.")
                .WithRelatedClaim(ClaimTypes.Email)
                .Build();
        }

        internal static IScope BuildWebApiScope()
        {
            return ScopeFactory.Create(ScopeHelper.WebApiScope, "this scope requests access to the OS Development Group Web API.")
                .WithRelatedClaim(ClaimHelper.ExternalUserIdentifierClaimType)
                .WithRelatedClaim(ClaimHelper.FriendlyNameClaimType)
                .WithRelatedClaim(ClaimHelper.SecurityAdminClaimType)
                .WithRelatedClaim(ClaimHelper.AccountingClaimType)
                .WithRelatedClaim(ClaimHelper.AccountingAdministratorClaimType)
                .WithRelatedClaim(ClaimHelper.AccountingCreatorClaimType)
                .WithRelatedClaim(ClaimHelper.AccountingModifierClaimType)
                .WithRelatedClaim(ClaimHelper.AccountingViewerClaimType)
                .WithRelatedClaim(ClaimHelper.MediaLibraryClaimType)
                .WithRelatedClaim(ClaimHelper.MediaLibraryModifierClaimType)
                .WithRelatedClaim(ClaimHelper.MediaLibraryLenderClaimType)
                .WithRelatedClaim(ClaimHelper.CommonDataClaimType)
                .WithRelatedClaim(ClaimHelper.ContactsClaimType)
                .WithRelatedClaim(ClaimHelper.CountryCodeClaimType)
                .WithRelatedClaim(ClaimHelper.CollectNewsClaimType)
                .Build();
        }

        private static IDictionary<string, IScope> BuildSupportedScopes()
        {
            IScope openIdScope = BuildOpenIdScope();
            IScope profileScope = BuildProfileScope();
            IScope emailScope = BuildEmailScope();
            IScope webApiScope = BuildWebApiScope();

            IDictionary<string, IScope> supportedScopes = new ConcurrentDictionary<string, IScope>();
            supportedScopes.Add(openIdScope.Name, openIdScope);
            supportedScopes.Add(profileScope.Name, profileScope);
            supportedScopes.Add(emailScope.Name, emailScope);
            supportedScopes.Add(webApiScope.Name, webApiScope);
            return supportedScopes;
        }

        #endregion
    }
}