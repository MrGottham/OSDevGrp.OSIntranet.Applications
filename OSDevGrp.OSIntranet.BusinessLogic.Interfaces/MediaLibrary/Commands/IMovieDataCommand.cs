using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IMovieDataCommand : IMovieIdentificationCommand, IMediaDataCommand<IMovie>
	{
		int MovieGenreIdentifier { get; }

		int? SpokenLanguageIdentifier { get; }

		short? Length { get; }

		IEnumerable<Guid> Directors { get; }

		IEnumerable<Guid> Actors { get; }
	}
}