using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class DeleteClientSecretIdentityCommandHandler : IdentityIdentificationCommandHandlerBase<IDeleteClientSecretIdentityCommand>
    {
        #region Constructor

        public DeleteClientSecretIdentityCommandHandler(IValidator validator, ISecurityRepository securityRepository)
            : base(validator, securityRepository)
        {
        }

        #endregion

        #region Methods

        protected async override Task ManageRepositoryAsync(IDeleteClientSecretIdentityCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await SecurityRepository.DeleteClientSecretIdentityAsync(command.Identifier);
        }

        #endregion
    }
}