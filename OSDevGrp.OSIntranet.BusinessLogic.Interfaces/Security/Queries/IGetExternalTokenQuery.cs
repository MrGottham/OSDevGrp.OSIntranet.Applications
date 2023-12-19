using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries
{
	public interface IGetExternalTokenQuery : IQuery
	{
		ClaimsPrincipal ClaimsPrincipal { get; }

		Func<string, string> Unprotect { get; }
	}
}