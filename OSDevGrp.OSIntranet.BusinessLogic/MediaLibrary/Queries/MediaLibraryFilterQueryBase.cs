using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries
{
	internal abstract class MediaLibraryFilterQueryBase : MediaLibraryQueryBase, IMediaLibraryFilterQuery
	{
		#region Constructor

		protected MediaLibraryFilterQueryBase(string filter, bool asUpperCase = false)
		{
			if (string.IsNullOrWhiteSpace(filter))
			{
				Filter = null;
				return;
			}

			Filter = asUpperCase
				? filter.Trim().ToUpper()
				: filter.Trim();
		}

		#endregion

		#region Properties

		public string Filter { get; }

		protected virtual int MinLength => 1;

		protected virtual int MaxLength => 256;

		#endregion

		#region Methods

		public sealed override IValidator Validate(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			return base.Validate(validator, mediaLibraryRepository)
				.String.ShouldHaveMinLength(Filter, MinLength, GetType(), nameof(Filter), true)
				.String.ShouldHaveMaxLength(Filter, MaxLength, GetType(), nameof(Filter), true);
		}

		#endregion
	}
}