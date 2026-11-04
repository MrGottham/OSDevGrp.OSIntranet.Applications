using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;

public class AccountingResponse : AccountingIdentificationResponseBase<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, ApplyPostingJournalModel, IReadOnlyCollection<LetterHeadIdentificationModel>>, IAccountingTexts>
{
    #region Constructor

    public AccountingResponse(Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, ApplyPostingJournalModel, IReadOnlyCollection<LetterHeadIdentificationModel>> model, IAccountingTexts dynamicTexts, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IReadOnlyCollection<IValidationRule> validationRuleSet)
        : base(model, dynamicTexts, staticTexts, validationRuleSet)
    {
    }

    #endregion

    #region Properties

    public AccountingModel Accounting => Model.Item1;

    public IReadOnlyCollection<PostingLineModel> PostingLines => Model.Item2;

    public ApplyPostingJournalModel PostingJournal => Model.Item3;

    public IReadOnlyCollection<LetterHeadIdentificationModel> LetterHeads => Model.Item4;

    #endregion
}