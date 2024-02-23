using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;
using OSDevGrp.OSIntranet.Domain.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	internal abstract class TokenCreatorBase<TToken> : ITokenCreator<TToken> where TToken : class, IToken
	{
		#region Private variables

		private string _tokenType;
		private string _accessToken;
		private DateTime? _expires;

		#endregion

		#region Methods

		public ITokenBuilder<TToken> WithTokenType(string tokenType)
		{
			NullGuard.NotNullOrWhiteSpace(tokenType, nameof(tokenType));

			_tokenType = tokenType;

			return this;
		}

		public ITokenBuilder<TToken> WithAccessToken(string accessToken)
		{
			NullGuard.NotNullOrWhiteSpace(accessToken, nameof(accessToken));

			_accessToken = accessToken;

			return this;
		}

		public abstract ITokenBuilder<TToken> WithRefreshToken(string refreshToken);

		public ITokenBuilder<TToken> WithExpires(DateTime expires)
		{
			_expires = expires;

			return this;
		}

		public TToken Build()
		{
			if (string.IsNullOrWhiteSpace(_tokenType))
			{
				throw new IntranetExceptionBuilder(ErrorCode.ValueNotSetByNamedMethod, nameof(WithTokenType), GetType().Name).Build();
			}

			if (string.IsNullOrWhiteSpace(_accessToken))
			{
				throw new IntranetExceptionBuilder(ErrorCode.ValueNotSetByNamedMethod, nameof(WithAccessToken), GetType().Name).Build();
			}

			if (_expires == null)
			{
				throw new IntranetExceptionBuilder(ErrorCode.ValueNotSetByNamedMethod, nameof(WithExpires), GetType().Name).Build();
			}

			return Build(_tokenType, _accessToken, _expires.Value);
		}

		public TToken FromByteArray(byte[] byteArray)
		{
			NullGuard.NotNull(byteArray, nameof(byteArray));

			if (typeof(TToken) == typeof(IToken))
			{
				return DomainHelper.FromByteArray<Token>(byteArray) as TToken;
			}

			if (typeof(TToken) == typeof(IRefreshableToken))
			{
				return DomainHelper.FromByteArray<RefreshableToken>(byteArray) as TToken;
			}

			return DomainHelper.FromByteArray<TToken>(byteArray);
		}

		public TToken FromBase64String(string base64String)
		{
			NullGuard.NotNullOrWhiteSpace(base64String, nameof(base64String));

			return FromByteArray(Convert.FromBase64String(base64String));
		}

		public abstract TToken FromTokenBasedQuery(ITokenBasedQuery tokenBasedQuery);

		public abstract TToken FromTokenBasedQuery(IRefreshableTokenBasedQuery refreshableTokenBasedQuery);

		public abstract TToken FromTokenBasedCommand(ITokenBasedCommand tokenBasedCommand);

		public abstract TToken FromTokenBasedCommand(IRefreshableTokenBasedCommand refreshableTokenBasedCommand);

		protected abstract TToken Build(string tokenType, string accessType, DateTime expires);

		#endregion
	}
}