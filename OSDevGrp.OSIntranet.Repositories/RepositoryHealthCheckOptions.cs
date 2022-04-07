namespace OSDevGrp.OSIntranet.Repositories
{
    public class RepositoryHealthCheckOptions
    {
        public bool ValidateRepositoryContext { get; private set; } = true;

        public bool ValidateConnectionStrings { get; private set; } = true;

        public RepositoryHealthCheckOptions WithRepositoryContextValidation()
        {
            ValidateRepositoryContext = true;

            return this;
        }

        public RepositoryHealthCheckOptions WithConnectionStringsValidation()
        {
            ValidateConnectionStrings = true;

            return this;
        }

        public RepositoryHealthCheckOptions WithoutRepositoryContextValidation()
        {
            ValidateRepositoryContext = false;

            return this;
        }

        public RepositoryHealthCheckOptions WithoutConnectionStringsValidation()
        {
            ValidateConnectionStrings = false;

            return this;
        }
    }
}