using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using System;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    [TestFixture]
    public class AcquireTokenForAuthorizationCodeGrantFlowTests : AcquireTokenTestBase
    {
        #region Private variables

        private Mock<ICommandBus> _commandBusMock;
        private Mock<IQueryBus> _queryBusMock;
        private Mock<IDataProtectionProvider> _dataProtectionProviderMock;
        private Mock<IDataProtector> _dataProtectorMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Fixture _fixture;
        private Random _random;

        #endregion

        #region Properties

        protected override Mock<ICommandBus> CommandBusMock => _commandBusMock;

        protected override Mock<IQueryBus> QueryBusMock => _queryBusMock;

        protected override Mock<IDataProtectionProvider> DataProtectionProviderMock => _dataProtectionProviderMock;

        protected override Mock<IDataProtector> DataProtectorMock => _dataProtectorMock;

        protected override Mock<IAuthenticationService> AuthenticationServiceMock => _authenticationServiceMock;

        protected override Fixture Fixture => _fixture;

        protected override Random Random => _random;

        protected string GrantType => "authorization_code";

        #endregion

        [SetUp]
        public void SetUp()
        {
            _commandBusMock = new Mock<ICommandBus>();
            _queryBusMock = new Mock<IQueryBus>();
            _dataProtectionProviderMock = new Mock<IDataProtectionProvider>();
            _dataProtectorMock = new Mock<IDataProtector>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();
            _fixture = new Fixture();
            _random = new Random(_fixture.Create<int>());
        }
    }
}