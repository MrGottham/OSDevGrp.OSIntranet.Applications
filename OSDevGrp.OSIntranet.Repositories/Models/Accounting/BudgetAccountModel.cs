using System;
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

        internal override DateTime GetFromDateForPostingLines() => new DateTime(StatusDate.AddYears(-1).Year, 1, 1);

        protected override AuditModelBase GetLastModifiedInfoModel()
        {
            return BudgetInfos?.AsParallel()
                .OrderByDescending(budgetInfoModel => budgetInfoModel.ModifiedUtcDateTime)
                .FirstOrDefault();
        }
    }

    internal static class BudgetAccountModelExtensions
    {
        internal static bool Convertible(this BudgetAccountModel budgetAccountModel)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel));

            return budgetAccountModel.Accounting != null && 
                   budgetAccountModel.Accounting.Convertible() &&
                   budgetAccountModel.BasicAccount != null &&
                   budgetAccountModel.BudgetAccountGroup != null;
        }

        internal static IBudgetAccount ToDomain(this BudgetAccountModel budgetAccountModel, IConverter accountingModelConverter, object syncRoot)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter))
                .NotNull(syncRoot, nameof(syncRoot));

            lock (syncRoot)
            {
                IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(budgetAccountModel.Accounting);

                return budgetAccountModel.ToDomain(accounting, accountingModelConverter);
            }
        }

        internal static IBudgetAccount ToDomain(this BudgetAccountModel budgetAccountModel, IAccounting accounting, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(accounting, nameof(accounting))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IBudgetAccount budgetAccount = budgetAccountModel.ResolveFromDomain(accounting.BudgetAccountCollection);
            if (budgetAccount != null)
            {
                return budgetAccount;
            }

            IBudgetAccountGroup budgetAccountGroup = accountingModelConverter.Convert<BudgetAccountGroupModel, IBudgetAccountGroup>(budgetAccountModel.BudgetAccountGroup);

            budgetAccount = new BudgetAccount(accounting, budgetAccountModel.AccountNumber, budgetAccountModel.BasicAccount.AccountName, budgetAccountGroup)
            {
                Description = budgetAccountModel.BasicAccount.Description,
                Note = budgetAccountModel.BasicAccount.Note
            };
            budgetAccountModel.CopyAuditInformationTo(budgetAccount);
            budgetAccount.SetDeletable(budgetAccountModel.Deletable);

            accounting.BudgetAccountCollection.Add(budgetAccount);

            if (budgetAccountModel.BudgetInfos != null)
            {
                budgetAccount.BudgetInfoCollection.Populate(budgetAccount,
                    budgetAccountModel.BudgetInfos
                        .Where(budgetInfoModel => budgetInfoModel.Convertible() &&
                                                  (budgetInfoModel.YearMonth.Year < budgetAccountModel.StatusDateForInfos.Year ||
                                                   budgetInfoModel.YearMonth.Year == budgetAccountModel.StatusDateForInfos.Year &&
                                                   budgetInfoModel.YearMonth.Month <= budgetAccountModel.StatusDateForInfos.Month))
                        .Select(budgetInfoModel => budgetInfoModel.ToDomain(budgetAccount))
                        .ToArray(),
                    budgetAccountModel.StatusDate,
                    budgetAccountModel.StatusDateForInfos);
            }

            if (budgetAccountModel.PostingLines != null)
            {
                budgetAccount.PostingLineCollection.Add(budgetAccountModel.PostingLines
                    .Where(postingLineModel => postingLineModel.Convertible() &&
                                               postingLineModel.PostingDate >= budgetAccountModel.GetFromDateForPostingLines() &&
                                               postingLineModel.PostingDate < budgetAccountModel.GetToDateForPostingLines(1))
                    .Select(postingLineModel => postingLineModel.ToDomain(accounting, budgetAccount, accountingModelConverter))
                    .Where(postingLine => budgetAccount.PostingLineCollection.Contains(postingLine) == false)
                    .ToArray());
            }

            return budgetAccount;
        }

        internal static IBudgetAccount ResolveFromDomain(this BudgetAccountModel budgetAccountModel, IBudgetAccountCollection budgetAccountCollection)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(budgetAccountCollection, nameof(budgetAccountCollection));

            return budgetAccountCollection.SingleOrDefault(budgetAccount => budgetAccount.Accounting.Number == budgetAccountModel.AccountingIdentifier && budgetAccount.AccountNumber == budgetAccountModel.AccountNumber);
        }

        internal static void ExtractBudgetInfos(this BudgetAccountModel budgetAccountModel, IReadOnlyCollection<BudgetInfoModel> budgetInfoModelCollection)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(budgetInfoModelCollection, nameof(budgetInfoModelCollection));

            budgetAccountModel.BudgetInfos = budgetInfoModelCollection.Where(budgetInfoModel => budgetInfoModel.BudgetAccountIdentifier == budgetAccountModel.BudgetAccountIdentifier).ToList();
        }

        internal static void ExtractPostingLines(this BudgetAccountModel budgetAccountModel, IReadOnlyCollection<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(budgetAccountModel, nameof(budgetAccountModel))
                .NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            budgetAccountModel.PostingLines = postingLineModelCollection
                .Where(postingLineModel => postingLineModel.BudgetAccountIdentifier != null &&
                                           postingLineModel.BudgetAccountIdentifier.Value == budgetAccountModel.BudgetAccountIdentifier &&
                                           postingLineModel.PostingDate >= budgetAccountModel.GetFromDateForPostingLines() &&
                                           postingLineModel.PostingDate < budgetAccountModel.GetToDateForPostingLines(1))
                .ToList();
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