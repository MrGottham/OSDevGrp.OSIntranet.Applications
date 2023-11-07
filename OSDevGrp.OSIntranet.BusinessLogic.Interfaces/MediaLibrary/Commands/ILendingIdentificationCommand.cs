using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands
{
	public interface ILendingIdentificationCommand : IMediaLibraryCommand
	{
		Guid LendingIdentifier { get; }
	}
}