using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Notification;

internal class PostingWarningModelToNotificationConverter : IPostingWarningModelToNotificationConverter
{
    #region Private variables

    private readonly IStaticTextProvider _staticTextProvider;

    #endregion

    #region Constructor

    public PostingWarningModelToNotificationConverter(IStaticTextProvider staticTextProvider)
    {
        _staticTextProvider = staticTextProvider ?? throw new ArgumentNullException(nameof(staticTextProvider));
    }

    #endregion

    #region Methods

    public async Task<INotification> ConvertAsync(PostingWarningModel postingWarningModel, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(postingWarningModel);
        ArgumentNullException.ThrowIfNull(formatProvider);

        StaticTextKey contentStaticTextKey = postingWarningModel.Reason switch
        {
            PostingWarningReason.AccountIsOverdrawn => StaticTextKey.AccountIsOverdrawn,
            PostingWarningReason.ExpectedIncomeHasNotBeenReachedYet => StaticTextKey.ExpectedIncomeHasNotBeenReachedYet,
            PostingWarningReason.ExpectedExpensesHaveAlreadyBeenReached => StaticTextKey.ExpectedExpensesHaveAlreadyBeenReached,
            _ => throw new NotSupportedException($"The posting warning reason {postingWarningModel.Reason} is not supported.")
        };

        IEnumerable<object> contentArguments =
        [
            postingWarningModel.Account.AccountName,
            postingWarningModel.Amount.ToString("C", formatProvider),
            Environment.NewLine,
            Environment.NewLine,
            postingWarningModel.PostingLine.PostingDate.ToString("d", formatProvider),
            postingWarningModel.PostingLine.Details
        ];

        string header = await _staticTextProvider.GetStaticTextAsync(StaticTextKey.Warning, StaticTextKey.Warning.DefaultArguments(), formatProvider, cancellationToken);
        string content = await _staticTextProvider.GetStaticTextAsync(contentStaticTextKey, contentArguments, formatProvider, cancellationToken);

        return new MarkdownNotification(header, content);
    }

    #endregion
}