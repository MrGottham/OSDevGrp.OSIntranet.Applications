using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetMediaCollectionQuery : GetMediaCollectionQueryBase, IGetMediaCollectionQuery
	{
		#region Constructor

		public GetMediaCollectionQuery(string filter) 
			: base(filter)
		{
		}

		#endregion
	}
}