using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Security
{
    internal class UserIdentityModel : AuditModelBase
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

            UserIdentityClaimModel latestUserIdentityClaimModel = userIdentityModel.UserIdentityClaims.OrderByDescending(model => model.ModifiedUtcDateTime).FirstOrDefault();

            IUserIdentity userIdentity = new UserIdentityBuilder(userIdentityModel.ExternalUserIdentifier)
                .WithIdentifier(userIdentityModel.UserIdentityIdentifier)
                .AddClaims(claimCollection)
                .Build();

            if (latestUserIdentityClaimModel == null || latestUserIdentityClaimModel.ModifiedUtcDateTime > userIdentityModel.ModifiedUtcDateTime)
            {
                userIdentity.AddAuditInformations(userIdentityModel.CreatedUtcDateTime, userIdentityModel.CreatedByIdentifier, userIdentityModel.ModifiedUtcDateTime, userIdentityModel.ModifiedByIdentifier);
            }
            else
            {
                userIdentity.AddAuditInformations(userIdentityModel.CreatedUtcDateTime, userIdentityModel.CreatedByIdentifier, latestUserIdentityClaimModel.ModifiedUtcDateTime, latestUserIdentityClaimModel.ModifiedByIdentifier);
            }

            return userIdentity;
        }
    }
}
