using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MusicModel : MediaModelBase
	{
		public virtual int MusicIdentifier { get; set; }

		public virtual int MusicGenreIdentifier { get; set; }

		public virtual MusicGenreModel MusicGenre { get; set; }

		public virtual short? Tracks { get; set; }

		public virtual List<MusicBindingModel> MusicBindings { get; set; }
	}

	internal static class MusicModelExtensions
	{
		#region Methods

		internal static IMusic ToDomain(this MusicModel musicModel, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(musicModel, nameof(musicModel))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			lock (mapperCache.SyncRoot)
			{
				Guid externalMediaIdentifier = ValueConverter.StringToGuid(musicModel.ExternalMediaIdentifier);
				if (mapperCache.MediaDictionary.TryGetValue(externalMediaIdentifier, out IMedia cachedMedia))
				{
					return (IMusic)cachedMedia;
				}

				IMusicGenre musicGenre = mediaLibraryModelConverter.Convert<MusicGenreModel, IMusicGenre>(musicModel.MusicGenre);
				IMediaType mediaType = mediaLibraryModelConverter.Convert<MediaTypeModel, IMediaType>(musicModel.CoreData.MediaType);

				IMusic music = new Music(
					externalMediaIdentifier,
					musicModel.CoreData.Title,
					musicModel.CoreData.Subtitle,
					musicModel.CoreData.Description,
					musicModel.CoreData.Details,
					musicGenre,
					mediaType,
					musicModel.CoreData.Published,
					musicModel.Tracks,
					ValueConverter.StringToUri(musicModel.CoreData.Url),
					ValueConverter.StringToByteArray(musicModel.CoreData.Image),
					media => (musicModel.MusicBindings ?? new List<MusicBindingModel>(0)).ToDomain(media, mapperCache, mediaLibraryModelConverter, commonModelConverter),
					_ => Array.Empty<ILending>());

				music.SetDeletable(musicModel.Deletable);
				musicModel.ApplyAuditInformation(model => model.MusicBindings, music);

				mapperCache.MediaDictionary.Add(music.MediaIdentifier, music);

				return music;
			}
		}

		internal static void CreateMusicModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<MusicModel>(entity =>
			{
				entity.HasKey(e => e.MusicIdentifier);
				entity.Property(e => e.MusicIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.ExternalMediaIdentifier).IsRequired().HasMaxLength(36);
				entity.Property(e => e.MusicGenreIdentifier).IsRequired();
				entity.Property(e => e.Tracks).IsRequired(false);
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => e.ExternalMediaIdentifier).IsUnique();
				entity.HasIndex(e => new { e.MusicGenreIdentifier, e.MusicIdentifier }).IsUnique();
				entity.HasOne(e => e.CoreData)
					.WithOne(e => e.Music)
					.HasForeignKey<MediaCoreDataModel>(e => e.MusicIdentifier)
					.IsRequired(false)
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.MusicGenre)
					.WithMany(e => e.Music)
					.HasForeignKey(e => e.MusicGenreIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		#endregion
	}
}