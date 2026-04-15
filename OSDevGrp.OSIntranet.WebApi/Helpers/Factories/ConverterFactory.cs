using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.WebApi.Models.Accounting;
using OSDevGrp.OSIntranet.WebApi.Models.Common;
using OSDevGrp.OSIntranet.WebApi.Models.Core;
using OSDevGrp.OSIntranet.WebApi.Models.Security;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Factories;

internal class ConverterFactory : IConverterFactory
{
    #region Private variables

    private readonly IOptions<LicensesOptions> _licensesOptions;
    private readonly ILoggerFactory _loggerFactory;

    #endregion

    #region Constructor

    public ConverterFactory(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
    {
        NullGuard.NotNull(licensesOptions, nameof(licensesOptions))
            .NotNull(loggerFactory, nameof(loggerFactory));

        _licensesOptions = licensesOptions;
        _loggerFactory = loggerFactory;
    }

    #endregion

    #region Methods

    public IConverter CreateCoreModelConverter() => CoreModelConverter.Create(_licensesOptions, _loggerFactory);

    public IConverter CreateAccountingModelConverter() => AccountingModelConverter.Create(_licensesOptions, _loggerFactory);

    public IConverter CreateCommonModelConverter() => CommonModelConverter.Create(_licensesOptions, _loggerFactory);

    public IConverter CreateSecurityModelConverter() => SecurityModelConverter.Create(_licensesOptions, _loggerFactory);

    #endregion
}