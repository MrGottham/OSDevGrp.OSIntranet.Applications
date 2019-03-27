using System.Collections.Generic;
using System.Security.Claims;
using AutoFixture;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Tests.Security.UserIdentity
{
    public abstract class UserIdentityTestBase
    {
        #region Protected variables

        protected Fixture Fixture;

        #endregion

        protected IUserIdentity CreateSut()
        {
            return new Domain.Security.UserIdentity(Fixture.Create<int>(), Fixture.Create<string>(), new List<Claim>(0));
        }
    }
}
