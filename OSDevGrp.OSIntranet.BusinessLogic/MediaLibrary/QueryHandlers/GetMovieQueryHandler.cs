using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
	internal class GetMovieQueryHandler : MediaLibraryQueryHandlerBase<IGetMovieQuery, IMovie>
	{
		#region Constructor

		public GetMovieQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
			: base(validator, mediaLibraryRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task<IMovie> GetResultAsync(IGetMovieQuery query)
		{
			NullGuard.NotNull(query, nameof(query));

			return MediaLibraryRepository.GetMediaAsync<IMovie>(query.MediaIdentifier);
		}

		#endregion
	}
}