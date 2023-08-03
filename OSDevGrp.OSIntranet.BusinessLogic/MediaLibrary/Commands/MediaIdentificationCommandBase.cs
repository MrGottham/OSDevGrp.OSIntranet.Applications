using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class MediaIdentificationCommandBase : MediaLibraryIdentificationCommandBase, IMediaIdentificationCommand
	{
		#region Constructor

		protected MediaIdentificationCommandBase(Guid mediaIdentifier)
		{
			MediaIdentifier = mediaIdentifier;
		}

		#endregion

		#region Properties

		public Guid MediaIdentifier { get; }

		#endregion

		#region Methods

		protected sealed override Guid GetIdentifier() => MediaIdentifier;

		protected sealed override string GetIdentifierName() => nameof(MediaIdentifier);

		protected sealed override async Task<bool> IsNonExistingIdentifier(Guid mediaIdentifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return await mediaLibraryRepository.MediaExistsAsync<IMovie>(mediaIdentifier) == false &&
			       await mediaLibraryRepository.MediaExistsAsync<IMusic>(mediaIdentifier) == false &&
			       await mediaLibraryRepository.MediaExistsAsync<IBook>(mediaIdentifier) == false;
		}

		#endregion
	}
}