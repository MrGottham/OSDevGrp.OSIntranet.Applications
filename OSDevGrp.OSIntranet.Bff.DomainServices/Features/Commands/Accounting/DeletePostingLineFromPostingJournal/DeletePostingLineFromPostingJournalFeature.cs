using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

internal sealed class DeletePostingLineFromPostingJournalFeature : PostingLineFeatureBase<DeletePostingLineFromPostingJournalRequest>
{
    #region Constructor

    public DeletePostingLineFromPostingJournalFeature(
        IPermissionChecker permissionChecker,
        IAccountingGateway accountingGateway,
        IStaticTextProvider staticTextProvider)
        : base(permissionChecker, accountingGateway, staticTextProvider)
    {
    }

    #endregion

    #region Methods

    protected override async Task<ApplyPostingJournalModel> ProcessPostingJournalAsync(
        ApplyPostingJournalModel postingJournal,
        DeletePostingLineFromPostingJournalRequest request,
        CancellationToken cancellationToken)
    {
        List<ApplyPostingLineModel> existingLines = postingJournal.ApplyPostingLines?.ToList() ?? [];
        if (existingLines.All(line => line.Identifier != request.Identifier))
        {
            string errorMessage = await StaticTextProvider.GetStaticTextAsync(
                StaticTextKey.UnknownIdentifier,
                StaticTextKey.UnknownIdentifier.DefaultArguments(),
                request.FormatProvider,
                cancellationToken);

            throw new UnknownIdentifierException(request.Identifier, errorMessage);
        }

        List<ApplyPostingLineModel> remainingLines = existingLines
            .Where(line => line.Identifier != request.Identifier)
            .ToList();

        return SortAndClonePostingJournal(postingJournal, remainingLines);
    }

    #endregion
}