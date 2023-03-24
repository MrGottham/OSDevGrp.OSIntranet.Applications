using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	internal static class MediaPersonalityExtensions
	{
		#region Methods

		internal static IMediaBinding AsDirector(this IMediaPersonality mediaPersonality, IMedia media)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality))
				.NotNull(media, nameof(media));

			return mediaPersonality.AsBinding(media, MediaRole.Director);
		}

		internal static IMediaBinding AsActor(this IMediaPersonality mediaPersonality, IMedia media)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality))
				.NotNull(media, nameof(media));

			return mediaPersonality.AsBinding(media, MediaRole.Actor);
		}

		internal static IMediaBinding AsArtist(this IMediaPersonality mediaPersonality, IMedia media)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality))
				.NotNull(media, nameof(media));

			return mediaPersonality.AsBinding(media, MediaRole.Artist);
		}

		internal static IMediaBinding AsAuthor(this IMediaPersonality mediaPersonality, IMedia media)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality))
				.NotNull(media, nameof(media));

			return mediaPersonality.AsBinding(media, MediaRole.Author);
		}

		private static IMediaBinding AsBinding(this IMediaPersonality mediaPersonality, IMedia media, MediaRole role)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality))
				.NotNull(media, nameof(media));

			return new MediaBinding(media, role, mediaPersonality);
		}

		#endregion
	}
}