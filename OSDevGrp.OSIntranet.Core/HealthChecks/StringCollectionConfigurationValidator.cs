using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.Core.HealthChecks
{
    internal class StringCollectionConfigurationValidator : StringConfigurationValidator
    {
        #region Private variables

        private readonly string _separator;
        private readonly int _minLength;
        private readonly int _maxLength;

        #endregion

        #region Constructor

        internal StringCollectionConfigurationValidator(IConfiguration configuration, string key, string separator, int minLength = 1, int maxLength = 32)
            : base(configuration, key)
        {
            NullGuard.NotNullOrWhiteSpace(separator, nameof(separator));

            _separator = separator;
            _minLength = minLength;
            _maxLength = maxLength;
        }

        #endregion

        #region Methods

        protected override Task ValidateAsync(string value)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            return Task.Run(() =>
            {
                string[] stringCollection = value.Split(_separator);
                if (stringCollection == null)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.InvalidConfigurationValue, Key).Build();
                }

                if (stringCollection.Length < _minLength)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.InvalidConfigurationValue, Key).Build();
                }

                if (stringCollection.Length > _maxLength)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.InvalidConfigurationValue, Key).Build();
                }
            });
       }

        #endregion
    }
}