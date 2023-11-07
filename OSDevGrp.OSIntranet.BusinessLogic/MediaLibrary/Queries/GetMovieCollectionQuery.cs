using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetMovieCollectionQuery : GetMediaCollectionQueryBase, IGetMovieCollectionQuery
	{
		#region Constructor

		public GetMovieCollectionQuery(string filter) 
			: base(filter)
		{
		}

		#endregion
	}
}