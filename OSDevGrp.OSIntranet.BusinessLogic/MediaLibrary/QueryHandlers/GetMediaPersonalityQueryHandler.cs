using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
	internal class GetMediaPersonalityQueryHandler : MediaLibraryQueryHandlerBase<IGetMediaPersonalityQuery, IMediaPersonality>
	{
		#region Constructor

		public GetMediaPersonalityQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository) 
			: base(validator, mediaLibraryRepository)
		{
		}

		#endregion

		#region Methods

		protected override Task<IMediaPersonality> GetResultAsync(IGetMediaPersonalityQuery query)
		{
			NullGuard.NotNull(query, nameof(query));

			return MediaLibraryRepository.GetMediaPersonalityAsync(query.MediaPersonalityIdentifier);
		}

		#endregion
	}
}