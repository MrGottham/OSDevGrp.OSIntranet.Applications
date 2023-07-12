using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
	internal class GetMusicCollectionQueryHandler : MediaLibraryFilterQueryHandlerBase<IGetMusicCollectionQuery, IMusic>
	{
		#region Constructor

		public GetMusicCollectionQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
			: base(validator, mediaLibraryRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task<IEnumerable<IMusic>> GetResultAsync(IGetMusicCollectionQuery query)
		{
			NullGuard.NotNull(query, nameof(query));

			return MediaLibraryRepository.GetMediasAsync<IMusic>(query.Filter);
		}

		#endregion
	}
}