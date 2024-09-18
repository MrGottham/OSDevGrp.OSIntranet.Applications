using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    internal class AuthenticateAuthorizationCodeCommand : AuthenticateCommandBase, IAuthenticateAuthorizationCodeCommand
    {
        #region Constructor

        public AuthenticateAuthorizationCodeCommand(string authorizationCode, string clientId, string clientSecret, Uri redirectUri, IReadOnlyCollection<Claim> claims, string authenticationType, IReadOnlyDictionary<string, string> authenticationSessionItems, Func<string, string> protector) 
            : base(claims, authenticationType, authenticationSessionItems, protector)
        {
            NullGuard.NotNullOrWhiteSpace(authorizationCode, nameof(authorizationCode))
                .NotNullOrWhiteSpace(clientId, nameof(clientId))
                .NotNullOrWhiteSpace(clientSecret, nameof(clientSecret))
                .NotNull(redirectUri, nameof(redirectUri));

            AuthorizationCode = authorizationCode;
            ClientId = clientId;
            ClientSecret = clientSecret;
            RedirectUri = redirectUri;
        }

        #endregion

        #region Properties

        public string AuthorizationCode { get; }

        public string ClientId { get; }

        public string ClientSecret { get; }

        public Uri RedirectUri { get; }

        #endregion

        #region Methods

        public async Task<bool> IsMatchAsync(IReadOnlyDictionary<string, string> authorizationData, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver)
        {
            NullGuard.NotNull(authorizationData, nameof(authorizationData))
                .NotNull(securityRepository, nameof(securityRepository))
                .NotNull(trustedDomainResolver, nameof(trustedDomainResolver));

            string clientIdFromAuthorizationData = ResolveValue(authorizationData, AuthorizationDataConverter.ClientIdKey);
            if (string.IsNullOrWhiteSpace(clientIdFromAuthorizationData) || string.CompareOrdinal(clientIdFromAuthorizationData, ClientId) != 0)
            {
                return false;
            }

            string clientSecretFromAuthorizationData = ResolveValue(authorizationData, AuthorizationDataConverter.ClientSecretKey);
            if (string.IsNullOrWhiteSpace(clientSecretFromAuthorizationData) || string.CompareOrdinal(clientSecretFromAuthorizationData, ClientSecret) != 0)
            {
                return false;
            }

            string redirectUriStringFromAuthorizationData = ResolveValue(authorizationData, AuthorizationDataConverter.RedirectUriKey);
            if (string.IsNullOrWhiteSpace(redirectUriStringFromAuthorizationData) || Uri.TryCreate(redirectUriStringFromAuthorizationData, UriKind.Absolute, out Uri redirectUriFromAuthorizationData) == false || redirectUriFromAuthorizationData != RedirectUri)
            {
                return false;
            }

            IClientSecretIdentity clientSecretIdentity = await securityRepository.GetClientSecretIdentityAsync(ClientId);
            if (clientSecretIdentity == null || string.CompareOrdinal(clientSecretIdentity.ClientSecret, ClientSecret) != 0)
            {
                return false;
            }

            return trustedDomainResolver.IsTrustedDomain(RedirectUri);
        }

        private static string ResolveValue(IReadOnlyDictionary<string, string> authorizationData, string key)
        {
            NullGuard.NotNull(authorizationData, nameof(authorizationData))
                .NotNullOrWhiteSpace(key, nameof(key));

            if (authorizationData.TryGetValue(key, out string value) == false)
            {
                return null;
            }

            return string.IsNullOrWhiteSpace(value) == false ? value : null;
        }

        #endregion
    }
}