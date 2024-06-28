using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IPrepareAuthorizationCodeFlowCommand : ICommand
    {
        string ResponseType { get; }

        string ClientId { get; }

        Uri RedirectUri { get; }

        IEnumerable<string> Scopes { get; }

        string State { get; }

        Func<byte[], byte[]> Protector { get; }

        IValidator Validate(IValidator validator, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider);

        IAuthorizationState ToDomain(IAuthorizationStateFactory authorizationStateFactory);
    }
}