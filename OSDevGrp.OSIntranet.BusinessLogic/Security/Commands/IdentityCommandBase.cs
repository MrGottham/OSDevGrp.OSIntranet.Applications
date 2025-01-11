using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public abstract class IdentityCommandBase : IdentityIdentificationCommandBase, IIdentityCommand
    {
        #region Properties

        public IEnumerable<Claim> Claims { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository));

            return base.Validate(validator, securityRepository)
                .Object.ShouldNotBeNull(Claims, GetType(), nameof(Claims));
        }

        #endregion
    }
}