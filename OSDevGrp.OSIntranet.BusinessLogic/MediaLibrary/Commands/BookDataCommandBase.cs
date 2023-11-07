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
	internal abstract class BookDataCommandBase : BookIdentificationCommandBase, IBookDataCommand
	{
		#region Constructor

		protected BookDataCommandBase(Guid mediaIdentifier, string title, string subtitle, string description, string details, int bookGenreIdentifier, int? writtenLanguageIdentifier, int mediaTypeIdentifier, string internationalStandardBookNumber, short? published, string url, byte[] image, IEnumerable<Guid> authors) 
			: base(mediaIdentifier)
		{
			NullGuard.NotNullOrWhiteSpace(title, nameof(title));

			Title = title.ToUpper().Trim();
			Subtitle = string.IsNullOrWhiteSpace(subtitle) ? null : subtitle.ToUpper().Trim();
			Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
			Details = string.IsNullOrWhiteSpace(details) ? null : details.Trim();
			BookGenreIdentifier = bookGenreIdentifier;
			WrittenLanguageIdentifier = writtenLanguageIdentifier;
			MediaTypeIdentifier = mediaTypeIdentifier;
			InternationalStandardBookNumber = string.IsNullOrWhiteSpace(internationalStandardBookNumber) ? null : internationalStandardBookNumber.Trim();
			Published = published;
			Url = string.IsNullOrWhiteSpace(url) ? null : url.Trim();
			Image = image ?? Array.Empty<byte>();
			Authors = authors ?? Array.Empty<Guid>();
		}

		#endregion

		#region Properties

		public string Title { get; }

		public string Subtitle { get; }

		public string Description { get; }

		public string Details { get; }

		public int BookGenreIdentifier { get; }

		public int? WrittenLanguageIdentifier { get; }

		public int MediaTypeIdentifier { get; }

		public string InternationalStandardBookNumber { get; }

		public short? Published { get; }

		public string Url { get; }

		public byte[] Image { get; }

		public IEnumerable<Guid> Authors { get; }

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.ValidateBookData(this, mediaLibraryRepository, commonRepository);
		}

		public async Task<IBook> ToDomainAsync(IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			IBookGenre bookGenre = await mediaLibraryRepository.GetBookGenreAsync(BookGenreIdentifier);
			ILanguage writtenLanguage = WrittenLanguageIdentifier.HasValue
				? await commonRepository.GetLanguageAsync(WrittenLanguageIdentifier.Value)
				: null;
			IMediaType mediaType = await mediaLibraryRepository.GetMediaTypeAsync(MediaTypeIdentifier);
			IEnumerable<IMediaPersonality> authors = await MediaPersonalityResolver.ResolveAsync(Authors, mediaLibraryRepository);

			return new Book(
				MediaIdentifier,
				Title,
				Subtitle,
				Description,
				Details,
				bookGenre,
				writtenLanguage,
				mediaType,
				InternationalStandardBookNumber,
				Published,
				string.IsNullOrWhiteSpace(Url) == false ? new Uri(Url, UriKind.Absolute) : null,
				Image,
				authors,
				_ => Array.Empty<ILending>());
		}

		#endregion
	}
}