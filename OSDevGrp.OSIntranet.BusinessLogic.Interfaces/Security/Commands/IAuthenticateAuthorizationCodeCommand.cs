using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IAuthenticateAuthorizationCodeCommand : IAuthenticateCommand
    {
        string AuthorizationCode { get; }

        string ClientId { get; }

        string ClientSecret { get; }

        Uri RedirectUri { get; }

        Action<IToken> OnIdTokenResolved { get; }

        Task<bool> IsMatchAsync(IReadOnlyDictionary<string, string> authorizationData, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver);
    }
}