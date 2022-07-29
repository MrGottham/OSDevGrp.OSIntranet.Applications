using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.HealthChecks
{
    internal class RegularExpressionConfigurationValidator : StringConfigurationValidator
    {
        #region Private variables

        private readonly Regex _regularExpression;

        #endregion

        #region Constructor

        internal RegularExpressionConfigurationValidator(IConfiguration configuration, string key, Regex regularExpression)
            : base(configuration, key)
        {
            NullGuard.NotNull(regularExpression, nameof(regularExpression));

            _regularExpression = regularExpression;
        }

        #endregion

        #region Methods

        protected override Task ValidateAsync(string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return Task.Run(() => 
            {
                if (_regularExpression.IsMatch(value) == false)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.InvalidConfigurationValue, Key).Build();
                }
            });
        }

        #endregion
    }
}