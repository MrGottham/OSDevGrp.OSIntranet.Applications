using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.CommandHandlers;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Security.CommandHandlers
{
    internal class GenerateTokenCommandHandler : CommandHandlerNonTransactionalBase, ICommandHandler<IGenerateTokenCommand, IToken>
	{
		#region Private variables

		private readonly IClaimsIdentityResolver _claimsIdentityResolver;
		private readonly ITokenGenerator _tokenGenerator;

		#endregion

		#region Constructor

		public GenerateTokenCommandHandler(IClaimsIdentityResolver claimsIdentityResolver, ITokenGenerator tokenGenerator)
		{
			NullGuard.NotNull(claimsIdentityResolver, nameof(claimsIdentityResolver))
				.NotNull(tokenGenerator, nameof(tokenGenerator));

			_claimsIdentityResolver = claimsIdentityResolver;
			_tokenGenerator = tokenGenerator;
		}

		#endregion

		#region Methods

		public Task<IToken> ExecuteAsync(IGenerateTokenCommand generateTokenCommand)
		{
			NullGuard.NotNull(generateTokenCommand, nameof(generateTokenCommand));

			return Task.FromResult(_tokenGenerator.Generate(_claimsIdentityResolver.GetCurrentClaimsIdentity(), TimeSpan.FromHours(1)));
		}

		#endregion
	}
}