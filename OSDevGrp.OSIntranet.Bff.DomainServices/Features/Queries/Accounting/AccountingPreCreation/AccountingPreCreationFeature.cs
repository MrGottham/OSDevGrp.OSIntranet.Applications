using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingPreCreation;

internal class AccountingPreCreationFeature : PageFeatureBase<AccountingPreCreationRequest, AccountingPreCreationResponse, object>, IPermissionVerifiable<AccountingPreCreationRequest>
{
    #region Private variables

    private readonly IPermissionChecker _permissionChecker;
    private readonly ICommonGateway _commonGateway;
    private readonly IAccountingRuleSetBuilder _accountingRuleSetBuilder;

    #endregion

    #region Constructor

    public AccountingPreCreationFeature(IPermissionChecker permissionChecker, ICommonGateway commonGateway, IStaticTextProvider staticTextProvider, IAccountingRuleSetBuilder accountingRuleSetBuilder)
        : base(staticTextProvider)
    {
        _permissionChecker = permissionChecker;
        _commonGateway = commonGateway;
        _accountingRuleSetBuilder = accountingRuleSetBuilder;
    }

    #endregion

    #region Methods

    public Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, AccountingPreCreationRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => VerifyPermission(securityContext.User), cancellationToken);
    }

    public async override Task<AccountingPreCreationResponse> ExecuteAsync(AccountingPreCreationRequest request, CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<LetterHeadIdentificationModel>? letterHeads = null;
        IReadOnlyDictionary<StaticTextKey, string>? staticTexts = null;
        IReadOnlyCollection<IValidationRule>? validationRuleSet = null;

        await Task.WhenAll(
            _commonGateway.GetLetterHeadsAsync(cancellationToken).ContinueWith(task => letterHeads = task.Result.Select(letterHead => new LetterHeadIdentificationModel(letterHead.Name, letterHead.Number)).ToArray(), cancellationToken),
            GetStaticTextsAsync(request, new object(), cancellationToken).ContinueWith(task => staticTexts = task.Result, cancellationToken),
            _accountingRuleSetBuilder.BuildAsync(request.FormatProvider, cancellationToken).ContinueWith(task => validationRuleSet = task.Result, cancellationToken));

        return new AccountingPreCreationResponse(letterHeads!, staticTexts!, validationRuleSet!);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccountingPreCreationRequest request, object argument)
    {
        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.CreateNewAccounting, StaticTextKey.CreateNewAccounting.DefaultArguments() },
            { StaticTextKey.AccountingNumber, StaticTextKey.AccountingNumber.DefaultArguments() },
            { StaticTextKey.AccountingName, StaticTextKey.AccountingName.DefaultArguments() },
            { StaticTextKey.LetterHead, StaticTextKey.LetterHead.DefaultArguments() },
            { StaticTextKey.BalanceBelowZero, StaticTextKey.BalanceBelowZero.DefaultArguments() },
            { StaticTextKey.Debtors, StaticTextKey.Debtors.DefaultArguments() },
            { StaticTextKey.Creditors, StaticTextKey.Creditors.DefaultArguments() },
            { StaticTextKey.BackDating, StaticTextKey.BackDating.DefaultArguments() },
            { StaticTextKey.Create, StaticTextKey.Create.DefaultArguments() },
            { StaticTextKey.Reset, StaticTextKey.Reset.DefaultArguments() },
            { StaticTextKey.Cancel, StaticTextKey.Cancel.DefaultArguments() },
        };
    }

    private bool VerifyPermission(ClaimsPrincipal user)
    {
        return _permissionChecker.IsAuthenticated(user) && _permissionChecker.HasAccountingAccess(user) && _permissionChecker.IsAccountingCreator(user) && _permissionChecker.HasCommonDataAccess(user);
    }

    #endregion
}