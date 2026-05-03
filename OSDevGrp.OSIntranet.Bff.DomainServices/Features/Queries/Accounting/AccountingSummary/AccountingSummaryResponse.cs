using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingSummary;

public class AccountingSummaryResponse : AccountingIdentificationResponseBase<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>>, IAccountingTexts>
{
    #region Constructor

    public AccountingSummaryResponse(Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>> model, IAccountingTexts accountingTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet) 
        : base(model, accountingTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion

    #region Properties

    public AccountingModel Accounting => Model.Item1;

    public IReadOnlyCollection<PostingLineModel> PostingLines => Model.Item2;

    #endregion
}