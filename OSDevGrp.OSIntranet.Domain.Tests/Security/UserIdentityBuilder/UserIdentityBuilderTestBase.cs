using AutoFixture;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserIdentityBuilder
{
    public abstract class UserIdentityBuilderTestBase
    {
        #region Protected vairables

        protected Fixture Fixture;

        #endregion

        protected IUserIdentityBuilder CreateSut(string externalUserIdentifier = null)
        {
            return new Domain.Security.UserIdentityBuilder(externalUserIdentifier ?? Fixture.Create<string>());
        }
    }
}
