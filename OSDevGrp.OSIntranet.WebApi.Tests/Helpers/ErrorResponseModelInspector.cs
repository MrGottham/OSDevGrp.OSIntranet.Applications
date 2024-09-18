using NUnit.Framework;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.WebApi.Models.Security;
using System;

namespace OSDevGrp.OSIntranet.WebApi.Tests.Helpers
{
    internal static class ErrorResponseModelInspector
    {
        #region Methods

        internal static void AssertExpectedValues(this ErrorResponseModel errorResponseModel, string expectedError, string expectedErrorDescription, Uri expectedErrorUri, string expectedState)
        {
            NullGuard.NotNull(errorResponseModel, nameof(errorResponseModel))
                .NotNullOrWhiteSpace(expectedError, nameof(expectedError));

            Assert.That(errorResponseModel.Error, Is.Not.Null);
            Assert.That(errorResponseModel.Error, Is.Not.Empty);
            Assert.That(errorResponseModel.Error, Is.EqualTo(expectedError));
            if (string.IsNullOrWhiteSpace(expectedErrorDescription))
            {
                Assert.That(errorResponseModel.ErrorDescription, Is.Null);
            }
            else
            {
                Assert.That(errorResponseModel.ErrorDescription, Is.Not.Null);
                Assert.That(errorResponseModel.ErrorDescription, Is.Not.Empty);
                Assert.That(errorResponseModel.ErrorDescription, Is.EqualTo(expectedErrorDescription));
            }
            if (expectedErrorUri == null)
            {
                Assert.That(errorResponseModel.ErrorUri, Is.Null);
            }
            else
            {
                Assert.That(errorResponseModel.ErrorUri, Is.Not.Null);
                Assert.That(errorResponseModel.ErrorUri, Is.Not.Empty);
                Assert.That(errorResponseModel.ErrorUri, Is.EqualTo(expectedErrorUri.AbsoluteUri));
            }
            if (expectedState == null)
            {
                Assert.That(errorResponseModel.State, Is.Null);
            }
            else
            {
                Assert.That(errorResponseModel.State, Is.Not.Null);
                Assert.That(errorResponseModel.State, Is.EqualTo(expectedState));
            }
        }

        #endregion
    }
}