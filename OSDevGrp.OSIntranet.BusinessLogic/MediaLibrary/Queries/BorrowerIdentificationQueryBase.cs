using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal abstract class BorrowerIdentificationQueryBase : MediaLibraryQueryBase, IBorrowerIdentificationQuery
	{
		#region Constructor

		protected BorrowerIdentificationQueryBase(Guid borrowerIdentifier)
		{
			BorrowerIdentifier = borrowerIdentifier;
		}

		#endregion

		#region Properties

		public Guid BorrowerIdentifier { get; }

		#endregion
	}
}