using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class CreateMediaPersonalityCommand : MediaPersonalityDataCommandBase, ICreateMediaPersonalityCommand
	{
		#region Constructor

		public CreateMediaPersonalityCommand(Guid mediaPersonalityIdentifier, string givenName, string middleName, string surname, int nationalityIdentifier, DateTime? birthDate, DateTime? dateOfDead, string url, byte[] image) 
			: base(mediaPersonalityIdentifier, givenName, middleName, surname, nationalityIdentifier, birthDate, dateOfDead, url, image)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => false;

		protected override bool ShouldBeUnknownValue => true;

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.Object.ShouldBeUnknownValue<ICreateMediaPersonalityCommand>(this, createMediaPersonalityCommand => createMediaPersonalityCommand.IsNonExistingFullNameAsync(mediaLibraryRepository), GetType(), $"{nameof(GivenName)},{nameof(MiddleName)},{nameof(Surname)},{nameof(BirthDate)}");
		}

		#endregion
	}
}