using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal sealed class DeleteLendingCommand : LendingIdentificationCommandBase, IDeleteLendingCommand
	{
		#region Constructor

		public DeleteLendingCommand(Guid lendingIdentifier) 
			: base(lendingIdentifier)
		{
		}

		#endregion

		#region Properties

		protected override bool ShouldBeKnownValue => true;

		protected override bool ShouldBeUnknownValue => false;

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			return base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository)
				.Object.ShouldBeDeletable(LendingIdentifier, mediaLibraryRepository.GetLendingAsync, GetType(), nameof(LendingIdentifier));
		}

		#endregion
	}
}