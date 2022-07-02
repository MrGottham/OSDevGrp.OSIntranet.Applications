using OSDevGrp.OSIntranet.Core.HealthChecks;

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

        #endregion
    }
}