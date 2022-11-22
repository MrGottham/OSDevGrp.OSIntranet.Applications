using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks;

namespace OSDevGrp.OSIntranet.Core.HealthChecks
{
    internal class ConnectionStringValidator : IConfigurationValueValidator
    {
        #region Private variables

        private readonly IConfiguration _configuration;
        private readonly string _name;

        #endregion

        #region Constructor

        internal ConnectionStringValidator(IConfiguration configuration, string name)
        {
            NullGuard.NotNull(configuration, nameof(configuration))
                .NotNullOrWhiteSpace(name, nameof(name));

            _configuration = configuration;
            _name = name;
        }

        #endregion

        #region Methods

        public Task ValidateAsync()
        {
            return Task.Run(() => 
            {
                try
                {
                    string value = _configuration.GetConnectionString(_name);
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new IntranetExceptionBuilder(ErrorCode.MissingConnectionString, _name).Build();
                    }
                }
                catch (NullReferenceException ex)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.MissingConnectionString, _name)
                        .WithInnerException(ex)
                        .Build();
                }
            });
        }

        #endregion
    }
}