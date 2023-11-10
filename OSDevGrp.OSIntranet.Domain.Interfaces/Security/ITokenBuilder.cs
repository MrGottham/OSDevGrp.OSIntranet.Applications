using System;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
	public interface ITokenBuilder<out TToken> where TToken : class, IToken
	{
		ITokenBuilder<TToken> WithTokenType(string tokenType);

		ITokenBuilder<TToken> WithAccessToken(string accessToken);

		ITokenBuilder<TToken> WithRefreshToken(string refreshToken);

		ITokenBuilder<TToken> WithExpires(DateTime expires);

		TToken Build();
	}
}