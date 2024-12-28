using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Extensions;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    internal class GenerateIdTokenCommandHandler : CommandHandlerNonTransactionalBase, ICommandHandler<IGenerateIdTokenCommand, IToken>
    {
        #region Private variables

        private readonly IIdTokenContentFactory _idTokenContentFactory;
        private readonly ITokenGenerator _tokenGenerator;

        #endregion

        #region Constructor

        public GenerateIdTokenCommandHandler(IIdTokenContentFactory idTokenContentFactory, ITokenGenerator tokenGenerator)
        {
            NullGuard.NotNull(idTokenContentFactory, nameof(idTokenContentFactory))
                .NotNull(tokenGenerator, nameof(tokenGenerator));

            _idTokenContentFactory = idTokenContentFactory;
            _tokenGenerator = tokenGenerator;
        }

        #endregion

        #region Methods

        public Task<IToken> ExecuteAsync(IGenerateIdTokenCommand generateIdTokenCommand)
        {
            NullGuard.NotNull(generateIdTokenCommand, nameof(generateIdTokenCommand));

            return Task.Run(() =>
            {
                Claim nameIdentifierClaim = generateIdTokenCommand.ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (nameIdentifierClaim == null || string.IsNullOrWhiteSpace(nameIdentifierClaim.Value))
                {
                    throw new IntranetExceptionBuilder(ErrorCode.UnableToGenerateIdTokenForAuthenticatedUser).Build();
                }

                IIdTokenContentBuilder idTokenContentBuilder = _idTokenContentFactory.Create(nameIdentifierClaim.Value.ComputeSha512Hash(), generateIdTokenCommand.AuthenticationTime);

                string nonce = generateIdTokenCommand.Nonce;
                if (string.IsNullOrWhiteSpace(nonce) == false)
                {
                    idTokenContentBuilder.WithNonce(nonce);
                }

                return _tokenGenerator.Generate(new ClaimsIdentity(idTokenContentBuilder.Build()));
            });
        }

        #endregion
    }
}