using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;

public class UserInfoResponse : PageResponseBase
{
    #region Constructor

    public UserInfoResponse(IUserInfoModel userInfo, IReadOnlyDictionary<StaticTextKey, string> staticTexts) 
        : base(staticTexts)
    {
        UserInfo = userInfo;
    }

    #endregion

    #region Properties

    public IUserInfoModel UserInfo { get; }

    #endregion
}