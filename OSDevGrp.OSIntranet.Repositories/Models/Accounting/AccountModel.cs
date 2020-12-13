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
    internal class AccountModel : AccountModelBase
    {
        public virtual int AccountIdentifier { get; set; }

        public virtual int AccountGroupIdentifier { get; set; }

        public virtual AccountGroupModel AccountGroup { get; set; }

        public virtual List<CreditInfoModel> CreditInfos { get; set; }

        protected override AuditModelBase GetLastModifiedInfoModel()
        {
            return CreditInfos?.AsParallel()
                .OrderByDescending(creditInfoModel => creditInfoModel.ModifiedUtcDateTime)
                .FirstOrDefault();
        }
    }

    internal static class AccountModelExtensions
    {
        internal static IAccount ToDomain(this AccountModel accountModel, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccount account;
            lock (accountingModelConverter.Cache.SyncRoot)
            {
                account = accountingModelConverter.Cache.FromMemory<IAccount>($"{accountModel.AccountNumber}@{accountModel.AccountingIdentifier}");
                if (account != null)
                {
                    return account;
                }

                IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(accountModel.Accounting);
                IAccountGroup accountGroup = accountingModelConverter.Convert<AccountGroupModel, IAccountGroup>(accountModel.AccountGroup);

                account = new Account(accounting, accountModel.AccountNumber, accountModel.BasicAccount.AccountName, accountGroup)
                {
                    Description = accountModel.BasicAccount.Description,
                    Note = accountModel.BasicAccount.Note
                };
                accountModel.CopyAuditInformationTo(account);
                account.SetDeletable(accountModel.Deletable);

                accountingModelConverter.Cache.Remember(account, m => $"{m.AccountNumber}@{m.Accounting.Number}");
            }

            try
            {
                if (accountModel.CreditInfos != null)
                {
                    account.CreditInfoCollection.Populate(account,
                        accountModel.CreditInfos
                            .Where(creditInfoModel => creditInfoModel.Account?.Accounting != null &&
                                                      creditInfoModel.Account.BasicAccount != null &&
                                                      creditInfoModel.Account.AccountGroup != null &&
                                                      creditInfoModel.YearMonth != null &&
                                                      (creditInfoModel.YearMonth.Year < accountModel.StatusDateForInfos.Year ||
                                                       creditInfoModel.YearMonth.Year == accountModel.StatusDateForInfos.Year &&
                                                       creditInfoModel.YearMonth.Month <= accountModel.StatusDateForInfos.Month))
                            .Select(accountingModelConverter.Convert<CreditInfoModel, ICreditInfo>)
                            .ToArray(),
                        accountModel.StatusDate,
                        accountModel.StatusDateForInfos);
                }

                return account;
            }
            finally
            {
                accountingModelConverter.Cache.Forget<IAccount>($"{accountModel.AccountNumber}@{accountModel.AccountingIdentifier}");
            }
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
                entity.Ignore(e => e.StatusDate);
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