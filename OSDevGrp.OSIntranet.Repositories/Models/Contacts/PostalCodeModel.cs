using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
    internal class PostalCodeModel : AuditModelBase
    {
        public virtual string CountryCode { get; set; }

        public virtual CountryModel Country { get; set; }

        public virtual string PostalCode { get; set; }

        public virtual string City { get; set; }

        public virtual string State { get; set; }

        public virtual bool Deletable { get; set; }
    }

    internal static class PostalCodeModelExtensions
    {
        internal static IPostalCode ToDomain(this PostalCodeModel postalCodeModel, IConverter contactModelConverter)
        {
            NullGuard.NotNull(postalCodeModel, nameof(postalCodeModel))
                .NotNull(contactModelConverter, nameof(contactModelConverter));

            ICountry country = contactModelConverter.Convert<CountryModel, ICountry>(postalCodeModel.Country);

            IPostalCode postalCode = new PostalCode(country, postalCodeModel.PostalCode, postalCodeModel.City, postalCodeModel.State);
            postalCode.AddAuditInformation(postalCodeModel.CreatedUtcDateTime, postalCodeModel.CreatedByIdentifier, postalCodeModel.ModifiedUtcDateTime, postalCodeModel.ModifiedByIdentifier);
            postalCode.SetDeletable(postalCodeModel.Deletable);

            return postalCode;
        }

        internal static void CreatePostalCodeModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<PostalCodeModel>(entity =>
            {
                entity.HasKey(e => new {e.CountryCode, e.PostalCode});
                entity.Property(e => e.CountryCode).IsRequired().IsUnicode().HasMaxLength(4);
                entity.Property(e => e.PostalCode).IsRequired().IsUnicode().HasMaxLength(16);
                entity.Property(e => e.City).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.State).IsRequired(false).IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
                entity.HasOne(e => e.Country)
                    .WithMany(e => e.PostalCodes)
                    .HasForeignKey(e => e.CountryCode)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}