using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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
        internal static IUserIdentity ToDomain(this UserIdentityModel userIdentityModel, IConverter securityModelConverter)
        {
            NullGuard.NotNull(userIdentityModel, nameof(userIdentityModel))
                .NotNull(securityModelConverter, nameof(securityModelConverter));

            IEnumerable<Claim> claimCollection = securityModelConverter.Convert<IEnumerable<UserIdentityClaimModel>, IEnumerable<Claim>>(userIdentityModel.UserIdentityClaims);

            UserIdentityClaimModel latestUserIdentityClaimModel = userIdentityModel.UserIdentityClaims.MaxBy(model => model.ModifiedUtcDateTime);

            IUserIdentity userIdentity = UserIdentityBuilderFactory.Create(userIdentityModel.ExternalUserIdentifier)
                .WithIdentifier(userIdentityModel.UserIdentityIdentifier)
                .AddClaims(claimCollection)
                .Build();

            if (latestUserIdentityClaimModel == null || latestUserIdentityClaimModel.ModifiedUtcDateTime < userIdentityModel.ModifiedUtcDateTime)
            {
                userIdentity.AddAuditInformation(userIdentityModel.CreatedUtcDateTime, userIdentityModel.CreatedByIdentifier, userIdentityModel.ModifiedUtcDateTime, userIdentityModel.ModifiedByIdentifier);
            }
            else
            {
                userIdentity.AddAuditInformation(userIdentityModel.CreatedUtcDateTime, userIdentityModel.CreatedByIdentifier, latestUserIdentityClaimModel.ModifiedUtcDateTime, latestUserIdentityClaimModel.ModifiedByIdentifier);
            }

            return userIdentity;
        }

        internal static UserIdentityModel With(this UserIdentityModel userIdentityModel, IEnumerable<Claim> claimCollection, RepositoryContext context, IConverter securityModelConverter)
        {
            NullGuard.NotNull(userIdentityModel, nameof(userIdentityModel))
                .NotNull(claimCollection, nameof(claimCollection))
                .NotNull(context, nameof(context))
                .NotNull(securityModelConverter, nameof(securityModelConverter));

            IList<ClaimModel> claimModelCollection = context.Claims.ToListAsync()
                .GetAwaiter()
                .GetResult();

            userIdentityModel.UserIdentityClaims = claimCollection.AsParallel()
                .Where(claim => claimModelCollection.Any(c => c.ClaimType == claim.Type))
                .Select(claim => securityModelConverter.Convert<Claim, UserIdentityClaimModel>(claim).With(userIdentityModel).With(claimModelCollection.Single(c => c.ClaimType == claim.Type)))
                .ToList();

            return userIdentityModel;
        }

        internal static void CreateUserIdentityModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<UserIdentityModel>(entity =>
            {
                entity.HasKey(e => e.UserIdentityIdentifier);
                entity.Property(e => e.UserIdentityIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.ExternalUserIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => e.ExternalUserIdentifier).IsUnique();
            });
        }
    }
}