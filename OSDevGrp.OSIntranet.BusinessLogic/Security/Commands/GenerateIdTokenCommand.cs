using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    internal class GenerateIdTokenCommand : IGenerateIdTokenCommand
    {
        #region Constructor

        public GenerateIdTokenCommand(ClaimsIdentity claimsIdentity, DateTimeOffset authenticationTime, string nonce)
        {
            NullGuard.NotNull(claimsIdentity, nameof(claimsIdentity));

            ClaimsIdentity = claimsIdentity;
            AuthenticationTime = authenticationTime;
            Nonce = nonce;
        }

        #endregion

        #region Properties

        public ClaimsIdentity ClaimsIdentity{ get; }

        public DateTimeOffset AuthenticationTime { get; }

        public string Nonce { get; }

        #endregion
    }
}