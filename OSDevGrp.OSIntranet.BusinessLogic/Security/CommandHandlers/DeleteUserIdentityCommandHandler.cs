using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class DeleteUserIdentityCommandHandler : IdentityIdentificationCommandHandlerBase<IDeleteUserIdentityCommand>
    {
        #region Constructor

        public DeleteUserIdentityCommandHandler(IValidator validator, ISecurityRepository securityRepository)
            : base(validator, securityRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(IDeleteUserIdentityCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            await SecurityRepository.DeleteUserIdentityAsync(command.Identifier);
        }

        #endregion
    }
}