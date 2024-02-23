using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary.Enums;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal abstract class MediaBindingModelBase : AuditModelBase
	{
		public virtual int MediaPersonalityIdentifier { get; set; }

		public virtual MediaPersonalityModel MediaPersonality { get; set; }

		public virtual short Role { get; set; }

		public virtual bool Deletable { get; set; }
	}

	internal static class MediaBindingModelBaseExtensions
	{
		#region Methods

		internal static MediaRole AsMediaRole(this MediaBindingModelBase mediaBindingModelBase)
		{
			NullGuard.NotNull(mediaBindingModelBase, nameof(mediaBindingModelBase));

			return (MediaRole)mediaBindingModelBase.Role;
		}

		internal static MediaBindingModelBase GetLatestModifiedMediaBinding(this IEnumerable<MediaBindingModelBase> mediaBindingModels)
		{
			NullGuard.NotNull(mediaBindingModels, nameof(mediaBindingModels));

			return mediaBindingModels.MaxBy(mediaBinding => mediaBinding.ModifiedUtcDateTime);
		}

		internal static bool IsMatchingMediaBindingModel<TMediaBindingModel>(this TMediaBindingModel mediaBindingModel, IMediaBinding mediaBinding) where TMediaBindingModel : MediaBindingModelBase
		{
			NullGuard.NotNull(mediaBindingModel, nameof(mediaBindingModel))
				.NotNull(mediaBinding, nameof(mediaBinding));

			return mediaBindingModel.MediaPersonality != null &&
			       mediaBindingModel.MediaPersonality.ExternalMediaPersonalityIdentifier == mediaBinding.MediaPersonality.MediaPersonalityIdentifier &&
			       mediaBindingModel.AsMediaRole() == mediaBinding.Role;
		}

		internal static TMediaBindingModel FindMatchingMediaBindingModel<TMediaBindingModel>(this IEnumerable<TMediaBindingModel> mediaBindingModels, IMediaBinding mediaBinding) where TMediaBindingModel : MediaBindingModelBase
		{
			NullGuard.NotNull(mediaBindingModels, nameof(mediaBindingModels))
				.NotNull(mediaBinding, nameof(mediaBinding));

			return mediaBindingModels.SingleOrDefault(mediaBindingModel => mediaBindingModel.IsMatchingMediaBindingModel(mediaBinding));
		}

		#endregion
	}
}