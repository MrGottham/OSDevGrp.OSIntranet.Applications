using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
	public interface IExternalTokenCreator
	{
		bool CanBuild(IDictionary<string, string> authenticationSessionItems);

		IToken Build(IDictionary<string, string> authenticationSessionItems);
	}
}