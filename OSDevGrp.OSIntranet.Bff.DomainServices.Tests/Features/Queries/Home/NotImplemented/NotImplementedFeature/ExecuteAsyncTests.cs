using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.NotImplemented;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Home.NotImplemented.NotImplementedFeature;

[TestFixture]
public class ExecuteAsyncTests : HomePageFeatureTestBase
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
    [TestCase(StaticTextKey.FunctionalityNotImplmented)]
    [TestCase(StaticTextKey.FunctionalityNotImplmentedDetails)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsAuthenticated_ReturnsNotImplementedResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<NotImplementedRequest, NotImplementedResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(_fixture!, withAuthenticatedUser: true);
        NotImplementedRequest notImplementedRequest = CreateNotImplementedRequest(securityContext: securityContext);
        NotImplementedResponse result = await sut.ExecuteAsync(notImplementedRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.FunctionalityNotImplmented)]
    [TestCase(StaticTextKey.FunctionalityNotImplmentedDetails)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsNotAuthenticated_ReturnsNotImplementedResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<NotImplementedRequest, NotImplementedResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(_fixture!, withAuthenticatedUser: false);
        NotImplementedRequest notImplementedRequest = CreateNotImplementedRequest(securityContext: securityContext);
        NotImplementedResponse result = await sut.ExecuteAsync(notImplementedRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private IQueryFeature<NotImplementedRequest, NotImplementedResponse> CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Home.NotImplemented.NotImplementedFeature(_staticTextProviderMock!.Object);
    }

    private NotImplementedRequest CreateNotImplementedRequest(ISecurityContext? securityContext = null)
    {
        return new NotImplementedRequest(Guid.NewGuid(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(_fixture!));
    }
}