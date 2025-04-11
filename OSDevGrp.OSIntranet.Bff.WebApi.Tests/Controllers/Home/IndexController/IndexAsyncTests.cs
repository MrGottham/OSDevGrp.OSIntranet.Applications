using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Index;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Home.Dtos;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.SecurityContextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Home.IndexController;

[TestFixture]
public class IndexAsyncTests
{
    #region Private variables

    private Mock<ISecurityContextProvider>? _securityContextProviderMock;
    private Mock<IQueryFeature<IndexRequest, IndexResponse>>? _queryFeatureMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _securityContextProviderMock = new Mock<ISecurityContextProvider>();
        _queryFeatureMock = new Mock<IQueryFeature<IndexRequest, IndexResponse>>();
        _fixture = new Fixture();
        _random = new Random();
    }

    [Test]
    [Category("UnitTest")]
    public async Task IndexAsync_WhenCalled_AssertGetCurrentSecurityContextAsyncWasCalledOnSecurityContextProviderWithGivenCancellationToken()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.IndexAsync(_queryFeatureMock!.Object, cancellationToken);

        _securityContextProviderMock!.Verify(m => m.GetCurrentSecurityContextAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task IndexAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithIndexRequestWhereRequestIdIsNotEqualToGuidEmpty()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.IndexAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IndexRequest>(value => value.RequestId != Guid.Empty),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task IndexAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithIndexRequestWhereExecutingAssemblyIsEqualToAssemblyForHomeController()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.IndexAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IndexRequest>(value => value.ExecutingAssembly == typeof(WebApi.Controllers.Home.HomeController).Assembly),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task IndexAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithIndexRequestWhereFormatProviderIsEqualToFormatProviderFromDependencies()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        WebApi.Controllers.Home.HomeController sut = CreateSut(formatProvider: formatProvider);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.IndexAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IndexRequest>(value => value.FormatProvider == formatProvider),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task IndexAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithIndexRequestWhereSecurityContextIsEqualToSecurityResolvedBySecurityContextProvider()
    {
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(_random!);
        WebApi.Controllers.Home.HomeController sut = CreateSut(securityContext: securityContext);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        await sut.IndexAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.Is<IndexRequest>(value => value.SecurityContext == securityContext),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task IndexAsync_WhenCalled_AssertExecuteAsyncWasCalledOnQueryFeatureWithGivenCancellationToken()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.IndexAsync(_queryFeatureMock!.Object, cancellationToken);

        _queryFeatureMock!.Verify(m => m.ExecuteAsync(
                It.IsAny<IndexRequest>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task IndexAsync_WhenCalled_ReturnsOkObjectResult()
    {
        WebApi.Controllers.Home.HomeController sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        IActionResult result = await sut.IndexAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        Assert.That(result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true)]
    [TestCase(true, true, true, false)]
    [TestCase(true, true, false, false)]
    [TestCase(true, false, true, true)]
    [TestCase(true, false, true, false)]
    [TestCase(true, false, false, false)]
    [TestCase(false, false, false, false)]
    public async Task IndexAsync_WhenCalled_ReturnsOkObjectResultWhereValueIsIndexResponseDto(bool hasUserInfo, bool hasName, bool hasAccountingAccess, bool hasDefaultAccountingNumber)
    {
        IUserInfoModel userInfo = CreateUserInfoModel(hasName: hasName, hasAccountingAccess: hasAccountingAccess);
        IndexResponse indexResponse = CreateIndexResponse(hasUserInfo: hasUserInfo, userInfo: userInfo);
        WebApi.Controllers.Home.HomeController sut = CreateSut(indexResponse: indexResponse);

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        OkObjectResult result = (OkObjectResult) await sut.IndexAsync(_queryFeatureMock!.Object, cancellationTokenSource.Token);

        Assert.That(result.Value, Is.TypeOf<IndexResponseDto>());
    }

    private WebApi.Controllers.Home.HomeController CreateSut(IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null, IndexResponse? indexResponse = null)
    {
        _securityContextProviderMock!.Setup(_fixture!, _random!, securityContext: securityContext);

        _queryFeatureMock!.Setup(m => m.ExecuteAsync(It.IsAny<IndexRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(indexResponse ?? CreateIndexResponse()));

        return new WebApi.Controllers.Home.HomeController(formatProvider ?? CultureInfo.InvariantCulture, _securityContextProviderMock!.Object);
    }

    private IndexResponse CreateIndexResponse(bool hasUserInfo = true, IUserInfoModel? userInfo = null)
    {
        StaticTextKey titleSelector = StaticTextKey.OSDevelopmentGroup;
        Dictionary<StaticTextKey, string> staticTexts = new Dictionary<StaticTextKey, string>
        {
            { titleSelector, _fixture!.Create<string>() },
            { StaticTextKey.Login, _fixture.Create<string>() },
            { StaticTextKey.Logout, _fixture.Create<string>() }
        };

        return new IndexResponse(titleSelector, hasUserInfo ? userInfo ?? CreateUserInfoModel() : null, staticTexts);
    }

    private IUserInfoModel CreateUserInfoModel(bool hasName = true, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true)
    {
        return CreateUserInfoModelMock(hasName, hasAccountingAccess, hasDefaultAccountingNumber).Object;
    }

    private Mock<IUserInfoModel> CreateUserInfoModelMock(bool hasName = true, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true)
    {
        Mock<IUserInfoModel> userInfoModelMock = new Mock<IUserInfoModel>();
        userInfoModelMock.Setup(m => m.Name)
            .Returns(hasName ? _fixture!.Create<string>() : null);
        userInfoModelMock.Setup(m => m.HasAccountingAccess)
            .Returns(hasAccountingAccess);
        userInfoModelMock.Setup(m => m.DefaultAccountingNumber)
            .Returns(hasAccountingAccess ? hasDefaultAccountingNumber ? _fixture!.Create<int>() : null : null);
        userInfoModelMock.Setup(m => m.Accountings)
            .Returns(hasAccountingAccess ? _fixture!.CreateMany<int>().ToDictionary(value => value, _ => _fixture!.Create<string>()) : new Dictionary<int, string>());
        return userInfoModelMock;
    }
}