using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic
{
    public interface IKeyGenerator
    {
        Task<string> GenerateGenericKeyAsync(IEnumerable<string> keyElementCollection);

        Task<string> GenerateUserSpecificKeyAsync(IEnumerable<string> keyElementCollection);
    }
}