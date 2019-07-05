using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public abstract class IdentityIdentificationCommandBase : IIdentityIdentificationCommand
    {
        #region Private variables

        private IUserIdentity _userIdentity;
        private IClientSecretIdentity _clientSecretIdentity;

        #endregion

        #region Properties

        public int Identifier { get; set; }

        #endregion

        #region Methods

        public virtual IValidator Validate(IValidator validator, ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository));

            return validator.ValidateIdentityIdentifier(Identifier, GetType(), nameof(Identifier));
        }

        protected Task<IUserIdentity> GetUserIdentity(ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            return Task.Run(() => Identifier.GetUserIdentity(securityRepository, ref _userIdentity));
        }

        protected Task<IClientSecretIdentity> GetClientSecretIdentity(ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            return Task.Run(() => Identifier.GetClientSecretIdentity(securityRepository, ref _clientSecretIdentity));
        }

        #endregion
    }
}