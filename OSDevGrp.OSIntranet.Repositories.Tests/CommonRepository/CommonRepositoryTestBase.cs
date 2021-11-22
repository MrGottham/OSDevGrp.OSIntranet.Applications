using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    public abstract class CommonRepositoryTestBase : DatabaseRepositoryTestBase
    {
        protected ICommonRepository CreateSut()
        {
            return new Repositories.CommonRepository(CreateTestRepositoryContext(), CreateTestConfiguration(), CreateLoggerFactory());
        }
    }
}