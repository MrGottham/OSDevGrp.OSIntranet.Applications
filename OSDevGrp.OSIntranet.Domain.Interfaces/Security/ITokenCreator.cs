using OSDevGrp.OSIntranet.Core.Interfaces.Commands;
using OSDevGrp.OSIntranet.Core.Interfaces.Queries;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Security
{
	public interface ITokenCreator<out TToken> : ITokenBuilder<TToken> where TToken : class, IToken
	{
		TToken FromByteArray(byte[] byteArray);

		TToken FromBase64String(string base64String);

		TToken FromTokenBasedQuery(ITokenBasedQuery tokenBasedQuery);

		TToken FromTokenBasedQuery(IRefreshableTokenBasedQuery refreshableTokenBasedQuery);

		TToken FromTokenBasedCommand(ITokenBasedCommand tokenBasedCommand);

		TToken FromTokenBasedCommand(IRefreshableTokenBasedCommand refreshableTokenBasedCommand);
	}
}