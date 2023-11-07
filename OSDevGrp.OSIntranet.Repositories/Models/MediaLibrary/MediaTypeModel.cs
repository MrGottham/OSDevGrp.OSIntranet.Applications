using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MediaTypeModel : GenericCategoryModelBase
    {
        public virtual int MediaTypeIdentifier { get; set; }

        public virtual List<MediaCoreDataModel> CoreData { get; set; }
    }

    internal static class MediaTypeModelExtensions
    {
        #region Methods

        internal static IMediaType ToDomain(this MediaTypeModel mediaTypeModel)
        {
            NullGuard.NotNull(mediaTypeModel, nameof(mediaTypeModel));

            IMediaType mediaType = new MediaType(mediaTypeModel.MediaTypeIdentifier, mediaTypeModel.Name);
            mediaType.SetDeletable(mediaTypeModel.Deletable);
            mediaType.AddAuditInformation(mediaTypeModel.CreatedUtcDateTime, mediaTypeModel.CreatedByIdentifier, mediaTypeModel.ModifiedUtcDateTime, mediaTypeModel.ModifiedByIdentifier);

            return mediaType;
        }

        internal static void CreateMediaTypeModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<MediaTypeModel>(entity =>
            {
                entity.HasKey(e => e.MediaTypeIdentifier);
                entity.Property(e => e.MediaTypeIdentifier).IsRequired();
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