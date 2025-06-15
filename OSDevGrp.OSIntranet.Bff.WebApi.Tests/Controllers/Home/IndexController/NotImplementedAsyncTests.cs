using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.NotImplemented;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Home.IndexController;

[TestFixture]
public class NotImplementedAsyncTests
{
    #region Private variables

    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<NotImplementedRequest, NotImplementedResponse>>? _queryFeatureMock;
    private Mock<TimeProvider>? _timeProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<NotImplementedRequest, NotImplementedResponse>>();
        _timeProviderMock = new Mock<TimeProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task NotImplementedAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.NotImplementedAsync(_queryFeatureMock!.Object, cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task NotImplementedAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithNotImplementedRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.NotImplementedAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<NotImplementedRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task NotImplementedAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithNotImplementedRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Home.HomeController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.NotImplementedAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<NotImplementedRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task NotImplementedAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithNotImplementedRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Home.HomeController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.NotImplementedAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<NotImplementedRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task NotImplementedAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.NotImplementedAsync(_queryFeatureMock!.Object, cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<NotImplementedRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task NotImplementedAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.NotImplementedAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task NotImplementedAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsNotImplementedResponseDto()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult) await sut.NotImplementedAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        Assert.That(result.Value, Is.TypeOf<NotImplementedResponseDto>());
    }

    private WebApi.Controllers.Home.HomeController CreateSut(IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, NotImplementedResponse? errorResponse = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<NotImplementedRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(errorResponse ?? CreateNotImplementedResponse()));

        return new WebApi.Controllers.Home.HomeController(formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private NotImplementedResponse CreateNotImplementedResponse()
    {
        return new NotImplementedResponse(_fixture!.CreateStaticTexts(_random!));
    }
}