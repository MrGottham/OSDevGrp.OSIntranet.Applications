using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class LendingModel : AuditModelBase
	{
		public virtual int LendingIdentifier { get; set; }

		public virtual Guid ExternalLendingIdentifier { get; set; }

		public virtual int BorrowerIdentifier { get; set; }

		public virtual BorrowerModel Borrower { get; set; }

		public virtual int? MovieIdentifier { get; set; }

		public virtual MovieModel Movie { get; set; }

		public virtual int? MusicIdentifier { get; set; }

		public virtual MusicModel Music { get; set; }

		public virtual int? BookIdentifier { get; set; }

		public virtual BookModel Book { get; set; }

		public virtual DateTime LendingDate { get; set;  }

		public virtual DateTime RecallDate { get; set; }

		public virtual DateTime? ReturnedDate { get; set; }

		public virtual bool Deletable { get; set; }
	}

	internal static class LendingModelExtensions
	{
		#region Methods

		internal static ILending ToDomain(this LendingModel lendingModel, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			IBorrower borrower = lendingModel.Borrower.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter);
			IMedia media = lendingModel.GetMedia(mapperCache, mediaLibraryModelConverter, commonModelConverter);

			return lendingModel.ToDomain(borrower, media);
		}

		internal static ILending ToDomain(this LendingModel lendingModel, IBorrower borrower, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(borrower, nameof(borrower))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			mapperCache.Cache(borrower);

			IMedia media = lendingModel.GetMedia(mapperCache, mediaLibraryModelConverter, commonModelConverter);

			return lendingModel.ToDomain(borrower, media);
		}

		internal static IEnumerable<ILending> ToDomain(this IEnumerable<LendingModel> lendingModels, IBorrower borrower, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(lendingModels, nameof(lendingModels))
				.NotNull(borrower, nameof(borrower))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			return lendingModels.Select(lendingModel => lendingModel.ToDomain(borrower, mapperCache, mediaLibraryModelConverter, commonModelConverter)).ToArray();
		}

		internal static ILending ToDomain(this LendingModel lendingModel, IMedia media, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(media, nameof(media))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			mapperCache.Cache(media);

			IBorrower borrower = lendingModel.Borrower.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter);

			return lendingModel.ToDomain(borrower, media);
		}

		internal static IEnumerable<ILending> ToDomain(this IEnumerable<LendingModel> lendingModels, IMedia media, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(lendingModels, nameof(lendingModels))
				.NotNull(media, nameof(media))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			return lendingModels.Select(lendingModel => lendingModel.ToDomain(media, mapperCache, mediaLibraryModelConverter, commonModelConverter)).ToArray();
		}

		internal static void CreateLendingModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<LendingModel>(entity =>
			{
				entity.HasKey(e => e.LendingIdentifier);
				entity.Property(e => e.LendingIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.ExternalLendingIdentifier).IsRequired().HasMaxLength(36);
				entity.Property(e => e.BorrowerIdentifier).IsRequired();
				entity.Property(e => e.MovieIdentifier).IsRequired(false);
				entity.Property(e => e.MusicIdentifier).IsRequired(false);
				entity.Property(e => e.BookIdentifier).IsRequired(false);
				entity.Property(e => e.LendingDate).IsRequired();
				entity.Property(e => e.RecallDate).IsRequired();
				entity.Property(e => e.ReturnedDate).IsRequired(false);
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => e.ExternalLendingIdentifier).IsUnique();
				entity.HasIndex(e => new { e.BorrowerIdentifier, e.LendingIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.MovieIdentifier, e.LendingIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.MusicIdentifier, e.LendingIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.BookIdentifier, e.LendingIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.LendingDate, e.LendingIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.RecallDate, e.LendingIdentifier }).IsUnique();
				entity.HasOne(e => e.Borrower)
					.WithMany(e => e.Lendings)
					.HasForeignKey(e => e.BorrowerIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Movie)
					.WithMany(e => e.Lendings)
					.HasForeignKey(e => e.MovieIdentifier)
					.IsRequired(false)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Music)
					.WithMany(e => e.Lendings)
					.HasForeignKey(e => e.MusicIdentifier)
					.IsRequired(false)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.Book)
					.WithMany(e => e.Lendings)
					.HasForeignKey(e => e.BookIdentifier)
					.IsRequired(false)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		internal static LendingModel GetLatestModifiedLendingModel(this IEnumerable<LendingModel> lendingModels)
		{
			NullGuard.NotNull(lendingModels, nameof(lendingModels));

			return lendingModels.MaxBy(lendingModel => lendingModel.ModifiedUtcDateTime);
		}

		private static ILending ToDomain(this LendingModel lendingModel, IBorrower borrower, IMedia media)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(borrower, nameof(borrower))
				.NotNull(media, nameof(media));

			ILending lending = new Lending(
				lendingModel.ExternalLendingIdentifier,
				borrower,
				media,
				lendingModel.LendingDate,
				lendingModel.RecallDate,
				lendingModel.ReturnedDate);

			lending.SetDeletable(lendingModel.Deletable);
			lending.AddAuditInformation(lendingModel.CreatedUtcDateTime, lendingModel.CreatedByIdentifier, lendingModel.ModifiedUtcDateTime, lendingModel.ModifiedByIdentifier);

			return lending;
		}

		private static IMedia GetMedia(this LendingModel lendingModel, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(lendingModel, nameof(lendingModel))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			if (lendingModel.Movie != null)
			{
				return lendingModel.Movie.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter);
			}

			if (lendingModel.Music != null)
			{
				return lendingModel.Music.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter);
			}

			if (lendingModel.Book != null)
			{
				return lendingModel.Book.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter);
			}

			throw new NotSupportedException($"Lending ({lendingModel.ExternalLendingIdentifier}) does not refer a supported media.");
		}

		#endregion
	}
}