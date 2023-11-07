using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
	internal abstract class MediaLibraryFilterQueryHandlerBase<TMediaLibraryFilterQuery, TResult> : MediaLibraryQueryHandlerBase<TMediaLibraryFilterQuery, IEnumerable<TResult>> where TMediaLibraryFilterQuery : IMediaLibraryFilterQuery
	{
		#region Constructor

		protected MediaLibraryFilterQueryHandlerBase(IValidator validator, IMediaLibraryRepository mediaLibraryRepository) 
			: base(validator, mediaLibraryRepository)
		{
		}

		#endregion

		#region Methods

		protected sealed override IEnumerable<TResult> GetDefault() => Array.Empty<TResult>();

		#endregion
	}
}