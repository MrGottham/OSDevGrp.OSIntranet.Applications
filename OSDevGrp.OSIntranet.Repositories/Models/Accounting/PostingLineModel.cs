using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal class PostingLineModel : AuditModelBase
    {
        public virtual int PostingLineIdentifier { get; set; }

        public virtual string PostingLineIdentification { get; set; }

        public virtual int AccountingIdentifier { get; set; }

        public virtual AccountingModel Accounting { get; set; }

        public virtual DateTime PostingDate { get; set; }

        public virtual string Reference { get; set; }

        public virtual int AccountIdentifier { get; set; }

        public virtual AccountModel Account { get; set; }

        public virtual string Details { get; set; }

        public virtual int? BudgetAccountIdentifier { get; set; }

        public virtual BudgetAccountModel BudgetAccount { get; set; }

        public virtual decimal? Debit { get; set; }

        public virtual decimal? Credit { get; set; }

        public virtual int? ContactAccountIdentifier { get; set; }

        public virtual ContactAccountModel ContactAccount { get; set; }
    }

    internal static class PostingLineModelExtensions
    {
        internal static bool Convertible(this PostingLineModel postingLineModel)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel));

            return postingLineModel.Accounting != null &&
                   postingLineModel.Accounting.Convertible() &&
                   postingLineModel.Account != null &&
                   postingLineModel.Account.Convertible() &&
                   (postingLineModel.BudgetAccount == null || postingLineModel.BudgetAccount.Convertible()) &&
                   (postingLineModel.ContactAccount == null || postingLineModel.ContactAccount.Convertible());
        }

        internal static IPostingLine ToDomain(this PostingLineModel postingLineModel, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            lock (mapperCache.SyncRoot)
            {
                IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(postingLineModel.Accounting);

                return postingLineModel.ToDomain(accounting, mapperCache, accountingModelConverter);
            }
        }

        internal static IPostingLine ToDomain(this PostingLineModel postingLineModel, IAccounting accounting, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel))
                .NotNull(accounting, nameof(accounting))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccount account = ResolveAccount(postingLineModel.Account, accounting, mapperCache, accountingModelConverter);
            IBudgetAccount budgetAccount = ResolveBudgetAccount(postingLineModel.BudgetAccount, accounting, mapperCache, accountingModelConverter);
            IContactAccount contactAccount = ResolveContactAccount(postingLineModel.ContactAccount, accounting, mapperCache, accountingModelConverter);

            return postingLineModel.ToDomain(account, budgetAccount, contactAccount, mapperCache);
        }

        internal static IPostingLine ToDomain(this PostingLineModel postingLineModel, IAccounting accounting, IAccount account, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel))
                .NotNull(accounting, nameof(accounting))
                .NotNull(account, nameof(account))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IBudgetAccount budgetAccount = ResolveBudgetAccount(postingLineModel.BudgetAccount, accounting, mapperCache, accountingModelConverter);
            IContactAccount contactAccount = ResolveContactAccount(postingLineModel.ContactAccount, accounting, mapperCache, accountingModelConverter);

            return postingLineModel.ToDomain(account, budgetAccount, contactAccount, mapperCache);
        }

        internal static IPostingLine ToDomain(this PostingLineModel postingLineModel, IAccounting accounting, IBudgetAccount budgetAccount, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel))
                .NotNull(accounting, nameof(accounting))
                .NotNull(budgetAccount, nameof(budgetAccount))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccount account = ResolveAccount(postingLineModel.Account, accounting, mapperCache, accountingModelConverter);
            IContactAccount contactAccount = ResolveContactAccount(postingLineModel.ContactAccount, accounting, mapperCache, accountingModelConverter);

            return postingLineModel.ToDomain(account, budgetAccount, contactAccount, mapperCache);
        }

        internal static IPostingLine ToDomain(this PostingLineModel postingLineModel, IAccounting accounting, IContactAccount contactAccount, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel))
                .NotNull(accounting, nameof(accounting))
                .NotNull(contactAccount, nameof(contactAccount))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccount account = ResolveAccount(postingLineModel.Account, accounting, mapperCache, accountingModelConverter);
            IBudgetAccount budgetAccount = ResolveBudgetAccount(postingLineModel.BudgetAccount, accounting, mapperCache, accountingModelConverter);

            return postingLineModel.ToDomain(account, budgetAccount, contactAccount, mapperCache);
        }

        internal static IPostingLine ToDomain(this PostingLineModel postingLineModel, IAccount account, IBudgetAccount budgetAccount, IContactAccount contactAccount, MapperCache mapperCache)
        {
            NullGuard.NotNull(postingLineModel, nameof(postingLineModel))
                .NotNull(account, nameof(account))
                .NotNull(mapperCache, nameof(mapperCache));

            Guid postingLineIdentification = Guid.Parse(postingLineModel.PostingLineIdentification);
            lock (mapperCache.SyncRoot)
            {
                if (mapperCache.PostingLineDictionary.TryGetValue(postingLineIdentification, out IPostingLine postingLine))
                {
                    return postingLine;
                }

                postingLine = new PostingLine(postingLineIdentification, postingLineModel.PostingDate, postingLineModel.Reference, account, postingLineModel.Details, budgetAccount, postingLineModel.Debit ?? 0M, postingLineModel.Credit ?? 0M, contactAccount, postingLineModel.PostingLineIdentifier);
                postingLine.AddAuditInformation(postingLineModel.CreatedUtcDateTime, postingLineModel.CreatedByIdentifier, postingLineModel.ModifiedUtcDateTime, postingLineModel.ModifiedByIdentifier);

                mapperCache.PostingLineDictionary.Add(postingLineIdentification, postingLine);

                if (account.PostingLineCollection.Contains(postingLine) == false)
                {
                    account.PostingLineCollection.Add(postingLine);
                }

                if (budgetAccount != null && budgetAccount.PostingLineCollection.Contains(postingLine) == false)
                {
                    budgetAccount.PostingLineCollection.Add(postingLine);
                }

                if (contactAccount != null && contactAccount.PostingLineCollection.Contains(postingLine) == false)
                {
                    contactAccount.PostingLineCollection.Add(postingLine);
                }

                return postingLine;
            }
        }

        internal static void CreatePostingLineModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<PostingLineModel>(entity =>
            {
                entity.HasKey(e => e.PostingLineIdentifier);
                entity.Property(e => e.PostingLineIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.PostingLineIdentification).IsRequired();
                entity.Property(e => e.AccountingIdentifier).IsRequired();
                entity.Property(e => e.PostingDate).IsRequired();
                entity.Property(e => e.Reference).IsRequired(false).IsUnicode().HasMaxLength(16);
                entity.Property(e => e.AccountIdentifier).IsRequired();
                entity.Property(e => e.Details).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.BudgetAccountIdentifier).IsRequired(false);
                entity.Property(e => e.Debit).IsRequired(false);
                entity.Property(e => e.Credit).IsRequired(false);
                entity.Property(e => e.ContactAccountIdentifier).IsRequired(false);
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => e.PostingLineIdentification).IsUnique();
                entity.HasIndex(e => new {e.AccountingIdentifier, e.PostingDate, e.PostingLineIdentifier}).IsUnique();
                entity.HasIndex(e => new {e.PostingDate, e.PostingLineIdentifier}).IsUnique();
                entity.HasIndex(e => new {e.AccountIdentifier, e.PostingDate, e.PostingLineIdentifier}).IsUnique();
                entity.HasIndex(e => new {e.BudgetAccountIdentifier, e.PostingDate, e.PostingLineIdentifier}).IsUnique();
                entity.HasIndex(e => new {e.ContactAccountIdentifier, e.PostingDate, e.PostingLineIdentifier}).IsUnique();
                entity.HasOne(e => e.Accounting)
                    .WithMany(e => e.PostingLines)
                    .HasForeignKey(e => e.AccountingIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Account)
                    .WithMany(e => e.PostingLines)
                    .HasForeignKey(e => e.AccountIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BudgetAccount)
                    .WithMany(e => e.PostingLines)
                    .HasForeignKey(e => e.BudgetAccountIdentifier)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.ContactAccount)
                    .WithMany(e => e.PostingLines)
                    .HasForeignKey(e => e.ContactAccountIdentifier)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

        private static IAccount ResolveAccount(AccountModel accountModel, IAccounting accounting, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel))
                .NotNull(accounting, nameof(accounting))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccount account = accountModel.Resolve(mapperCache.AccountDictionary) ?? accountModel.ToDomain(accounting, mapperCache, accountingModelConverter);
            if (accounting.AccountCollection.Contains(account) == false)
            {
                accounting.AccountCollection.Add(account);
            }

            return account;
        }

        private static IBudgetAccount ResolveBudgetAccount(BudgetAccountModel budgetAccountModel, IAccounting accounting, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            if (budgetAccountModel == null)
            {
                return null;
            }

            IBudgetAccount budgetAccount = budgetAccountModel.Resolve(mapperCache.BudgetAccountDictionary) ?? budgetAccountModel.ToDomain(accounting, mapperCache, accountingModelConverter);
            if (accounting.BudgetAccountCollection.Contains(budgetAccount) == false)
            {
                accounting.BudgetAccountCollection.Add(budgetAccount);
            }

            return budgetAccount;
        }

        private static IContactAccount ResolveContactAccount(ContactAccountModel contactAccountModel, IAccounting accounting, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            if (contactAccountModel == null)
            {
                return null;
            }

            IContactAccount contactAccount = contactAccountModel.Resolve(mapperCache.ContactAccountDictionary) ?? contactAccountModel.ToDomain(accounting, mapperCache, accountingModelConverter);
            if (accounting.ContactAccountCollection.Contains(contactAccount) == false)
            {
                accounting.ContactAccountCollection.Add(contactAccount);
            }

            return contactAccount;
        }
    }
}