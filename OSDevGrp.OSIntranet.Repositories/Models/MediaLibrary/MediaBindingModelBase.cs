using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
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

		internal static IEnumerable<IMediaPersonality> AsMediaPersonalities(this IEnumerable<MediaBindingModelBase> mediaBindingModels, MediaRole mediaRole, MapperCache mapperCache, IConverter mediaLibraryModelConverter, IConverter commonModelConverter)
		{
			NullGuard.NotNull(mediaBindingModels, nameof(mediaBindingModels))
				.NotNull(mapperCache, nameof(mapperCache))
				.NotNull(mediaLibraryModelConverter, nameof(mediaLibraryModelConverter))
				.NotNull(commonModelConverter, nameof(commonModelConverter));

			return mediaBindingModels.Where(mediaBindingModel => mediaBindingModel.MediaPersonality != null && mediaBindingModel.AsMediaRole() == mediaRole)
				.Select(mediaBindingModel => mediaBindingModel.MediaPersonality.ToDomain(mapperCache, mediaLibraryModelConverter, commonModelConverter));
		}

		#endregion
	}
}