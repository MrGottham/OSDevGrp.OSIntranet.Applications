using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.Security.GenerateVerification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

[TestFixture]
public class GenerateVerificationAsyncTests
{
    #region Private variables

    private Mock<IProblemDetailsFactory>? _problemDetailsFactoryMock;
    private Mock<ITrustedDomainResolver>? _trustedDomainResolverMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<ICommandFeature<GenerateVerificationRequest>>? _commandFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _problemDetailsFactoryMock = new Mock<IProblemDetailsFactory>();
        _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _commandFeatureMock = new Mock<ICommandFeature<GenerateVerificationRequest>>();
        _fixture = new Fixture();
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateVerificationAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GenerateVerificationAsync(_commandFeatureMock!.Object, cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateVerificationAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithGenerateVerificationRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.GenerateVerificationAsync(_commandFeatureMock!.Object, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<GenerateVerificationRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateVerificationAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithGenerateVerificationRequestWhereOnVerificationCreatedIsNotNull()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.GenerateVerificationAsync(_commandFeatureMock!.Object, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<GenerateVerificationRequest>(value => value.OnVerificationCreated != null),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateVerificationAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithGenerateVerificationRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Security.SecurityController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.GenerateVerificationAsync(_commandFeatureMock!.Object, cancellationTokenSource.Token);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<GenerateVerificationRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateVerificationAsync_WhenCalled_AssertExecuteAsyncWasCalledOnCommandFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GenerateVerificationAsync(_commandFeatureMock!.Object, cancellationToken);

        _commandFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<GenerateVerificationRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateVerificationAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.GenerateVerificationAsync(_commandFeatureMock!.Object, cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateVerificationAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsGenerateVerificationResponseDto()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult) await sut.GenerateVerificationAsync(_commandFeatureMock!.Object, cancellationTokenSource.Token);

        Assert.That(result.Value, Is.TypeOf<GenerateVerificationResponseDto>());
    }

    private WebApi.Controllers.Security.SecurityController CreateSut(ISecurityContext? securityContext = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, securityContext: securityContext);

        _commandFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<GenerateVerificationRequest>(), It.IsAny<CancellationToken>()))
            .Callback<GenerateVerificationRequest, CancellationToken>((request, _) => request.OnVerificationCreated(_fixture!.Create<string>(), _fixture!.CreateMany<byte>(_random!.Next(1024, 4096)).ToArray(), DateTimeOffset.UtcNow.AddSeconds(_random!.Next(60, 120))));

        return new WebApi.Controllers.Security.SecurityController(_problemDetailsFactoryMock!.Object, _trustedDomainResolverMock!.Object, CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }
}