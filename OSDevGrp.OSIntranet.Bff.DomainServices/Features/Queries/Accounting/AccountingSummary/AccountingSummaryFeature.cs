using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingSummary;

internal class AccountingSummaryFeature : AccountingIdentificationFeatureBase<AccountingSummaryRequest, AccountingSummaryResponse, Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>>, IAccountingTexts, IAccountingTextsBuilder, IEmptyRuleSetBuilder>
{
    #region Constructor

    public AccountingSummaryFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, IAccountingTextsBuilder accountingTextsBuilder, IEmptyRuleSetBuilder emptyRuleSetBuilder)
        : base(permissionChecker, accountingGateway, staticTextProvider, accountingTextsBuilder, emptyRuleSetBuilder)
    {
    }

    #endregion

    #region Methods

    protected override async Task<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>>> GetModelAsync(AccountingSummaryRequest request, CancellationToken cancellationToken)
    {
        AccountingModel? accountingModel = null;
        IReadOnlyCollection<PostingLineModel> postingLineModels = [];

        Task getAccountingTask = AccountingGateway.GetAccountingAsync(request.AccountingNumber, request.StatusDate, cancellationToken).ContinueWith(task => accountingModel = task.Result, cancellationToken);
        Task getPostingLinesTask = request.NumberOfPostingLines > 0
            ? AccountingGateway.GetPostingLinesAsync(request.AccountingNumber, request.StatusDate, request.NumberOfPostingLines, _ => true, cancellationToken).ContinueWith(task => postingLineModels = task.Result.ToArray(), cancellationToken)
            : Task.CompletedTask;

        await Task.WhenAll(getAccountingTask, getPostingLinesTask);

        return Tuple.Create(accountingModel!, postingLineModels!);
    }

    protected override Task<AccountingSummaryResponse> BuildResponseAsync(Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>> model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, IAccountingTexts accountingTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken)
    {
        return Task.FromResult(new AccountingSummaryResponse(model, accountingTexts, staticTexts, validationRuleSet));
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccountingSummaryRequest request, Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>> model)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.AccountingNumber, StaticTextKey.AccountingNumber.DefaultArguments() },
            { StaticTextKey.AccountingName, StaticTextKey.AccountingName.DefaultArguments() }
        };
    }

    #endregion
}