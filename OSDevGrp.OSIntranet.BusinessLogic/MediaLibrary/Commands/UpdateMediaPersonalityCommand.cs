using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class UpdateMediaPersonalityCommand : MediaPersonalityDataCommandBase, IUpdateMediaPersonalityCommand
	{
		#region Constructor

		public UpdateMediaPersonalityCommand(Guid mediaPersonalityIdentifier, string givenName, string middleName, string surname, int nationalityIdentifier, DateTime? birthDate, DateTime? dateOfDead, string url, byte[] image) 
			: base(mediaPersonalityIdentifier, givenName, middleName, surname, nationalityIdentifier, birthDate, dateOfDead, url, image)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => true;

		protected override bool ShouldBeUnknownValue => false;

		#endregion
	}
}