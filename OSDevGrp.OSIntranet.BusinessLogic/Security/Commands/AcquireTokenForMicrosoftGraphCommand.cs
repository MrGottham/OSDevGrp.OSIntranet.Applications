using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public class AcquireTokenForMicrosoftGraphCommand : IAcquireTokenForMicrosoftGraphCommand
    {
        #region Constructor

        public AcquireTokenForMicrosoftGraphCommand(Uri redirectUri, string code)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNullOrWhiteSpace(code, nameof(code));

            RedirectUri = redirectUri;
            Code = code;
        }

        #endregion

        #region Properties

        public Uri RedirectUri { get; }

        public string Code { get; }

        #endregion
    }
}
