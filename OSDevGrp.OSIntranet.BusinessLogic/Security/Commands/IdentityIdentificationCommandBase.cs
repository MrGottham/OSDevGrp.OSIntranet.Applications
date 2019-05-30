using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
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

            return validator.Integer.ShouldBeGreaterThanZero(Identifier, GetType(), nameof(Identifier));
        }

        protected Task<IUserIdentity> GetUserIdentity(ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            return Task.Run(async () =>  _userIdentity ?? (_userIdentity = await securityRepository.GetUserIdentityAsync(Identifier)));
        }

        protected Task<IClientSecretIdentity> GetClientSecretIdentity(ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(securityRepository, nameof(securityRepository));

            return Task.Run(async () =>  _clientSecretIdentity ?? (_clientSecretIdentity = await securityRepository.GetClientSecretIdentityAsync(Identifier)));
        }

        #endregion
    }
}