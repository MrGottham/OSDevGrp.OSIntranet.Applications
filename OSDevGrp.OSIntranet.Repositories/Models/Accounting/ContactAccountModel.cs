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
    internal class ContactAccountModel : AccountModelBase
    {
        public virtual int ContactAccountIdentifier { get; set; }

        public virtual string MailAddress { get; set; }

        public virtual string PrimaryPhone { get; set; }

        public virtual string SecondaryPhone { get; set; }

        public virtual int PaymentTermIdentifier { get; set; }

        public virtual PaymentTermModel PaymentTerm { get; set; }

        protected override AuditModelBase GetLastModifiedInfoModel() => null;
    }

    internal static class ContactAccountModelExtensions
    {
        internal static bool Convertible(this ContactAccountModel contactAccountModel)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel));

            return contactAccountModel.Accounting != null &&
                   contactAccountModel.Accounting.Convertible() &&
                   contactAccountModel.BasicAccount != null &&
                   contactAccountModel.PaymentTerm != null;
        }

        internal static IContactAccount ToDomain(this ContactAccountModel contactAccountModel, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            lock (mapperCache.SyncRoot)
            {
                IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(contactAccountModel.Accounting);

                return contactAccountModel.ToDomain(accounting, mapperCache, accountingModelConverter);
            }
        }

        internal static IContactAccount ToDomain(this ContactAccountModel contactAccountModel, IAccounting accounting, MapperCache mapperCache, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel))
                .NotNull(accounting, nameof(accounting))
                .NotNull(mapperCache, nameof(mapperCache))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            lock (mapperCache.SyncRoot)
            {
                IContactAccount contactAccount = contactAccountModel.Resolve(mapperCache.ContactAccountDictionary);
                if (contactAccount != null)
                {
                    return contactAccount;
                }

                IPaymentTerm paymentTerm = accountingModelConverter.Convert<PaymentTermModel, IPaymentTerm>(contactAccountModel.PaymentTerm);

                contactAccount = new ContactAccount(accounting, contactAccountModel.AccountNumber, contactAccountModel.BasicAccount.AccountName, paymentTerm)
                {
                    Description = contactAccountModel.BasicAccount.Description,
                    Note = contactAccountModel.BasicAccount.Note,
                    MailAddress = contactAccountModel.MailAddress,
                    PrimaryPhone = contactAccountModel.PrimaryPhone,
                    SecondaryPhone = contactAccountModel.SecondaryPhone
                };
                contactAccountModel.CopyAuditInformationTo(contactAccount);
                contactAccount.SetDeletable(contactAccountModel.Deletable);

                mapperCache.ContactAccountDictionary.Add(contactAccountModel.ContactAccountIdentifier, contactAccount);

                accounting.ContactAccountCollection.Add(contactAccount);

                contactAccount.ContactInfoCollection.Populate(contactAccount, contactAccountModel.StatusDate, contactAccountModel.StatusDateForInfos);

                if (contactAccountModel.PostingLines != null)
                {
                    contactAccount.PostingLineCollection.Add(contactAccountModel.PostingLines
                        .Where(postingLineModel => postingLineModel.Convertible() && 
                            postingLineModel.PostingDate >= contactAccountModel.GetFromDateForPostingLines() &&
                            postingLineModel.PostingDate < contactAccountModel.GetToDateForPostingLines(1))
                        .Select(postingLineModel => postingLineModel.ToDomain(accounting, contactAccount, mapperCache, accountingModelConverter))
                        .Where(postingLine => contactAccount.PostingLineCollection.Contains(postingLine) == false)
                        .ToArray());
                }

                return contactAccount;
            }
        }

        internal static IContactAccount Resolve(this ContactAccountModel contactAccountModel, IDictionary<int, IContactAccount> contactAccountDictionary)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel))
                .NotNull(contactAccountDictionary, nameof(contactAccountDictionary));

            return contactAccountDictionary.TryGetValue(contactAccountModel.ContactAccountIdentifier, out IContactAccount contactAccount)
                ? contactAccount
                : null;
        }

        internal static void ExtractPostingLines(this ContactAccountModel contactAccountModel, IReadOnlyCollection<PostingLineModel> postingLineModelCollection)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel))
                .NotNull(postingLineModelCollection, nameof(postingLineModelCollection));

            contactAccountModel.PostingLines = postingLineModelCollection
                .Where(postingLineModel => postingLineModel.ContactAccountIdentifier != null &&
                                           postingLineModel.ContactAccountIdentifier.Value == contactAccountModel.ContactAccountIdentifier &&
                                           postingLineModel.PostingDate >= contactAccountModel.GetFromDateForPostingLines() &&
                                           postingLineModel.PostingDate < contactAccountModel.GetToDateForPostingLines(1))
                .ToList();
        }

        internal static void CreateContactAccountModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<ContactAccountModel>(entity => 
            {
                entity.HasKey(e => e.ContactAccountIdentifier);
                entity.Property(e => e.ContactAccountIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.AccountingIdentifier).IsRequired();
                entity.Property(e => e.AccountNumber).IsRequired().IsUnicode().HasMaxLength(16);
                entity.Property(e => e.BasicAccountIdentifier).IsRequired();
                entity.Property(e => e.PaymentTermIdentifier).IsRequired();
                entity.Property(e => e.MailAddress).IsRequired(false).IsUnicode().HasMaxLength(256);
                entity.Property(e => e.PrimaryPhone).IsRequired(false).IsUnicode().HasMaxLength(32);
                entity.Property(e => e.SecondaryPhone).IsRequired(false).IsUnicode().HasMaxLength(32);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.StatusDate);
                entity.Ignore(e => e.Deletable);
                entity.HasIndex(e => new {e.AccountingIdentifier, e.AccountNumber}).IsUnique();
                entity.HasIndex(e => e.BasicAccountIdentifier).IsUnique();
                entity.HasIndex(e => new {e.PaymentTermIdentifier, e.AccountingIdentifier, e.AccountNumber}).IsUnique();
                entity.HasOne(e => e.Accounting)
                    .WithMany(e => e.ContactAccounts)
                    .HasForeignKey(e => e.AccountingIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BasicAccount)
                    .WithMany(e => e.ContactAccounts)
                    .HasForeignKey(e => e.BasicAccountIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.PaymentTerm)
                    .WithMany(e => e.ContactAccounts)
                    .HasForeignKey(e => e.PaymentTermIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}