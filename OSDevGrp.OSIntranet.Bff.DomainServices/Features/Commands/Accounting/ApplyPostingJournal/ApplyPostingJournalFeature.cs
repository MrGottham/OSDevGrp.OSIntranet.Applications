using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Notification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Accounting;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

internal sealed class ApplyPostingJournalFeature : AccountingIdentificationFeatureBase<ApplyPostingJournalRequest>
{
    #region Private variables

    private readonly IPostingWarningModelToNotificationConverter _postingWarningModelToNotificationConverter;
    private readonly INotificationPublisher _notificationPublisher;

    #endregion

    #region Constructor

    public ApplyPostingJournalFeature(
        IPermissionChecker permissionChecker,
        IAccountingGateway accountingGateway,
        IPostingWarningModelToNotificationConverter postingWarningModelToNotificationConverter,
        INotificationPublisher notificationPublisher)
        : base(permissionChecker, accountingGateway)
    {
        _postingWarningModelToNotificationConverter = postingWarningModelToNotificationConverter ?? throw new ArgumentNullException(nameof(postingWarningModelToNotificationConverter));
        _notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
    }

    #endregion

    #region Methods

    public override async Task ExecuteAsync(ApplyPostingJournalRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        ApplyPostingJournalModel postingJournal = await AccountingGateway.GetPostingJournalAsync(request.AccountingNumber, cancellationToken);
        ApplyPostingJournalResultModel result = await AccountingGateway.ApplyPostingJournalAsync(request.AccountingNumber, postingJournal, cancellationToken);

        HashSet<Guid> postedIdentifiers = result.PostingLines
            .Select(line => line.Identifier)
            .ToHashSet();

        List<ApplyPostingLineModel> remainingLines = (postingJournal.ApplyPostingLines ?? [])
            .Where(line => line.Identifier is not { } identifier || !postedIdentifiers.Contains(identifier))
            .ToList();

        ApplyPostingJournalModel remainingJournal = PostingJournalCloner.SortAndClonePostingJournal(postingJournal, remainingLines);
        await AccountingGateway.SavePostingJournalAsync(request.AccountingNumber, remainingJournal, cancellationToken);

        foreach (PostingWarningModel postingWarning in result.PostingWarnings)
        {
            INotification notification = await _postingWarningModelToNotificationConverter.ConvertAsync(postingWarning, request.FormatProvider, cancellationToken);
            await _notificationPublisher.PublishAsync(notification, cancellationToken);
        }
    }

    #endregion
}