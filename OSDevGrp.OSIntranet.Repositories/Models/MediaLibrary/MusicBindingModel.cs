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
	internal class MusicBindingModel : MediaBindingModelBase
	{
		public virtual int MusicBindingIdentifier { get; set; }

		public virtual int MusicIdentifier { get; set; }

		public virtual MusicModel Music { get; set; }
	}

	internal static class MusicBindingModelExtensions
	{
		#region Methods

		internal static IMediaBinding ToDomain(this MusicBindingModel musicBindingModel, IMedia media, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(musicBindingModel, nameof(musicBindingModel))
				.NotNull(media, nameof(media))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			IMediaPersonality mediaPersonality = musicBindingModel.MediaPersonality.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter);

			IMediaBinding mediaBinding = new MediaBinding(media, musicBindingModel.AsMediaRole(), mediaPersonality);
			mediaBinding.SetDeletable(musicBindingModel.Deletable);
			mediaBinding.AddAuditInformation(musicBindingModel.CreatedUtcDateTime, musicBindingModel.CreatedByIdentifier, musicBindingModel.ModifiedUtcDateTime, musicBindingModel.ModifiedByIdentifier);

			return mediaBinding;
		}

		internal static IEnumerable<IMediaBinding> ToDomain(this IEnumerable<MusicBindingModel> musicBindingModels, IMedia media, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(musicBindingModels, nameof(musicBindingModels))
				.NotNull(media, nameof(media))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			return musicBindingModels.Select(musicBindingModel => musicBindingModel.ToDomain(media, mapperCache, mediaLibraryModelConverter, commonModelConverter)).ToArray();
		}

		internal static void CreateMusicBindingModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<MusicBindingModel>(entity =>
			{
				entity.HasKey(e => e.MusicBindingIdentifier);
				entity.Property(e => e.MusicBindingIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.MusicIdentifier).IsRequired();
				entity.Property(e => e.MediaPersonalityIdentifier).IsRequired();
				entity.Property(e => e.Role).IsRequired();
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => new { e.MusicIdentifier, e.MediaPersonalityIdentifier, e.Role }).IsUnique();
				entity.HasIndex(e => new { e.MusicIdentifier, e.MusicBindingIdentifier }).IsUnique();
				entity.HasIndex(e => new { e.MediaPersonalityIdentifier, e.MusicBindingIdentifier }).IsUnique();
				entity.HasOne(e => e.Music)
					.WithMany(e => e.MusicBindings)
					.HasForeignKey(e => e.MusicIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
				entity.HasOne(e => e.MediaPersonality)
					.WithMany(e => e.MusicBindings)
					.HasForeignKey(e => e.MediaPersonalityIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		#endregion
	}
}