using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class PostingJournalTextsBuilder : DynamicTextsBuilderBase<Tuple<ApplyPostingJournalModel, Predicate<int>>, IPostingJournalTexts>, IPostingJournalTextsBuilder
{
    #region Constructor

    public PostingJournalTextsBuilder(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<IPostingJournalTexts> BuildAsync(Tuple<ApplyPostingJournalModel, Predicate<int>> model, IFormatProvider formatProvider, CancellationToken cancellationToken)
    {
        string postingJournalHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingJournal, StaticTextKey.PostingJournal.DefaultArguments(), formatProvider, cancellationToken);
        string postingDateHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingDate, StaticTextKey.PostingDate.DefaultArguments(), formatProvider, cancellationToken);
        string postingReferenceHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingReference, StaticTextKey.PostingReference.DefaultArguments(), formatProvider, cancellationToken);
        string accountHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Account, StaticTextKey.Account.DefaultArguments(), formatProvider, cancellationToken);
        string accountNameLabel = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.AccountName, StaticTextKey.AccountName.DefaultArguments(), formatProvider, cancellationToken);
        string postingTextHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingText, StaticTextKey.PostingText.DefaultArguments(), formatProvider, cancellationToken);
        string budgetAccountHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.BudgetAccount, StaticTextKey.BudgetAccount.DefaultArguments(), formatProvider, cancellationToken);
        string debitHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Debit, StaticTextKey.Debit.DefaultArguments(), formatProvider, cancellationToken);
        string creditHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Credit, StaticTextKey.Credit.DefaultArguments(), formatProvider, cancellationToken);
        string postingValueHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingValue, StaticTextKey.PostingValue.DefaultArguments(), formatProvider, cancellationToken);
        string balanceLabel = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Balance, StaticTextKey.Balance.DefaultArguments(), formatProvider, cancellationToken);
        string budgetLabel = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Budget, StaticTextKey.Budget.DefaultArguments(), formatProvider, cancellationToken);
        string postedLabel = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Posted, StaticTextKey.Posted.DefaultArguments(), formatProvider, cancellationToken);
        string availableLabel = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Available, StaticTextKey.Available.DefaultArguments(), formatProvider, cancellationToken);
        string contactAccountHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.ContactAccount, StaticTextKey.ContactAccount.DefaultArguments(), formatProvider, cancellationToken);

        bool modifiable = model.Item2(model.Item1.AccountingNumber);

        return new PostingJournalTexts(model.Item1, postingJournalHeader, postingDateHeader, postingReferenceHeader, accountHeader, accountNameLabel, creditHeader, balanceLabel, availableLabel, postingTextHeader, budgetAccountHeader, accountNameLabel, budgetLabel, postedLabel, availableLabel, debitHeader, creditHeader, postingValueHeader, contactAccountHeader, accountNameLabel, balanceLabel, modifiable, formatProvider);
    }

    #endregion
}