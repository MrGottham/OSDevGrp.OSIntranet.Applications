using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands
{
    public interface IAuthorizationStateCommand : ICommand
    {
        string AuthorizationState { get; }

        Func<byte[], byte[]> Unprotect { get; }

        IValidator Validate(IValidator validator);

        IAuthorizationState ToDomain(IAuthorizationStateFactory authorizationStateFactory, IValidator validator, ISecurityRepository securityRepository, ITrustedDomainResolver trustedDomainResolver, ISupportedScopesProvider supportedScopesProvider);
    }
}