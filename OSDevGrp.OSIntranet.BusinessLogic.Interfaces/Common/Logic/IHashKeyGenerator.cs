using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Logic
{
    public interface IHashKeyGenerator
    {
        Task<string> ComputeHashAsync(IEnumerable<byte> byteCollection);
    }
}