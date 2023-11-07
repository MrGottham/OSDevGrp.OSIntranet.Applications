using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class UpdateMovieCommand : MovieDataCommandBase, IUpdateMovieCommand
	{
		#region Constructor

		public UpdateMovieCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int movieGenreIdentifier, int? spokenLanguageIdentifier, int mediaTypeIdentifier, short? published, short? length, string url, byte[] image, IEnumerable<Guid> directors, IEnumerable<Guid> actors) 
			: base(mediaIdentifier, title, subtitle, description, details, movieGenreIdentifier, spokenLanguageIdentifier, mediaTypeIdentifier, published, length, url, image, directors, actors)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => true;

		protected override bool ShouldBeUnknownValue => false;

		#endregion
	}
}