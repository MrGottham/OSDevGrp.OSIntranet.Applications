using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountGroupModel : AccountGroupModelBase
    {
        public virtual int AccountGroupIdentifier { get; set; }

        public virtual AccountGroupType AccountGroupType { get; set; }
    }

    internal static class AccountGroupModelExtensions
    {
        internal static IAccountGroup ToDomain(this AccountGroupModel accountGroupModel)
        {
            NullGuard.NotNull(accountGroupModel, nameof(accountGroupModel));

            IAccountGroup accountGroup = new AccountGroup(accountGroupModel.AccountGroupIdentifier, accountGroupModel.Name, accountGroupModel.AccountGroupType);
            accountGroup.AddAuditInformations(accountGroupModel.CreatedUtcDateTime, accountGroupModel.CreatedByIdentifier, accountGroupModel.ModifiedUtcDateTime, accountGroupModel.ModifiedByIdentifier);
            accountGroup.SetDeletable(accountGroupModel.Deletable);

            return accountGroup;
        }

        internal static void CreateAccountGroupModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<AccountGroupModel>(entity =>
            {
                entity.HasKey(e => e.AccountGroupIdentifier);
                entity.Property(e => e.AccountGroupIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.AccountGroupType).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
            });
        }
    }
}
