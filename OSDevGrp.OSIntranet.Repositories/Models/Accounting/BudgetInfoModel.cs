using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetInfoModel : InfoModelBase
    {
        public virtual int BudgetInfoIdentifier { get; set; }

        public virtual int BudgetAccountIdentifier { get; set; }

        public virtual BudgetAccountModel BudgetAccount { get; set; }

        public virtual decimal Income { get; set; }

        public virtual decimal Expenses { get; set; }
    }

    internal static class BudgetInfoModelExtensions
    {
        internal static bool Convertible(this BudgetInfoModel budgetInfoModel)
        {
            NullGuard.NotNull(budgetInfoModel, nameof(budgetInfoModel));

            return budgetInfoModel.YearMonth != null &&
                   budgetInfoModel.BudgetAccount != null &&
                   budgetInfoModel.BudgetAccount.Convertible();
        }

        internal static IBudgetInfo ToDomain(this BudgetInfoModel budgetInfoModel, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(budgetInfoModel, nameof(budgetInfoModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IBudgetAccount budgetAccount = accountingModelConverter.Convert<BudgetAccountModel, IBudgetAccount>(budgetInfoModel.BudgetAccount);

            return budgetInfoModel.ToDomain(budgetAccount);
        }

        internal static IBudgetInfo ToDomain(this BudgetInfoModel budgetInfoModel, IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetInfoModel, nameof(budgetInfoModel))
                .NotNull(budgetAccount, nameof(budgetAccount));

            IBudgetInfo budgetInfo = new BudgetInfo(budgetAccount, budgetInfoModel.YearMonth.Year, budgetInfoModel.YearMonth.Month, budgetInfoModel.Income, budgetInfoModel.Expenses);
            budgetInfoModel.CopyAuditInformationTo(budgetInfo);
            budgetInfo.SetDeletable(budgetInfoModel.Deletable);

            return budgetInfo;
        }

        internal static void CreateBudgetInfoModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<BudgetInfoModel>(entity =>
            {
                entity.HasKey(e => e.BudgetInfoIdentifier);
                entity.Property(e => e.BudgetInfoIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.BudgetAccountIdentifier).IsRequired();
                entity.Property(e => e.YearMonthIdentifier).IsRequired();
                entity.Property(e => e.Income).IsRequired();
                entity.Property(e => e.Expenses).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
                entity.HasIndex(e => new {e.BudgetAccountIdentifier, e.YearMonthIdentifier}).IsUnique();
                entity.HasOne(e => e.BudgetAccount)
                    .WithMany(e => e.BudgetInfos)
                    .HasForeignKey(e => e.BudgetAccountIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.YearMonth)
                    .WithMany(e => e.BudgetInfos)
                    .HasForeignKey(e => e.YearMonthIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}