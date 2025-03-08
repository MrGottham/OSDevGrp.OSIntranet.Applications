using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Index;

public class IndexResponse : PageResponseBase
{
    #region Constructor

    public IndexResponse(StaticTextKey titleSelector, IUserInfoModel? userInfo, IReadOnlyDictionary<StaticTextKey, string> staticTexts) 
        : base(staticTexts)
    {
        TitleSelector = titleSelector;
        UserInfo = userInfo;
    }

    #endregion

    #region Properties

    public StaticTextKey TitleSelector { get; }

    public IUserInfoModel? UserInfo { get; }

    #endregion
}