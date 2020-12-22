using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Converters.Extensions;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class BudgetAccountModel : AccountModelBase
    {
        public virtual int BudgetAccountIdentifier { get; set; }

        public virtual int BudgetAccountGroupIdentifier { get; set; }

        public virtual BudgetAccountGroupModel BudgetAccountGroup { get; set; }

        public virtual List<BudgetInfoModel> BudgetInfos { get; set; }

        protected override AuditModelBase GetLastModifiedInfoModel()
        {
            return BudgetInfos?.AsParallel()
                .OrderByDescending(budgetInfoModel => budgetInfoModel.ModifiedUtcDateTime)
                .FirstOrDefault();
        }
    }

    internal static class BudgetAccountModelExtensions
    {
        internal static IBudgetAccount ToDomain(this BudgetAccountModel budgetAccountModel, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IBudgetAccount budgetAccount;
            lock (accountingModelConverter.Cache.SyncRoot)
            {
                budgetAccount = accountingModelConverter.Cache.FromMemory<IBudgetAccount>($"{budgetAccountModel.AccountNumber}@{budgetAccountModel.AccountingIdentifier}");
                if (budgetAccount != null)
                {
                    return budgetAccount;
                }

                IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(budgetAccountModel.Accounting);
                IBudgetAccountGroup budgetAccountGroup = accountingModelConverter.Convert<BudgetAccountGroupModel, IBudgetAccountGroup>(budgetAccountModel.BudgetAccountGroup);

                budgetAccount = new BudgetAccount(accounting, budgetAccountModel.AccountNumber, budgetAccountModel.BasicAccount.AccountName, budgetAccountGroup)
                {
                    Description = budgetAccountModel.BasicAccount.Description,
                    Note = budgetAccountModel.BasicAccount.Note
                };
                budgetAccountModel.CopyAuditInformationTo(budgetAccount);
                budgetAccount.SetDeletable(budgetAccountModel.Deletable);

                accountingModelConverter.Cache.Remember(budgetAccount, m => $"{m.AccountNumber}@{m.Accounting.Number}");
            }

            try
            {
                if (budgetAccountModel.BudgetInfos != null)
                {
                    budgetAccount.BudgetInfoCollection.Populate(budgetAccount,
                        budgetAccountModel.BudgetInfos
                            .Where(budgetInfoModel => budgetInfoModel.BudgetAccount?.Accounting != null &&
                                                      budgetInfoModel.BudgetAccount.BasicAccount != null &&
                                                      budgetInfoModel.BudgetAccount.BudgetAccountGroup != null &&
                                                      budgetInfoModel.YearMonth != null &&
                                                      (budgetInfoModel.YearMonth.Year < budgetAccountModel.StatusDateForInfos.Year ||
                                                       budgetInfoModel.YearMonth.Year == budgetAccountModel.StatusDateForInfos.Year &&
                                                       budgetInfoModel.YearMonth.Month <= budgetAccountModel.StatusDateForInfos.Month))
                            .Select(accountingModelConverter.Convert<BudgetInfoModel, IBudgetInfo>)
                            .ToArray(),
                        budgetAccountModel.StatusDate,
                        budgetAccountModel.StatusDateForInfos);
                }

                return budgetAccount;
            }
            finally
            {
                accountingModelConverter.Cache.Forget<IBudgetAccount>($"{budgetAccountModel.AccountNumber}@{budgetAccountModel.AccountingIdentifier}");
            }
        }

        internal static void ExtractBudgetInfos(this BudgetAccountModel budgetAccountModel, BudgetInfoModel[] budgetInfoModelCollection)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(budgetInfoModelCollection, nameof(budgetInfoModelCollection));

            budgetAccountModel.BudgetInfos = budgetInfoModelCollection.Where(budgetInfoModel => budgetInfoModel.BudgetAccountIdentifier == budgetAccountModel.BudgetAccountIdentifier).ToList();
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
                entity.Ignore(e => e.StatusDate);
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