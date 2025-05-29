using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;

internal class UserInfoFeature : PageFeatureBase<UserInfoRequest, UserInfoResponse, IUserInfoModel>, IPermissionVerifiable<UserInfoRequest>
{
    #region Private variables

    private readonly IPermissionChecker _permissionChecker;
    private readonly IUserInfoProvider _userInfoProvider;

    #endregion

    #region Constructor

    public UserInfoFeature(IPermissionChecker permissionChecker, IUserInfoProvider userInfoProvider, IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
        _permissionChecker = permissionChecker;
        _userInfoProvider = userInfoProvider;
    }

    #endregion

    #region Methods

    public Task<bool> VerifyPermissionAsync(ISecurityContext securityContext, UserInfoRequest request, CancellationToken cancellationToken = default)
    {
        return Task.Run(() => _permissionChecker.IsAuthenticated(request.User), cancellationToken);
    }

    public override async Task<UserInfoResponse> ExecuteAsync(UserInfoRequest request, CancellationToken cancellationToken = default)
    {
        IUserInfoModel? userInfo = await _userInfoProvider.GetUserInfoAsync(request.User, cancellationToken);
        if (userInfo is null)
        {
            throw new InvalidOperationException("Unable to retrieve user information from the user info provider.");
        }

        IReadOnlyDictionary<StaticTextKey, string> staticTexts = await GetStaticTextsAsync(request, userInfo, cancellationToken);

        return new UserInfoResponse(userInfo, staticTexts);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(UserInfoRequest request, IUserInfoModel userInfo)
    {
        object[] noArguments = Array.Empty<object>();

        return new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.MailAddress, noArguments },
            { StaticTextKey.Permissions, noArguments },
            { StaticTextKey.FinancialManagement, noArguments },
            { StaticTextKey.Administrator, noArguments },
            { StaticTextKey.Creator, noArguments },
            { StaticTextKey.Modifier, noArguments },
            { StaticTextKey.Viewer, noArguments },
            { StaticTextKey.CommonData, noArguments },
            { StaticTextKey.PrimaryAccounting, noArguments },
            { StaticTextKey.Accountings, noArguments },
        };
    }

    #endregion
}