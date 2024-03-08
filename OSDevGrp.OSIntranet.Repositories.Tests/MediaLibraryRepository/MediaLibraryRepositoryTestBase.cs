using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    public class MediaLibraryRepositoryTestBase : DatabaseRepositoryTestBase
    {
	    #region Private variables

        private static IOptions<MediaLibraryRepositoryTestOptions> _mediaLibraryRepositoryTestOptions;

        #endregion

        #region Methods

        protected IMediaLibraryRepository CreateSut()
        {
            return new Repositories.MediaLibraryRepository(CreateTestRepositoryContext(), CreateLoggerFactory());
        }

		protected Guid WithExistingMediaPersonalityIdentifier()
        {
            return GetMediaLibraryRepositoryTestOptions().Value.ExistingMediaPersonalityIdentifier;
		}

		protected Guid WithExistingMovieIdentifier()
		{
            return GetMediaLibraryRepositoryTestOptions().Value.ExistingMovieIdentifier;
		}

        protected Guid WithExistingMusicIdentifier()
		{
            return GetMediaLibraryRepositoryTestOptions().Value.ExistingMusicIdentifier;
		}

        protected Guid WithExistingBookIdentifier()
		{
            return GetMediaLibraryRepositoryTestOptions().Value.ExistingBookIdentifier;
		}

        protected Guid? WithExistingBorrowerIdentifier()
		{
            return GetMediaLibraryRepositoryTestOptions().Value.ExistingBorrowerIdentifier;
		}

        protected Guid? WithExistingLendingIdentifier()
		{
            return GetMediaLibraryRepositoryTestOptions().Value.ExistingLendingIdentifier;
		}

        private IOptions<MediaLibraryRepositoryTestOptions> GetMediaLibraryRepositoryTestOptions()
        {
            lock (SyncRoot)
            {
                if (_mediaLibraryRepositoryTestOptions != null)
                {
                    return _mediaLibraryRepositoryTestOptions;
                }

                return _mediaLibraryRepositoryTestOptions = Microsoft.Extensions.Options.Options.Create(CreateTestConfiguration().GetSection("TestData:MediaLibrary").Get<MediaLibraryRepositoryTestOptions>());
            }
        }

        #endregion
    }
}