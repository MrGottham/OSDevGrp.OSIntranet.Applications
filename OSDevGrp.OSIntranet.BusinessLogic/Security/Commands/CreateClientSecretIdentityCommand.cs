using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public class CreateClientSecretIdentityCommand : ClientSecretIdentityCommandBase, ICreateClientSecretIdentityCommand
    {
        #region Properties

        protected override bool MapIdentifier => false;

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository));

            return base.Validate(validator, securityRepository)
                .Object.ShouldBeUnknownValue(Identifier, number => Task.Run(async () => await GetClientSecretIdentityAsync(securityRepository) == null), GetType(), nameof(Identifier));
        }

        #endregion
    }
}