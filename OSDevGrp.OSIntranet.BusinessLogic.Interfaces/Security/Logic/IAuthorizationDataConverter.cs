using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IAuthorizationDataConverter
    {
        Task<IKeyValueEntry> ToKeyValueEntryAsync(IAuthorizationCode authorizationCode, IReadOnlyCollection<Claim> claims, IReadOnlyDictionary<string, string> authorizationData);

        Task<IAuthorizationCode> ToAuthorizationCodeAsync(IKeyValueEntry keyValueEntry, out IReadOnlyCollection<Claim> claims, out IReadOnlyDictionary<string, string> authorizationData);
    }
}