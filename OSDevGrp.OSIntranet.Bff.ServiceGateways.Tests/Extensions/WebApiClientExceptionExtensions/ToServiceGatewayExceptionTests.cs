using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Extensions;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Net;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.Tests.Extensions.WebApiClientExceptionExtensions;

[TestFixture]
[Category("UnitTest")]
public class ToServiceGatewayExceptionTests : ServiceGatewayTestBase
{
    #region Private constants

    private const string UnauthorizedText = "You are not authorized to perform the requested operation.";
    private const string ServerErrorText = "An external server error has occurred.";

    #endregion

    #region Prviate variables

    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestException()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayBadRequestException>());
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereMessageIsEqualToMessageFromWebApiClientException()
    {
        string message = _fixture.Create<string>();
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, message: message);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Does.StartWith(message));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereInnerExceptionIsNotNull()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereInnerExceptionIsEqualToWebApiClientException()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualToUnauthorized_ReturnsServiceGatewayUnauthorizedException()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayUnauthorizedException>());
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereMessageIsEqualToSpecificUnauthorizedText()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Does.StartWith(UnauthorizedText));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereInnerExceptionIsNotNull()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereInnerExceptionIsEqualToWebApiClientException()
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorException(HttpStatusCode statusCode)
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayServerErrorException>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereMessageIsEqualToSpecificServerErrorText(HttpStatusCode statusCode)
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Does.StartWith(ServerErrorText));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereInnerExceptionIsNotNull(HttpStatusCode statusCode)
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereInnerExceptionIsEqualToWebApiClientException(HttpStatusCode statusCode)
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeIsNotSupported_ThrowsNotSupportedException(HttpStatusCode statusCode)
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode);

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWithSpecificMessage(HttpStatusCode statusCode)
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode);

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.Message, Is.EqualTo($"Unsupported status code: {(int) statusCode}"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull(HttpStatusCode statusCode)
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode);

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithNonGenericWebApiClientExceptionWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWhereInnerExceptionIsEqualToWebApiClientException(HttpStatusCode statusCode)
    {
        WebApiClientException webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode);

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestException()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayBadRequestException>());
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereMessageIsEqualToErrorMessageFromErrorModel()
    {
        string errorMessage = _fixture.Create<string>();
        ErrorModel errorModel = _fixture!.CreateErrorModel(errorMessage: errorMessage);
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, result: errorModel);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Is.EqualTo(errorMessage));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereInnerExceptionIsNotNull()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereInnerExceptionIsEqualToWebApiClientException()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEquaToUnauthorized_ReturnsServiceGatewayUnauthorizedException()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayUnauthorizedException>());
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEquaToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereMessageIsEqualToSpecificUnauthorizedText()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Does.StartWith(UnauthorizedText));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEquaToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereInnerExceptionIsNotNull()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEquaToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereInnerExceptionIsEqualToWebApiClientException()
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorException(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayServerErrorException>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereMessageIsEqualToSpecificServerErrorText(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Does.StartWith(ServerErrorText));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereInnerExceptionIsNotNull(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereInnerExceptionIsEqualToWebApiClientException(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeIsNotSupported_ThrowsNotSupportedException(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorModel());

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWithSpecificMessage(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorModel());

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.Message, Is.EqualTo($"Unsupported status code: {(int) statusCode}"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorModel());

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorModelWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWhereInnerExceptionIsEqualToWebApiClientException(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorModel());

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestException()
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayBadRequestException>());
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereMessageIsEqualToCombinationOfErrorAndErrorDescriptionFromErrorResponseModel()
    {
        string error = _fixture.Create<string>();
        string errorDescription = _fixture.Create<string>();
        ErrorResponseModel errorResponseModel = _fixture!.CreateErrorResponseModel(error: error, errorDescription: errorDescription);
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, result: errorResponseModel);

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Is.EqualTo($"{error}{Environment.NewLine}{errorDescription}"));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereInnerExceptionIsNotNull()
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualToBadRequest_ReturnsServiceGatewayBadRequestExceptionWhereInnerExceptionIsEqualToWebApiClientException()
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.BadRequest, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualToUnauthorized_ReturnsServiceGatewayUnauthorizedException()
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayUnauthorizedException>());
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereMessageIsEqualToSpecificUnauthorizedText()
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Does.StartWith(UnauthorizedText));
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereInnerExceptionIsNotNull()
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualToUnauthorized_ReturnsServiceGatewayUnauthorizedExceptionWhereInnerExceptionIsEqualToWebApiClientException()
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) HttpStatusCode.Unauthorized, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorException(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result, Is.TypeOf<ServiceGatewayServerErrorException>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereMessageIsEqualToSpecificServerErrorText(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.Message, Does.StartWith(ServerErrorText));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereInnerExceptionIsNotNull(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.InternalServerError)]
    [TestCase(HttpStatusCode.NotImplemented)]
    [TestCase(HttpStatusCode.BadGateway)]
    [TestCase(HttpStatusCode.ServiceUnavailable)]
    [TestCase(HttpStatusCode.GatewayTimeout)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeEqualOrGreatherThanEqualOrGreaterThan500_ReturnsServiceGatewayServerErrorExceptionWhereInnerExceptionIsEqualToWebApiClientException(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorResponseModel());

        ServiceGatewayExceptionBase result = webApiClientException.ToServiceGatewayException(); 

        Assert.That(result.InnerException, Is.EqualTo(webApiClientException));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeIsNotSupported_ThrowsNotSupportedException(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorResponseModel());

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWithSpecificMessage(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorResponseModel());

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.Message, Is.EqualTo($"Unsupported status code: {(int) statusCode}"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWhereInnerExceptionIsNotNull(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorResponseModel());

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.InnerException, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(HttpStatusCode.PaymentRequired)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.MethodNotAllowed)]
    [TestCase(HttpStatusCode.NotAcceptable)]
    public void ToServiceGatewayException_WithGenericWebApiClientExceptionWithErrorResponseModelWhereStatusCodeIsNotSupported_ThrowsNotSupportedExceptionWhereInnerExceptionIsEqualToWebApiClientException(HttpStatusCode statusCode)
    {
        WebApiClientException<ErrorResponseModel> webApiClientException = _fixture!.CreateWebApiClientException(statusCode: (int) statusCode, result: _fixture!.CreateErrorResponseModel());

        NotSupportedException? result = Assert.Throws<NotSupportedException>(() => webApiClientException.ToServiceGatewayException());

        Assert.That(result!.InnerException, Is.EqualTo(webApiClientException));
    }
}