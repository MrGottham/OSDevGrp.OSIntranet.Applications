using OSDevGrp.OSIntranet.BusinessLogic.Core.QueryHandlers;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
    internal class GetBookGenreQueryHandler : GetGenericCategoryQueryHandlerBase<IGetBookGenreQuery, IBookGenre>
    {
        #region Private variables

        private readonly IMediaLibraryRepository _mediaLibraryRepository;

        #endregion

        #region Constructor

        public GetBookGenreQueryHandler(IValidator validator, IMediaLibraryRepository mediaLibraryRepository) 
            : base(validator)
        {
            NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

            _mediaLibraryRepository = mediaLibraryRepository;
        }

        #endregion

        #region Methods

        protected override async Task<IBookGenre> ReadFromRepository(IGetBookGenreQuery query)
        {
            NullGuard.NotNull(query, nameof(query));

            return await _mediaLibraryRepository.GetBookGenreAsync(query.Number);
        }

        #endregion
    }
}