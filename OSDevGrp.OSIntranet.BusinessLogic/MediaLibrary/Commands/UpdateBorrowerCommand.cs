using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class UpdateBorrowerCommand : BorrowerDataCommandBase, IUpdateBorrowerCommand
	{
		#region Constructor

		public UpdateBorrowerCommand(Guid borrowerIdentifier, string fullName, string mailAddress, string primaryPhone, string secondaryPhone, int lendingLimit) 
			: base(borrowerIdentifier, fullName, mailAddress, primaryPhone, secondaryPhone, lendingLimit)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => true;

		protected override bool ShouldBeUnknownValue => false;

		#endregion
	}
}