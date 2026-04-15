using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountTexts : DynamicTextsBase<AccountModel>, IAccountTexts
{
    #region Constructor

    public AccountTexts(AccountModel model, IFormatProvider formatProvider) 
        : base(model, formatProvider)
    {
    }

    #endregion
}