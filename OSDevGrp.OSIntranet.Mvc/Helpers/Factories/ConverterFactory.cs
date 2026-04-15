using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Home;
using OSDevGrp.OSIntranet.Mvc.Models.MediaLibrary;
using OSDevGrp.OSIntranet.Mvc.Models.Security;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Factories;

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

    public IConverter CreateHomeViewModelConverter() => HomeViewModelConverter.Create(_licensesOptions, _loggerFactory);

    public IConverter CreateContactViewModelConverter() => ContactViewModelConverter.Create(_licensesOptions, _loggerFactory);

    public IConverter CreateAccountingViewModelConverter() => AccountingViewModelConverter.Create(_licensesOptions, _loggerFactory);

    public IConverter CreateMediaLibraryViewModelConverter() => MediaLibraryViewModelConverter.Create(_licensesOptions, _loggerFactory);

    public IConverter CreateCommonViewModelConverter() => CommonViewModelConverter.Create(_licensesOptions, _loggerFactory);

    public IConverter CreateSecurityViewModelConverter() => SecurityViewModelConverter.Create(_licensesOptions, _loggerFactory);

    #endregion
}