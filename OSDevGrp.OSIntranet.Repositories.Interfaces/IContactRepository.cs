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
    }
}
