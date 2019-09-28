using System.Collections.Generic;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic
{
    public interface ICountryHelper
    {
        ICountry ApplyLogicForPrincipal(ICountry country);

        IEnumerable<ICountry> ApplyLogicForPrincipal(IEnumerable<ICountry> countryCollection);
    }
}
