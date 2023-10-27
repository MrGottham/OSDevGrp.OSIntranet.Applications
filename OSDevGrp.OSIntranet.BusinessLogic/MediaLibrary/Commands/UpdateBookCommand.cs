using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class UpdateBookCommand : BookDataCommandBase, IUpdateBookCommand
	{
		#region Constructor

		public UpdateBookCommand(Guid mediaIdentifier, string title, string subtitle, string description, string details, int bookGenreIdentifier, int? writtenLanguageIdentifier, int mediaTypeIdentifier, string internationalStandardBookNumber, short? published, string url, byte[] image, IEnumerable<Guid> authors) 
			: base(mediaIdentifier, title, subtitle, description, details, bookGenreIdentifier, writtenLanguageIdentifier, mediaTypeIdentifier, internationalStandardBookNumber, published, url, image, authors)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => true;

		protected override bool ShouldBeUnknownValue => false;

		#endregion
	}
}