using System;
using System.Security.Claims;
using System.Text.Json;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Domain.Security;
using OSDevGrp.OSIntranet.WebApi.Tests.Helpers.Factories;
using Controller = OSDevGrp.OSIntranet.WebApi.Controllers.ContactController;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Controllers.ContactController
{
    public abstract class ContactControllerTestBase
    {
        #region Private variables

        private Mock<IQueryBus> _queryBusMock;
        private Fixture _fixture;

        #endregion

        #region Properties

        protected Mock<IQueryBus> QueryBusMock => _queryBusMock;

        protected Fixture Fixture => _fixture;

        #endregion

        [SetUp]
        public void SetUp()
        {
            _queryBusMock = new Mock<IQueryBus>();
            _fixture = new Fixture();
        }

        protected Controller CreateSut(ClaimsPrincipal user)
        {
            return new Controller(_queryBusMock.Object, ConverterFactoryCreator.Create())
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user
                    }
                }
            };
        }

        protected static ClaimsPrincipal CreatePrincipalWithoutMicrosoftToken()
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        protected static ClaimsPrincipal CreatePrincipalWithMicrosoftToken(string tokenValue)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimHelper.MicrosoftTokenClaimType, tokenValue) }));
        }

        protected static string BuildMicrosoftToken(string accessToken, DateTime expires)
        {
            byte[] tokenByteArray = JsonSerializer.SerializeToUtf8Bytes(new
            {
                TokenType = "Bearer",
                AccessToken = accessToken,
                RefreshToken = "refresh-token",
                Expires = expires
            });

            return Convert.ToBase64String(tokenByteArray);
        }

        protected static string BuildValidMicrosoftToken(string accessToken)
        {
            return BuildMicrosoftToken(accessToken, DateTime.UtcNow.AddHours(1));
        }

        protected static string BuildExpiredMicrosoftToken(string accessToken)
        {
            return BuildMicrosoftToken(accessToken, DateTime.UtcNow.AddHours(-1));
        }
    }
}