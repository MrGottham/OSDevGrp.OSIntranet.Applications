using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class MovieDataCommandBase : MovieIdentificationCommandBase, IMovieDataCommand
	{
		#region Constructor

		protected MovieDataCommandBase(Guid mediaIdentifier, string title, string subtitle, string description, string details, int movieGenreIdentifier, int? spokenLanguageIdentifier, int mediaTypeIdentifier, short? published, short? length, string url, byte[] image, IEnumerable<Guid> directors, IEnumerable<Guid> actors) 
			: base(mediaIdentifier)
		{
			NullGuard.NotNullOrWhiteSpace(title, nameof(title));

			Title = title.ToUpper().Trim();
			Subtitle = string.IsNullOrWhiteSpace(subtitle) ? null : subtitle.ToUpper().Trim();
			Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
			Details = string.IsNullOrWhiteSpace(details) ? null : details.Trim();
			MovieGenreIdentifier = movieGenreIdentifier;
			SpokenLanguageIdentifier = spokenLanguageIdentifier;
			MediaTypeIdentifier = mediaTypeIdentifier;
			Published = published;
			Length = length;
			Url = string.IsNullOrWhiteSpace(url) ? null : url.Trim();
			Image = image ?? Array.Empty<byte>();
			Directors = directors ?? Array.Empty<Guid>();
			Actors = actors ?? Array.Empty<Guid>();
		}

		#endregion

		#region Properties

		public string Title { get; }

		public string Subtitle { get; }

		public string Description { get; }

		public string Details { get; }

		public int MovieGenreIdentifier { get; }

		public int? SpokenLanguageIdentifier { get; }

		public int MediaTypeIdentifier { get; }

		public short? Published { get; }

		public short? Length { get; }

		public string Url { get; }

		public byte[] Image { get; }

		public IEnumerable<Guid> Directors { get; }

		public IEnumerable<Guid> Actors { get; }

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.ValidateMovieData(this, mediaLibraryRepository, commonRepository);
		}

		public async Task<IMovie> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			IMovieGenre movieGenre = await mediaLibraryRepository.GetMovieGenreAsync(MovieGenreIdentifier);
			ILanguage spokenLanguage = SpokenLanguageIdentifier.HasValue
				? await commonRepository.GetLanguageAsync(SpokenLanguageIdentifier.Value)
				: null;
			IMediaType mediaType = await mediaLibraryRepository.GetMediaTypeAsync(MediaTypeIdentifier);
			IEnumerable<IMediaPersonality> directors = await MediaPersonalityResolver.ResolveAsync(Directors, mediaLibraryRepository);
			IEnumerable<IMediaPersonality> actors = await MediaPersonalityResolver.ResolveAsync(Actors, mediaLibraryRepository);

			return new Movie(
				MediaIdentifier,
				Title,
				Subtitle,
				Description,
				Details,
				movieGenre,
				spokenLanguage,
				mediaType,
				Published,
				Length,
				string.IsNullOrWhiteSpace(Url) == false ? new Uri(Url, UriKind.Absolute) : null,
				Image,
				directors,
				actors,
				_ => Array.Empty<ILending>());
		}

		#endregion
	}
}