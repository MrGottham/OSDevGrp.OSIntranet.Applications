using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
	internal abstract class MediaLibraryQueryHandlerBase<TMediaLibraryQuery, TResult> : IQueryHandler<TMediaLibraryQuery, TResult> where TMediaLibraryQuery : IMediaLibraryQuery
	{
		#region Constructor

		protected MediaLibraryQueryHandlerBase(IValidator validator, IMediaLibraryRepository mediaLibraryRepository)
		{
			NullGuard.NotNull(validator, nameof(validator))
				.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

			Validator = validator;
			MediaLibraryRepository = mediaLibraryRepository;
		}

		#endregion

		#region Properties

		protected IValidator Validator { get; }

		protected IMediaLibraryRepository MediaLibraryRepository { get; }

		#endregion

		#region Methods

		public async Task<TResult> QueryAsync(TMediaLibraryQuery query)
		{
			NullGuard.NotNull(query, nameof(query));

			query.Validate(Validator, MediaLibraryRepository);

			TResult result = await GetResultAsync(query);

			return result ?? GetDefault();
		}

		protected abstract Task<TResult> GetResultAsync(TMediaLibraryQuery query);

		protected virtual TResult GetDefault() => default;

		#endregion
	}
}