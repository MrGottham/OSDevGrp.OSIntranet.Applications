using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries
{
	public interface IMediaPersonalityIdentificationQuery : IMediaLibraryQuery
	{
		Guid MediaPersonalityIdentifier { get; }
	}
}