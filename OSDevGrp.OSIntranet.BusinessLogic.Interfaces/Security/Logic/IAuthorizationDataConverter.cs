using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IAuthorizationDataConverter
    {
        Task<IKeyValueEntry> ToKeyValueEntryAsync(IAuthorizationCode authorizationCode, IEnumerable<Claim> claims);

        Task<IAuthorizationCode> ToAuthorizationCodeAsync(IKeyValueEntry keyValueEntry, out IEnumerable<Claim> claims);
    }
}