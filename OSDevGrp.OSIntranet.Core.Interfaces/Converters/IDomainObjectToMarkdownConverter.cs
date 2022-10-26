using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.Converters
{
    public interface IDomainObjectToMarkdownConverter<in TDomainObject> where TDomainObject : class
    {
        Task<string> ConvertAsync(TDomainObject domainObject);
    }
}