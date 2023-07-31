using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IBookDataCommand : IBookIdentificationCommand, IMediaDataCommand<IBook>
	{
		int BookGenreIdentifier { get; }

		int? WrittenLanguageIdentifier { get; }

		string InternationalStandardBookNumber { get; }

		IEnumerable<Guid> Authors { get; }
	}
}