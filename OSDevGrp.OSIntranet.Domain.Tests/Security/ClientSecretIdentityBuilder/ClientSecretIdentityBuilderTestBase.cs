using AutoFixture;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentityBuilder
{
    public abstract class ClientSecretIdentityBuilderTestBase
    {
        #region Protected vairables

        protected Fixture Fixture;

        #endregion

        protected IClientSecretIdentityBuilder CreateSut(string friendlyName = null)
        {
            return new Domain.Security.ClientSecretIdentityBuilder(friendlyName ?? Fixture.Create<string>());
        }
    }
}
