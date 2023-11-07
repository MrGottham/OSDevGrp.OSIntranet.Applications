using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class CreateMusicCommand : MusicDataCommandBase, ICreateMusicCommand
	{
		#region Constructor

		public CreateMusicCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int musicGenreIdentifier, int mediaTypeIdentifier, short? published, short? tracks, string url, byte[] image, IEnumerable<Guid> artists) 
			: base(mediaIdentifier, title, subtitle, description, details, musicGenreIdentifier, mediaTypeIdentifier, published, tracks, url, image, artists)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => false;

		protected override bool ShouldBeUnknownValue => true;

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.Object.ShouldBeUnknownValue<ICreateMusicCommand>(this, createMusicCommand => createMusicCommand.IsNonExistingTitleAsync<ICreateMusicCommand, IMusic>(mediaLibraryRepository), GetType(), $"{nameof(Title)},{nameof(Subtitle)},{nameof(MediaTypeIdentifier)}");
		}

		#endregion
	}
}