using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BasicAccountModel : AuditModelBase
    {
        public virtual int BasicAccountIdentifier { get; set; }

        public virtual string AccountName { get; set; }

        public virtual string Description { get; set; }

        public virtual string Note { get; set; }

        public virtual List<AccountModel> Accounts { get; set; }

        public virtual List<BudgetAccountModel> BudgetAccounts { get; set; }

        public virtual List<ContactAccountModel> ContactAccounts { get; set; }
    }

    internal static class BasicAccountModelExtensions
    {
        internal static void CreateBasicAccountModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<BasicAccountModel>(entity =>
            {
                entity.HasKey(e => e.BasicAccountIdentifier);
                entity.Property(e => e.BasicAccountIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.AccountName).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.Description).IsRequired(false).IsUnicode().HasMaxLength(512);
                entity.Property(e => e.Note).IsRequired(false).IsUnicode().HasMaxLength(4096);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
            });
        }
    }
}