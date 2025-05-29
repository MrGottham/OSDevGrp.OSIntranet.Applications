using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Security.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.ErrorHandling;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Shared.Dtos;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Security.SecurityController;

[TestFixture]
public class UserInfoAsyncTests : SecurityControllerTestBase<UserInfoResponse>
{
    #region Private variables

    private Mock<IProblemDetailsFactory>? _problemDetailsFactoryMock;
    private Mock<ITrustedDomainResolver>? _trustedDomainResolverMock;
    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<UserInfoRequest, UserInfoResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _problemDetailsFactoryMock = new Mock<IProblemDetailsFactory>();
        _trustedDomainResolverMock = new Mock<ITrustedDomainResolver>();
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<UserInfoRequest, UserInfoResponse>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task UserInfoAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.UserInfoAsync(_queryFeatureMock!.Object, cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task UserInfoAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithUserInfoRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.UserInfoAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<UserInfoRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task UserInfoAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithUserInfoRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Security.SecurityController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.UserInfoAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<UserInfoRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task UserInfoAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithUserInfoRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(_random!);
        WebApi.Controllers.Security.SecurityController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.UserInfoAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<UserInfoRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task UserInfoAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.UserInfoAsync(_queryFeatureMock!.Object, cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<UserInfoRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task UserInfoAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Security.SecurityController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.UserInfoAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true, true)]
    [TestCase(true, true, true, true, false)]
    [TestCase(true, true, true, false, true)]
    [TestCase(true, true, true, false, false)]
    [TestCase(true, true, false, true, true)]
    [TestCase(true, true, false, true, false)]
    [TestCase(true, true, false, false, true)]
    [TestCase(true, true, false, false, false)]
    [TestCase(true, false, true, true, true)]
    [TestCase(true, false, true, true, false)]
    [TestCase(true, false, true, false, true)]
    [TestCase(true, false, true, false, false)]
    [TestCase(true, false, false, true, true)]
    [TestCase(true, false, false, true, false)]
    [TestCase(true, false, false, false, true)]
    [TestCase(true, false, false, false, false)]
    [TestCase(false, true, true, true, true)]
    [TestCase(false, true, true, true, false)]
    [TestCase(false, true, true, false, true)]
    [TestCase(false, true, true, false, false)]
    [TestCase(false, true, false, true, true)]
    [TestCase(false, true, false, true, false)]
    [TestCase(false, true, false, false, true)]
    [TestCase(false, true, false, false, false)]
    [TestCase(false, false, true, true, true)]
    [TestCase(false, false, true, true, false)]
    [TestCase(false, false, true, false, true)]
    [TestCase(false, false, true, false, false)]
    [TestCase(false, false, false, true, true)]
    [TestCase(false, false, false, true, false)]
    [TestCase(false, false, false, false, true)]
    [TestCase(false, false, false, false, false)]
    public async Task UserInfoAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsUserInfoResponseDto(bool hasNameIdentifier, bool hasName, bool hasMailAddress, bool hasAccountingAccess, bool hasCommonDataAccess)
    {
        IUserInfoModel userInfoModel = _fixture!.CreateUserInfoModel(_random!, hasNameIdentifier: hasNameIdentifier, hasName: hasName, hasMailAddress: hasMailAddress, hasAccountingAccess: hasAccountingAccess, hasCommonDataAccess: hasCommonDataAccess);
        UserInfoResponse userInfoResponse = CreateUserInfoResponse(userInfoModel: userInfoModel);
        WebApi.Controllers.Security.SecurityController sut = CreateSut(userInfoResponse: userInfoResponse);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult)await sut.UserInfoAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        Assert.That(result.Value, Is.TypeOf<UserInfoResponseDto>());
    }

    protected override WebApi.Controllers.Security.SecurityController CreateSut(HttpContext? httpContext = null, ProblemDetails? problemDetails = null, bool isTrustedDomain = true, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, UserInfoResponse? userInfoResponse = null)
    {
        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<UserInfoRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(userInfoResponse ?? CreateUserInfoResponse()));

        return CreateSut(_problemDetailsFactoryMock!, _trustedDomainResolverMock!, _securityContextProviderMock!, _fixture!, _random!, httpContext, problemDetails, isTrustedDomain, formatProvider, securityContext);
    }

    private UserInfoResponse CreateUserInfoResponse(IUserInfoModel? userInfoModel = null)
    {
        return new UserInfoResponse(userInfoModel ?? _fixture!.CreateUserInfoModel(_random!), _fixture!.CreateStaticTexts(_random!));
    }
}