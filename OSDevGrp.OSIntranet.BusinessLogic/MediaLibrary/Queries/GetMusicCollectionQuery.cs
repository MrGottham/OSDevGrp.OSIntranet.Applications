using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetMusicCollectionQuery : GetMediaCollectionQueryBase, IGetMusicCollectionQuery
	{
		#region Constructor

		public GetMusicCollectionQuery(string filter) 
			: base(filter)
		{
		}

		#endregion
	}
}