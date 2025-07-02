using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accounting;

internal class AccountingFeature : AccountingIdentificationFeatureBase<AccountingRequest, AccountingResponse, AccountingModel, IAccountingTexts, IAccountingTextsBuilder>
{
    #region Private variables

    private readonly ICommonGateway _commonGateway;
    private readonly IList<LetterHeadIdentificationModel> _letterHeads = new List<LetterHeadIdentificationModel>();

    #endregion

    #region Construcot

    public AccountingFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, ICommonGateway commonGateway, IStaticTextProvider staticTextProvider, IAccountingTextsBuilder dynamicTextsBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, dynamicTextsBuilder)
    {
        _commonGateway = commonGateway;
    }

    #endregion

    #region Methods

    protected override async Task<AccountingModel> GetModelAsync(AccountingRequest request, CancellationToken cancellationToken)
    {
        AccountingModel accountingModel = await AccountingGateway.GetAccountingAsync(request.AccountingNumber, request.StatusDate, cancellationToken);

        await ResolveLetterHeadsAsync(accountingModel, request.SecurityContext, cancellationToken);

        return accountingModel;
    }

    protected override Task<AccountingResponse> BuildResponseAsync(AccountingModel accountingModel, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IAccountingTexts accountingTexts, CancellationToken cancellationToken)
    {
        return Task.Run(() => new AccountingResponse(accountingModel, accountingTexts, _letterHeads.AsReadOnly(), staticTexts), cancellationToken);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccountingRequest request, AccountingModel accountingModel)
    {
        object[] noArguments = Array.Empty<object>();

        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.UpdateAccounting, noArguments },
            { StaticTextKey.DeleteAccounting, noArguments },
            { StaticTextKey.MasterData, noArguments },
            { StaticTextKey.AccountingNumber, noArguments },
            { StaticTextKey.AccountingName, noArguments },
            { StaticTextKey.LetterHead, noArguments },
            { StaticTextKey.BalanceBelowZero, [ 0 ] },
            { StaticTextKey.Debtors, noArguments },
            { StaticTextKey.Creditors, noArguments },
            { StaticTextKey.BackDating, noArguments },
        };
    }

    private async Task ResolveLetterHeadsAsync(AccountingModel accountingModel, ISecurityContext securityContext, CancellationToken cancellationToken)
    {
        if (PermissionChecker.HasCommonDataAccess(securityContext.User) == false)
        {
            _letterHeads.Add(accountingModel.LetterHead);
            return;
        }

        IEnumerable<LetterHeadModel> letterHeadModels = await _commonGateway.GetLetterHeadsAsync(cancellationToken);
        foreach (LetterHeadModel letterHeadModel in letterHeadModels)
        {
            _letterHeads.Add(new LetterHeadIdentificationModel(letterHeadModel.Name, letterHeadModel.Number));
        }
    }

    #endregion
}