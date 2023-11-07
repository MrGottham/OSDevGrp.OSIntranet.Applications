using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries
{
	public interface ILendingIdentificationQuery : IMediaLibraryQuery
	{
		Guid LendingIdentifier { get; }
	}
}