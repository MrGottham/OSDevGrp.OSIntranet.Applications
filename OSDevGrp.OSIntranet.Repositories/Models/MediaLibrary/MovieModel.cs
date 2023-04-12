using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MovieModel : MediaModelBase
	{
		public virtual int MovieIdentifier { get; set; }

		public virtual int MovieGenreIdentifier { get; set; }

		public virtual MovieGenreModel MovieGenre { get; set; }

		public virtual int? SpokenLanguageIdentifier { get; set; }

		public virtual LanguageModel SpokenLanguage { get; set; }

		public virtual short? Length { get; set; }

		public virtual List<MovieBindingModel> MovieBindings { get; set; }
	}

	internal static class MovieModelExtensions
	{
		#region Methods

		internal static IMovie ToDomain(this MovieModel movieModel, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(movieModel, nameof(movieModel))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			lock (mapperCache.SyncRoot)
			{
				Guid externalMediaIdentifier = ValueConverter.StringToGuid(movieModel.ExternalMediaIdentifier);
				if (mapperCache.MediaDictionary.TryGetValue(externalMediaIdentifier, out IMedia cachedMedia))
				{
					return (IMovie)cachedMedia;
				}

				IMovieGenre movieGenre = mediaLibraryModelConverter.Convert<MovieGenreModel, IMovieGenre>(movieModel.MovieGenre);
				ILanguage spokenLanguage = movieModel.SpokenLanguage != null ? commonModelConverter.Convert<LanguageModel, ILanguage>(movieModel.SpokenLanguage) : null;
				IMediaType mediaType = mediaLibraryModelConverter.Convert<MediaTypeModel, IMediaType>(movieModel.CoreData.MediaType);
				IMediaPersonality[] directors = (movieModel.MovieBindings ?? new List<MovieBindingModel>())
					.AsMediaPersonalities(MediaRole.Director, mapperCache, mediaLibraryModelConverter, commonModelConverter)
					.ToArray();
				IMediaPersonality[] actors = (movieModel.MovieBindings ?? new List<MovieBindingModel>())
					.AsMediaPersonalities(MediaRole.Actor, mapperCache, mediaLibraryModelConverter, commonModelConverter)
					.ToArray();

				IMovie movie = new Movie(
					externalMediaIdentifier,
					movieModel.CoreData.Title,
					movieModel.CoreData.Subtitle,
					movieModel.CoreData.Description,
					movieModel.CoreData.Details,
					movieGenre,
					spokenLanguage,
					mediaType,
					movieModel.CoreData.Published,
					movieModel.Length,
					ValueConverter.StringToUri(movieModel.CoreData.Url),
					ValueConverter.StringToByteArray(movieModel.CoreData.Image),
					directors,
					actors);

				movieModel.ApplyAuditInformation(model => model.MovieBindings, movie);

				mapperCache.MediaDictionary.Add(movie.MediaIdentifier, movie);

				return movie;
			}
		}

		internal static void CreateMovieModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<MovieModel>(entity =>
			{
				entity.HasKey(e => e.MovieIdentifier);
				entity.Property(e => e.MovieIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.ExternalMediaIdentifier).IsRequired().HasMaxLength(36);
				entity.Property(e => e.MovieGenreIdentifier).IsRequired();
				entity.Property(e => e.SpokenLanguageIdentifier).IsRequired(false);
				entity.Property(e => e.Length).IsRequired(false);
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => e.ExternalMediaIdentifier).IsUnique();
				entity.HasIndex(e => new { e.MovieGenreIdentifier, e.MovieIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.SpokenLanguageIdentifier, e.MovieIdentifier }).IsUnique();
				entity.HasOne(e => e.CoreData)
					.WithOne(e => e.Movie)
					.HasForeignKey<MediaCoreDataModel>(e => e.MovieIdentifier)
					.IsRequired(false)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.MovieGenre)
					.WithMany(e => e.Movies)
					.HasForeignKey(e => e.MovieGenreIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.SpokenLanguage)
					.WithMany(e => e.Movies)
					.HasForeignKey(e => e.SpokenLanguageIdentifier)
					.IsRequired(false)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		#endregion
	}
}