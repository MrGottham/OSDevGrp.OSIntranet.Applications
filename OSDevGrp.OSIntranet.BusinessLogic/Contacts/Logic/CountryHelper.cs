using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Logic
{
    public class CountryHelper : ICountryHelper
    {
        #region Private variables

        private readonly IClaimResolver _claimResolver;

        #endregion

        #region Constructor

        public CountryHelper(IClaimResolver claimResolver)
        {
            NullGuard.NotNull(claimResolver, nameof(claimResolver));

            _claimResolver = claimResolver;
        }

        #endregion

        #region Methods

        public ICountry ApplyLogicForPrincipal(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            string countryCode = _claimResolver.GetCountryCode();
            country.ApplyDefaultForPrincipal(countryCode);

            return country;
        }

        public IEnumerable<ICountry> ApplyLogicForPrincipal(IEnumerable<ICountry> countryCollection)
        {
            NullGuard.NotNull(countryCollection, nameof(countryCollection));

            string countryCode = _claimResolver.GetCountryCode();

            return countryCollection.Select(country =>
                {
                    country.ApplyDefaultForPrincipal(countryCode);
                    return country;
                })
                .ToList();
        }

        #endregion
    }
}
