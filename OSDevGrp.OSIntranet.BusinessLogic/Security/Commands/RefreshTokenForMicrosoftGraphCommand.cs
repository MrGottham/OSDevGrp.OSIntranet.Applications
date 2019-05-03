using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    public class RefreshTokenForMicrosoftGraphCommand : IRefreshTokenForMicrosoftGraphCommand
    {
        #region Constructor

        public RefreshTokenForMicrosoftGraphCommand(Uri redirectUri, IRefreshableToken refreshableToken)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri))
                .NotNull(refreshableToken, nameof(refreshableToken));

            RedirectUri = redirectUri;
            RefreshableToken = refreshableToken;
        }

        #endregion

        #region variables

        public Uri RedirectUri { get; }

        public IRefreshableToken RefreshableToken { get; }

        #endregion
    }
}
