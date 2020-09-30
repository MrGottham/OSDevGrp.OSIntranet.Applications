using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountModel : AccountModelBase
    {
        public virtual int AccountIdentifier { get; set; }

        public virtual int AccountGroupIdentifier { get; set; }

        public virtual AccountGroupModel AccountGroup { get; set; }
    }

    internal static class AccountModelExtensions
    {
        internal static IAccount ToDomain(this AccountModel accountModel, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(accountModel.Accounting);
            IAccountGroup accountGroup = accountingModelConverter.Convert<AccountGroupModel, IAccountGroup>(accountModel.AccountGroup);

            IAccount account = new Account(accounting, accountModel.AccountNumber, accountModel.BasicAccount.AccountName, accountGroup)
            {
                Description = accountModel.BasicAccount.Description,
                Note = accountModel.BasicAccount.Note
            };
            accountModel.CopyAuditInformationTo(account);
            account.SetDeletable(accountModel.Deletable);

            return account;
        }

        internal static void CreateAccountModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<AccountModel>(entity => 
            {
                entity.HasKey(e => e.AccountIdentifier);
                entity.Property(e => e.AccountIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.AccountingIdentifier).IsRequired();
                entity.Property(e => e.AccountNumber).IsRequired().IsUnicode().HasMaxLength(16);
                entity.Property(e => e.BasicAccountIdentifier).IsRequired();
                entity.Property(e => e.AccountGroupIdentifier).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
                entity.HasIndex(e => new {e.AccountingIdentifier, e.AccountNumber}).IsUnique();
                entity.HasIndex(e => e.BasicAccountIdentifier).IsUnique();
                entity.HasIndex(e => new {e.AccountGroupIdentifier, e.AccountingIdentifier, e.AccountNumber}).IsUnique();
                entity.HasOne(e => e.Accounting)
                    .WithMany(e => e.Accounts)
                    .HasForeignKey(e => e.AccountingIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BasicAccount)
                    .WithMany(e => e.Accounts)
                    .HasForeignKey(e => e.BasicAccountIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.AccountGroup)
                    .WithMany(e => e.Accounts)
                    .HasForeignKey(e => e.AccountGroupIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}