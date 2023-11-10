using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

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
            return RefreshableTokenFactory.Create()
	            .WithTokenType(TokenType)
	            .WithAccessToken(AccessToken)
	            .WithRefreshToken(RefreshToken)
	            .WithExpires(DateTime.UtcNow.AddHours(1))
	            .Build();
        }

        #endregion
    }
}