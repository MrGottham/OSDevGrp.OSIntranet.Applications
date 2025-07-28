using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountingPreCreation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingPreCreation.AccountingPreCreationFeature;

[TestFixture]
public class ExecuteAsyncTests : AccountingPreCreationFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<ICommonGateway>? _commonGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IAccountingRuleSetBuilder>? _accountingRuleSetBuilderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _commonGatewayMock = new Mock<ICommonGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _accountingRuleSetBuilderMock = new Mock<IAccountingRuleSetBuilder>();
        _fixture = new Fixture();
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetLetterHeadsAsyncWasCalledOnCommonGatewayWithGivenCancellationToken()
    {
        IQueryFeature<AccountingPreCreationRequest, AccountingPreCreationResponse> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(CreateAccountingPreCreationRequestRequest(_fixture!), cancellationToken);

        _commonGatewayMock!.Verify(m => m.GetLetterHeadsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountingRuleSetBuilderWithFormatProviderFromRequestAccountingPreCreationRequest()
    {
        IQueryFeature<AccountingPreCreationRequest, AccountingPreCreationResponse> sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        AccountingPreCreationRequest accountingPreCreationRequest = CreateAccountingPreCreationRequestRequest(_fixture!, formatProvider: formatProvider);
        await sut.ExecuteAsync(accountingPreCreationRequest);

        _accountingRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnAccountingRuleSetBuilderWithGivenCancellationToken()
    {
        IQueryFeature<AccountingPreCreationRequest, AccountingPreCreationResponse> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(CreateAccountingPreCreationRequestRequest(_fixture!), cancellationToken);

        _accountingRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingPreCreationResponseWhereLetterHeadsContainsLetterHeadsResovledByCommonGateway()
    {
        LetterHeadModel[] letterHeadModels = _fixture!.CreateLetterHeadModels(_random!);
        IQueryFeature<AccountingPreCreationRequest, AccountingPreCreationResponse> sut = CreateSut(letterHeadModels: letterHeadModels);

        AccountingPreCreationResponse result = await sut.ExecuteAsync(CreateAccountingPreCreationRequestRequest(_fixture!));

        Assert.That(letterHeadModels.All(letterHeadModel => result.LetterHeads.Any(m => m.Number == letterHeadModel.Number && m.Name == letterHeadModel.Name)), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.CreateNewAccounting)]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    [TestCase(StaticTextKey.LetterHead)]
    [TestCase(StaticTextKey.BalanceBelowZero)]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    [TestCase(StaticTextKey.BackDating)]
    [TestCase(StaticTextKey.Create)]
    [TestCase(StaticTextKey.Reset)]
    [TestCase(StaticTextKey.Cancel)]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingPreCreationResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        IQueryFeature<AccountingPreCreationRequest, AccountingPreCreationResponse> sut = CreateSut();

        AccountingPreCreationResponse result = await sut.ExecuteAsync(CreateAccountingPreCreationRequestRequest(_fixture!));

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingPreCreationResponseWhereValidationRuleSetIsEqualToValidationRuleSetResolvedByAccountingRuleSetBuilder()
    {
        IReadOnlyCollection<IValidationRule> validationRuleSet =
        [
            _fixture!.CreateRequiredValueRule(),
            _fixture!.CreateMinLengthRule(),
            _fixture!.CreateMaxLengthRule()
        ];
        IQueryFeature<AccountingPreCreationRequest, AccountingPreCreationResponse> sut = CreateSut(validationRuleSet: validationRuleSet);

        AccountingPreCreationResponse result = await sut.ExecuteAsync(CreateAccountingPreCreationRequestRequest(_fixture!));

        Assert.That(result.ValidationRuleSet, Is.EqualTo(validationRuleSet));
    }

    private IQueryFeature<AccountingPreCreationRequest, AccountingPreCreationResponse> CreateSut(IEnumerable<LetterHeadModel>? letterHeadModels = null, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        _permissionCheckerMock!.Setup(_fixture!);
        _staticTextProviderMock!.Setup(_fixture!);
        _accountingRuleSetBuilderMock!.Setup(_fixture!, validationRuleSet: validationRuleSet);

        _commonGatewayMock!.Setup(m => m.GetLetterHeadsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(letterHeadModels ?? _fixture!.CreateLetterHeadModels(_random!)));

        return new DomainServices.Features.Queries.Accounting.AccountingPreCreation.AccountingPreCreationFeature(_permissionCheckerMock!.Object, _commonGatewayMock!.Object, _staticTextProviderMock!.Object, _accountingRuleSetBuilderMock!.Object);
    }
}