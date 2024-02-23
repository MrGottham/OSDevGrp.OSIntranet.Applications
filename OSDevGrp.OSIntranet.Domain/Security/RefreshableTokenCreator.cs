using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	internal sealed class RefreshableTokenCreator : TokenCreatorBase<IRefreshableToken>
	{
		#region Private variables

		private string _refreshToken;

		#endregion

		#region Methods

		public override ITokenBuilder<IRefreshableToken> WithRefreshToken(string refreshToken)
		{
			NullGuard.NotNullOrWhiteSpace(refreshToken, nameof(refreshToken));

			_refreshToken = refreshToken;

			return this;
		}

		public override IRefreshableToken FromTokenBasedQuery(ITokenBasedQuery tokenBasedQuery) => throw new NotSupportedException($"Cannot build an {nameof(IRefreshableToken)} from {tokenBasedQuery?.GetType().Name ?? nameof(ITokenBasedQuery)}.");

		public override IRefreshableToken FromTokenBasedQuery(IRefreshableTokenBasedQuery refreshableTokenBasedQuery)
		{
			NullGuard.NotNull(refreshableTokenBasedQuery, nameof(refreshableTokenBasedQuery));

			return WithTokenType(refreshableTokenBasedQuery.TokenType)
				.WithAccessToken(refreshableTokenBasedQuery.AccessToken)
				.WithRefreshToken(refreshableTokenBasedQuery.RefreshToken)
				.WithExpires(refreshableTokenBasedQuery.Expires)
			.Build();
		}

		public override IRefreshableToken FromTokenBasedCommand(ITokenBasedCommand tokenBasedCommand) => throw new NotSupportedException($"Cannot build an {nameof(IRefreshableToken)} from {tokenBasedCommand?.GetType().Name ?? nameof(ITokenBasedCommand)}.");

		public override IRefreshableToken FromTokenBasedCommand(IRefreshableTokenBasedCommand refreshableTokenBasedCommand)
		{
			NullGuard.NotNull(refreshableTokenBasedCommand, nameof(refreshableTokenBasedCommand));

			return WithTokenType(refreshableTokenBasedCommand.TokenType)
				.WithAccessToken(refreshableTokenBasedCommand.AccessToken)
				.WithRefreshToken(refreshableTokenBasedCommand.RefreshToken)
				.WithExpires(refreshableTokenBasedCommand.Expires)
				.Build();
		}

		protected override IRefreshableToken Build(string tokenType, string accessType, DateTime expires)
		{
			NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType))
				.NotNullOrWhiteSpace(accessType, nameof(accessType));

			if (string.IsNullOrWhiteSpace(_refreshToken))
			{
				throw new IntranetExceptionBuilder(ErrorCode.ValueNotSetByNamedMethod, nameof(WithRefreshToken), GetType().Name).Build();
			}

			return new RefreshableToken(tokenType, accessType, _refreshToken, expires);
		}

		#endregion
	}
}