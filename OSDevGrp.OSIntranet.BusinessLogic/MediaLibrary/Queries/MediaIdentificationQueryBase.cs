using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal abstract class MediaIdentificationQueryBase : MediaLibraryQueryBase, IMediaIdentificationQuery
	{
		#region Constructor

		protected MediaIdentificationQueryBase(Guid mediaIdentifier)
		{
			MediaIdentifier = mediaIdentifier;
		}

		#endregion

		#region Properties

		public Guid MediaIdentifier { get; }

		#endregion
	}
}