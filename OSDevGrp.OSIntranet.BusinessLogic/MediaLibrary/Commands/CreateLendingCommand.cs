using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class CreateLendingCommand : LendingDataCommandBase, ICreateLendingCommand
	{
		#region Constructor

		public CreateLendingCommand(Guid lendingIdentifier, Guid borrowerIdentifier, Guid mediaIdentifier, DateTime lendingDate, DateTime recallDate, DateTime? returnedDate) 
			: base(lendingIdentifier, borrowerIdentifier, mediaIdentifier, lendingDate, recallDate, returnedDate)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => false;

		protected override bool ShouldBeUnknownValue => true;

		#endregion
	}
}