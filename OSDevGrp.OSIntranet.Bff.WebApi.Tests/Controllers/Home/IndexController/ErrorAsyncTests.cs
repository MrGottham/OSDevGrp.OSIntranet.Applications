using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Error;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Home.IndexController;

[TestFixture]
public class ErrorAsyncTests
{
    #region Private variables

    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<ErrorRequest, ErrorResponse>>? _queryFeatureMock;
    private Mock<TimeProvider>? _timeProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<ErrorRequest, ErrorResponse>>();
        _timeProviderMock = new Mock<TimeProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ErrorAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ErrorAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ErrorAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithErrorRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ErrorAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ErrorRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ErrorAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithErrorRequestWhereErrorMessageIsEqualToGivenErrorMessage()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        string errorMessage = _fixture!.Create<string>();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ErrorAsync(_queryFeatureMock!.Object, errorMessage, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ErrorRequest>(value => value.ErrorMessage == errorMessage),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ErrorAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithErrorRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Home.HomeController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ErrorAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ErrorRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ErrorAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithErrorRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Home.HomeController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.ErrorAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<ErrorRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ErrorAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ErrorAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<ErrorRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ErrorAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.ErrorAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ErrorAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsErrorResponseDto()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult) await sut.ErrorAsync(_queryFeatureMock!.Object, _fixture!.Create<string>(), cancellationTokenSource.Token);

        Assert.That(result.Value, Is.TypeOf<ErrorResponseDto>());
    }

    private WebApi.Controllers.Home.HomeController CreateSut(IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, DateTimeOffset? utcNow = null, ErrorResponse? errorResponse = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _timeProviderMock!.Setup(m => m.GetUtcNow())
            .Returns(utcNow ?? DateTimeOffset.UtcNow);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<ErrorRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(errorResponse ?? CreateErrorResponse()));

        return new WebApi.Controllers.Home.HomeController(formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private ErrorResponse CreateErrorResponse()
    {
        return new ErrorResponse(_fixture.Create<string>(), _fixture!.CreateStaticTexts(_random!));
    }
}