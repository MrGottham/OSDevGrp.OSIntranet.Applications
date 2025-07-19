using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting;

internal abstract class AccountingIdentificationFeatureBase<TAccountingIdentificationRequest, TAccountingIdentificationResponse, TModel, TDynamicTexts, TDynamicTextsBuilder, TValidationRuleSetBuilder> : PageFeatureBase<TAccountingIdentificationRequest, TAccountingIdentificationResponse, TModel>, IPermissionVerifiable<TAccountingIdentificationRequest> where TAccountingIdentificationRequest : AccountingIdentificationRequestBase where TAccountingIdentificationResponse : AccountingIdentificationResponseBase<TModel, TDynamicTexts> where TModel : class where TDynamicTexts : IDynamicTexts where TDynamicTextsBuilder : IDynamicTextsBuilder<TModel, TDynamicTexts> where TValidationRuleSetBuilder : IValidationRuleSetBuilder
{
    #region Prviate variables

    private readonly TDynamicTextsBuilder _dynamicTextsBuilder;
    private readonly TValidationRuleSetBuilder _validationRuleSetBuilder;

    #endregion

    #region Constructor

    protected AccountingIdentificationFeatureBase(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider, TDynamicTextsBuilder dynamicTextsBuilder, TValidationRuleSetBuilder validationRuleSetBuilder)
        : base(staticTextProvider)
    {
        AccountingGateway = accountingGateway;
        PermissionChecker = permissionChecker;

        _dynamicTextsBuilder = dynamicTextsBuilder;
        _validationRuleSetBuilder = validationRuleSetBuilder;
    }

    #endregion

    #region Properties

    protected IAccountingGateway AccountingGateway { get; }

    protected IPermissionChecker PermissionChecker { get; }

    #endregion

    #region Methods

    public Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, TAccountingIdentificationRequest request, CancellationToken cancellationToken)
    {
        return Task.Run(() => VerifyPermission(securityContext.User, request.AccountingNumber), cancellationToken);
    }

    public sealed override async Task<TAccountingIdentificationResponse> ExecuteAsync(TAccountingIdentificationRequest request, CancellationToken cancellationToken)
    {
        TModel model = await GetModelAsync(request, cancellationToken);

        IReadOnlyDictionary<StaticTextKey, string>? staticTexts = null;
        TDynamicTexts? dynamicTexts = default;
        IReadOnlyCollection<IValidationRule>? validationRuleSet = null;

        await Task.WhenAll(
            GetStaticTextsAsync(request, model, cancellationToken).ContinueWith(task => staticTexts = task.Result, cancellationToken),
            _dynamicTextsBuilder.BuildAsync(model, request.FormatProvider, cancellationToken).ContinueWith(task => dynamicTexts = task.Result, cancellationToken),
            _validationRuleSetBuilder.BuildAsync(request.FormatProvider, cancellationToken).ContinueWith(task => validationRuleSet = task.Result, cancellationToken));

        return await BuildResponseAsync(model, staticTexts!, dynamicTexts!, validationRuleSet!, cancellationToken);
    }

    protected abstract Task<TModel> GetModelAsync(TAccountingIdentificationRequest request, CancellationToken cancellationToken);

    protected abstract Task<TAccountingIdentificationResponse> BuildResponseAsync(TModel model, IReadOnlyDictionary<StaticTextKey, string> staticTexts, TDynamicTexts dynamicTexts, IReadOnlyCollection<IValidationRule> validationRuleSet, CancellationToken cancellationToken);

    private bool VerifyPermission(ClaimsPrincipal user, int accountingNumber)
    {
        return PermissionChecker.IsAuthenticated(user) && PermissionChecker.HasAccountingAccess(user) && PermissionChecker.IsAccountingViewer(user, accountingNumber);
    }

    #endregion
}