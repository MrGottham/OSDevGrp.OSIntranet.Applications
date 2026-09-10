using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.ContactAccountSummary;

public class ContactAccountSummaryResponse : AccountIdentificationResponseBase<ContactAccountModel, IContactAccountTexts>
{
    #region Constructor

    public ContactAccountSummaryResponse(ContactAccountModel model, IContactAccountTexts contactAccountTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(model, contactAccountTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion

    #region Properties

    public ContactAccountModel ContactAccount => Model;

    #endregion
}