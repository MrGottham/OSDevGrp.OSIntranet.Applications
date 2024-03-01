using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Options;
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
            return new Repositories.MicrosoftGraphRepository(CreateMicrosoftSecurityOptions(), CreateLoggerFactory());
        }

        private IOptions<MicrosoftSecurityOptions> CreateMicrosoftSecurityOptions()
        {
            return Microsoft.Extensions.Options.Options.Create(CreateTestConfiguration().GetMicrosoftSecurityOptions());
        }

        protected static IRefreshableToken CreateToken()
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