using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class BudgetAccountTextsBuilder : DynamicTextsBuilderBase<BudgetAccountModel, IBudgetAccountTexts>, IBudgetAccountTextsBuilder
{
    #region Constructor

    public BudgetAccountTextsBuilder(IStaticTextProvider staticTextProvider) 
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<IBudgetAccountTexts> BuildAsync(BudgetAccountModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(formatProvider);

        IValueDisplayer? statusDate = null;
        IBudgetAccountValuesDisplayer? valuesForMonthOfStatusDate = null;
        IBudgetAccountValuesDisplayer? valuesForLastMonthOfStatusDate = null;
        IBudgetAccountValuesDisplayer? valuesForYearToDateOfStatusDate = null;
        IBudgetAccountValuesDisplayer? valuesForLastYearOfStatusDate = null;

        Task buildStatusDateTask = GetStatusDateAsync(model.StatusDate, "d", formatProvider, cancellationToken).ContinueWith(task => statusDate = task.Result, cancellationToken);
        Task buildValuesForMonthOfStatusDateTask = BudgetAccountValuesDisplayer.CreateAsync(StaticTextKey.BudgetAccountValuesForMonthOfStatusDate, model.ValuesForMonthOfStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesForMonthOfStatusDate = task.Result, cancellationToken);
        Task buildValuesForLastMonthOfStatusDateTask = BudgetAccountValuesDisplayer.CreateAsync(StaticTextKey.BudgetAccountValuesForLastMonthOfStatusDate, model.ValuesForLastMonthOfStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesForLastMonthOfStatusDate = task.Result, cancellationToken);
        Task buildValuesForYearToDateOfStatusDateTask = BudgetAccountValuesDisplayer.CreateAsync(StaticTextKey.BudgetAccountValuesForYearToDateOfStatusDate, model.ValuesForYearToDateOfStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesForYearToDateOfStatusDate = task.Result, cancellationToken);
        Task buildValuesForLastYearOfStatusDateTask = BudgetAccountValuesDisplayer.CreateAsync(StaticTextKey.BudgetAccountValuesForLastYearOfStatusDate, model.ValuesForLastYearOfStatusDate, StaticTextProvider, formatProvider, cancellationToken).ContinueWith(task => valuesForLastYearOfStatusDate = task.Result, cancellationToken);

        await Task.WhenAll(buildStatusDateTask, buildValuesForMonthOfStatusDateTask, buildValuesForLastMonthOfStatusDateTask, buildValuesForYearToDateOfStatusDateTask, buildValuesForLastYearOfStatusDateTask);

        return new BudgetAccountTexts(model, statusDate!, valuesForMonthOfStatusDate!, valuesForLastMonthOfStatusDate!, valuesForYearToDateOfStatusDate!, valuesForLastYearOfStatusDate!, formatProvider);
    }

    #endregion
}