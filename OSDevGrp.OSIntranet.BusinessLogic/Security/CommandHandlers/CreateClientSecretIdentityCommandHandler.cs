using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class CreateClientSecretIdentityCommandHandler : IdentityIdentificationCommandHandlerBase<ICreateClientSecretIdentityCommand>
    {
        #region Constructor

        public CreateClientSecretIdentityCommandHandler(IValidator validator, ISecurityRepository securityRepository)
            : base(validator, securityRepository)
        {
        }

        #endregion

        #region Methods

        protected async override Task ManageRepositoryAsync(ICreateClientSecretIdentityCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IClientSecretIdentity clientSecretIdentity = command.ToDomain();

            await SecurityRepository.CreateClientSecretIdentityAsync(clientSecretIdentity);
        }

        #endregion
    }
}