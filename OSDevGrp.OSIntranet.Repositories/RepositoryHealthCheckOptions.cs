using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.HealthChecks;
using OSDevGrp.OSIntranet.Repositories.Contexts;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class RepositoryHealthCheckOptions : ConfigurationHealthCheckOptionsBase<RepositoryHealthCheckOptions>
    {
        #region Properties

        public bool ValidateRepositoryContext { get; private set; } = true;

        protected override RepositoryHealthCheckOptions HealthCheckOptions => this;

        #endregion

        #region Methods

        public RepositoryHealthCheckOptions WithRepositoryContextValidation()
        {
            ValidateRepositoryContext = true;

            return HealthCheckOptions;
        }

        public RepositoryHealthCheckOptions WithoutRepositoryContextValidation()
        {
            ValidateRepositoryContext = false;

            return HealthCheckOptions;
        }

        public RepositoryHealthCheckOptions WithConnectionStringsValidation(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            return AddConnectionStringValidation(configuration, ConnectionStringNames.IntranetName);
        }

        #endregion
    }
}