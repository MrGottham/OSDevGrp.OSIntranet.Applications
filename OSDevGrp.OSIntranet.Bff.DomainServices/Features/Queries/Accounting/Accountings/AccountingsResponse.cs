using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.Accountings;

public class AccountingsResponse : PageResponseBase
{
    #region Constructor

    public AccountingsResponse(bool creationAllowed, IReadOnlyCollection<AccountingModel> accountings, IReadOnlyDictionary<StaticTextKey, string> staticTexts)
        : base(staticTexts)
    {
        CreationAllowed = creationAllowed;
        Accountings = accountings;
    }

    #endregion

    #region Properties

    public bool CreationAllowed { get; }

    public IReadOnlyCollection<AccountingModel> Accountings { get; }

    #endregion
}