﻿using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries
{
	public interface IMediaIdentificationQuery : IMediaLibraryQuery
	{
		Guid MediaIdentifier { get; }
	}
}