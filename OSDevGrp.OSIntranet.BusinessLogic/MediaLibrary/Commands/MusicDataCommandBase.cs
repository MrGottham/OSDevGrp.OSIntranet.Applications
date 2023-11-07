using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class MusicDataCommandBase : MusicIdentificationCommandBase, IMusicDataCommand
	{
		#region Constructor

		protected MusicDataCommandBase(Guid mediaIdentifier, string title, string subtitle, string description, string details, int musicGenreIdentifier, int mediaTypeIdentifier, short? published, short? tracks, string url, byte[] image, IEnumerable<Guid> artists) 
			: base(mediaIdentifier)
		{
			NullGuard.NotNullOrWhiteSpace(title, nameof(title));

			Title = title.ToUpper().Trim();
			Subtitle = string.IsNullOrWhiteSpace(subtitle) ? null : subtitle.ToUpper().Trim();
			Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
			Details = string.IsNullOrWhiteSpace(details) ? null : details.Trim();
			MusicGenreIdentifier = musicGenreIdentifier;
			MediaTypeIdentifier = mediaTypeIdentifier;
			Published = published;
			Tracks = tracks;
			Url = string.IsNullOrWhiteSpace(url) ? null : url.Trim();
			Image = image ?? Array.Empty<byte>();
			Artists = artists ?? Array.Empty<Guid>();
		}

		#endregion

		#region Properties

		public string Title { get; }

		public string Subtitle { get; }

		public string Description { get; }

		public string Details { get; }

		public int MusicGenreIdentifier { get; }

		public int MediaTypeIdentifier { get; }

		public short? Published { get; }

		public short? Tracks { get; }

		public string Url { get; }

		public byte[] Image { get; }

		public IEnumerable<Guid> Artists { get; }

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.ValidateMusicData(this, mediaLibraryRepository, commonRepository);
		}

		public async Task<IMusic> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			IMusicGenre musicGenre = await mediaLibraryRepository.GetMusicGenreAsync(MusicGenreIdentifier);
			IMediaType mediaType = await mediaLibraryRepository.GetMediaTypeAsync(MediaTypeIdentifier);
			IEnumerable<IMediaPersonality> artists = await MediaPersonalityResolver.ResolveAsync(Artists, mediaLibraryRepository);

			return new Music(
				MediaIdentifier,
				Title,
				Subtitle,
				Description,
				Details,
				musicGenre,
				mediaType,
				Published,
				Tracks,
				string.IsNullOrWhiteSpace(Url) == false ? new Uri(Url, UriKind.Absolute) : null,
				Image,
				artists,
				_ => Array.Empty<ILending>());
		}

		#endregion
	}
}