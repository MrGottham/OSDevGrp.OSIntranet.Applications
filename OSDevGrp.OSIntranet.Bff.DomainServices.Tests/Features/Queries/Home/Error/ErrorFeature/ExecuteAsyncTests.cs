using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Home.Error;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.BuildInfo.BuildInfoProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Home.Error.ErrorFeature;

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
    [TestCase(true)]
    [TestCase(false)]
    public async Task ExecuteAsync_WhenCalled_ReturnsErrorResponseWhereErrorMessageIsEqualToErrorMessageFromErrorRequest(bool withAuthenticatedUser)
    {
        IQueryFeature<ErrorRequest, ErrorResponse> sut = CreateSut();

        string errorMessage = _fixture!.Create<string>();
        ISecurityContext securityContext = CreateSecurityContext(_fixture!, withAuthenticatedUser: withAuthenticatedUser);
        ErrorRequest errorRequest = CreateErrorRequest(errorMessage: errorMessage, securityContext: securityContext);
        ErrorResponse result = await sut.ExecuteAsync(errorRequest);

        Assert.That(result.ErrorMessage, Is.EqualTo(errorMessage));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.SomethingWentWrong)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsAuthenticated_ReturnsErrorResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<ErrorRequest, ErrorResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(_fixture!, withAuthenticatedUser: true);
        ErrorRequest errorRequest = CreateErrorRequest(securityContext: securityContext);
        ErrorResponse result = await sut.ExecuteAsync(errorRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.SomethingWentWrong)]
    public async Task ExecuteAsync_WhenUserFromSecurityConextIsNotAuthenticated_ReturnsErrorResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<ErrorRequest, ErrorResponse> sut = CreateSut();

        ISecurityContext securityContext = CreateSecurityContext(_fixture!, withAuthenticatedUser: false);
        ErrorRequest errorRequest = CreateErrorRequest(securityContext: securityContext);
        ErrorResponse result = await sut.ExecuteAsync(errorRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private IQueryFeature<ErrorRequest, ErrorResponse> CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Features.Queries.Home.Error.ErrorFeature(_staticTextProviderMock!.Object);
    }

    private ErrorRequest CreateErrorRequest(string? errorMessage = null, ISecurityContext? securityContext = null)
    {
        return new ErrorRequest(Guid.NewGuid(), errorMessage ?? _fixture!.Create<string>(), CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(_fixture!));
    }
}