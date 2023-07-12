using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal abstract class MediaPersonalityIdentificationQueryBase : MediaLibraryQueryBase, IMediaPersonalityIdentificationQuery
	{
		#region Constructor

		protected MediaPersonalityIdentificationQueryBase(Guid mediaPersonalityIdentifier)
		{
			MediaPersonalityIdentifier = mediaPersonalityIdentifier;
		}

		#endregion

		#region Properties

		public Guid MediaPersonalityIdentifier { get; }

		#endregion
	}
}