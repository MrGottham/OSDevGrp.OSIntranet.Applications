using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic
{
	internal static class MediaPersonalityDataCommandExtensions
	{
		#region Methods

		internal static Task<bool> IsExistingFullNameAsync(this IMediaPersonalityDataCommand mediaPersonalityDataCommand, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaPersonalityDataCommand, nameof(mediaPersonalityDataCommand))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return mediaLibraryRepository.MediaPersonalityExistsAsync(mediaPersonalityDataCommand.GivenName, mediaPersonalityDataCommand.MiddleName, mediaPersonalityDataCommand.Surname, mediaPersonalityDataCommand.BirthDate);
		}

		internal static async Task<bool> IsNonExistingFullNameAsync(this IMediaPersonalityDataCommand mediaPersonalityDataCommand, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaPersonalityDataCommand, nameof(mediaPersonalityDataCommand))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return await IsExistingFullNameAsync(mediaPersonalityDataCommand, mediaLibraryRepository) == false;
		}

		#endregion
	}
}