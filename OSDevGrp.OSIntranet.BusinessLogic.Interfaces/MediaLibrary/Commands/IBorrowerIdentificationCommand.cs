using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IBorrowerIdentificationCommand : IMediaLibraryCommand
	{
		Guid BorrowerIdentifier { get; }
	}
}