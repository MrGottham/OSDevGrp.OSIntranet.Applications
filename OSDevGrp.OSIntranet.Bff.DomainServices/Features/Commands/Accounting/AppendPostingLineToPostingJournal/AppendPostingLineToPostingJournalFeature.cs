using System.Globalization;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

internal sealed class AppendPostingLineToPostingJournalFeature : PostingLineFeatureBase<AppendPostingLineToPostingJournalRequest>
{
    #region Constructor

    public AppendPostingLineToPostingJournalFeature(
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
        AppendPostingLineToPostingJournalRequest request,
        CancellationToken cancellationToken)
    {
        if (postingJournal.ApplyPostingLines?.Any(line => line.Identifier == request.Identifier) == true)
        {
            string errorMessage = await StaticTextProvider.GetStaticTextAsync(
                StaticTextKey.IdentifierAlreadyExists,
                StaticTextKey.IdentifierAlreadyExists.DefaultArguments(),
                CultureInfo.InvariantCulture,
                cancellationToken);

            throw new IdentifierAlreadyExistsException(request.Identifier, errorMessage);
        }

        int maxSortOrder = postingJournal.ApplyPostingLines?.Any() == true 
            ? postingJournal.ApplyPostingLines.Max(line => line.SortOrder ?? 0)
            : 0;
        int newSortOrder = maxSortOrder + 1;

        var newLine = new ApplyPostingLineModel(
            request.Account,
            request.BudgetAccount,
            request.ContactAccount,
            request.Credit.HasValue ? (double?)request.Credit.Value : null,
            request.Debit.HasValue ? (double?)request.Debit.Value : null,
            request.PostingText,
            request.Identifier,
            request.PostingDate,
            request.PostingReference,
            newSortOrder);

        var updatedLines = new List<ApplyPostingLineModel>(postingJournal.ApplyPostingLines ?? [])
        {
            newLine
        };

        return SortAndClonePostingJournal(postingJournal, updatedLines);
    }

    #endregion
}