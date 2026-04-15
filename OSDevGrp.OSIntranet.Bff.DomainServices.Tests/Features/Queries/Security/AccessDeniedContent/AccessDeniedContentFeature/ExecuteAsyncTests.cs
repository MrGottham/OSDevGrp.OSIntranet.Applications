using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Security.AccessDeniedContent;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Security.AccessDeniedContent.AccessDeniedContentFeature;

[TestFixture]
public class ExecuteAsyncTests : SecurityPageFeatureTestBase
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

        ISecurityContext securityContext = CreateSecurityContext(_fixture!, withAuthenticatedUser: true);
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

        ISecurityContext securityContext = CreateSecurityContext(_fixture!, withAuthenticatedUser: false);
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
        return new AccessDeniedContentRequest(Guid.NewGuid(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(_fixture!));
    }
}