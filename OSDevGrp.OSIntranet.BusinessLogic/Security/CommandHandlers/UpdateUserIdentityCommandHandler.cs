using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class UpdateUserIdentityCommandHandler : IdentityIdentificationCommandHandlerBase<IUpdateUserIdentityCommand>
    {
        #region Constructor

        public UpdateUserIdentityCommandHandler(IValidator validator, ISecurityRepository securityRepository)
            : base(validator, securityRepository)
        {
        }

        #endregion

        #region Methods

        protected async override Task ManageRepositoryAsync(IUpdateUserIdentityCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IUserIdentity userIdentity = command.ToDomain();

            await SecurityRepository.UpdateUserIdentityAsync(userIdentity);
        }

        #endregion
    }
}