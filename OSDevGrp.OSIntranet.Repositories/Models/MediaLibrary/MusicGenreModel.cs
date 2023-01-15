using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal class MusicGenreModel : GenericCategoryModelBase
    {
        public virtual int MusicGenreIdentifier { get; set; }
    }

    internal static class MusicGenreModelExtensions
    {
        #region Methods

        internal static IMusicGenre ToDomain(this MusicGenreModel musicGenreModel)
        {
            NullGuard.NotNull(musicGenreModel, nameof(musicGenreModel));

            IMusicGenre musicGenre = new MusicGenre(musicGenreModel.MusicGenreIdentifier, musicGenreModel.Name);
            musicGenre.SetDeletable(musicGenreModel.Deletable);
            musicGenre.AddAuditInformation(musicGenreModel.CreatedUtcDateTime, musicGenreModel.CreatedByIdentifier, musicGenreModel.ModifiedUtcDateTime, musicGenreModel.ModifiedByIdentifier);

            return musicGenre;
        }

        internal static void CreateMusicGenreModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<MusicGenreModel>(entity =>
            {
                entity.HasKey(e => e.MusicGenreIdentifier);
                entity.Property(e => e.MusicGenreIdentifier).IsRequired();
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