using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.BusinessLogic.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.Commands
{
    internal class GenerateAuthorizationCodeCommand : AuthorizationStateCommandBase, IGenerateAuthorizationCodeCommand
    {
        #region Constructor

        public GenerateAuthorizationCodeCommand(string authorizationState, IReadOnlyCollection<Claim> claims, IToken idToken, Func<byte[], byte[]> unprotect)
            : base(authorizationState, unprotect)
        {
            NullGuard.NotNull(claims, nameof(claims))
                .NotNull(idToken, nameof(idToken));

            Claims = claims;
            IdToken = idToken;
        }

        #endregion

        #region Properties

        public IReadOnlyCollection<Claim> Claims { get; }

        public IToken IdToken { get; }

        #endregion

        #region Methods

        public override IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return base.Validate(validator)
                .ValidateClaims(Claims, GetType(), nameof(Claims))
                .ValidateIdToken(IdToken, GetType(), nameof(IdToken));
        }

        #endregion
    }
}