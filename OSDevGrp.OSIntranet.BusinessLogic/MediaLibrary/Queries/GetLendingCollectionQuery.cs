using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetLendingCollectionQuery : MediaLibraryQueryBase, IGetLendingCollectionQuery
	{
		#region Constructor

		public GetLendingCollectionQuery(bool includeReturned)
		{
			IncludeReturned = includeReturned;
		}

		#endregion

		#region Properties

		public bool IncludeReturned { get; }

		#endregion
	}
}