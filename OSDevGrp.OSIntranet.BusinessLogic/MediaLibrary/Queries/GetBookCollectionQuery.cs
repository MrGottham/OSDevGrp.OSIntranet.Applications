using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetBookCollectionQuery : GetMediaCollectionQueryBase, IGetBookCollectionQuery
	{
		#region Constructor

		public GetBookCollectionQuery(string filter) 
			: base(filter)
		{
		}

		#endregion
	}
}