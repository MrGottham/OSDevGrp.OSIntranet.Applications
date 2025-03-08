using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Index;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.BuildInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.BuildInfo.BuildInfoProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Models.Home.UserInfoModel;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Home.Index.IndexFeature;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IUserInfoProvider>? _userInfoProviderMock;
    private Mock<IBuildInfoProvider>? _buildInfoProviderMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _userInfoProviderMock = new Mock<IUserInfoProvider>();
        _buildInfoProviderMock = new Mock<IBuildInfoProvider>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertUserWasCalledOnSecurityContextAtIndexRequest(bool withAuthenticatedUser)
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        Mock<ISecurityContext> securityContextMock = CreateSecurityContextMock(withAuthenticatedUser: withAuthenticatedUser);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContextMock.Object);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertIsAuthenticatedWasCalledTwiceOnUserInfoProviderWithUserFromSecurityContextAtIndexRequest(bool withAuthenticatedUser)
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        ClaimsPrincipal user = withAuthenticatedUser ? CreateAuthenticatedUser() : CreateNonAuthenticated();
        ISecurityContext securityContext = CreateSecurityContext(user: user);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        _userInfoProviderMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Exactly(2));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertIsGetUserInfoAsyncWasCalledOnUserInfoProviderWithUserFromSecurityContextAtIndexRequest(bool withAuthenticatedUser)
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        ClaimsPrincipal user = withAuthenticatedUser ? CreateAuthenticatedUser() : CreateNonAuthenticated();
        ISecurityContext securityContext = CreateSecurityContext(user: user);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        _userInfoProviderMock!.Verify(m => m.GetUserInfoAsync(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertIsGetUserInfoAsyncWasCalledOnUserInfoProviderWithSameCancellationToken(bool withAuthenticatedUser)
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        IndexResponse result = await sut.ExecuteAsync(indexRequest, cancellationToken);

        _userInfoProviderMock!.Verify(m => m.GetUserInfoAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertGetBuildInfoWasCalledOnBuildInfoProviderWithExecutingAssemblyAtIndexRequest(bool withAuthenticatedUser)
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        IndexRequest indexRequest = CreateIndexRequest(executingAssembly: executingAssembly, securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        _buildInfoProviderMock!.Verify(m => m.GetBuildInfo(It.Is<Assembly>(value => value == executingAssembly)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_AssertBuildTimeWasCalledOnBuildInfoResolvedByBuildInfoProvider(bool withAuthenticatedUser)
    {
        Mock<IBuildInfo> buildInfoMock = _fixture!.CreateBuildInfoMock(_random!);
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut(buildInfo: buildInfoMock.Object);

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        buildInfoMock.Verify(m => m.BuildTime, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsAuthenticated_ReturnsIndexResponseWhereTitleSelectorIsEqualToOSDevelopmentGroup()
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: true);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        Assert.That(result.TitleSelector, Is.EqualTo(StaticTextKey.OSDevelopmentGroup));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenUserInfoWasReturnedFromUserInfoProvider_ReturnsIndexResponseWhereUserInfoIsEqualToUserInfoFromUserInfoProvider(bool withAuthenticatedUser)
    {
        IUserInfoModel userInfo = _fixture!.CreateUserInfoModel(_random!);
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut(hasUserInfo: true, userInfo: userInfo);

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        Assert.That(result.UserInfo, Is.EqualTo(userInfo));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenNoUserInfoWasReturnedFromUserInfoProvider_ReturnsIndexResponseWhereUserInfoIsNull(bool withAuthenticatedUser)
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut(hasUserInfo: false);

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: withAuthenticatedUser);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        Assert.That(result.UserInfo, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.OSDevelopmentGroup)]
    [TestCase(StaticTextKey.Copyright)]
    [TestCase(StaticTextKey.BuildInfo)]
    [TestCase(StaticTextKey.Start)]
    [TestCase(StaticTextKey.Login)]
    [TestCase(StaticTextKey.Logout)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsAuthenticated_ReturnsIndexResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: true);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsNotAuthenticated_ReturnsIndexResponseWhereTitleSelectorIsEqualToMrGotthamsHomepage()
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: false);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        Assert.That(result.TitleSelector, Is.EqualTo(StaticTextKey.MrGotthamsHomepage));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.MrGotthamsHomepage)]
    [TestCase(StaticTextKey.Copyright)]
    [TestCase(StaticTextKey.BuildInfo)]
    [TestCase(StaticTextKey.Start)]
    [TestCase(StaticTextKey.Login)]
    [TestCase(StaticTextKey.Logout)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsNotAuthenticated_ReturnsIndexResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<IndexRequest, IndexResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: false);
        IndexRequest indexRequest = CreateIndexRequest(securityContext: securityContext);
        IndexResponse result = await sut.ExecuteAsync(indexRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private IQueryFeature<IndexRequest, IndexResponse> CreateSut(bool hasUserInfo = true, IUserInfoModel? userInfo = null, IBuildInfo? buildInfo = null)
    {
        _userInfoProviderMock!.Setup(_fixture!, _random!, hasUserInfo: hasUserInfo, userInfoModel: userInfo);
        _buildInfoProviderMock!.Setup(_fixture!, _random!, buildInfo);
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Home.Index.IndexFeature(_userInfoProviderMock!.Object, _buildInfoProviderMock!.Object, _staticTextProviderMock!.Object);
    }

    private IndexRequest CreateIndexRequest(Assembly? executingAssembly = null, ISecurityContext? securityContext = null)
    {
        return new IndexRequest(Guid.NewGuid(), executingAssembly ?? Assembly.GetExecutingAssembly(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext());
    }

    private ISecurityContext CreateSecurityContext(bool withAuthenticatedUser = true)
    {
        return CreateSecurityContextMock(withAuthenticatedUser).Object;
    }

    private ISecurityContext CreateSecurityContext(ClaimsPrincipal user)
    {
        return CreateSecurityContextMock(user).Object;
    }

    private Mock<ISecurityContext> CreateSecurityContextMock(bool withAuthenticatedUser = true)
    {
        return CreateSecurityContextMock(user: withAuthenticatedUser ? CreateAuthenticatedUser() : CreateNonAuthenticated());
    }

    private Mock<ISecurityContext> CreateSecurityContextMock(ClaimsPrincipal user)
    {
        return _fixture!.CreateSecurityContextMock(user: user);
    }

    private ClaimsPrincipal CreateAuthenticatedUser()
    {
        return _fixture!.CreateAuthenticatedClaimsPrincipal();
    }

    private ClaimsPrincipal CreateNonAuthenticated()
    {
        return _fixture!.CreateNonAuthenticatedClaimsPrincipal();
    }
}