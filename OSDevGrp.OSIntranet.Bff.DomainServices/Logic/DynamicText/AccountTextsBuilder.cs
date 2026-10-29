using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountTextsBuilder : DynamicTextsBuilderBase<AccountModel, IAccountTexts>, IAccountTextsBuilder
{
    #region Constructor

    public AccountTextsBuilder(IStaticTextProvider staticTextProvider) 
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<IAccountTexts> BuildAsync(AccountModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(formatProvider);

        IValueDisplayer? statusDate = null;
        IAccountValuesDisplayer? valuesAtStatusDate = null;
        IAccountValuesDisplayer? valuesAtEndOfLastMonthFromStatusDate = null;
        IAccountValuesDisplayer? valuesAtEndOfLastYearFromStatusDate = null;

        Task buildStatusDateTask = GetStatusDateAsync(model.StatusDate, "d", formatProvider, cancellationToken).ContinueWith(task => statusDate = task.Result, cancellationToken);
        Task buildValuesAtStatusDateTask = AccountValuesDisplayer.CreateAsync(StaticTextKey.AccountValuesAtStatusDate, model.ValuesAtStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesAtStatusDate = task.Result, cancellationToken);
        Task buildValuesAtEndOfLastMonthFromStatusDateTask = AccountValuesDisplayer.CreateAsync(StaticTextKey.AccountValuesAtEndOfLastMonthFromStatusDateAtStatusDate, model.ValuesAtEndOfLastMonthFromStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesAtEndOfLastMonthFromStatusDate = task.Result, cancellationToken);
        Task buildValuesAtEndOfLastYearFromStatusDateTask = AccountValuesDisplayer.CreateAsync(StaticTextKey.AccountValuesAtEndOfLastYearFromStatusDate, model.ValuesAtEndOfLastYearFromStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesAtEndOfLastYearFromStatusDate = task.Result, cancellationToken);

        await Task.WhenAll(buildStatusDateTask, buildValuesAtStatusDateTask, buildValuesAtEndOfLastMonthFromStatusDateTask, buildValuesAtEndOfLastYearFromStatusDateTask);

        return new AccountTexts(model, statusDate!, valuesAtStatusDate!, valuesAtEndOfLastMonthFromStatusDate!, valuesAtEndOfLastYearFromStatusDate!, formatProvider);
    }

    #endregion
}