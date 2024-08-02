using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface IAuthorizationCodeGenerator
    {
        Task<IAuthorizationCode> GenerateAsync();
    }
}