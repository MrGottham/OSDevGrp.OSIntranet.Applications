using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetMediaPersonalityCollectionQuery : MediaLibraryFilterQueryBase, IGetMediaPersonalityCollectionQuery
	{
		#region Constructor

		public GetMediaPersonalityCollectionQuery(string filter) 
			: base(filter, false)
		{
		}

		#endregion

		#region Properties

		protected override int MaxLength => 32;

		#endregion
	}
}