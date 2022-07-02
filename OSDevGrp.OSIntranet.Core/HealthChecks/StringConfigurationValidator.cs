using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.HealthChecks
{
    internal class StringConfigurationValidator : IConfigurationValueValidator
    {
        #region Private variables

        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        internal StringConfigurationValidator(IConfiguration configuration, string key)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNullOrWhiteSpace(key, nameof(key));

            _configuration = configuration;
            Key = key;
        }

        #endregion

        #region Properties

        protected string Key { get; }

        #endregion

        #region Methods

        public async Task ValidateAsync()
        {
            string value = _configuration[Key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new IntranetExceptionBuilder(ErrorCode.MissingConfiguration, Key).Build();
            }

            await ValidateAsync(value);
        }

        protected virtual Task ValidateAsync(string value)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}