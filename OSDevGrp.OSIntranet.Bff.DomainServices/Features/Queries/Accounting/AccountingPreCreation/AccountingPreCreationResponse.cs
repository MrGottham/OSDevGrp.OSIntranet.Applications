using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingPreCreation;

public class AccountingPreCreationResponse : PageResponseBase
{
    #region Constructor

    public AccountingPreCreationResponse(IReadOnlyCollection<LetterHeadIdentificationModel> letterHeads, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(staticTexts)
    {
        LetterHeads = letterHeads;
        ValidationRuleSet = validationRuleSet;
    }

    #endregion

    #region Properties

    public IReadOnlyCollection<LetterHeadIdentificationModel> LetterHeads { get; }

    public IReadOnlyCollection<IValidationRule> ValidationRuleSet { get; }

    #endregion
}