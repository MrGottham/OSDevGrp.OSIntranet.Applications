using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Home;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Collections.Concurrent;
using System.Globalization;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.UserInfo;

internal class UserInfoProvider : IUserInfoProvider
{
    #region Private variables

    private readonly IAccountingGateway _accountingGateway;

    #endregion

    #region Constructor

    public UserInfoProvider(IAccountingGateway accountingGateway)
    {
        _accountingGateway = accountingGateway;
    }

    #endregion

    #region Methods

    public bool IsAuthenticated(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity?.IsAuthenticated ?? false;
    }

    public async Task<IUserInfoModel?> GetUserInfoAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        if (IsAuthenticated(claimsPrincipal) == false)
        {
            return null;
        }

        string? name = null;
        Task getNameTask = GetNameAsync(claimsPrincipal, cancellationToken).ContinueWith(task => name = task.Result, cancellationToken);

        Claim? accountingClaim = claimsPrincipal.FindFirst(Security.ClaimTypes.AccountingClaimType);
        bool hasAccountingAccess = accountingClaim != null;
        int? defaultAccountingNumber = null;
        Task getDefaultAccountingNumberTask = GetDefaultAccountingNumberAsync(accountingClaim, cancellationToken).ContinueWith(task => defaultAccountingNumber = task.Result, cancellationToken);
        IDictionary<int, string> accountings = new ConcurrentDictionary<int, string>();
        Task getAccountingsTask = GetAccountingsAsync(hasAccountingAccess, accountings, cancellationToken).ContinueWith(task => accountings = task.Result, cancellationToken);

        await Task.WhenAll(getNameTask, getDefaultAccountingNumberTask, getAccountingsTask);

        return new UserInfoModel(name, hasAccountingAccess, defaultAccountingNumber, accountings.AsReadOnly());
    }

    private async Task<IDictionary<int, string>> GetAccountingsAsync(bool hasAccountingAccess, IDictionary<int, string> accountings, CancellationToken cancellationToken = default)
    {
        if (hasAccountingAccess == false)
        {
            return accountings;
        }

        IEnumerable<AccountingModel> accountingModels = await _accountingGateway.GetAccountingsAsync(cancellationToken);

        return new ConcurrentDictionary<int, string>(accountingModels.ToDictionary(accountingModel => accountingModel.Number, accountingModel => accountingModel.Name));
    }

    private static Task<string?> GetNameAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => 
        {
            cancellationToken.ThrowIfCancellationRequested();

            Claim? nameClaim = claimsPrincipal.FindFirst(ClaimTypes.Name);
            if (nameClaim != null && string.IsNullOrWhiteSpace(nameClaim.Value) == false)
            {
                return nameClaim.Value;
            }

            Claim? emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);
            if (emailClaim != null && string.IsNullOrWhiteSpace(emailClaim.Value) == false)
            {
                return emailClaim.Value;
            }

            return null;
        });
    }

    private static Task<int?> GetDefaultAccountingNumberAsync(Claim? accountingClaim, CancellationToken cancellationToken = default)
    {
        return Task.Run<int?>(() =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (accountingClaim == null || string.IsNullOrWhiteSpace(accountingClaim.Value))
            {
                return null;
            }

            if (int.TryParse(accountingClaim.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
            {
                return result;
            }

            return null;
        });
    }

    #endregion
}