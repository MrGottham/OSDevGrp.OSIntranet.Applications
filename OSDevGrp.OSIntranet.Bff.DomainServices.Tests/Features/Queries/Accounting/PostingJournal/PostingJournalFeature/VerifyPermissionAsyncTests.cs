using System.Security.Claims;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.PostingJournalTextsBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.PostingJournalRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using System.Globalization;
using PostingJournalFeatureType = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal.PostingJournalFeature;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.PostingJournal.PostingJournalFeature;

[TestFixture]
public class VerifyPermissionAsyncTests
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IPostingJournalTextsBuilder>? _postingJournalTextsBuilderMock;
    private Mock<IPostingJournalRuleSetBuilder>? _postingJournalRuleSetBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _postingJournalTextsBuilderMock = new Mock<IPostingJournalTextsBuilder>();
        _postingJournalRuleSetBuilderMock = new Mock<IPostingJournalRuleSetBuilder>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true)]
    [TestCase(true, true, false)]
    [TestCase(true, false, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, true)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    [TestCase(false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertUserWasCalledOnGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingModifier: isAccountingModifier);

        Mock<ISecurityContext> securityContextMock = _fixture!.CreateSecurityContextMock();
        await sut.VerifyPermissionAsync(securityContextMock.Object, CreatePostingJournalRequest(_fixture!));

        securityContextMock.Verify(m => m.User, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true)]
    [TestCase(true, true, false)]
    [TestCase(true, false, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, true)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    [TestCase(false, false, false)]
    public async Task VerifyPermissionAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool isAuthenticated, bool hasAccountingAccess, bool isAccountingModifier)
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingModifier: isAccountingModifier);

        ClaimsPrincipal user = isAuthenticated ? _fixture!.CreateAuthenticatedClaimsPrincipal() : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreatePostingJournalRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticated_AssertHasAccountingAccessWasCalledOnPermissionCheckerWithUserFromGivenSecurityContext(bool hasAccountingAccess, bool isAccountingModifier)
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(hasAccountingAccess: hasAccountingAccess, isAccountingModifier: isAccountingModifier);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreatePostingJournalRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.Is<ClaimsPrincipal>(value => value == user)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccess_AssertIsAccountingModifierWasCalledOnPermissionCheckerWithUserFromGivenSecurityContextAndAccountingNumberFromGivenPostingJournalRequest(bool isAccountingModifier)
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(isAccountingModifier: isAccountingModifier);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        int accountingNumber = _fixture!.Create<int>();
        await sut.VerifyPermissionAsync(securityContext, CreatePostingJournalRequest(_fixture!, accountingNumber: accountingNumber));

        _permissionCheckerMock!.Verify(m => m.IsAccountingModifier(
                It.Is<ClaimsPrincipal>(value => value == user),
                It.Is<int?>(value => value == accountingNumber)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_AssertIsAccountingModifierWasNotCalledOnPermissionChecker(bool isAccountingModifier)
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(hasAccountingAccess: false, isAccountingModifier: isAccountingModifier);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        await sut.VerifyPermissionAsync(securityContext, CreatePostingJournalRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.IsAccountingModifier(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_AssertHasAccountingAccessWasNotCalledOnPermissionChecker(bool hasAccountingAccess, bool isAccountingModifier)
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(isAuthenticated: false, hasAccountingAccess: hasAccountingAccess, isAccountingModifier: isAccountingModifier);

        await sut.VerifyPermissionAsync(_fixture!.CreateSecurityContext(), CreatePostingJournalRequest(_fixture!));

        _permissionCheckerMock!.Verify(m => m.HasAccountingAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenUserIsNotAuthenticated_ReturnsFalse()
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(isAuthenticated: false);

        bool result = await sut.VerifyPermissionAsync(_fixture!.CreateSecurityContext(), CreatePostingJournalRequest(_fixture!));

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedButDoesNotHaveAccountingAccess_ReturnsFalse()
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: false);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        bool result = await sut.VerifyPermissionAsync(securityContext, CreatePostingJournalRequest(_fixture!));

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccessButIsNotAccountingModifier_ReturnsFalse()
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: true, isAccountingModifier: false);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        bool result = await sut.VerifyPermissionAsync(securityContext, CreatePostingJournalRequest(_fixture!));

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task VerifyPermissionAsync_WhenUserIsAuthenticatedAndHasAccountingAccessAndIsAccountingModifier_ReturnsTrue()
    {
        IPermissionVerifiable<PostingJournalRequest> sut = CreateSut(isAuthenticated: true, hasAccountingAccess: true, isAccountingModifier: true);

        ClaimsPrincipal user = _fixture!.CreateAuthenticatedClaimsPrincipal();
        ISecurityContext securityContext = _fixture!.CreateSecurityContext(user: user);
        bool result = await sut.VerifyPermissionAsync(securityContext, CreatePostingJournalRequest(_fixture!));

        Assert.That(result, Is.True);
    }

    private IPermissionVerifiable<PostingJournalRequest> CreateSut(bool isAuthenticated = true, bool? hasAccountingAccess = null, bool? isAccountingModifier = null)
    {
        _permissionCheckerMock!.Setup(_fixture!, isAuthenticated: isAuthenticated, hasAccountingAccess: hasAccountingAccess, isAccountingModifier: isAccountingModifier);
        _staticTextProviderMock!.Setup(_fixture!);
        _postingJournalTextsBuilderMock!.Setup();
        _postingJournalRuleSetBuilderMock!.Setup(_fixture!);

        return new PostingJournalFeatureType(_permissionCheckerMock!.Object, _accountingGatewayMock!.Object, _staticTextProviderMock!.Object, _postingJournalTextsBuilderMock!.Object, _postingJournalRuleSetBuilderMock!.Object);
    }

    private static PostingJournalRequest CreatePostingJournalRequest(Fixture fixture, int? accountingNumber = null, DateTimeOffset? statusDate = null, ISecurityContext? securityContext = null)
    {
        return new PostingJournalRequest(Guid.NewGuid(), accountingNumber ?? fixture.Create<int>(), statusDate ?? DateTimeOffset.Now.Date, CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(fixture));
    }

    private static ISecurityContext CreateSecurityContext(Fixture fixture)
    {
        return fixture.CreateSecurityContext();
    }
}