using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ExternalDashboardRepository
{
    public abstract class ExternalDashboardRepositoryBase : RepositoryTestBase
    {
        #region Methods

        protected IExternalDashboardRepository CreateSut()
        {
            return new Repositories.ExternalDashboardRepository(CreateTestConfiguration(), CreatePrincipalResolverMock().Object, CreateLoggerFactory());
        }

        #endregion
    }
}