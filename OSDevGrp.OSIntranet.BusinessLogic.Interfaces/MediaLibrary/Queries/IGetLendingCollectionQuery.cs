namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries
{
	public interface IGetLendingCollectionQuery : IMediaLibraryQuery
	{
		bool IncludeReturned { get; }
	}
}