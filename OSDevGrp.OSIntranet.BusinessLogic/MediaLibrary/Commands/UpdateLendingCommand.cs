using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class UpdateLendingCommand : LendingDataCommandBase, IUpdateLendingCommand
	{
		#region Constructor

		public UpdateLendingCommand(Guid lendingIdentifier, Guid borrowerIdentifier, Guid mediaIdentifier, DateTime lendingDate, DateTime recallDate, DateTime? returnedDate) 
			: base(lendingIdentifier, borrowerIdentifier, mediaIdentifier, lendingDate, recallDate, returnedDate)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => true;

		protected override bool ShouldBeUnknownValue => false;

		#endregion
	}
}