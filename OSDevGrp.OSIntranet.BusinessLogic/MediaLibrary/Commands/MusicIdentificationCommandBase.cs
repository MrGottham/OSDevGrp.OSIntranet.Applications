﻿using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class MusicIdentificationCommandBase : MediaIdentificationCommandBase, IMusicIdentificationCommand
	{
		#region Constructor

		protected MusicIdentificationCommandBase(Guid mediaIdentifier) 
			: base(mediaIdentifier)
		{
		}

		#endregion

		#region Methods

		protected sealed override Task<bool> IsExistingIdentifierAsync(Guid mediaIdentifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return mediaLibraryRepository.MediaExistsAsync<IMusic>(mediaIdentifier);
		}

		#endregion
	}
}