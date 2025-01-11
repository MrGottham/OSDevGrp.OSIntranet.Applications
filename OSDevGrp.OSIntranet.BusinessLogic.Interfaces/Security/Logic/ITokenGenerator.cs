using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic
{
    public interface ITokenGenerator
	{
        string SigningAlgorithm { get; }

        IToken Generate(ClaimsIdentity claimsIdentity, TimeSpan expiresIn, string audience = null);
    }
}