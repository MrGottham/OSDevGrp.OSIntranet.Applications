using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    public class CreateUserIdentityCommandHandler : IdentityIdentificationCommandHandlerBase<ICreateUserIdentityCommand>
    {
        #region Constructor

        public CreateUserIdentityCommandHandler(IValidator validator, ISecurityRepository securityRepository)
            : base(validator, securityRepository)
        {
        }

        #endregion

        #region Methods

        protected override async Task ManageRepositoryAsync(ICreateUserIdentityCommand command)
        {
            NullGuard.NotNull(command, nameof(command));

            IUserIdentity userIdentity = command.ToDomain();

            await SecurityRepository.CreateUserIdentityAsync(userIdentity);
        }

        #endregion
    }
}