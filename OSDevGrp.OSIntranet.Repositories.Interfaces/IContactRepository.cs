using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IContactRepository : IRepository
    {
        Task<IEnumerable<ICountry>> GetCountriesAsync();

        Task<ICountry> GetCountryAsync(string code);

        Task<ICountry> CreateCountryAsync(ICountry country);

        Task<ICountry> UpdateCountryAsync(ICountry country);

        Task<ICountry> DeleteCountryAsync(string code);

        Task<IEnumerable<IPostalCode>> GetPostalCodesAsync(string countryCode);

        Task<IPostalCode> GetPostalCodeAsync(string countryCode, string postalCode);

        Task<IPostalCode> CreatePostalCodeAsync(IPostalCode postalCode);

        Task<IPostalCode> UpdatePostalCodeAsync(IPostalCode postalCode);

        Task<IPostalCode> DeletePostalCodeAsync(string countryCode, string postalCode);
    }
}
