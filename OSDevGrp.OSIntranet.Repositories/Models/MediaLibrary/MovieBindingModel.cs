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
	internal class MovieBindingModel : MediaBindingModelBase
	{
		public virtual int MovieBindingIdentifier { get; set; }

		public virtual int MovieIdentifier { get; set; }

		public virtual MovieModel Movie { get; set; }
	}

	internal static class MovieBindingModelExtensions
	{
		#region Methods

		internal static IMediaBinding ToDomain(this MovieBindingModel movieBindingModel, IMedia media, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(movieBindingModel, nameof(movieBindingModel))
				.NotNull(media, nameof(media))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			mapperCache.Cache(media);

			IMediaPersonality mediaPersonality = movieBindingModel.MediaPersonality.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter);

			IMediaBinding mediaBinding = new MediaBinding(media, movieBindingModel.AsMediaRole(), mediaPersonality);
			mediaBinding.SetDeletable(movieBindingModel.Deletable);
			mediaBinding.AddAuditInformation(movieBindingModel.CreatedUtcDateTime, movieBindingModel.CreatedByIdentifier, movieBindingModel.ModifiedUtcDateTime, movieBindingModel.ModifiedByIdentifier);

			return mediaBinding;
		}

		internal static IEnumerable<IMediaBinding> ToDomain(this IEnumerable<MovieBindingModel> movieBindingModels, IMedia media, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(movieBindingModels, nameof(movieBindingModels))
				.NotNull(media, nameof(media))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			return movieBindingModels.Select(movieBindingModel => movieBindingModel.ToDomain(media, mapperCache, mediaLibraryModelConverter, commonModelConverter)).ToArray();
		}

		internal static void CreateMovieBindingModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<MovieBindingModel>(entity =>
			{
				entity.HasKey(e => e.MovieBindingIdentifier);
				entity.Property(e => e.MovieBindingIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.MovieIdentifier).IsRequired();
				entity.Property(e => e.MediaPersonalityIdentifier).IsRequired();
				entity.Property(e => e.Role).IsRequired();
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => new { e.MovieIdentifier, e.MediaPersonalityIdentifier, e.Role }).IsUnique();
				entity.HasIndex(e => new { e.MovieIdentifier, e.MovieBindingIdentifier}).IsUnique();
				entity.HasIndex(e => new { e.MediaPersonalityIdentifier, e.MovieBindingIdentifier }).IsUnique();
				entity.HasOne(e => e.Movie)
					.WithMany(e => e.MovieBindings)
					.HasForeignKey(e => e.MovieIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.MediaPersonality)
					.WithMany(e => e.MovieBindings)
					.HasForeignKey(e => e.MediaPersonalityIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		#endregion
	}
}