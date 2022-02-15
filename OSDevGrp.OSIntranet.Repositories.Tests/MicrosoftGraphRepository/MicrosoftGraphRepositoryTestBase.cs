using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.MicrosoftGraphRepository
{
    public abstract class MicrosoftGraphRepositoryTestBase : RepositoryTestBase
    {
        #region Private constants

        private const string TokenType = "[TBD]";
        private const string AccessToken = "[TBD]";
        private const string RefreshToken = "[TBD]";

        #endregion

        #region Methods

        protected IMicrosoftGraphRepository CreateSut()
        {
            return new Repositories.MicrosoftGraphRepository(CreateTestConfiguration(), CreateLoggerFactory());
        }

        protected IRefreshableToken CreateToken()
        {
            return new RefreshableToken(TokenType, AccessToken, RefreshToken, DateTime.Today.AddDays(1));
        }

        #endregion
    }
}