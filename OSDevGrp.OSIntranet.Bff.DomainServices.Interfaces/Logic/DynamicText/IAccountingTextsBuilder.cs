using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IAccountingTextsBuilder : IDynamicTextsBuilder<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>>, IAccountingTexts>, IDynamicTextsBuilder<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, ApplyPostingJournalModel, IReadOnlyCollection<LetterHeadIdentificationModel>>, IAccountingTexts>
{
}