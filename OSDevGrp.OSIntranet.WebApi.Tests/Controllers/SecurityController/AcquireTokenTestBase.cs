using AutoFixture;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Commands;
using OSDevGrp.OSIntranet.Core.Extensions;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Domain.TestHelpers;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.SecurityController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.SecurityController
{
    public abstract class AcquireTokenTestBase
    {
        #region Properties

        protected abstract Mock<ICommandBus> CommandBusMock { get; }

        protected abstract Mock<IQueryBus> QueryBusMock { get; }

        protected abstract Mock<IDataProtectionProvider> DataProtectionProviderMock { get; }

        protected abstract Mock<IDataProtector> DataProtectorMock { get; }

        protected abstract Mock<IAuthenticationService> AuthenticationServiceMock { get; }

        protected abstract Fixture Fixture { get; }

        protected abstract Random Random { get; }

        protected static string GrantTypePattern => "^(authorization_code|client_credentials){1}$";

        protected static string AuthorizationPattern => $"^(Basic){{1}}\\s+({Base64Pattern}){{1}}$";

        protected static readonly string AuthorizationParameterForClientIdAndClientSecretPattern = "^([a-f0-9]{32}){1}:([a-f0-9]{32}){1}$";

        private static string Base64Pattern => @"([A-Za-z0-9+\/]{4})*([A-Za-z0-9+\/]{3}=|[A-Za-z0-9+\/]{2}==)?";

        #endregion

        protected Controller CreateSut(HttpContext httpContext = null, bool hasToken = true, IToken token = null)
        {
            AuthenticationServiceMock.Setup(m => m.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);
            AuthenticationServiceMock.Setup(m => m.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            DataProtectionProviderMock.Setup(m => m.CreateProtector(It.IsAny<string>()))
                .Returns(DataProtectorMock.Object);

            CommandBusMock.Setup(m => m.PublishAsync<IGenerateTokenCommand, IToken>(It.IsAny<IGenerateTokenCommand>()))
                .Returns(Task.FromResult(hasToken ? token ?? Fixture.BuildTokenMock().Object : null));

            return new Controller(CommandBusMock.Object, QueryBusMock.Object, DataProtectionProviderMock.Object)
            {
                ControllerContext =
                {
                    HttpContext = httpContext ?? CreateHttpContext()
                }
            };
        }

        protected HttpContext CreateHttpContext()
        {
            return new DefaultHttpContext
            {
                RequestServices = CreateServiceProvider()
            };
        }

        private IServiceProvider CreateServiceProvider()
        {
            return CreateServiceCollection().BuildServiceProvider();
        }

        private IServiceCollection CreateServiceCollection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient(_ => AuthenticationServiceMock.Object);
            return serviceCollection;
        }

        protected string CreateAuthorization(string scheme = null, string value = null)
        {
            return $"{scheme ?? "Basic"} {Convert.ToBase64String(Encoding.UTF8.GetBytes(value ?? CreateAuthorizationParameterForClientIdAndClientSecret()))}";
        }

        protected string CreateAuthorizationParameterForClientIdAndClientSecret(string clientId = null, string clientSecret = null)
        {
            return $"{clientId ?? CreateClientId()}:{clientSecret ?? CreateClientSecret()}";
        }

        protected string CreateClientId()
        {
            return CreateClientValue();

        }

        protected string CreateClientSecret()
        {
            return CreateClientValue();
        }

        private string CreateClientValue()
        {
            return Fixture.Create<string>().ComputeMd5Hash();
        }

        protected ClaimsPrincipal CreateClaimsPrincipal(bool hasClaimsIdentity = true, ClaimsIdentity claimsIdentity = null)
        {
            return hasClaimsIdentity
                ? new ClaimsPrincipal(claimsIdentity ?? CreateClaimsIdentity())
                : new ClaimsPrincipal();
        }

        protected ClaimsIdentity CreateClaimsIdentity(bool isAuthenticated = true)
        {
            return isAuthenticated
                ? new ClaimsIdentity(Array.Empty<Claim>(), Fixture.Create<string>())
                : new ClaimsIdentity(Array.Empty<Claim>());
        }

        protected IntranetValidationException CreateIntranetValidationException(string message = null, string validatingField = null)
        {
            return new IntranetValidationException(Fixture.Create<ErrorCode>(), message ?? Fixture.Create<string>())
            {
                ValidatingField = validatingField ?? Fixture.Create<string>()
            };
        }

        protected IntranetBusinessException CreateIntranetBusinessException(ErrorCode? errorCode = null, string message = null)
        {
            return new IntranetBusinessException(errorCode ?? Fixture.Create<ErrorCode>(), message ?? Fixture.Create<string>());
        }

        protected IntranetExceptionBase CreateIntranetExceptionBase()
        {
            return new IntranetSystemException(Fixture.Create<ErrorCode>(), Fixture.Create<string>());
        }

        protected Exception CreateException()
        {
            return new Exception(Fixture.Create<string>());
        }
    }
}