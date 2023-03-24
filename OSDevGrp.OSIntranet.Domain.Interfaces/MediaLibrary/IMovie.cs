using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
	public interface IMovie : IMedia
	{
		IMovieGenre MovieGenre { get; }

		ILanguage SpokenLanguage { get; }

		short? Length { get; }

		IEnumerable<IMediaPersonality> Directors { get; }

		IEnumerable<IMediaPersonality> Actors { get; }
	}
}