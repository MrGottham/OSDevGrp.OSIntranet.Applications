using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
    internal class ContactSupplementBindingModel : AuditModelBase
    {
        public virtual int ContactSupplementBindingIdentifier { get; set; }
        
        public virtual int ContactSupplementIdentifier { get; set; }

        public virtual ContactSupplementModel ContactSupplement { get; set; }

        public virtual string ExternalIdentifier { get; set; }
    }

    public static class ContactSupplementBindingModelExtensions
    {
        internal static void CreateContactSupplementBindingModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<ContactSupplementBindingModel>(entity =>
            {
                entity.HasKey(e => e.ContactSupplementBindingIdentifier);
                entity.Property(e => e.ContactSupplementBindingIdentifier).IsRequired().HasAnnotation("MySQL:AutoIncrement", true);
                entity.Property(e => e.ContactSupplementIdentifier).IsRequired();
                entity.Property(e => e.ExternalIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.HasIndex(e => new {e.ContactSupplementIdentifier, e.ExternalIdentifier}).IsUnique();
                entity.HasIndex(e => e.ExternalIdentifier).IsUnique();
                entity.HasOne(e => e.ContactSupplement).WithMany(e => e.ContactSupplementBindings).HasForeignKey(e => e.ContactSupplementIdentifier).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}