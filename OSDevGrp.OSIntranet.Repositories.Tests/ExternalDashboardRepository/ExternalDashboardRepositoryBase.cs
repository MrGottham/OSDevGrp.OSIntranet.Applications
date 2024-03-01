using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Options;

namespace OSDevGrp.OSIntranet.Repositories.Tests.ExternalDashboardRepository
{
    public abstract class ExternalDashboardRepositoryBase : RepositoryTestBase
    {
        #region Methods

        protected IExternalDashboardRepository CreateSut()
        {
            return new Repositories.ExternalDashboardRepository(CreateExternalDashboardOptions(), CreateLoggerFactory());
        }

        private IOptions<ExternalDashboardOptions> CreateExternalDashboardOptions()
        {
            return Microsoft.Extensions.Options.Options.Create(CreateTestConfiguration().GetExternalDashboardOptions());
        }

        #endregion
    }
}