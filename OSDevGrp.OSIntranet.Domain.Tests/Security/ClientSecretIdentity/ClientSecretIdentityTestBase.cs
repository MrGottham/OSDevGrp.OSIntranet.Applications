using System.Collections.Generic;
using System.Security.Claims;
using AutoFixture;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.ClientSecretIdentity
{
    public abstract class ClientSecretIdentityTestBase
    {
        #region Protected variables

        protected Fixture Fixture;

        #endregion

        protected IClientSecretIdentity CreateSut()
        {
            return new Domain.Security.ClientSecretIdentity(Fixture.Create<int>(), Fixture.Create<string>(), Fixture.Create<string>(), Fixture.Create<string>(), new List<Claim>(0));
        }
    }
}
