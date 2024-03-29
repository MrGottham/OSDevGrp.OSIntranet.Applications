﻿using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Commands
{
	internal abstract class MediaLibraryIdentificationCommandBase : MediaLibraryCommandBase
	{
		#region Properties

		protected virtual bool ShouldBeKnownValue => false;

		protected virtual bool ShouldBeUnknownValue => false;

		#endregion

		#region Methods

		public override IValidator Validate(IValidator validator, IClaimResolver claimResolver, IMediaLibraryRepository mediaLibraryRepository, ICommonRepository commonRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(claimResolver, nameof(claimResolver))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository))
				.NotNull(commonRepository, nameof(commonRepository));

			base.Validate(validator, claimResolver, mediaLibraryRepository, commonRepository);

			if (ShouldBeKnownValue)
			{
				validator.Object.ShouldBeKnownValue(GetIdentifier(), identifier => IsExistingIdentifierAsync(identifier, mediaLibraryRepository), GetType(), GetIdentifierName());
			}

			if (ShouldBeUnknownValue)
			{
				validator.Object.ShouldBeUnknownValue(GetIdentifier(), identifier => IsNonExistingIdentifierAsync(identifier, mediaLibraryRepository), GetType(), GetIdentifierName());
			}

			return validator;
		}

		protected abstract Guid GetIdentifier();

		protected abstract string GetIdentifierName();

		protected abstract Task<bool> IsExistingIdentifierAsync(Guid identifier, IMediaLibraryRepository mediaLibraryRepository);

		protected virtual async Task<bool> IsNonExistingIdentifierAsync(Guid identifier, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return await IsExistingIdentifierAsync(identifier, mediaLibraryRepository) == false;
		}

		#endregion
	}
}