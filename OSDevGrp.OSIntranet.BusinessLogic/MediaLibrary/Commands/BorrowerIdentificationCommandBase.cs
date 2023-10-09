using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class BorrowerIdentificationCommandBase : MediaLibraryIdentificationCommandBase, IBorrowerIdentificationCommand
	{
		#region Constructor

		protected BorrowerIdentificationCommandBase(Guid borrowerIdentifier)
		{
			BorrowerIdentifier = borrowerIdentifier;
		}

		#endregion

		#region Properties

		public Guid BorrowerIdentifier { get; }

		#endregion

		#region Methods

		protected sealed override bool EvaluateNecessaryPermission(IClaimResolver claimResolver)
		{
			NullGuard.NotNull(claimResolver, nameof(claimResolver));

			return claimResolver.IsMediaLibraryLender();
		}

		protected sealed override Guid GetIdentifier() => BorrowerIdentifier;

		protected sealed override string GetIdentifierName() => nameof(BorrowerIdentifier);

		protected sealed override Task<bool> IsExistingIdentifierAsync(Guid borrowerIdentifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return mediaLibraryRepository.BorrowerExistsAsync(borrowerIdentifier);
		}

		#endregion
	}
}