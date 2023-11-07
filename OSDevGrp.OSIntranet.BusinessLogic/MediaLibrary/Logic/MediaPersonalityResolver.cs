using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Logic
{
	internal static class MediaPersonalityResolver
	{
		#region Methods

		internal static async Task<IEnumerable<IMediaPersonality>> ResolveAsync(IEnumerable<Guid> mediaPersonalityIdentifiers, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaPersonalityIdentifiers, nameof(mediaPersonalityIdentifiers))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			IDictionary<Guid, IMediaPersonality> mediaPersonalities = new ConcurrentDictionary<Guid, IMediaPersonality>();
			foreach (Guid mediaPersonalityIdentifier in mediaPersonalityIdentifiers)
			{
				if (mediaPersonalities.ContainsKey(mediaPersonalityIdentifier))
				{
					continue;
				}

				IMediaPersonality mediaPersonality = await mediaLibraryRepository.GetMediaPersonalityAsync(mediaPersonalityIdentifier);
				if (mediaPersonality == null)
				{
					continue;
				}

				mediaPersonalities.Add(mediaPersonality.MediaPersonalityIdentifier, mediaPersonality);
			}

			return new HashSet<IMediaPersonality>(mediaPersonalities.Values);
		}

		#endregion
	}
}