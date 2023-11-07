using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal abstract class MediaModelBase : AuditModelBase
    {
        public virtual string ExternalMediaIdentifier { get; set; }

        public virtual MediaCoreDataModel CoreData { get; set; }

        public virtual bool Deletable { get; set; }

		public virtual List<LendingModel> Lendings { get; set; }
    }

	internal static class MediaModelBaseExtensions
	{
		#region Methods

		internal static void ApplyAuditInformation<TMediaModel>(this TMediaModel mediaModel, Func<TMediaModel, IEnumerable<MediaBindingModelBase>> mediaBindingModelsGetter, IAuditable auditable) where TMediaModel : MediaModelBase
		{
			NullGuard.NotNull(mediaModel, nameof(mediaModel))
				.NotNull(mediaBindingModelsGetter, nameof(mediaBindingModelsGetter))
				.NotNull(auditable, nameof(auditable));

			DateTime modifiedUtcDateTime = mediaModel.ModifiedUtcDateTime;
			string modifiedByIdentifier = mediaModel.ModifiedByIdentifier;

			MediaBindingModelBase latestModifiedMediaBinding = (mediaBindingModelsGetter(mediaModel) ?? Array.Empty<MediaBindingModelBase>()).GetLatestModifiedMediaBinding();
			if (latestModifiedMediaBinding != null && latestModifiedMediaBinding.ModifiedUtcDateTime > modifiedUtcDateTime)
			{
				modifiedUtcDateTime = latestModifiedMediaBinding.ModifiedUtcDateTime;
				modifiedByIdentifier = latestModifiedMediaBinding.ModifiedByIdentifier;
			}

			LendingModel latestModifiedLending = (mediaModel.Lendings ?? new List<LendingModel>(0)).GetLatestModifiedLendingModel();
			if (latestModifiedLending != null && latestModifiedLending.ModifiedUtcDateTime > modifiedUtcDateTime)
			{
				modifiedUtcDateTime = latestModifiedLending.ModifiedUtcDateTime;
				modifiedByIdentifier = latestModifiedLending.ModifiedByIdentifier;
			}

			auditable.AddAuditInformation(mediaModel.CreatedUtcDateTime, mediaModel.CreatedByIdentifier, modifiedUtcDateTime, modifiedByIdentifier);
		}

		#endregion
	}
}