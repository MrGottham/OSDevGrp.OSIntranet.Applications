using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic
{
	internal static class MediaIdentifierHelper
	{
		#region Methods

		internal static async Task<bool> IsExistingMediaIdentifierAsync(this Guid mediaIdentifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return await mediaLibraryRepository.MediaExistsAsync<IMovie>(mediaIdentifier) ||
			       await mediaLibraryRepository.MediaExistsAsync<IMusic>(mediaIdentifier) ||
			       await mediaLibraryRepository.MediaExistsAsync<IBook>(mediaIdentifier);
		}

		internal static async Task<bool> IsNonExistingMediaIdentifierAsync(this Guid mediaIdentifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return await mediaLibraryRepository.MediaExistsAsync<IMovie>(mediaIdentifier) == false &&
			       await mediaLibraryRepository.MediaExistsAsync<IMusic>(mediaIdentifier) == false &&
			       await mediaLibraryRepository.MediaExistsAsync<IBook>(mediaIdentifier) == false;
		}

		#endregion
	}
}