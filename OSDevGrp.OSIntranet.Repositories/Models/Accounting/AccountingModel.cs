using System;
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
    }

    internal static class AccountingModelExtensions
    {
        internal static IAccounting ToDomain(this AccountingModel accountingModel, IConverter accountingModelConverter, IConverter commonModelConverter)
        {
            NullGuard.NotNull(accountingModel, nameof(accountingModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter))
                .NotNull(commonModelConverter, nameof(commonModelConverter));

            IAccounting accounting;
            lock (accountingModelConverter.Cache.SyncRoot)
            {
                accounting = accountingModelConverter.Cache.FromMemory<IAccounting>(Convert.ToString(accountingModel.AccountingIdentifier));
                if (accounting != null)
                {
                    return accounting;
                }

                ILetterHead letterHead = commonModelConverter.Convert<LetterHeadModel, ILetterHead>(accountingModel.LetterHead);

                accounting = new Domain.Accounting.Accounting(accountingModel.AccountingIdentifier, accountingModel.Name, letterHead, accountingModel.BalanceBelowZero, accountingModel.BackDating);
                accounting.AddAuditInformation(accountingModel.CreatedUtcDateTime, accountingModel.CreatedByIdentifier, accountingModel.ModifiedUtcDateTime, accountingModel.ModifiedByIdentifier);
                accounting.SetDeletable(accountingModel.Deletable);

                accountingModelConverter.Cache.Remember(accounting, m => Convert.ToString(m.Number));
            }

            try
            {
                if (accountingModel.Accounts != null)
                {
                    accounting.AccountCollection.Add(accountingModel.Accounts
                        .Where(accountModel => accountModel.Accounting != null && accountModel.BasicAccount != null && accountModel.AccountGroup != null)
                        .Select(accountingModelConverter.Convert<AccountModel, IAccount>)
                        .ToArray());
                }

                if (accountingModel.BudgetAccounts != null)
                {
                    accounting.BudgetAccountCollection.Add(accountingModel.BudgetAccounts
                        .Where(budgetAccountModel => budgetAccountModel.Accounting != null && budgetAccountModel.BasicAccount != null && budgetAccountModel.BudgetAccountGroup != null)
                        .Select(accountingModelConverter.Convert<BudgetAccountModel, IBudgetAccount>)
                        .ToArray());
                }

                if (accountingModel.ContactAccounts != null)
                {
                    accounting.ContactAccountCollection.Add(accountingModel.ContactAccounts
                        .Where(contactAccountModel => contactAccountModel.Accounting != null && contactAccountModel.BasicAccount != null && contactAccountModel.PaymentTerm != null)
                        .Select(accountingModelConverter.Convert<ContactAccountModel, IContactAccount>)
                        .ToArray());
                }

                return accounting;
            }
            finally
            {
                accountingModelConverter.Cache.Forget<IAccounting>(Convert.ToString(accountingModel.AccountingIdentifier));
            }
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