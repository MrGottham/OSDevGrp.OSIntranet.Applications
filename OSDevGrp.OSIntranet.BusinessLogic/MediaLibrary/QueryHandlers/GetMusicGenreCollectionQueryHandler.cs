using OSDevGrp.OSIntranet.BusinessLogic.Core.QueryHandlers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.QueryHandlers
{
    internal class GetMusicGenreCollectionQueryHandler : GetGenericCategoryCollectionQueryHandlerBase<IMusicGenre>
    {
        #region Private variables

        private readonly IMediaLibraryRepository _mediaLibraryRepository;

        #endregion

        #region Constructor

        public GetMusicGenreCollectionQueryHandler(IMediaLibraryRepository mediaLibraryRepository)
        {
            NullGuard.NotNull(mediaLibraryRepository, nameof(mediaLibraryRepository));

            _mediaLibraryRepository = mediaLibraryRepository;
        }

        #endregion

        #region Methods

        protected override Task<IEnumerable<IMusicGenre>> ReadFromRepository() => _mediaLibraryRepository.GetMusicGenresAsync();

        #endregion
    }
}