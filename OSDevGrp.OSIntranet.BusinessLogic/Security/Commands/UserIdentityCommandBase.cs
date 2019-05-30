using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public abstract class UserIdentityCommandBase : IdentityCommandBase, IUserIdentityCommand
    {
        #region Properties

        public string ExternalUserIdentifier { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository));

            return base.Validate(validator, securityRepository)
                .String.ShouldNotBeNullOrWhiteSpace(ExternalUserIdentifier, GetType(), nameof(ExternalUserIdentifier))
                .String.ShouldHaveMinLength(ExternalUserIdentifier, 1, GetType(), nameof(ExternalUserIdentifier))
                .String.ShouldHaveMaxLength(ExternalUserIdentifier, 256, GetType(), nameof(ExternalUserIdentifier));
        }

        public IUserIdentity ToDomain()
        {
            return new UserIdentityBuilder(ExternalUserIdentifier, Claims)
                .WithIdentifier(Identifier)
                .Build();
        }

        #endregion
    }
}