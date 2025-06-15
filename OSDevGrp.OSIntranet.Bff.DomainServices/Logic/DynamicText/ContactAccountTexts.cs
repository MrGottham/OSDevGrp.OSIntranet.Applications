using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class ContactAccountTexts : DynamicTextsBase<ContactAccountModel>, IContactAccountTexts
{
    #region Constructor

    public ContactAccountTexts(ContactAccountModel model, IFormatProvider formatProvider) 
        : base(model, formatProvider)
    {
    }

    #endregion
}