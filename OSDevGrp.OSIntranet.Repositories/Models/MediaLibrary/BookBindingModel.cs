using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class BookBindingModel : MediaBindingModelBase
	{
		public virtual int BookBindingIdentifier { get; set; }

		public virtual int BookIdentifier { get; set; }

		public virtual BookModel Book { get; set; }
	}

	internal static class BookBindingModelExtensions
	{
		#region Methods

		internal static IMediaBinding ToDomain(this BookBindingModel bookBindingModel, IMedia media, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(bookBindingModel, nameof(bookBindingModel))
				.NotNull(media, nameof(media))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			mapperCache.Cache(media);

			IMediaPersonality mediaPersonality = bookBindingModel.MediaPersonality.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter);

			IMediaBinding mediaBinding = new MediaBinding(media, bookBindingModel.AsMediaRole(), mediaPersonality);
			mediaBinding.SetDeletable(bookBindingModel.Deletable);
			mediaBinding.AddAuditInformation(bookBindingModel.CreatedUtcDateTime, bookBindingModel.CreatedByIdentifier, bookBindingModel.ModifiedUtcDateTime, bookBindingModel.ModifiedByIdentifier);

			return mediaBinding;
		}

		internal static IEnumerable<IMediaBinding> ToDomain(this IEnumerable<BookBindingModel> bookBindingModels, IMedia media, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(bookBindingModels, nameof(bookBindingModels))
				.NotNull(media, nameof(media))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			return bookBindingModels.Select(bookBindingModel => bookBindingModel.ToDomain(media, mapperCache, mediaLibraryModelConverter, commonModelConverter)).ToArray();
		}

		internal static void CreateBookBindingModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<BookBindingModel>(entity =>
			{
				entity.HasKey(e => e.BookBindingIdentifier);
				entity.Property(e => e.BookBindingIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.BookIdentifier).IsRequired();
				entity.Property(e => e.MediaPersonalityIdentifier).IsRequired();
				entity.Property(e => e.Role).IsRequired();
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => new { e.BookIdentifier, e.MediaPersonalityIdentifier, e.Role }).IsUnique();
				entity.HasIndex(e => new { e.BookIdentifier, e.BookBindingIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.MediaPersonalityIdentifier, e.BookBindingIdentifier }).IsUnique();
				entity.HasOne(e => e.Book)
					.WithMany(e => e.BookBindings)
					.HasForeignKey(e => e.BookIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.MediaPersonality)
					.WithMany(e => e.BookBindings)
					.HasForeignKey(e => e.MediaPersonalityIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		#endregion
	}
}