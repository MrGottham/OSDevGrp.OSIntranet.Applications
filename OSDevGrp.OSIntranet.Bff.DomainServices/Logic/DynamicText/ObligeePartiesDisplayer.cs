using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ObligeePartiesDisplayer : IObligeePartiesDisplayer
{
    #region Constructor

    private ObligeePartiesDisplayer(string header, IValueDisplayer debtors, IValueDisplayer creditors)
    {
        Header = header;
        Debtors = debtors;
        Creditors = creditors;
    }

    #endregion

    #region Properties

    public string Header { get; }

    public IValueDisplayer Debtors { get; }

    public IValueDisplayer Creditors { get; }

    #endregion

    #region Methods

    internal static async Task<IObligeePartiesDisplayer> CreateAsync<TModel>(StaticTextKey header, StaticTextKey debtors, StaticTextKey creditors, IStaticTextProvider staticTextProvider, TModel model, Func<TModel, decimal> debtorsCalculator, Func<TModel, decimal> creditorsCalculator, IFormatProvider formatProvider, CancellationToken cancellationToken = default) where TModel : class
    {
        string headerText = await staticTextProvider.GetStaticTextAsync(header, header.DefaultArguments(), formatProvider, cancellationToken);
        string debtorsText = await staticTextProvider.GetStaticTextAsync(debtors, debtors.DefaultArguments(), formatProvider, cancellationToken);
        string creditorsText = await staticTextProvider.GetStaticTextAsync(creditors, creditors.DefaultArguments(), formatProvider, cancellationToken);

        return new ObligeePartiesDisplayer(
            headerText,
            new ValueDisplayer<decimal>(debtorsText, debtorsCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)),
            new ValueDisplayer<decimal>(creditorsText, creditorsCalculator(model), formatProvider, (v, fp) => v.ToString("C", fp)));
    }

    #endregion
}