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
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;
using PostingJournalFeatureType = OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.PostingJournal.PostingJournalFeature;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.PostingJournal.PostingJournalFeature;

[TestFixture]
public class ExecuteAsyncTests
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IPostingJournalTextsBuilder>? _postingJournalTextsBuilderMock;
    private Mock<IPostingJournalRuleSetBuilder>? _postingJournalRuleSetBuilderMock;
    private Fixture? _fixture;
    private Random? _random;

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
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetPostingJournalAsyncWasCalledOnAccountingGatewayWithAccountingNumberFromPostingJournalRequest()
    {
        IQueryFeature<PostingJournalRequest, PostingJournalResponse> sut = CreateSut();

        int accountingNumber = _fixture!.Create<int>();
        PostingJournalRequest postingJournalRequest = CreatePostingJournalRequest(_fixture!, accountingNumber: accountingNumber);
        await sut.ExecuteAsync(postingJournalRequest);

        _accountingGatewayMock!.Verify(m => m.GetPostingJournalAsync(
                It.Is<int>(value => value == accountingNumber),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetPostingJournalAsyncWasCalledOnAccountingGatewayWithGivenCancellationToken()
    {
        IQueryFeature<PostingJournalRequest, PostingJournalResponse> sut = CreateSut();

        PostingJournalRequest postingJournalRequest = CreatePostingJournalRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(postingJournalRequest, cancellationToken);

        _accountingGatewayMock!.Verify(m => m.GetPostingJournalAsync(
                It.IsAny<int>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsPostingJournalResponseWherePostingJournalIsEqualToPostingJournalModelResolvedByAccountingGateway()
    {
        ApplyPostingJournalModel postingJournalModel = _fixture!.CreateApplyPostingJournalModel(_random!);
        IQueryFeature<PostingJournalRequest, PostingJournalResponse> sut = CreateSut(postingJournalModel: postingJournalModel);

        PostingJournalRequest postingJournalRequest = CreatePostingJournalRequest(_fixture!);
        PostingJournalResponse result = await sut.ExecuteAsync(postingJournalRequest);

        Assert.That(result.PostingJournal, Is.EqualTo(postingJournalModel));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsPostingJournalResponseWhereDynamicTextsIsEqualToDynamicTextsResolvedByPostingJournalTextsBuilder()
    {
        Mock<IPostingJournalTexts> postingJournalTextsMock = new Mock<IPostingJournalTexts>();
        IPostingJournalTexts postingJournalTexts = postingJournalTextsMock.Object;
        IQueryFeature<PostingJournalRequest, PostingJournalResponse> sut = CreateSut(postingJournalTexts: postingJournalTexts);

        PostingJournalRequest postingJournalRequest = CreatePostingJournalRequest(_fixture!);
        PostingJournalResponse result = await sut.ExecuteAsync(postingJournalRequest);

        Assert.That(result.DynamicTexts, Is.EqualTo(postingJournalTexts));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsPostingJournalResponseWhereValidationRuleSetIsEqualToValidationRuleSetResolvedByValidationRuleSetBuilder()
    {
        IReadOnlyCollection<IValidationRule> validationRuleSet =
        [
            _fixture!.CreateRequiredValueRule(),
            _fixture!.CreateMinLengthRule(),
            _fixture!.CreateMaxLengthRule()
        ];
        IQueryFeature<PostingJournalRequest, PostingJournalResponse> sut = CreateSut(validationRuleSet: validationRuleSet);

        PostingJournalRequest postingJournalRequest = CreatePostingJournalRequest(_fixture!);
        PostingJournalResponse result = await sut.ExecuteAsync(postingJournalRequest);

        Assert.That(result.ValidationRuleSet, Is.EqualTo(validationRuleSet));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.PostingJournal)]
    [TestCase(StaticTextKey.PostingDate)]
    [TestCase(StaticTextKey.PostingReference)]
    [TestCase(StaticTextKey.Account)]
    [TestCase(StaticTextKey.PostingText)]
    [TestCase(StaticTextKey.BudgetAccount)]
    [TestCase(StaticTextKey.Debit)]
    [TestCase(StaticTextKey.Credit)]
    [TestCase(StaticTextKey.ContactAccount)]
    [TestCase(StaticTextKey.AccountName)]
    [TestCase(StaticTextKey.Posted)]
    [TestCase(StaticTextKey.Available)]
    [TestCase(StaticTextKey.Balance)]
    [TestCase(StaticTextKey.PostingValue)]
    [TestCase(StaticTextKey.AddPostingJournalLine)]
    [TestCase(StaticTextKey.UpdatePostingJournalLine)]
    [TestCase(StaticTextKey.DeletePostingJournalLine)]
    [TestCase(StaticTextKey.PostingJournalLineDeletionQuestion)]
    [TestCase(StaticTextKey.Create)]
    [TestCase(StaticTextKey.Update)]
    [TestCase(StaticTextKey.Delete)]
    [TestCase(StaticTextKey.ConfirmDeletion)]
    [TestCase(StaticTextKey.DeleteVerificationInfo)]
    [TestCase(StaticTextKey.Reset)]
    [TestCase(StaticTextKey.Cancel)]
    [TestCase(StaticTextKey.Warning)]
    public async Task ExecuteAsync_WhenCalled_ReturnsPostingJournalResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<PostingJournalRequest, PostingJournalResponse> sut = CreateSut();

        PostingJournalRequest postingJournalRequest = CreatePostingJournalRequest(_fixture!);
        PostingJournalResponse result = await sut.ExecuteAsync(postingJournalRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private IQueryFeature<PostingJournalRequest, PostingJournalResponse> CreateSut(ApplyPostingJournalModel? postingJournalModel = null, IPostingJournalTexts? postingJournalTexts = null, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        _permissionCheckerMock!.Setup(_fixture!);
        _staticTextProviderMock!.Setup(_fixture!);
        _postingJournalTextsBuilderMock!.Setup(postingJournalTexts: postingJournalTexts);
        _postingJournalRuleSetBuilderMock!.Setup(_fixture!, validationRuleSet: validationRuleSet);

        _accountingGatewayMock!.Setup(m => m.GetPostingJournalAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Returns((int accountingNumber, CancellationToken _) => Task.FromResult(postingJournalModel ?? _fixture!.CreateApplyPostingJournalModel(_random!, accountingNumber: accountingNumber, applyPostingLines: [])));

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