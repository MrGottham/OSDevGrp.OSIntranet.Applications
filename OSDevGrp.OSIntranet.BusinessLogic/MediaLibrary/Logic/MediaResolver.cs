using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic
{
	internal static class MediaResolver
	{
		internal static async Task<IMedia> ResolveAsync(Guid mediaIdentifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return await mediaLibraryRepository.GetMediaAsync<IMovie>(mediaIdentifier) ??
			       await mediaLibraryRepository.GetMediaAsync<IMusic>(mediaIdentifier) ??
			       await mediaLibraryRepository.GetMediaAsync<IBook>(mediaIdentifier) as IMedia;
		}
	}
}