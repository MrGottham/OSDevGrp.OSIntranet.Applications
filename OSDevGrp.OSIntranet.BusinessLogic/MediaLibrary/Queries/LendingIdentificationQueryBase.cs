using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal abstract class LendingIdentificationQueryBase : MediaLibraryQueryBase, ILendingIdentificationQuery
	{
		#region Constructor

		protected LendingIdentificationQueryBase(Guid lendingIdentifier)
		{
			LendingIdentifier = lendingIdentifier;
		}

		#endregion

		#region Properties

		public Guid LendingIdentifier { get; }

		#endregion
	}
}