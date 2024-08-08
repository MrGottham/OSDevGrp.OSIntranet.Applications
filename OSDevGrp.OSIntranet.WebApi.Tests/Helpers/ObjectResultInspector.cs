using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers
{
    internal static class ObjectResultInspector
    {
        #region Methods

        internal static void AssertExpectedStatusCode(this ObjectResult objectResult, int statusCode)
        {
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(statusCode));
        }

        internal static void AssertExpectedErrorResponseModel(this ObjectResult objectResult, string expectedError, string expectedErrorDescription, Uri expectedErrorUri, string expectedState)
        {
            NullGuard.NotNullOrWhiteSpace(expectedError, nameof(expectedError));

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.Value, Is.Not.Null);
            Assert.That(objectResult.Value, Is.TypeOf<ErrorResponseModel>());

            ((ErrorResponseModel) objectResult.Value).AssertExpectedValues(expectedError, expectedErrorDescription, expectedErrorUri, expectedState);
        }

        internal static void AssertExpectedAccessTokenModel(this ObjectResult objectResult, string expectedTokenType, string expectedAccessToken, DateTimeOffset expectedExpires)
        {
            NullGuard.NotNullOrWhiteSpace(expectedTokenType, nameof(expectedTokenType))
                .NotNullOrWhiteSpace(expectedAccessToken, nameof(expectedAccessToken));

            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.Value, Is.Not.Null);
            Assert.That(objectResult.Value, Is.TypeOf<AccessTokenModel>());

            ((AccessTokenModel) objectResult.Value).AssertExpectedValues(expectedTokenType, expectedAccessToken, expectedExpires);
        }

        #endregion
    }
}