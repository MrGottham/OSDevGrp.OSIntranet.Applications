using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class PostingLineCollectionTextsBuilder : DynamicTextsBuilderBase<IReadOnlyCollection<PostingLineModel>, IPostingLineCollectionTexts>, IPostingLineCollectionTextsBuilder
{
    #region Constructor

    public PostingLineCollectionTextsBuilder(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<IPostingLineCollectionTexts> BuildAsync(IReadOnlyCollection<PostingLineModel> postingLines, IFormatProvider formatProvider, CancellationToken cancellationToken)
    {
        string latestPostingsHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.LatestPostings, StaticTextKey.LatestPostings.DefaultArguments(), formatProvider, cancellationToken);
        string postingDateHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingDate, StaticTextKey.PostingDate.DefaultArguments(), formatProvider, cancellationToken);
        string postingReferenceHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingReference, StaticTextKey.PostingReference.DefaultArguments(), formatProvider, cancellationToken);
        string accountHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Account, StaticTextKey.Account.DefaultArguments(), formatProvider, cancellationToken);
        string postingTextHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingText, StaticTextKey.PostingText.DefaultArguments(), formatProvider, cancellationToken);
        string budgetAccountHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.BudgetAccount, StaticTextKey.BudgetAccount.DefaultArguments(), formatProvider, cancellationToken);
        string debitHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Debit, StaticTextKey.Debit.DefaultArguments(), formatProvider, cancellationToken);
        string creditHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Credit, StaticTextKey.Credit.DefaultArguments(), formatProvider, cancellationToken);
        string postingValueHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.PostingValue, StaticTextKey.PostingValue.DefaultArguments(), formatProvider, cancellationToken);
        string contactAccountHeader = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.ContactAccount, StaticTextKey.ContactAccount.DefaultArguments(), formatProvider, cancellationToken);

        return new PostingLineCollectionTexts(postingLines, latestPostingsHeader, postingDateHeader, postingReferenceHeader, accountHeader, postingTextHeader, budgetAccountHeader, debitHeader, creditHeader, postingValueHeader, contactAccountHeader, latestPostingsHeader, formatProvider);
    }

    #endregion
}