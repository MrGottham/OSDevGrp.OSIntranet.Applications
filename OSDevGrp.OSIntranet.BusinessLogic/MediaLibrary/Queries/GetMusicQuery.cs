using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetMusicQuery : MediaIdentificationQueryBase, IGetMusicQuery
	{
		#region Constructor

		public GetMusicQuery(Guid mediaIdentifier) 
			: base(mediaIdentifier)
		{
		}

		#endregion
	}
}