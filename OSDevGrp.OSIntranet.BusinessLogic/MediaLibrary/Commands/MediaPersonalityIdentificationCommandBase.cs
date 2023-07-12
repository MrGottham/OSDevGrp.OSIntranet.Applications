using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class MediaPersonalityIdentificationCommandBase : MediaLibraryIdentificationCommandBase, IMediaPersonalityIdentificationCommand
	{
		#region Constructor

		protected MediaPersonalityIdentificationCommandBase(Guid mediaPersonalityIdentifier)
		{
			MediaPersonalityIdentifier = mediaPersonalityIdentifier;
		}

		#endregion

		#region Properties

		public Guid MediaPersonalityIdentifier { get; }

		#endregion

		#region Methods

		protected sealed override Guid GetIdentifier() => MediaPersonalityIdentifier;

		protected sealed override string GetIdentifierName() => nameof(MediaPersonalityIdentifier);

		protected sealed override Task<bool> IsIdentifierExisting(Guid mediaPersonalityIdentifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return mediaLibraryRepository.MediaPersonalityExistsAsync(mediaPersonalityIdentifier);
		}

		#endregion
	}
}