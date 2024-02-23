using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	internal sealed class TokenCreator : TokenCreatorBase<IToken>
	{
		#region Methods

		public override ITokenBuilder<IToken> WithRefreshToken(string refreshToken) => throw new NotSupportedException($"Refresh token is not supported for {typeof(IToken)}.");

		public override IToken FromTokenBasedQuery(ITokenBasedQuery tokenBasedQuery)
		{
			NullGuard.NotNull(tokenBasedQuery, nameof(tokenBasedQuery));

			return WithTokenType(tokenBasedQuery.TokenType)
				.WithAccessToken(tokenBasedQuery.AccessToken)
				.WithExpires(tokenBasedQuery.Expires)
				.Build();
		}

		public override IToken FromTokenBasedQuery(IRefreshableTokenBasedQuery refreshableTokenBasedQuery)
		{
			NullGuard.NotNull(refreshableTokenBasedQuery, nameof(refreshableTokenBasedQuery));

			return WithTokenType(refreshableTokenBasedQuery.TokenType)
				.WithAccessToken(refreshableTokenBasedQuery.AccessToken)
				.WithExpires(refreshableTokenBasedQuery.Expires)
				.Build();
		}

		public override IToken FromTokenBasedCommand(ITokenBasedCommand tokenBasedCommand)
		{
			NullGuard.NotNull(tokenBasedCommand, nameof(tokenBasedCommand));

			return WithTokenType(tokenBasedCommand.TokenType)
				.WithAccessToken(tokenBasedCommand.AccessToken)
				.WithExpires(tokenBasedCommand.Expires)
				.Build();
		}

		public override IToken FromTokenBasedCommand(IRefreshableTokenBasedCommand refreshableTokenBasedCommand)
		{
			NullGuard.NotNull(refreshableTokenBasedCommand, nameof(refreshableTokenBasedCommand));

			return WithTokenType(refreshableTokenBasedCommand.TokenType)
				.WithAccessToken(refreshableTokenBasedCommand.AccessToken)
				.WithExpires(refreshableTokenBasedCommand.Expires)
				.Build();
		}

		protected override IToken Build(string tokenType, string accessType, DateTime expires)
		{
			NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
				.NotNullOrWhiteSpace(accessType, nameof(accessType));

			return new Token(tokenType, accessType, expires);
		}

		#endregion
	}
}