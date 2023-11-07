using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetBorrowerCollectionQuery : MediaLibraryFilterQueryBase, IGetBorrowerCollectionQuery
	{
		#region Constructor

		public GetBorrowerCollectionQuery(string filter) 
			: base(filter, false)
		{
		}

		#endregion
	}
}