using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class UserIdentityModel
    {
        public virtual int UserIdentityIdentifier { get; set; }
        
        public virtual string ExternalUserIdentifier { get; set; }
    }

    internal static class UserIdentityModelExtensions
    {
        internal static IUserIdentity ToDomain(this UserIdentityModel userIdentityModel)
        {
            NullGuard.NotNull(userIdentityModel, nameof(userIdentityModel));

            return new UserIdentityBuilder(userIdentityModel.ExternalUserIdentifier)
                .WithIdentifier(userIdentityModel.UserIdentityIdentifier)
                .Build();
        }
    }
}
