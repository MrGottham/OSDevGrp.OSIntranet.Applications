using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers
{
    internal static class AccessTokenModelInspector
    {
        #region Methods

        internal static void AssertExpectedValues(this AccessTokenModel accessTokenModel, string expectedTokenType, string expectedAccessToken, DateTimeOffset expectedExpires)
        {
            NullGuard.NotNull(accessTokenModel, nameof(accessTokenModel))
                .NotNullOrWhiteSpace(expectedTokenType, nameof(expectedTokenType))
                .NotNullOrWhiteSpace(expectedAccessToken, nameof(expectedAccessToken));

            Assert.That(accessTokenModel.TokenType, Is.Not.Null);
            Assert.That(accessTokenModel.TokenType, Is.Not.Empty);
            Assert.That(accessTokenModel.TokenType, Is.EqualTo(expectedTokenType));
            Assert.That(accessTokenModel.AccessToken, Is.Not.Null);
            Assert.That(accessTokenModel.AccessToken, Is.Not.Empty);
            Assert.That(accessTokenModel.AccessToken, Is.EqualTo(expectedAccessToken));
            Assert.That(accessTokenModel.ExpiresIn, Is.GreaterThanOrEqualTo(1));
            Assert.That(accessTokenModel.ExpiresIn, Is.LessThanOrEqualTo(3600));
            Assert.That(DateTime.UtcNow.AddSeconds(accessTokenModel.ExpiresIn), Is.EqualTo(expectedExpires.UtcDateTime).Within(1).Seconds);
        }

        #endregion
    }
}