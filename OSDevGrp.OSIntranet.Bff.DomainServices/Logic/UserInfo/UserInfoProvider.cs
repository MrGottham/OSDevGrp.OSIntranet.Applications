using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.UserInfo;

internal class UserInfoProvider : IUserInfoProvider
{
    #region Private variables

    private readonly IUserHelper _userHelper;
    private readonly IAccountingGateway _accountingGateway;

    #endregion

    #region Constructor

    public UserInfoProvider(IUserHelper userHelper, IAccountingGateway accountingGateway)
    {
        _userHelper = userHelper;
        _accountingGateway = accountingGateway;
    }

    #endregion

    #region Methods

    public bool IsAuthenticated(ClaimsPrincipal claimsPrincipal)
    {
        return _userHelper.IsAuthenticated(claimsPrincipal);
    }

    public async Task<IUserInfoModel?> GetUserInfoAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        if (IsAuthenticated(claimsPrincipal) == false)
        {
            return null;
        }

        string? nameIdentifier = _userHelper.GetNameIdentifier(claimsPrincipal);
        string? fullName = _userHelper.GetFullName(claimsPrincipal);
        string? mailAddress = _userHelper.GetMailAddress(claimsPrincipal);

        bool hasAccountingAccess = _userHelper.HasAccountingAccess(claimsPrincipal);
        int? defaultAccountingNumber = _userHelper.GetDefaultAccountingNumber(claimsPrincipal);
        bool isAccountingAdministrator = _userHelper.IsAccountingAdministrator(claimsPrincipal);
        bool isAccountingCreator = _userHelper.IsAccountingCreator(claimsPrincipal);
        bool isAccountingModifier = _userHelper.IsAccountingModifier(claimsPrincipal);
        bool isAccountingViewer = _userHelper.IsAccountingViewer(claimsPrincipal);

        IReadOnlyDictionary<int, string> accountings = await GetAccountingsAsync(hasAccountingAccess, cancellationToken);
        IReadOnlyDictionary<int, string> modifiableAccountings = FilterAccountings(accountings, isAccountingModifier, item => _userHelper.IsAccountingModifier(claimsPrincipal, item.Key));
        IReadOnlyDictionary<int, string> viewableAccountings = FilterAccountings(accountings, isAccountingViewer, item => _userHelper.IsAccountingViewer(claimsPrincipal, item.Key));

        bool hasCommonDataAccess = _userHelper.HasCommonDataAccess(claimsPrincipal);

        return new UserInfoModel(nameIdentifier, fullName, mailAddress, hasAccountingAccess, defaultAccountingNumber, accountings, isAccountingAdministrator, isAccountingCreator, isAccountingModifier, modifiableAccountings, isAccountingViewer, viewableAccountings, hasCommonDataAccess);
    }

    private async Task<IReadOnlyDictionary<int, string>> GetAccountingsAsync(bool hasAccountingAccess, CancellationToken cancellationToken = default)
    {
        if (hasAccountingAccess == false)
        {
            return new ConcurrentDictionary<int, string>();
        }

        IEnumerable<AccountingModel> accountingModels = await _accountingGateway.GetAccountingsAsync(cancellationToken);

        return new ConcurrentDictionary<int, string>(accountingModels.ToDictionary(accountingModel => accountingModel.Number, accountingModel => accountingModel.Name));
    }

    private static IReadOnlyDictionary<int, string> FilterAccountings(IReadOnlyDictionary<int, string> accountings, bool accountingPermission, Func<KeyValuePair<int, string>, bool> predicate)
    {
        if (accountingPermission == false)
        {
            return new ConcurrentDictionary<int, string>();
        }

        return new ConcurrentDictionary<int, string>(accountings.Where(predicate).ToDictionary(pair => pair.Key, pair => pair.Value));
    }

    #endregion
}