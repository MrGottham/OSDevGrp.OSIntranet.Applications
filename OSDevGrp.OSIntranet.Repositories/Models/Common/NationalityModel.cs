using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Common;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Common
{
    internal class NationalityModel : GenericCategoryModelBase
    {
        public virtual int NationalityIdentifier { get; set; }
    }

    internal static class NationalityModelExtensions
    {
        #region Methods

        internal static INationality ToDomain(this NationalityModel nationalityModel)
        {
            NullGuard.NotNull(nationalityModel, nameof(nationalityModel));

            INationality nationality = new Nationality(nationalityModel.NationalityIdentifier, nationalityModel.Name);
            nationality.SetDeletable(nationalityModel.Deletable);
            nationality.AddAuditInformation(nationalityModel.CreatedUtcDateTime, nationalityModel.CreatedByIdentifier, nationalityModel.ModifiedUtcDateTime, nationalityModel.ModifiedByIdentifier);

            return nationality;
        }

        internal static void CreateNationalityModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<NationalityModel>(entity =>
            {
                entity.HasKey(e => e.NationalityIdentifier);
                entity.Property(e => e.NationalityIdentifier).IsRequired();
                entity.Property(e => e.Name).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.CreatedUtcDateTime).IsRequired();
                entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
                entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
                entity.Ignore(e => e.Deletable);
            });
        }

        #endregion
    }
}