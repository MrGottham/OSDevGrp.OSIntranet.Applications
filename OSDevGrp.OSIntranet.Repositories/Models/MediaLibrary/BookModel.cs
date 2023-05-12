using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class BookModel : MediaModelBase
	{
		public virtual int BookIdentifier { get; set; }

		public virtual int BookGenreIdentifier { get; set; }

		public virtual BookGenreModel BookGenre { get; set; }

		public virtual int? WrittenLanguageIdentifier { get; set; }

		public virtual LanguageModel WrittenLanguage { get; set; }

		public string InternationalStandardBookNumber { get; set;  }

		public virtual List<BookBindingModel> BookBindings { get; set; }
	}

	internal static class BookModelExtensions
	{
		#region Methods

		internal static IBook ToDomain(this BookModel bookModel, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(bookModel, nameof(bookModel))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			lock (mapperCache.SyncRoot)
			{
				Guid externalMediaIdentifier = ValueConverter.StringToGuid(bookModel.ExternalMediaIdentifier);
				if (mapperCache.MediaDictionary.TryGetValue(externalMediaIdentifier, out IMedia cachedMedia))
				{
					return (IBook)cachedMedia;
				}

				IBookGenre bookGenre = mediaLibraryModelConverter.Convert<BookGenreModel, IBookGenre>(bookModel.BookGenre);
				ILanguage writtenLanguage = bookModel.WrittenLanguage != null ? commonModelConverter.Convert<LanguageModel, ILanguage>(bookModel.WrittenLanguage) : null;
				IMediaType mediaType = mediaLibraryModelConverter.Convert<MediaTypeModel, IMediaType>(bookModel.CoreData.MediaType);

				IBook book = new Book(
					externalMediaIdentifier,
					bookModel.CoreData.Title,
					bookModel.CoreData.Subtitle,
					bookModel.CoreData.Description,
					bookModel.CoreData.Details,
					bookGenre,
					writtenLanguage,
					mediaType,
					bookModel.InternationalStandardBookNumber,
					bookModel.CoreData.Published,
					ValueConverter.StringToUri(bookModel.CoreData.Url),
					ValueConverter.StringToByteArray(bookModel.CoreData.Image),
					media => (bookModel.BookBindings ?? new List<BookBindingModel>(0)).ToDomain(media, mapperCache, mediaLibraryModelConverter, commonModelConverter),
					media => (bookModel.Lendings ?? new List<LendingModel>(0)).ToDomain(media, mapperCache, mediaLibraryModelConverter, commonModelConverter));

				book.SetDeletable(bookModel.Deletable);
				bookModel.ApplyAuditInformation(model => model.BookBindings, book);

				mapperCache.Cache(book);

				return book;
			}
		}

		internal static void CreateBookModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<BookModel>(entity =>
			{
				entity.HasKey(e => e.BookIdentifier);
				entity.Property(e => e.BookIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.ExternalMediaIdentifier).IsRequired().HasMaxLength(36);
				entity.Property(e => e.BookGenreIdentifier).IsRequired();
				entity.Property(e => e.WrittenLanguageIdentifier).IsRequired(false);
				entity.Property(e => e.InternationalStandardBookNumber).IsRequired(false).IsUnicode().HasMaxLength(32);
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => e.ExternalMediaIdentifier).IsUnique();
				entity.HasIndex(e => new { e.BookGenreIdentifier, e.BookIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.WrittenLanguageIdentifier, e.BookIdentifier }).IsUnique();
				entity.HasOne(e => e.CoreData)
					.WithOne(e => e.Book)
					.HasForeignKey<MediaCoreDataModel>(e => e.BookIdentifier)
					.IsRequired(false)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.BookGenre)
					.WithMany(e => e.Books)
					.HasForeignKey(e => e.BookGenreIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.WrittenLanguage)
					.WithMany(e => e.Books)
					.HasForeignKey(e => e.WrittenLanguageIdentifier)
					.IsRequired(false)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		#endregion
	}
}