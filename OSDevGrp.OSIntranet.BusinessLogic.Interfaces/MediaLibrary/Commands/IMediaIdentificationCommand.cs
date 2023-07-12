using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface IMediaIdentificationCommand : IMediaLibraryCommand
	{
		Guid MediaIdentifier { get; }
	}
}