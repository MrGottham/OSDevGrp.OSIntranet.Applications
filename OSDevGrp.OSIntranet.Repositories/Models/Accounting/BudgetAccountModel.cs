using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetAccountModel : AccountModelBase
    {
        public virtual int BudgetAccountIdentifier { get; set; }

        public virtual int BudgetAccountGroupIdentifier { get; set; }

        public virtual BudgetAccountGroupModel BudgetAccountGroup { get; set; }
    }

    internal static class BudgetAccountModelExtensions
    {
        internal static IBudgetAccount ToDomain(this BudgetAccountModel budgetAccountModel, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(budgetAccountModel.Accounting);
            IBudgetAccountGroup budgetAccountGroup = accountingModelConverter.Convert<BudgetAccountGroupModel, IBudgetAccountGroup>(budgetAccountModel.BudgetAccountGroup);

            IBudgetAccount budgetAccount = new BudgetAccount(accounting, budgetAccountModel.AccountNumber, budgetAccountModel.BasicAccount.AccountName, budgetAccountGroup)
            {
                Description = budgetAccountModel.BasicAccount.Description,
                Note = budgetAccountModel.BasicAccount.Note
            };
            budgetAccountModel.CopyAuditInformationTo(budgetAccount);
            budgetAccount.SetDeletable(budgetAccountModel.Deletable);

            return budgetAccount;
        }

        internal static void CreateBudgetAccountModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<BudgetAccountModel>(entity => 
            {
                entity.HasKey(e => e.BudgetAccountIdentifier);
                entity.Property(e => e.BudgetAccountIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.AccountingIdentifier).IsRequired();
                entity.Property(e => e.AccountNumber).IsRequired().IsUnicode().HasMaxLength(16);
                entity.Property(e => e.BasicAccountIdentifier).IsRequired();
                entity.Property(e => e.BudgetAccountGroupIdentifier).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
                entity.HasIndex(e => new {e.AccountingIdentifier, e.AccountNumber}).IsUnique();
                entity.HasIndex(e => e.BasicAccountIdentifier).IsUnique();
                entity.HasIndex(e => new {e.BudgetAccountGroupIdentifier, e.AccountingIdentifier, e.AccountNumber}).IsUnique();
                entity.HasOne(e => e.Accounting)
                    .WithMany(e => e.BudgetAccounts)
                    .HasForeignKey(e => e.AccountingIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BasicAccount)
                    .WithMany(e => e.BudgetAccounts)
                    .HasForeignKey(e => e.BasicAccountIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BudgetAccountGroup)
                    .WithMany(e => e.BudgetAccounts)
                    .HasForeignKey(e => e.BudgetAccountGroupIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}