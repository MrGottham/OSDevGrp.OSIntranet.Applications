using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;
using System.Globalization;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Index;

internal class IndexFeature : PageFeatureBase<IndexRequest, IndexResponse, ClaimsPrincipal>
{
    #region Private variables

    private readonly IUserInfoProvider _userInfoProvider;
    private readonly IBuildInfoProvider _buildInfoProvider;

    #endregion

    #region Constructor

    public IndexFeature(IUserInfoProvider userInfoProvider, IBuildInfoProvider buildInfoProvider, IStaticTextProvider staticTextProvider) 
        : base(staticTextProvider)
    {
        _userInfoProvider = userInfoProvider;
        _buildInfoProvider = buildInfoProvider;
    }

    #endregion

    #region Methods

    public async override Task<IndexResponse> ExecuteAsync(IndexRequest request, CancellationToken cancellationToken = default)
    {
        ClaimsPrincipal user = request.SecurityContext.User;

        IUserInfoModel? userInfo = null;
        Task getUserInfoTask = _userInfoProvider.GetUserInfoAsync(user, cancellationToken).ContinueWith(task => userInfo = task.Result, cancellationToken);

        IReadOnlyDictionary<StaticTextKey, string> staticTexts = new Dictionary<StaticTextKey, string>();
        Task getStaticTextsTask = GetStaticTextsAsync(request, user, cancellationToken).ContinueWith(task => staticTexts = task.Result, cancellationToken);

        await Task.WhenAll(getUserInfoTask, getStaticTextsTask);

        return new IndexResponse(
            _userInfoProvider.IsAuthenticated(user) ? StaticTextKey.OSDevelopmentGroup : StaticTextKey.MrGotthamsHomepage, 
            userInfo,
            staticTexts);
    }

    protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(IndexRequest request, ClaimsPrincipal user)
    {
        bool isAuthenticated = _userInfoProvider.IsAuthenticated(user);

        IBuildInfo buildInfo = _buildInfoProvider.GetBuildInfo(request.ExecutingAssembly);
        DateTimeOffset buildDateTime = buildInfo.BuildTime;

        object[] noArguments = Array.Empty<object>();

        IDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>();
        if (isAuthenticated)
        {
            staticTextSpecifications.Add(StaticTextKey.OSDevelopmentGroup, noArguments);
        }
        else
        {
            staticTextSpecifications.Add(StaticTextKey.MrGotthamsHomepage, noArguments);
        }
        staticTextSpecifications.Add(StaticTextKey.Copyright, [buildDateTime.Year]);
        staticTextSpecifications.Add(StaticTextKey.BuildInfo, [buildDateTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture)]);
        staticTextSpecifications.Add(StaticTextKey.Start, noArguments);
        staticTextSpecifications.Add(StaticTextKey.Login, noArguments);
        staticTextSpecifications.Add(StaticTextKey.Logout, noArguments);

        return staticTextSpecifications.AsReadOnly();
    }

    #endregion
}