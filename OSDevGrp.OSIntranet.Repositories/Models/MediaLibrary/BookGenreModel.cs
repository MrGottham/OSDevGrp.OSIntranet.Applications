using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
    internal class BookGenreModel : GenericCategoryModelBase
    {
        public virtual int BookGenreIdentifier { get; set; }
    }

    internal static class BookGenreModelExtensions
    {
        #region Methods

        internal static IBookGenre ToDomain(this BookGenreModel bookGenreModel)
        {
            NullGuard.NotNull(bookGenreModel, nameof(bookGenreModel));

            IBookGenre bookGenre = new BookGenre(bookGenreModel.BookGenreIdentifier, bookGenreModel.Name);
            bookGenre.SetDeletable(bookGenreModel.Deletable);
            bookGenre.AddAuditInformation(bookGenreModel.CreatedUtcDateTime, bookGenreModel.CreatedByIdentifier, bookGenreModel.ModifiedUtcDateTime, bookGenreModel.ModifiedByIdentifier);

            return bookGenre;
        }

        internal static void CreateBookGenreModel(this ModelBuilder modelBuilder)
        {
            NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

            modelBuilder.Entity<BookGenreModel>(entity =>
            {
                entity.HasKey(e => e.BookGenreIdentifier);
                entity.Property(e => e.BookGenreIdentifier).IsRequired();
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