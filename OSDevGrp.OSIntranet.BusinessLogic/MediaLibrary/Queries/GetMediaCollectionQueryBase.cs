namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal abstract class GetMediaCollectionQueryBase : MediaLibraryFilterQueryBase
	{
		#region Constructor

		protected GetMediaCollectionQueryBase(string filter) 
			: base(filter, true)
		{
		}

		#endregion
	}
}