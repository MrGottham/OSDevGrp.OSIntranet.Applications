using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetMovieQuery : MediaIdentificationQueryBase, IGetMovieQuery
	{
		#region Constructor

		public GetMovieQuery(Guid mediaIdentifier) 
			: base(mediaIdentifier)
		{
		}

		#endregion
	}
}