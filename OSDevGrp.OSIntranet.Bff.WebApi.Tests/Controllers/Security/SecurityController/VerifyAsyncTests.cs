using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.Verification;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

[TestFixture]
public class VerifyAsyncTests : SecurityControllerTestBase<VerificationResponse>
{
    #region Private variables

    private Mock<IProblemDetailsFactory>? _problemDetailsFactoryMock;
    private Mock<ITrustedDomainResolver>? _trustedDomainResolverMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<VerificationRequest, VerificationResponse>>? _queryFeatureMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _problemDetailsFactoryMock = new Mock<IProblemDetailsFactory>();
        _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<VerificationRequest, VerificationResponse>>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        VerificationRequestDto verificationRequestDto = CreateVerificationRequestDto();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.VerifyAsync(_queryFeatureMock!.Object, verificationRequestDto, cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithVerificationRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        VerificationRequestDto verificationRequestDto = CreateVerificationRequestDto();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.VerifyAsync(_queryFeatureMock!.Object, verificationRequestDto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<VerificationRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithVerificationRequestWhereVerificationKeyIsEqualToVerificationKeyFromGivenVerificationRequestDto()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        string verificationKey = _fixture!.Create<string>();
        VerificationRequestDto verificationRequestDto = CreateVerificationRequestDto(verificationKey: verificationKey);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.VerifyAsync(_queryFeatureMock!.Object, verificationRequestDto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<VerificationRequest>(value => value.VerificationKey == verificationKey),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithVerificationRequestWhereVerificationCodeIsEqualToVerificationCodeFromGivenVerificationRequestDto()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        string verificationCode = _fixture!.Create<string>();
        VerificationRequestDto verificationRequestDto = CreateVerificationRequestDto(verificationCode: verificationCode);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.VerifyAsync(_queryFeatureMock!.Object, verificationRequestDto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<VerificationRequest>(value => value.VerificationCode == verificationCode),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithVerificationRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext();
        WebApi.Controllers.Security.SecurityController sut = CreateSut(securityContext: securityContext);

        VerificationRequestDto verificationRequestDto = CreateVerificationRequestDto();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.VerifyAsync(_queryFeatureMock!.Object, verificationRequestDto, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<VerificationRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        VerificationRequestDto verificationRequestDto = CreateVerificationRequestDto();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.VerifyAsync(_queryFeatureMock!.Object, verificationRequestDto, cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<VerificationRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        VerificationRequestDto verificationRequestDto = CreateVerificationRequestDto();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.VerifyAsync(_queryFeatureMock!.Object, verificationRequestDto, cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsVerificationResponseDto(bool verified)
    {
        VerificationResponse verificationResponse = CreateVerificationResponse(verified);
        WebApi.Controllers.Security.SecurityController sut = CreateSut(verificationResponse: verificationResponse);

        VerificationRequestDto verificationRequestDto = CreateVerificationRequestDto();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult)await sut.VerifyAsync(_queryFeatureMock!.Object, verificationRequestDto, cancellationTokenSource.Token);

        Assert.That(result.Value, Is.TypeOf<VerificationResponseDto>());
    }

    protected override WebApi.Controllers.Security.SecurityController CreateSut(HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, VerificationResponse? verificationResponse = null)
    {
        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<VerificationRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(verificationResponse ?? CreateVerificationResponse()));

        return CreateSut(_problemDetailsFactoryMock!, _trustedDomainResolverMock!, _securityContextProviderMock!, _fixture!, httpContext, problemDetails, isTrustedDomain, formatProvider, securityContext);
    }

    private VerificationRequestDto CreateVerificationRequestDto(string? verificationKey = null, string? verificationCode = null)
    {
        return new VerificationRequestDto
        {
            VerificationKey = verificationKey ?? _fixture!.Create<string>(),
            VerificationCode = verificationCode ?? _fixture.Create<string>()
        };
    }

    private VerificationResponse CreateVerificationResponse(bool verified = true)
    {
        return new VerificationResponse(verified);
    }
}