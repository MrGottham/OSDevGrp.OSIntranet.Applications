using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IMusicDataCommand : IMusicIdentificationCommand, IMediaDataCommand<IMusic>
	{
		int MusicGenreIdentifier { get; }

		short? Tracks { get; }

		IEnumerable<Guid> Artists { get; }
	}
}