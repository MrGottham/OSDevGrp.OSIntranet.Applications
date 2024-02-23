using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
	public interface ITokenGenerator
	{
        IToken Generate(ClaimsIdentity claimsIdentity);
    }
}