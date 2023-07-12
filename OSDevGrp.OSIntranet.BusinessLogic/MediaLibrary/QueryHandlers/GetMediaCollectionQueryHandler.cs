using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
	internal class GetMediaCollectionQueryHandler : MediaLibraryFilterQueryHandlerBase<IGetMediaCollectionQuery, IMedia>
	{
		#region Constructor

		public GetMediaCollectionQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository) 
			: base(validator, mediaLibraryRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task<IEnumerable<IMedia>> GetResultAsync(IGetMediaCollectionQuery query)
		{
			NullGuard.NotNull(query, nameof(query));

			return MediaLibraryRepository.GetMediasAsync(query.Filter);
		}

		#endregion
	}
}