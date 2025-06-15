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

    public override Task<IBudgetAccountTexts> BuildAsync(BudgetAccountModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return Task.Run<IBudgetAccountTexts>(() => new BudgetAccountTexts(model, formatProvider), cancellationToken);
    }

    #endregion
}