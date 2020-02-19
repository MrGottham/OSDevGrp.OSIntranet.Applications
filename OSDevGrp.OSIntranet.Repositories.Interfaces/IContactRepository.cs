using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IContactRepository : IRepository
    {
        Task<IContact> ApplyContactSupplementAsync(IContact contact);

        Task<IEnumerable<IContact>> ApplyContactSupplementAsync(IEnumerable<IContact> contacts);

        Task<IContact> CreateOrUpdateContactSupplementAsync(IContact contact, string existingExternalIdentifier = null);

        Task<IContact> DeleteContactSupplementAsync(IContact contact);

        Task<IEnumerable<IContactGroup>> GetContactGroupsAsync();

        Task<IContactGroup> GetContactGroupAsync(int number);

        Task<IContactGroup> CreateContactGroupAsync(IContactGroup contactGroup);

        Task<IContactGroup> UpdateContactGroupAsync(IContactGroup contactGroup);

        Task<IContactGroup> DeleteContactGroupAsync(int number);

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
