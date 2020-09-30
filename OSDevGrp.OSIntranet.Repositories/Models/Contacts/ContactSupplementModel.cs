using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
    internal class ContactSupplementModel : AuditModelBase
    {
        public virtual int ContactSupplementIdentifier { get; set; }

        public virtual DateTime? Birthday { get; set; }

        public virtual int ContactGroupIdentifier { get; set; }

        public virtual ContactGroupModel ContactGroup { get; set; }

        public virtual string Acquaintance { get; set; }

        public virtual string PersonalHomePage { get; set; }

        public virtual int LendingLimit { get; set; }

        public virtual int PaymentTermIdentifier { get; set; }

        public virtual PaymentTermModel PaymentTerm { get; set; }

        public virtual List<ContactSupplementBindingModel> ContactSupplementBindings { get; set; }
    }

    internal static class ContactSupplementModelExtensions
    {
        internal static IContact ApplyContactSupplement(this ContactSupplementModel contactSupplementModel, IContact contact, IConverter contactModelConverter, IConverter accountingModelConverter)
        {
            NullGuard.NotNull(contactSupplementModel, nameof(contactSupplementModel))
                .NotNull(contact, nameof(contact))
                .NotNull(contactModelConverter, nameof(contactModelConverter))
                .NotNull(accountingModelConverter, nameof(accountingModelConverter));

            contact.InternalIdentifier = Convert.ToString(contactSupplementModel.ContactSupplementIdentifier);

            if (contact.Birthday.HasValue == false && contactSupplementModel.Birthday.HasValue)
            {
                contact.Birthday = contactSupplementModel.Birthday.Value.Date;
            }
            contact.ContactGroup = contactModelConverter.Convert<ContactGroupModel, IContactGroup>(contactSupplementModel.ContactGroup);
            contact.Acquaintance = contactSupplementModel.Acquaintance;
            contact.PersonalHomePage = contactSupplementModel.PersonalHomePage;
            contact.LendingLimit = contactSupplementModel.LendingLimit;
            contact.PaymentTerm = accountingModelConverter.Convert<PaymentTermModel, IPaymentTerm>(contactSupplementModel.PaymentTerm);

            DateTime createdUtcDateTime = contact.CreatedDateTime.ToUniversalTime();
            string createdByIdentifier = contact.CreatedByIdentifier;
            if (createdUtcDateTime == DateTime.MinValue || string.IsNullOrWhiteSpace(createdByIdentifier) || contactSupplementModel.CreatedUtcDateTime < createdUtcDateTime)
            {
                createdUtcDateTime = contactSupplementModel.CreatedUtcDateTime;
                createdByIdentifier = contactSupplementModel.CreatedByIdentifier;
            }

            DateTime modifiedUtcDateTime = contact.ModifiedDateTime.ToUniversalTime();
            string modifiedByIdentifier = contact.ModifiedByIdentifier;
            if (modifiedUtcDateTime == DateTime.MinValue || string.IsNullOrWhiteSpace(modifiedByIdentifier) || contactSupplementModel.ModifiedUtcDateTime > modifiedUtcDateTime)
            {
                modifiedUtcDateTime = contactSupplementModel.ModifiedUtcDateTime;
                modifiedByIdentifier = contactSupplementModel.ModifiedByIdentifier;
            }

            contact.AddAuditInformation(createdUtcDateTime, createdByIdentifier, modifiedUtcDateTime, modifiedByIdentifier);

            return contact;
        }

        internal static void CreateContactSupplementModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<ContactSupplementModel>(entity =>
            {
                entity.HasKey(e => e.ContactSupplementIdentifier);
                entity.Property(e => e.ContactSupplementIdentifier).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Birthday).IsRequired(false);
                entity.Property(e => e.ContactGroupIdentifier).IsRequired();
                entity.Property(e => e.Acquaintance).IsRequired(false).IsUnicode().HasMaxLength(4096);
                entity.Property(e => e.PersonalHomePage).IsRequired(false).IsUnicode().HasMaxLength(256);
                entity.Property(e => e.LendingLimit).IsRequired();
                entity.Property(e => e.PaymentTermIdentifier).IsRequired();
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasOne(e => e.ContactGroup)
                    .WithMany(e => e.ContactSupplements)
                    .HasForeignKey(e => e.ContactGroupIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.PaymentTerm)
                    .WithMany(e => e.ContactSupplements)
                    .HasForeignKey(e => e.PaymentTermIdentifier)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}