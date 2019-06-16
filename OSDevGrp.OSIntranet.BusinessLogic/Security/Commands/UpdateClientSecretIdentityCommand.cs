using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public class UpdateClientSecretIdentityCommand : ClientSecretIdentityCommandBase, IUpdateClientSecretIdentityCommand
    {
        #region Methods

        public override IValidator Validate(IValidator validator, ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository));

            return base.Validate(validator, securityRepository)
                .Object.ShouldBeKnownValue(Identifier, number => Task.Run(async () => await GetClientSecretIdentity(securityRepository) != null), GetType(), nameof(Identifier));
        }

        #endregion
    }
}