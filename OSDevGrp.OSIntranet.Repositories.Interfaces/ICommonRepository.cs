using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface ICommonRepository : IRepository
    {
        Task<IEnumerable<ILetterHead>> GetLetterHeadsAsync();

        Task<ILetterHead> GetLetterHeadAsync(int number);

        Task<ILetterHead> CreateLetterHeadAsync(ILetterHead letterHead);

        Task<ILetterHead> UpdateLetterHeadAsync(ILetterHead letterHead);

        Task<ILetterHead> DeleteLetterHeadAsync(int number);

        Task<IKeyValueEntry> PullKeyValueEntryAsync(string key);

        Task<IKeyValueEntry> PushKeyValueEntryAsync(IKeyValueEntry keyValueEntry);

        Task<IKeyValueEntry> DeleteKeyValueEntryAsync(string key);

        Task<IEnumerable<INationality>> GetNationalitiesAsync();

        Task<INationality> GetNationalityAsync(int number);

        Task CreateNationalityAsync(INationality nationality);

        Task UpdateNationalityAsync(INationality nationality);

        Task DeleteNationalityAsync(int number);

        Task<IEnumerable<ILanguage>> GetLanguagesAsync();

        Task<ILanguage> GetLanguageAsync(int number);

        Task CreateLanguageAsync(ILanguage language);

        Task UpdateLanguageAsync(ILanguage language);

        Task DeleteLanguageAsync(int number);
    }
}