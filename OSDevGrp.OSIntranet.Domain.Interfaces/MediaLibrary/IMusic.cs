using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary
{
	public interface IMusic : IMedia
	{
		IMusicGenre MusicGenre { get; }

		short? Tracks { get; }

		IEnumerable<IMediaPersonality> Artists { get; }
	}
}