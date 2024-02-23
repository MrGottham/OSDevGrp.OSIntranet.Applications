using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MediaPersonalityModel : AuditModelBase
	{
		public virtual int MediaPersonalityIdentifier { get; set; }

		public virtual Guid ExternalMediaPersonalityIdentifier { get; set; }

		public virtual string GivenName { get; set;  }

		public virtual string MiddleName { get; set; }

		public virtual string Surname { get; set; }

		public virtual int NationalityIdentifier { get; set; }

		public virtual NationalityModel Nationality { get; set; }

		public virtual DateTime? BirthDate { get; set; }

		public virtual DateTime? DateOfDead { get; set; }

		public virtual string Url { get; set; }

		public virtual string Image { get; set; }

		public virtual bool Deletable { get; set; }

		public virtual List<MovieBindingModel> MovieBindings { get; set; }

		public virtual List<MusicBindingModel> MusicBindings { get; set; }

		public virtual List<BookBindingModel> BookBindings { get; set; }
	}

	internal static class MediaPersonalityModelExtensions
	{
		#region Methods

		internal static IMediaPersonality ToDomain(this MediaPersonalityModel mediaPersonalityModel, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			lock (mapperCache.SyncRoot)
			{
				if (mapperCache.MediaPersonalityDictionary.TryGetValue(mediaPersonalityModel.ExternalMediaPersonalityIdentifier, out IMediaPersonality cachedMediaPersonality))
				{
					return cachedMediaPersonality;
				}

				INationality nationality = commonModelConverter.Convert<NationalityModel, INationality>(mediaPersonalityModel.Nationality);

				IMediaPersonality mediaPersonality = new MediaPersonality(
					mediaPersonalityModel.ExternalMediaPersonalityIdentifier,
					mediaPersonalityModel.GivenName,
					mediaPersonalityModel.MiddleName,
					mediaPersonalityModel.Surname,
					nationality,
					mediaPersonalityModel.CalculateMediaRoles(),
					mediaPersonalityModel.BirthDate,
					mediaPersonalityModel.DateOfDead,
					ValueConverter.StringToUri(mediaPersonalityModel.Url),
					ValueConverter.StringToByteArray(mediaPersonalityModel.Image));

				mediaPersonality.SetDeletable(mediaPersonalityModel.Deletable);

				DateTime modifiedUtcDateTime = mediaPersonalityModel.ModifiedUtcDateTime;
				string modifiedByIdentifier = mediaPersonalityModel.ModifiedByIdentifier;
				List<MediaBindingModelBase> mediaBindingModelCollection = new List<MediaBindingModelBase>();
				mediaBindingModelCollection.AddRange(mediaPersonalityModel.MovieBindings?.OfType<MediaBindingModelBase>() ?? Array.Empty<MediaBindingModelBase>());
				mediaBindingModelCollection.AddRange(mediaPersonalityModel.MusicBindings?.OfType<MediaBindingModelBase>() ?? Array.Empty<MediaBindingModelBase>());
				mediaBindingModelCollection.AddRange(mediaPersonalityModel.BookBindings?.OfType<MediaBindingModelBase>() ?? Array.Empty<MediaBindingModelBase>());
				MediaBindingModelBase latestModifiedMediaBinding = mediaBindingModelCollection.GetLatestModifiedMediaBinding();
				if (latestModifiedMediaBinding != null && latestModifiedMediaBinding.ModifiedUtcDateTime > modifiedUtcDateTime)
				{
					modifiedUtcDateTime = latestModifiedMediaBinding.ModifiedUtcDateTime;
					modifiedByIdentifier = latestModifiedMediaBinding.ModifiedByIdentifier;
				}
				mediaPersonality.AddAuditInformation(mediaPersonalityModel.CreatedUtcDateTime, mediaPersonalityModel.CreatedByIdentifier, modifiedUtcDateTime, modifiedByIdentifier);

				mapperCache.Cache(mediaPersonality);

				return mediaPersonality;
			}
		}

		internal static void CreateMediaPersonalityModel(this ModelBuilder modelBuilder)
		{
			NullGuard.NotNull(modelBuilder, nameof(modelBuilder));

			modelBuilder.Entity<MediaPersonalityModel>(entity =>
			{
				entity.HasKey(e => e.MediaPersonalityIdentifier);
				entity.Property(e => e.MediaPersonalityIdentifier).IsRequired().ValueGeneratedOnAdd();
				entity.Property(e => e.ExternalMediaPersonalityIdentifier).IsRequired().HasMaxLength(36);
				entity.Property(e => e.GivenName).IsRequired(false).IsUnicode().HasMaxLength(32);
				entity.Property(e => e.MiddleName).IsRequired(false).IsUnicode().HasMaxLength(32);
				entity.Property(e => e.Surname).IsRequired().IsUnicode().HasMaxLength(32);
				entity.Property(e => e.NationalityIdentifier).IsRequired();
				entity.Property(e => e.BirthDate).IsRequired(false);
				entity.Property(e => e.DateOfDead).IsRequired(false);
				entity.Property(e => e.Url).IsRequired(false).IsUnicode().HasMaxLength(256);
				entity.Property(e => e.Image).HasColumnType("TEXT").IsRequired(false).IsUnicode();
				entity.Property(e => e.CreatedUtcDateTime).IsRequired();
				entity.Property(e => e.CreatedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Property(e => e.ModifiedUtcDateTime).IsRequired();
				entity.Property(e => e.ModifiedByIdentifier).IsRequired().IsUnicode().HasMaxLength(256);
				entity.Ignore(e => e.Deletable);
				entity.HasIndex(e => e.ExternalMediaPersonalityIdentifier).IsUnique();
				entity.HasIndex(e => new { e.GivenName, e.MiddleName, e.Surname, e.BirthDate }).IsUnique();
				entity.HasIndex(e => new { e.NationalityIdentifier, e.MediaPersonalityIdentifier }).IsUnique();
				entity.HasOne(e => e.Nationality)
					.WithMany(e => e.MediaPersonalities)
					.HasForeignKey(e => e.NationalityIdentifier)
					.IsRequired()
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		private static MediaRole[] CalculateMediaRoles(this MediaPersonalityModel mediaPersonalityModel)
		{
			NullGuard.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			List<MediaRole> mediaRoles = new List<MediaRole>();
			mediaRoles.AddRange(mediaPersonalityModel.MovieBindings?.Select(movieBinding => movieBinding.AsMediaRole()) ?? Array.Empty<MediaRole>());
			mediaRoles.AddRange(mediaPersonalityModel.MusicBindings?.Select(musicBinding => musicBinding.AsMediaRole()) ?? Array.Empty<MediaRole>());
			mediaRoles.AddRange(mediaPersonalityModel.BookBindings?.Select(bookBinding => bookBinding.AsMediaRole()) ?? Array.Empty<MediaRole>());

			return mediaRoles.Distinct().OrderBy(mediaRole => (int) mediaRole).ToArray();
		}

		#endregion
	}
}