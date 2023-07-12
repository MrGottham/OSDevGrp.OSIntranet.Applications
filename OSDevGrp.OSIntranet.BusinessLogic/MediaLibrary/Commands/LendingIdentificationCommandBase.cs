using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class LendingIdentificationCommandBase : MediaLibraryIdentificationCommandBase, ILendingIdentificationCommand
	{
		#region Constructor

		protected LendingIdentificationCommandBase(Guid lendingIdentifier)
		{
			LendingIdentifier = lendingIdentifier;
		}

		#endregion

		#region Properties

		public Guid LendingIdentifier { get; }

		#endregion

		#region Methods

		protected sealed override bool EvaluateNecessaryPermission(IClaimResolver claimResolver)
		{
			NullGuard.NotNull(claimResolver, nameof(claimResolver));

			return claimResolver.IsMediaLibraryLender();
		}

		protected sealed override Guid GetIdentifier() => LendingIdentifier;

		protected sealed override string GetIdentifierName() => nameof(LendingIdentifier);

		protected sealed override Task<bool> IsIdentifierExisting(Guid lendingIdentifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return mediaLibraryRepository.LendingExistsAsync(lendingIdentifier);
		}

		#endregion
	}
}