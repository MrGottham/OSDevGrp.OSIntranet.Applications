using System;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Queries;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Queries
{
    public class GetAuthorizeUriForMicrosoftGraphQuery : IGetAuthorizeUriForMicrosoftGraphQuery
    {
        #region Constructor

        public GetAuthorizeUriForMicrosoftGraphQuery(Uri redirectUri, Guid stateIdentifier)
        {
            NullGuard.NotNull(redirectUri, nameof(redirectUri));

            RedirectUri = redirectUri;
            StateIdentifier = stateIdentifier;
        }

        #endregion

        #region Properties

        public Uri RedirectUri { get; }

        public Guid StateIdentifier { get; }

        #endregion
    }
}
