using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;

public class PostingJournalResponse : AccountingIdentificationResponseBase<Tuple<ApplyPostingJournalModel, Predicate<int>>, IPostingJournalTexts>
{
    #region Constructor

    public PostingJournalResponse(Tuple<ApplyPostingJournalModel, Predicate<int>> model, IPostingJournalTexts dynamicTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(model, dynamicTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion

    #region Properties

    public ApplyPostingJournalModel PostingJournal => Model.Item1;

    #endregion
}
