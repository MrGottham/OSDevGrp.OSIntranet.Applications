using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using System;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    internal class GenerateIdTokenCommand : AuthorizationStateCommandBase, IGenerateIdTokenCommand
    {
        #region Constructor

        public GenerateIdTokenCommand(ClaimsPrincipal claimsPrincipal, DateTimeOffset authenticationTime, string authorizationState, Func<byte[], byte[]> unprotect)
            : base(authorizationState, unprotect)
        {
            NullGuard.NotNull(claimsPrincipal, nameof(claimsPrincipal));

            ClaimsPrincipal = claimsPrincipal;
            AuthenticationTime = authenticationTime;
        }

        #endregion

        #region Properties

        public ClaimsPrincipal ClaimsPrincipal { get; }

        public DateTimeOffset AuthenticationTime { get; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return base.Validate(validator)
                .Object.ShouldNotBeNull(ClaimsPrincipal, GetType(), nameof(ClaimsPrincipal))
                .DateTime.ShouldBePastDateTime(AuthenticationTime.UtcDateTime, GetType(), nameof(AuthenticationTime));
        }

        #endregion
    }
}