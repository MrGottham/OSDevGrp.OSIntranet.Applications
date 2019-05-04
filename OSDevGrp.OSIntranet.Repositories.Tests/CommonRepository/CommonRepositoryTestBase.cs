using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.CommonRepository
{
    public abstract class CommonRepositoryTestBase : RepositoryTestBase
    {
        protected ICommonRepository CreateSut()
        {
            return new Repositories.CommonRepository(CreateTestConfiguration(), CreatePrincipalResolverMock().Object);
        }
    }
}