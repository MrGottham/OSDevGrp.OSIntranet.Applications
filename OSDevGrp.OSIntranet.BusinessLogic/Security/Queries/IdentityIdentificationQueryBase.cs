using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Queries
{
    public abstract class IdentityIdentificationQueryBase : IIdentityIdentificationQuery
    {
        #region Properties

        public int Identifier { get; set; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator, ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository));

            return validator.ValidateIdentityIdentifier(Identifier, GetType(), nameof(Identifier));
        }

        #endregion
    }
}
