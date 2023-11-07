using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetBookQuery : MediaIdentificationQueryBase, IGetBookQuery
	{
		#region Constructor

		public GetBookQuery(Guid mediaIdentifier) 
			: base(mediaIdentifier)
		{
		}

		#endregion
	}
}