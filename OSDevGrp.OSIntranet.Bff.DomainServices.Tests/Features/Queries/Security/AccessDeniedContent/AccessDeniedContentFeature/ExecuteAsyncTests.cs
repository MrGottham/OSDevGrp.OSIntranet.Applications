using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Security.AccessDeniedContent.AccessDeniedContentFeature;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccessDenied)]
    [TestCase(StaticTextKey.MissingPermissionToPage)]
    [TestCase(StaticTextKey.CheckYourCredentials)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsAuthenticated_ReturnsIndexResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<AccessDeniedContentRequest, AccessDeniedContentResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: true);
        AccessDeniedContentRequest accessDeniedContentRequest = CreateAccessDeniedContentRequest(securityContext: securityContext);
        AccessDeniedContentResponse result = await sut.ExecuteAsync(accessDeniedContentRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccessDenied)]
    [TestCase(StaticTextKey.MissingPermissionToPage)]
    [TestCase(StaticTextKey.CheckYourCredentials)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsNotAuthenticated_ReturnsIndexResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<AccessDeniedContentRequest, AccessDeniedContentResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(withAuthenticatedUser: false);
        AccessDeniedContentRequest accessDeniedContentRequest = CreateAccessDeniedContentRequest(securityContext: securityContext);
        AccessDeniedContentResponse result = await sut.ExecuteAsync(accessDeniedContentRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private IQueryFeature<AccessDeniedContentRequest, AccessDeniedContentResponse> CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Security.AccessDeniedContent.AccessDeniedContentFeature( _staticTextProviderMock!.Object);
    }

    private AccessDeniedContentRequest CreateAccessDeniedContentRequest(ISecurityContext? securityContext = null)
    {
        return new AccessDeniedContentRequest(Guid.NewGuid(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext());
    }

    private ISecurityContext CreateSecurityContext(bool withAuthenticatedUser = true)
    {
        return CreateSecurityContextMock(withAuthenticatedUser).Object;
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