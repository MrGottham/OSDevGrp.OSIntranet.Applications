using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
	internal class GetLendingQueryHandler : MediaLibraryQueryHandlerBase<IGetLendingQuery, ILending>
	{
		#region Constructor

		public GetLendingQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository) 
			: base(validator, mediaLibraryRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task<ILending> GetResultAsync(IGetLendingQuery query)
		{
			NullGuard.NotNull(query, nameof(query));

			return MediaLibraryRepository.GetLendingAsync(query.LendingIdentifier);
		}

		#endregion
	}
}