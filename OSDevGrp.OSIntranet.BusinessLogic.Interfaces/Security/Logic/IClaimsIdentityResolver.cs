using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
	public interface IClaimsIdentityResolver
	{
		ClaimsIdentity GetCurrentClaimsIdentity();
	}
}