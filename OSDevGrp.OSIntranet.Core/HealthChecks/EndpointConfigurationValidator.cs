using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.HealthChecks
{
    internal class EndpointConfigurationValidator : StringConfigurationValidator
    {
        #region Constructor

        internal EndpointConfigurationValidator(IConfiguration configuration, string key)
            : base(configuration, key)
        {
        }

        #endregion

        #region Methods

        protected override Task ValidateAsync(string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return Task.Run(() =>
            {
                if (Uri.TryCreate(value, UriKind.Absolute, out Uri result) == false)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.InvalidConfigurationValue, Key).Build();
                }
            });
       }

        #endregion
    }
}