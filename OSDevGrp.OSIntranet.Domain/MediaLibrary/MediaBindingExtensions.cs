using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Domain.MediaLibrary
{
	internal static class MediaBindingExtensions
	{
		#region Methods

		internal static string CalculateKey(this IMediaBinding mediaBinding)
		{
			NullGuard.NotNull(mediaBinding, nameof(mediaBinding));

			return $"{mediaBinding.Media.MediaIdentifier}|{mediaBinding.Role}|{mediaBinding.MediaPersonality.MediaPersonalityIdentifier}";
		}

		internal static IEnumerable<IMediaPersonality> Filter(this IEnumerable<IMediaBinding> mediaBindings, MediaRole role)
		{
			NullGuard.NotNull(mediaBindings, nameof(mediaBindings));

			return mediaBindings.Where(mediaBinding => mediaBinding.Role == role)
				.Select(mediaBinding => mediaBinding.MediaPersonality)
				.OrderBy(mediaPersonality => mediaPersonality.ToString())
				.ToArray();
		}

		#endregion
	}
}