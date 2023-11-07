using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetLendingQuery : LendingIdentificationQueryBase, IGetLendingQuery
	{
		#region Constructor

		public GetLendingQuery(Guid lendingIdentifier) 
			: base(lendingIdentifier)
		{
		}

		#endregion
	}
}