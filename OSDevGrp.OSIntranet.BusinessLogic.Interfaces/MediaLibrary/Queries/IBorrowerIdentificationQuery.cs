using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries
{
	public interface IBorrowerIdentificationQuery : IMediaLibraryQuery
	{
		Guid BorrowerIdentifier { get; }
	}
}