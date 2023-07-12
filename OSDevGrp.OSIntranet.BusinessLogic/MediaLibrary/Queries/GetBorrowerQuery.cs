using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal class GetBorrowerQuery : BorrowerIdentificationQueryBase, IGetBorrowerQuery
	{
		#region Constructor

		public GetBorrowerQuery(Guid borrowerIdentifier) 
			: base(borrowerIdentifier)
		{
		}

		#endregion
	}
}