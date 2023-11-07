using OSDevGrp.OSIntranet.BusinessLogic.Core.QueryHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
    internal class GetMovieGenreCollectionQueryHandler : GetGenericCategoryCollectionQueryHandlerBase<IMovieGenre>
    {
        #region Private variables

        private readonly IMediaLibraryRepository _mediaLibraryRepository;

        #endregion

        #region Constructor

        public GetMovieGenreCollectionQueryHandler(IMediaLibraryRepository mediaLibraryRepository)
        {
            NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

            _mediaLibraryRepository = mediaLibraryRepository;
        }

        #endregion

        #region Methods

        protected override Task<IEnumerable<IMovieGenre>> ReadFromRepository() => _mediaLibraryRepository.GetMovieGenresAsync();

        #endregion
    }
}