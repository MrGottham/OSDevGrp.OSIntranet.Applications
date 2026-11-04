using System.Globalization;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Accounting;

internal sealed class ModifyPostingLineInPostingJournalFeature : PostingLineFeatureBase<ModifyPostingLineInPostingJournalRequest>
{
    #region Constructor

    public ModifyPostingLineInPostingJournalFeature(
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
        ModifyPostingLineInPostingJournalRequest request,
        CancellationToken cancellationToken)
    {
        ApplyPostingLineModel? existingLine = postingJournal.ApplyPostingLines?
            .FirstOrDefault(line => line.Identifier == request.Identifier);

        if (existingLine == null)
        {
            string errorMessage = await StaticTextProvider.GetStaticTextAsync(
                StaticTextKey.UnknownIdentifier,
                StaticTextKey.UnknownIdentifier.DefaultArguments(),
                request.FormatProvider,
                cancellationToken);

            throw new UnknownIdentifierException(request.Identifier, errorMessage);
        }

        var replacementLine = new ApplyPostingLineModel(
            request.Account,
            request.BudgetAccount,
            request.ContactAccount,
            request.Credit.HasValue ? (double?)request.Credit.Value : null,
            request.Debit.HasValue ? (double?)request.Debit.Value : null,
            request.PostingText,
            existingLine.Identifier,  // Preserve original identifier
            request.PostingDate,
            request.PostingReference,
            existingLine.SortOrder);  // Preserve original sort order

        var updatedLines = new List<ApplyPostingLineModel>();
        foreach (var line in postingJournal.ApplyPostingLines ?? [])
        {
            if (line.Identifier == request.Identifier)
            {
                updatedLines.Add(replacementLine);
            }
            else
            {
                updatedLines.Add(line);
            }
        }

        return SortAndClonePostingJournal(postingJournal, updatedLines);
    }

    #endregion
}
