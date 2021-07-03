using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class AccountingModel : AuditModelBase
    {
        public virtual int AccountingIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual int LetterHeadIdentifier { get; set; }

        public virtual LetterHeadModel LetterHead { get; set; }

        public virtual BalanceBelowZeroType BalanceBelowZero { get; set; }

        public virtual int BackDating { get; set; }

        public virtual bool Deletable { get; set; }

        public virtual List<AccountModel> Accounts { get; set; }

        public virtual List<BudgetAccountModel> BudgetAccounts { get; set; }

        public virtual List<ContactAccountModel> ContactAccounts { get; set; }

        public virtual List<PostingLineModel> PostingLines { get; set; }
    }

    internal static class AccountingModelExtensions
    {
        internal static bool Convertible(this AccountingModel accountingModel)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel));

            return accountingModel.LetterHead != null;
        }

        internal static IAccounting ToDomain(this AccountingModel accountingModel, IDictionary<int, IAccounting> accountingDictionary, IConverter accountingModelConverter, IConverter commonModelConverter, object syncRoot)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel))
                .NotNull(accountingDictionary, nameof(accountingDictionary))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter))
                .NotNull(commonModelConverter, nameof(commonModelConverter))
                .NotNull(syncRoot, nameof(syncRoot));

            lock (syncRoot)
            {
                if (accountingDictionary.TryGetValue(accountingModel.AccountingIdentifier, out IAccounting accounting))
                {
                    return accounting;
                }

                ILetterHead letterHead = commonModelConverter.Convert<LetterHeadModel, ILetterHead>(accountingModel.LetterHead);

                accounting = new Domain.Accounting.Accounting(accountingModel.AccountingIdentifier, accountingModel.Name, letterHead, accountingModel.BalanceBelowZero, accountingModel.BackDating);
                accounting.AddAuditInformation(accountingModel.CreatedUtcDateTime, accountingModel.CreatedByIdentifier, accountingModel.ModifiedUtcDateTime, accountingModel.ModifiedByIdentifier);
                accounting.SetDeletable(accountingModel.Deletable);

                accountingDictionary.Add(accounting.Number, accounting);

                if (accountingModel.Accounts != null)
                {
                    accounting.AccountCollection.Add(accountingModel.Accounts
                        .Where(accountModel => accountModel.Convertible())
                        .Select(accountModel => accountModel.ToDomain(accounting, accountingModelConverter))
                        .Where(account => accounting.AccountCollection.Contains(account) == false)
                        .ToArray());
                }

                if (accountingModel.BudgetAccounts != null)
                {
                    accounting.BudgetAccountCollection.Add(accountingModel.BudgetAccounts
                        .Where(budgetAccountModel => budgetAccountModel.Convertible())
                        .Select(budgetAccountModel => budgetAccountModel.ToDomain(accounting, accountingModelConverter))
                        .Where(budgetAccount => accounting.BudgetAccountCollection.Contains(budgetAccount) == false)
                        .ToArray());
                }

                if (accountingModel.ContactAccounts != null)
                {
                    accounting.ContactAccountCollection.Add(accountingModel.ContactAccounts
                        .Where(contactAccountModel => contactAccountModel.Convertible())
                        .Select(contactAccountModel => contactAccountModel.ToDomain(accounting, accountingModelConverter))
                        .Where(contactAccount => accounting.ContactAccountCollection.Contains(contactAccount) == false)
                        .ToArray());
                }

                return accounting;
            }
        }

        internal static void ExtractAccounts(this AccountingModel accountingModel, IReadOnlyCollection<AccountModel> accountModelCollection)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel))
                .NotNull(accountModelCollection, nameof(accountModelCollection));

            accountingModel.Accounts = accountModelCollection.Where(accountModel => accountModel.AccountingIdentifier == accountingModel.AccountingIdentifier).ToList();
        }

        internal static void ExtractBudgetAccounts(this AccountingModel accountingModel, IReadOnlyCollection<BudgetAccountModel> budgetAccountModelCollection)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel))
                .NotNull(budgetAccountModelCollection, nameof(budgetAccountModelCollection));

            accountingModel.BudgetAccounts = budgetAccountModelCollection.Where(budgetAccountModel => budgetAccountModel.AccountingIdentifier == accountingModel.AccountingIdentifier).ToList();
        }

        internal static void ExtractContactAccounts(this AccountingModel accountingModel, IReadOnlyCollection<ContactAccountModel> contactAccountModelCollection)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel))
                .NotNull(contactAccountModelCollection, nameof(contactAccountModelCollection));

            accountingModel.ContactAccounts = contactAccountModelCollection.Where(contactAccountModel => contactAccountModel.AccountingIdentifier == accountingModel.AccountingIdentifier).ToList();
        }

        internal static void ExtractPostingLines(this AccountingModel accountingModel, IReadOnlyCollection<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel))
                .NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            accountingModel.PostingLines = postingLineModelCollection.Where(postingLineModel => postingLineModel.AccountingIdentifier == accountingModel.AccountingIdentifier).ToList();
        }

        internal static void CreateAccountingModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<AccountingModel>(entity =>
            {
                entity.HasKey(e => e.AccountingIdentifier);
                entity.Property(e => e.AccountingIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.LetterHeadIdentifier).IsRequired();
                entity.Property(e => e.BalanceBelowZero).IsRequired();
                entity.Property(e => e.BackDating).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
                entity.HasIndex(e => new {e.LetterHeadIdentifier, e.AccountingIdentifier}).IsUnique();
                entity.HasOne(e => e.LetterHead)
                    .WithMany(e => e.Accountings)
                    .HasForeignKey(e => e.LetterHeadIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}