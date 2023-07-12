using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
	internal class GetLendingCollectionQueryHandler : MediaLibraryQueryHandlerBase<IGetLendingCollectionQuery, IEnumerable<ILending>>
	{
		#region Constructor

		public GetLendingCollectionQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository) 
			: base(validator, mediaLibraryRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task<IEnumerable<ILending>> GetResultAsync(IGetLendingCollectionQuery query)
		{
			NullGuard.NotNull(query, nameof(query));

			return MediaLibraryRepository.GetLendingsAsync(query.IncludeReturned);
		}

		protected override IEnumerable<ILending> GetDefault() => Array.Empty<ILending>();

		#endregion
	}
}