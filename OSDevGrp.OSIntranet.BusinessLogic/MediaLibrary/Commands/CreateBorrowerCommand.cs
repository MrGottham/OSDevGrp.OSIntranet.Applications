using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class CreateBorrowerCommand : BorrowerDataCommandBase, ICreateBorrowerCommand
	{
		#region Constructor

		public CreateBorrowerCommand(Guid borrowerIdentifier, string fullName, string mailAddress, string primaryPhone, string secondaryPhone, int lendingLimit) 
			: base(borrowerIdentifier, fullName, mailAddress, primaryPhone, secondaryPhone, lendingLimit)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => false;

		protected override bool ShouldBeUnknownValue => true;

		#endregion
	}
}