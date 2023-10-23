﻿using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic
{
	internal static class MediaDataCommandExtensions
	{
		#region Methods

		internal static Task<bool> IsExistingTitleAsync<TMediaDataCommand, TMedia>(this TMediaDataCommand mediaDataCommand, IMediaLibraryRepository mediaLibraryRepository) where TMediaDataCommand : IMediaDataCommand<TMedia> where TMedia : class, IMedia
		{
			NullGuard.NotNull(mediaDataCommand, nameof(mediaDataCommand))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return IsExistingTitleAsync<TMedia>(mediaDataCommand.Title, mediaDataCommand.Subtitle, mediaLibraryRepository);
		}

		internal static async Task<bool> IsNonExistingTitleAsync<TMediaDataCommand, TMedia>(this TMediaDataCommand mediaDataCommand, IMediaLibraryRepository mediaLibraryRepository) where TMediaDataCommand : IMediaDataCommand<TMedia> where TMedia : class, IMedia
		{
			NullGuard.NotNull(mediaDataCommand, nameof(mediaDataCommand))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			string title = mediaDataCommand.Title;
			string subtitle = mediaDataCommand.Subtitle;

			return await IsExistingTitleAsync<IMovie>(title, subtitle, mediaLibraryRepository) == false &&
			       await IsExistingTitleAsync<IMusic>(title, subtitle, mediaLibraryRepository) == false &&
			       await IsExistingTitleAsync<IBook>(title, subtitle, mediaLibraryRepository) == false;
		}

		private static Task<bool> IsExistingTitleAsync<TMedia>(string title, string subtitle, IMediaLibraryRepository mediaLibraryRepository) where TMedia : class, IMedia
		{
			NullGuard.NotNullOrWhiteSpace(title, nameof(title))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return mediaLibraryRepository.MediaExistsAsync<TMedia>(title, subtitle);
		}

		#endregion
	}
}