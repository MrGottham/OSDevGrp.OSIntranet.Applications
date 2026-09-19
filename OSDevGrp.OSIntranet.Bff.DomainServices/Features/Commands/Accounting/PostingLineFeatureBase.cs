using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

internal abstract class PostingLineFeatureBase<TPostingJournalLineRequest> : AccountingIdentificationFeatureBase<TPostingJournalLineRequest>
    where TPostingJournalLineRequest : PostingJournalLineIdentificationRequestBase
{
    #region Private variables

    private readonly IStaticTextProvider _staticTextProvider;

    #endregion

    #region Constructor

    protected PostingLineFeatureBase(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider)
        : base(permissionChecker, accountingGateway)
    {
        _staticTextProvider = staticTextProvider ?? throw new ArgumentNullException(nameof(staticTextProvider));
    }

    #endregion

    #region Properties

    protected IStaticTextProvider StaticTextProvider => _staticTextProvider;

    #endregion

    #region Methods

    public sealed override async Task ExecuteAsync(TPostingJournalLineRequest request, CancellationToken cancellationToken = default)
    {
        ApplyPostingJournalModel postingJournal = await AccountingGateway.GetPostingJournalAsync(request.AccountingNumber, cancellationToken);

        ApplyPostingJournalModel modifiedJournal = await ProcessPostingJournalAsync(postingJournal, request, cancellationToken);

        await AccountingGateway.SavePostingJournalAsync(request.AccountingNumber, modifiedJournal, cancellationToken);
    }

    protected abstract Task<ApplyPostingJournalModel> ProcessPostingJournalAsync(ApplyPostingJournalModel postingJournal, TPostingJournalLineRequest request, CancellationToken cancellationToken);

    #endregion
}