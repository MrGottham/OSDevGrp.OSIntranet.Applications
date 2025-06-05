using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accountings;

internal class AccountingsFeature : PageFeatureBase<AccountingsRequest, AccountingsResponse, IReadOnlyCollection<AccountingModel>>, IPermissionVerifiable<AccountingsRequest>
{
    #region Private variables

    private readonly IPermissionChecker _permissionChecker;
    private readonly IAccountingGateway _accountingGateway;

    #endregion

    #region Constructor

    public AccountingsFeature(IPermissionChecker permissionChecker, IAccountingGateway accountingGateway, IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
        _permissionChecker = permissionChecker;
        _accountingGateway = accountingGateway;
    }

    #endregion

    #region Methods

    public Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, AccountingsRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
            {
                ClaimsPrincipal user = securityContext.User;
                return _permissionChecker.IsAuthenticated(user) && _permissionChecker.HasAccountingAccess(user);
            }, cancellationToken);
    }

    public override async Task<AccountingsResponse> ExecuteAsync(AccountingsRequest request, CancellationToken cancellationToken = default)
    {
        bool creationAllowed = _permissionChecker.IsAccountingCreator(request.SecurityContext.User);

        IReadOnlyCollection<AccountingModel> accountings = (await _accountingGateway.GetAccountingsAsync(cancellationToken)).ToArray();

        IReadOnlyDictionary<StaticTextKey, string> staticTexts = await GetStaticTextsAsync(request, accountings, cancellationToken);

        return new AccountingsResponse(creationAllowed, accountings, staticTexts);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(AccountingsRequest request, IReadOnlyCollection<AccountingModel> argument)
    {
        object[] noArguments = Array.Empty<object>();

        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Accountings, noArguments },
            { StaticTextKey.CreateNewAccounting, noArguments }
        };
    }

    #endregion
}