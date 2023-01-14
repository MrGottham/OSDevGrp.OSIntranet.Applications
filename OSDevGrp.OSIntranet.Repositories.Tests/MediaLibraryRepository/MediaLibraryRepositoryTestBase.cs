using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MediaLibraryRepository
{
    public class MediaLibraryRepositoryTestBase : DatabaseRepositoryTestBase
    {
        #region Methods

        protected IMediaLibraryRepository CreateSut()
        {
            return new Repositories.MediaLibraryRepository(CreateTestRepositoryContext(), CreateTestConfiguration(), CreateLoggerFactory());
        }

        #endregion
    }
}