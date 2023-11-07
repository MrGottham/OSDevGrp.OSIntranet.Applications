using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetMediaPersonalityQuery : MediaPersonalityIdentificationQueryBase, IGetMediaPersonalityQuery
	{
		#region Constructor

		public GetMediaPersonalityQuery(Guid mediaPersonalityIdentifier) 
			: base(mediaPersonalityIdentifier)
		{
		}

		#endregion
	}
}