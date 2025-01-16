using AutoFixture;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Extensions;

internal static class FixtureExtensions
{
    #region Methods

    internal static WebApiClientException CreateWebApiClientException(this Fixture fixture, int statusCode, string? message = null)
    {
        return new WebApiClientException(message ?? fixture.Create<string>(), statusCode, null, new Dictionary<string, IEnumerable<string>>(), null);
    }

    internal static WebApiClientException<TResult> CreateWebApiClientException<TResult>(this Fixture fixture, int statusCode, TResult result, string? message = null)
    {
        return new WebApiClientException<TResult>(message ?? fixture.Create<string>(), statusCode, null, new Dictionary<string, IEnumerable<string>>(), result, null);
    }

    internal static ErrorModel CreateErrorModel(this Fixture fixture, string? errorMessage = null)
    {
        return new ErrorModel(fixture.Create<int>(), errorMessage ?? fixture.Create<string>(), fixture.Create<string>(), null, null, null);
    }

    internal static ErrorResponseModel CreateErrorResponseModel(this Fixture fixture, string? error = null, string? errorDescription = null)
    {
        return new ErrorResponseModel(error ?? fixture.Create<string>(), errorDescription ?? fixture.Create<string>(), null, null);
    }

    #endregion
}