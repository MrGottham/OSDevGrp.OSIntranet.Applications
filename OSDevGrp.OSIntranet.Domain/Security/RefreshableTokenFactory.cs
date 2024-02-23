using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
	public class RefreshableTokenFactory
	{
		#region Methods

		public static ITokenCreator<IRefreshableToken> Create()
		{
			return new RefreshableTokenCreator();
		}

		#endregion
	}
}