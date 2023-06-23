using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal abstract class MediaLibraryQueryBase : IMediaLibraryQuery
	{
		#region Methods

		public virtual IValidator Validate(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return validator;
		}

		#endregion
	}
}