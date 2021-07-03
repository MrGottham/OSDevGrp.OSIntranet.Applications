using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Contacts;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
    internal class CountryModel : AuditModelBase
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string UniversalName { get; set; }

        public virtual string PhonePrefix { get; set; }

        public virtual bool Deletable { get; set; }

        public virtual List<PostalCodeModel> PostalCodes { get; set; }
    }

    internal static class CountryModelExtensions
    {
        internal static ICountry ToDomain(this CountryModel countryModel)
        {
            NullGuard.NotNull(countryModel, nameof(countryModel));

            ICountry country = new Country(countryModel.Code, countryModel.Name, countryModel.UniversalName, countryModel.PhonePrefix);
            country.AddAuditInformation(countryModel.CreatedUtcDateTime, countryModel.CreatedByIdentifier, countryModel.ModifiedUtcDateTime, countryModel.ModifiedByIdentifier);
            country.SetDeletable(countryModel.Deletable);

            return country;
        }

        internal static void CreateCountryModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<CountryModel>(entity =>
            {
                entity.HasKey(e => e.Code);
                entity.Property(e => e.Code).IsRequired().IsUnicode().HasMaxLength(4);
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.UniversalName).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.PhonePrefix).IsRequired().IsUnicode().HasMaxLength(4);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
            });
        }
    }
}