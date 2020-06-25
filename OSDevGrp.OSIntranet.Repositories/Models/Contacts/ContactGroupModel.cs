using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
    internal class ContactGroupModel : AuditModelBase
    {
        public virtual int ContactGroupIdentifier { get; set; }

        public virtual string Name { get; set; }

        public virtual bool Deletable { get; set; }

        public virtual List<ContactSupplementModel> ContactSupplements { get; set; }
    }

    internal static class ContactGroupModelExtensions
    {
        internal static IContactGroup ToDomain(this ContactGroupModel contactGroupModel)
        {
            NullGuard.NotNull(contactGroupModel, nameof(contactGroupModel));

            IContactGroup contactGroup = new ContactGroup(contactGroupModel.ContactGroupIdentifier, contactGroupModel.Name);
            contactGroup.AddAuditInformation(contactGroupModel.CreatedUtcDateTime, contactGroupModel.CreatedByIdentifier, contactGroupModel.ModifiedUtcDateTime, contactGroupModel.ModifiedByIdentifier);
            contactGroup.SetDeletable(contactGroupModel.Deletable);

            return contactGroup;
        }

        internal static void CreateContactGroupModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<ContactGroupModel>(entity =>
            {
                entity.HasKey(e => e.ContactGroupIdentifier);
                entity.Property(e => e.ContactGroupIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
            });
        }
    }
}