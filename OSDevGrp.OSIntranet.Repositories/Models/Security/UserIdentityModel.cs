using System.Collections.Generic;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Converters;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class UserIdentityModel
    {
        public virtual int UserIdentityIdentifier { get; set; }
        
        public virtual string ExternalUserIdentifier { get; set; }

        public virtual List<UserIdentityClaimModel> UserIdentityClaims { get; set; }
    }

    internal static class UserIdentityModelExtensions
    {
        internal static IUserIdentity ToDomain(this UserIdentityModel userIdentityModel)
        {
            NullGuard.NotNull(userIdentityModel, nameof(userIdentityModel));

            IConverter securityModelConverter = new SecurityModelConverter();
            IEnumerable<Claim> claimCollection = securityModelConverter.Convert<IEnumerable<UserIdentityClaimModel>, IEnumerable<Claim>>(userIdentityModel.UserIdentityClaims); 

            return new UserIdentityBuilder(userIdentityModel.ExternalUserIdentifier)
                .WithIdentifier(userIdentityModel.UserIdentityIdentifier)
                .AddClaims(claimCollection)
                .Build();
        }
    }
}
