using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class UpdateClientSecretIdentityCommandHandler : IdentityIdentificationCommandHandlerBase<IUpdateClientSecretIdentityCommand>
    {
        #region Constructor

        public UpdateClientSecretIdentityCommandHandler(IValidator validator, ISecurityRepository securityRepository)
            : base(validator, securityRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IUpdateClientSecretIdentityCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IClientSecretIdentity existingClientSecretIdentity = await SecurityRepository.GetClientSecretIdentityAsync(command.Identifier);
            if (existingClientSecretIdentity == null)
            {
                throw new IntranetExceptionBuilder(ErrorCode.ObjectIsNull, "existingClientSecretIdentity").Build();
            }

            IClientSecretIdentity clientSecretIdentity = command.ToDomain(existingClientSecretIdentity.ClientId, existingClientSecretIdentity.ClientSecret);

            await SecurityRepository.UpdateClientSecretIdentityAsync(clientSecretIdentity);
        }

        #endregion
    }
}