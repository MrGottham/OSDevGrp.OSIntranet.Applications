using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers
{
    internal static class BadRequestObjectResultInspector
    {
        #region Methods

        internal static void AssertExpectedErrorResponseModel(this BadRequestObjectResult badRequestObjectResult, string expectedError, string expectedErrorDescription, Uri expectedErrorUri, string expectedState)
        {
            NullGuard.NotNullOrWhiteSpace(expectedError, nameof(expectedError));

            Assert.That(badRequestObjectResult, Is.Not.Null);
            Assert.That(badRequestObjectResult.Value, Is.Not.Null);
            Assert.That(badRequestObjectResult.Value, Is.TypeOf<ErrorResponseModel>());

            ((ErrorResponseModel) badRequestObjectResult.Value).AssertExpectedValues(expectedError, expectedErrorDescription, expectedErrorUri, expectedState);
        }

        #endregion
    }
}