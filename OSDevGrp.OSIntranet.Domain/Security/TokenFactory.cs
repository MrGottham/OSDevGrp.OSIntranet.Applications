using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	public static class TokenFactory
	{
		#region Methods

		public static ITokenCreator<IToken> Create()
		{
			return new TokenCreator();
		}

		#endregion
	}
}