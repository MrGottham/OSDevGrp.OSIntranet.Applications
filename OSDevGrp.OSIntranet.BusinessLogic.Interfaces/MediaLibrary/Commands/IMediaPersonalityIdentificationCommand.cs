using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IMediaPersonalityIdentificationCommand : IMediaLibraryCommand
	{
		Guid MediaPersonalityIdentifier { get; }
	}
}