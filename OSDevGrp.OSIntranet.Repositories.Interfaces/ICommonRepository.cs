using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;

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
    }
}