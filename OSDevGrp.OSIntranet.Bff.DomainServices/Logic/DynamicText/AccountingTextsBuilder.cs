using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountingTextsBuilder : DynamicTextsBuilderBase<AccountingModel, IAccountingTexts>, IAccountingTextsBuilder
{
    #region Constructor

    public AccountingTextsBuilder(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<IAccountingTexts> BuildAsync(AccountingModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        IValueDisplayer? balanceBelowZero = null;
        IValueDisplayer? backDating = null;

        Task buildBalanceBelowZeroTask = BuildBalanceBelowZeroAsync(model, formatProvider, cancellationToken).ContinueWith(task => balanceBelowZero = task.Result, cancellationToken);
        Task buildBackDatingTask = BuildBackDatingAsync(model, formatProvider, cancellationToken).ContinueWith(task => backDating = task.Result, cancellationToken);
        await Task.WhenAll(buildBalanceBelowZeroTask, buildBackDatingTask);

        return new AccountingTexts(model, balanceBelowZero!, backDating!, formatProvider);
    }

    private async Task<IValueDisplayer> BuildBalanceBelowZeroAsync(AccountingModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        IDictionary<BalanceBelowZeroType, string> staticTexts = new Dictionary<BalanceBelowZeroType, string>
        {
            {BalanceBelowZeroType.Debtors, await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Debtors, [], formatProvider, cancellationToken)},
            {BalanceBelowZeroType.Creditors, await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Creditors, [], formatProvider, cancellationToken)}
        };

        return await GetValueDisplayerAsync(StaticTextKey.BalanceBelowZero, [0], model.BalanceBelowZero, formatProvider, (value, _) => Resolve(value, staticTexts), cancellationToken);
    }

    private async Task<IValueDisplayer> BuildBackDatingAsync(AccountingModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string days = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Days, [], formatProvider, cancellationToken);
        string day = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Day, [], formatProvider, cancellationToken);

        return await GetValueDisplayerAsync<int>(StaticTextKey.BackDating, model.BackDating, formatProvider, (value, fp) => $"{value.ToString(fp)} {(value == 1 ? day : days).ToLower()}", cancellationToken);
    }

    #endregion
}