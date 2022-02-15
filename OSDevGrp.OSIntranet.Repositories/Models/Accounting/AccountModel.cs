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
        internal static bool Convertible(this AccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            return accountModel.Accounting != null &&
                   accountModel.Accounting.Convertible() &&
                   accountModel.BasicAccount != null &&
                   accountModel.AccountGroup != null;
        }

        internal static IAccount ToDomain(this AccountModel accountModel, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            lock (mapperCache.SyncRoot)
            {
                IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(accountModel.Accounting);

                return accountModel.ToDomain(accounting, mapperCache, accountingModelConverter);
            }
        }

        internal static IAccount ToDomain(this AccountModel accountModel, IAccounting accounting, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(accounting, nameof(accounting))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            lock (mapperCache.SyncRoot)
            {
                IAccount account = accountModel.Resolve(mapperCache.AccountDictionary);
                if (account != null)
                {
                    return account;
                }

                IAccountGroup accountGroup = accountingModelConverter.Convert<AccountGroupModel, IAccountGroup>(accountModel.AccountGroup);

                account = new Account(accounting, accountModel.AccountNumber, accountModel.BasicAccount.AccountName, accountGroup)
                {
                    Description = accountModel.BasicAccount.Description,
                    Note = accountModel.BasicAccount.Note
                };
                accountModel.CopyAuditInformationTo(account);
                account.SetDeletable(accountModel.Deletable);

                mapperCache.AccountDictionary.Add(accountModel.AccountIdentifier, account);

                accounting.AccountCollection.Add(account);

                if (accountModel.CreditInfos != null)
                {
                    account.CreditInfoCollection.Populate(account,
                        accountModel.CreditInfos
                            .Where(creditInfoModel => creditInfoModel.Convertible() &&  
                                (creditInfoModel.YearMonth.Year < accountModel.StatusDateForInfos.Year ||
                                creditInfoModel.YearMonth.Year == accountModel.StatusDateForInfos.Year &&
                                creditInfoModel.YearMonth.Month <= accountModel.StatusDateForInfos.Month))
                            .Select(creditInfoModel => creditInfoModel.ToDomain(account))
                            .ToArray(),
                        accountModel.StatusDate,
                        accountModel.StatusDateForInfos);
                }

                if (accountModel.PostingLines != null)
                {
                    account.PostingLineCollection.Add(accountModel.PostingLines
                        .Where(postingLineModel => postingLineModel.Convertible() && 
                            postingLineModel.PostingDate >= accountModel.GetFromDateForPostingLines() &&
                            postingLineModel.PostingDate < accountModel.GetToDateForPostingLines(1))
                        .Select(postingLineModel => postingLineModel.ToDomain(accounting, account, mapperCache, accountingModelConverter))
                        .Where(postingLine => account.PostingLineCollection.Contains(postingLine) == false)
                        .ToArray());
                }

                return account;
            }
        }

        internal static IAccount Resolve(this AccountModel accountModel, IDictionary<int, IAccount> accountDictionary)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(accountDictionary, nameof(accountDictionary));

            return accountDictionary.TryGetValue(accountModel.AccountIdentifier, out IAccount account)
                ? account
                : null;
        }

        internal static void ExtractCreditInfos(this AccountModel accountModel, IReadOnlyCollection<CreditInfoModel> creditInfoModelCollection)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(creditInfoModelCollection, nameof(creditInfoModelCollection));

            accountModel.CreditInfos = creditInfoModelCollection.Where(creditInfoModel => creditInfoModel.AccountIdentifier == accountModel.AccountIdentifier).ToList();
        }

        internal static void ExtractPostingLines(this AccountModel accountModel, IReadOnlyCollection<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            accountModel.PostingLines = postingLineModelCollection
                .Where(postingLineModel => postingLineModel.AccountIdentifier == accountModel.AccountIdentifier &&
                                           postingLineModel.PostingDate >= accountModel.GetFromDateForPostingLines() &&
                                           postingLineModel.PostingDate < accountModel.GetToDateForPostingLines(1))
                .ToList();
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