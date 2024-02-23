using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security;

namespace OSDevGrp.OSIntranet.Mvc.Tests.Helpers.Security.MicrosoftGraphTokenHelper
{
    [TestFixture]
    public class StoreTokenAsyncTests
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Mock<ICommandBus> _commandBusMock;
        private Mock<ITrustedDomainHelper> _trustedDomainHelperMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _commandBusMock = new Mock<ICommandBus>();
            _trustedDomainHelperMock = new Mock<ITrustedDomainHelper>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }

        [Test]
        [Category("UnitTest")]
        public async Task StoreTokenAsync_WhenTokenWasGenerated_AssertProtectWasCalledOnDataProtectorWithTokenByteArray()
        {
            ITokenHelper sut = CreateSut();

            IRefreshableToken token = RefreshableTokenFactory.Create()
	            .WithTokenType(_fixture.Create<string>())
	            .WithAccessToken(_fixture.Create<string>())
	            .WithRefreshToken(_fixture.Create<string>())
	            .WithExpires(_fixture.Create<DateTime>())
	            .Build();

            HttpContext httpContext = CreateHttpContext();
            string base64Token = token.ToBase64String();
            await sut.StoreTokenAsync(httpContext, base64Token);

            _dataProtectorMock.Verify(m => m.Protect(It.Is<byte[]>(value => value != null && string.CompareOrdinal(Encoding.UTF8.GetString(value), Encoding.UTF8.GetString(token.ToByteArray())) == 0)), Times.Once);
        }

        private ITokenHelper CreateSut()
        {
            _dataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(_dataProtectorMock.Object);
            _dataProtectorMock.Setup(m => m.Protect(It.IsAny<byte[]>()))
                .Returns(_fixture.CreateMany<byte>(_random.Next(512, 1024)).ToArray);

            return new Mvc.Helpers.Security.MicrosoftGraphTokenHelper(_queryBusMock.Object, _commandBusMock.Object, _trustedDomainHelperMock.Object, _dataProtectionProviderMock.Object);
        }

        private HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext();
        }
    }
}