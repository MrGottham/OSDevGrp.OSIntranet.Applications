using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Mvc.Helpers.Factories;
using OSDevGrp.OSIntranet.Mvc.Tests.Models.Converters;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Factories;

internal static class ConverterFactoryCreator
{
    #region Methods

    internal static IConverterFactory Create() => new ConverterFactory(GetLicensesOptions(), GetLoggerFactory());

    internal static IOptions<LicensesOptions> GetLicensesOptions() => Options.Create(CreateTestConfiguration().GetLicensesOptions());

    internal static ILoggerFactory GetLoggerFactory() => NullLoggerFactory.Instance;

    private static IConfiguration CreateTestConfiguration()
    {
        return new ConfigurationBuilder()
            .AddUserSecrets<ConverterBaseTests>()
            .Build();
    }

    #endregion
}