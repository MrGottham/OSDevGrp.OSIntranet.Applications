using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal class MovieGenreModel : GenericCategoryModelBase
    {
        public virtual int MovieGenreIdentifier { get; set; }
    }

    internal static class MovieGenreModelExtensions
    {
        #region Methods

        internal static IMovieGenre ToDomain(this MovieGenreModel movieGenreModel)
        {
            NullGuard.NotNull(movieGenreModel, nameof(movieGenreModel));

            IMovieGenre movieGenre = new MovieGenre(movieGenreModel.MovieGenreIdentifier, movieGenreModel.Name);
            movieGenre.SetDeletable(movieGenreModel.Deletable);
            movieGenre.AddAuditInformation(movieGenreModel.CreatedUtcDateTime, movieGenreModel.CreatedByIdentifier, movieGenreModel.ModifiedUtcDateTime, movieGenreModel.ModifiedByIdentifier);

            return movieGenre;
        }

        internal static void CreateMovieGenreModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<MovieGenreModel>(entity =>
            {
                entity.HasKey(e => e.MovieGenreIdentifier);
                entity.Property(e => e.MovieGenreIdentifier).IsRequired();
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