using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;

public class AccountingResponse : AccountingIdentificationResponseBase<AccountingModel, IAccountingTexts>
{
    #region Constructor

    public AccountingResponse(AccountingModel model, IAccountingTexts dynamicTexts, IReadOnlyCollection<LetterHeadIdentificationModel> letterHeads, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(model, dynamicTexts, staticTexts, validationRuleSet)
    {
        LetterHeads = letterHeads;
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<LetterHeadIdentificationModel> LetterHeads { get; }

    #endregion
}