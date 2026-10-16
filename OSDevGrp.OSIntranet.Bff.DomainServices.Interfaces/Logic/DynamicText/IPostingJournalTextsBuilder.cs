using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IPostingJournalTextsBuilder : IDynamicTextsBuilder<Tuple<ApplyPostingJournalModel, Predicate<int>>, IPostingJournalTexts>
{
}