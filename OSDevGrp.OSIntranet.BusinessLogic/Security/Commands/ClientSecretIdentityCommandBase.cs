using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public abstract class ClientSecretIdentityCommandBase : IdentityCommandBase, IClientSecretIdentityCommand
    {
        #region Properties

        public string FriendlyName { get; set; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator, ISecurityRepository securityRepository)
        {
            NullGuard.NotNull(validator, nameof(validator))
                .NotNull(securityRepository, nameof(securityRepository));

            return base.Validate(validator, securityRepository)
                .String.ShouldNotBeNullOrWhiteSpace(FriendlyName, GetType(), nameof(FriendlyName))
                .String.ShouldHaveMinLength(FriendlyName, 1, GetType(), nameof(FriendlyName))
                .String.ShouldHaveMaxLength(FriendlyName, 256, GetType(), nameof(FriendlyName));
        }

        public IClientSecretIdentity ToDomain()
        {
            return CreateClientSecretIdentityBuilder().Build();
        }

        public IClientSecretIdentity ToDomain(string clientId, string clientSecret)
        {
            return CreateClientSecretIdentityBuilder()
                .WithClientId(clientId)
                .WithClientSecret(clientSecret)
                .Build();
        }

        private IClientSecretIdentityBuilder CreateClientSecretIdentityBuilder()
        {
            return new ClientSecretIdentityBuilder(FriendlyName, Claims).WithIdentifier(Identifier);
        }

        #endregion
    }
}