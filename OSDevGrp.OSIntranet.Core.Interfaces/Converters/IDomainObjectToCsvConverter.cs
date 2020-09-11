using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Converters
{
    public interface IDomainObjectToCsvConverter<in TDomainObject> where TDomainObject : class
    {
        Task<IEnumerable<string>> GetColumnNamesAsync();

        Task<IEnumerable<string>> ConvertAsync(TDomainObject domainObject);
    }
}