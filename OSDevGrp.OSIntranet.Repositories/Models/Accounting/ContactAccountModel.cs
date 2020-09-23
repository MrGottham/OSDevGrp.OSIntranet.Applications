using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Accounting;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

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
    }

    internal static class ContactAccountModelExtensions
    {
        internal static IContactAccount ToDomain(this ContactAccountModel contactAccountModel, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(contactAccountModel, nameof(contactAccountModel))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            IAccounting accounting = accountingModelConverter.Convert<AccountingModel, IAccounting>(contactAccountModel.Accounting);
            IPaymentTerm paymentTerm = accountingModelConverter.Convert<PaymentTermModel, IPaymentTerm>(contactAccountModel.PaymentTerm);

            IContactAccount contactAccount = new ContactAccount(accounting, contactAccountModel.AccountNumber, contactAccountModel.BasicAccount.AccountName, paymentTerm)
            {
                Description = contactAccountModel.BasicAccount.Description,
                Note = contactAccountModel.BasicAccount.Note,
                MailAddress = contactAccountModel.MailAddress,
                PrimaryPhone = contactAccountModel.PrimaryPhone,
                SecondaryPhone = contactAccountModel.SecondaryPhone
            };
            contactAccountModel.CopyAuditInformationTo(contactAccount);
            contactAccount.SetDeletable(contactAccountModel.Deletable);

            return contactAccount;
        }

        internal static void CreateContactAccountModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<ContactAccountModel>(entity => 
            {
                entity.HasKey(e => e.ContactAccountIdentifier);
                entity.Property(e => e.ContactAccountIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
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