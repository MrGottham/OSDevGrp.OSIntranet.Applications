using System.Globalization;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountIdentificationFeatureBase;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.AccountingRuleSetBuilder;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountSummary;

[TestFixture]
public class ExecuteAsyncTests : AccountIdentificationFeatureTestBase
{
    #region Private variables

    private Mock<IPermissionChecker>? _permissionCheckerMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Mock<IDynamicTextsBuilder<object, IDynamicTexts>>? _dynamicTextsBuilderMock;
    private Mock<IValidationRuleSetBuilder>? _validationRuleSetBuilderMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _permissionCheckerMock = new Mock<IPermissionChecker>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _dynamicTextsBuilderMock = new Mock<IDynamicTextsBuilder<object, IDynamicTexts>>();
        _validationRuleSetBuilderMock = new Mock<IValidationRuleSetBuilder>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetModelAsyncWasCalledOnAccountIdentificationFeatureBaseWithGivenAccountIdentificationRequest()
    {
        MyAccountIdentificationRequest? getModelAsyncWasCalledWith = null;
        Func<MyAccountIdentificationRequest, CancellationToken, Task<object>> modelGetter = (req, _) =>
        {
            getModelAsyncWasCalledWith = req;
            return Task.FromResult(new object());
        };
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, modelGetter: modelGetter);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(getModelAsyncWasCalledWith, Is.EqualTo(request));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetModelAsyncWasCalledOnAccountIdentificationFeatureBaseWithGivenCancellationToken()
    {
        CancellationToken? getModelAsyncWasCalledWith = null;
        Func<MyAccountIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, ct) =>
        {
            getModelAsyncWasCalledWith = ct;
            return Task.FromResult(new object());
        };
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, modelGetter: modelGetter);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        Assert.That(getModelAsyncWasCalledWith, Is.EqualTo(cancellationToken));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetStaticTextSpecificationsWasCalledOnAccountIdentificationFeatureBaseWithGivenAccountIdentificationRequest()
    {
        MyAccountIdentificationRequest? getStaticTextSpecificationsWasCalledWith = null;
        Func<MyAccountIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (req, _) =>
        {
            getStaticTextSpecificationsWasCalledWith = req;
            return new Dictionary<StaticTextKey, IEnumerable<object>>();
        };
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(getStaticTextSpecificationsWasCalledWith, Is.EqualTo(request));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetStaticTextSpecificationsWasCalledOnAccountIdentificationFeatureBaseWithModelReturnedByGetModelAsync()
    {
        object model = new object();
        Func<MyAccountIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, _) => Task.FromResult(model);
        object? getStaticTextSpecificationsWasCalledWith = null;
        Func<MyAccountIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, m) =>
        {
            getStaticTextSpecificationsWasCalledWith = m;
            return new Dictionary<StaticTextKey, IEnumerable<object>>();
        };
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, modelGetter: modelGetter, staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(getStaticTextSpecificationsWasCalledWith, Is.EqualTo(model));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderForEachStaticTextSpecificationReturnedByGetStaticTextSpecifications()
    {
        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Accountings, new object[] { 1, 2, 3 } },
            { StaticTextKey.Debtors, new object[] { 4, 5, 6 } },
            { StaticTextKey.Creditors, new object[] { 7, 8, 9 } }
        };
        Func<MyAccountIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, _) => staticTextSpecifications;
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        foreach (KeyValuePair<StaticTextKey, IEnumerable<object>> staticTextSpecification in staticTextSpecifications)
        {
            _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                    It.Is<StaticTextKey>(value => value == staticTextSpecification.Key),
                    It.Is<IEnumerable<object>>(value => value == staticTextSpecification.Value),
                    It.IsAny<IFormatProvider>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithFormatProviderFromAccountIdentificationRequest()
    {
        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Accountings, new object[] { 1 } }
        };
        Func<MyAccountIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, _) => staticTextSpecifications;
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == request.FormatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnDynamicTextsBuilderWithModelReturnedByGetModelAsync()
    {
        object model = new object();
        Func<MyAccountIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, _) => Task.FromResult(model);
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, modelGetter: modelGetter);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        _dynamicTextsBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<object>(value => value == model),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnDynamicTextsBuilderWithFormatProviderFromAccountIdentificationRequest()
    {
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        _dynamicTextsBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<object>(),
                It.Is<IFormatProvider>(value => value == request.FormatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnValidationRuleSetBuilderWithFormatProviderFromAccountIdentificationRequest()
    {
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        _validationRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == request.FormatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildResponseAsyncWasCalledOnAccountIdentificationFeatureBaseWithModelReturnedByGetModelAsync()
    {
        object model = new object();
        Func<MyAccountIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, _) => Task.FromResult(model);
        object? buildResponseAsyncWasCalledWithModel = null;
        Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountIdentificationResponse>> responseBuilder = (m, _, _, _, _) =>
        {
            buildResponseAsyncWasCalledWithModel = m;
            return Task.FromResult(new MyAccountIdentificationResponse(m, _dynamicTextsBuilderMock!.Object.BuildAsync(m, CultureInfo.InvariantCulture).Result, new Dictionary<StaticTextKey, string>(), new List<IValidationRule>()));
        };
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, modelGetter: modelGetter, responseBuilder: responseBuilder);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(buildResponseAsyncWasCalledWithModel, Is.EqualTo(model));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildResponseAsyncWasCalledOnAccountIdentificationFeatureBaseWithDynamicTextsReturnedByBuildAsyncOnDynamicTextsBuilder()
    {
        Mock<IDynamicTexts> dynamicTextsMock = new Mock<IDynamicTexts>();
        IDynamicTexts dynamicTexts = dynamicTextsMock.Object;
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, dynamicTexts: dynamicTexts);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        MyAccountIdentificationResponse result = await sut.ExecuteAsync(request);

        Assert.That(result.DynamicTexts, Is.EqualTo(dynamicTexts));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildResponseAsyncWasCalledOnAccountIdentificationFeatureBaseWithValidationRuleSetReturnedByBuildAsyncOnValidationRuleSetBuilder()
    {
        IReadOnlyCollection<IValidationRule> validationRuleSet = new[] { new Mock<IValidationRule>().Object }.AsReadOnly();
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, validationRuleSet: validationRuleSet);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        MyAccountIdentificationResponse result = await sut.ExecuteAsync(request);

        Assert.That(result.ValidationRuleSet, Is.EqualTo(validationRuleSet));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnNotNull()
    {
        IQueryFeature<MyAccountIdentificationRequest, MyAccountIdentificationResponse> sut = CreateSutAsQueryFeature(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!);

        MyAccountIdentificationRequest request = CreateAccountIdentificationRequest(_fixture!);
        MyAccountIdentificationResponse result = await sut.ExecuteAsync(request);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountNumberShort)]
    [TestCase(StaticTextKey.AccountName)]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountSummaryResponseWhereStaticTextsContainsExpectedStaticTextKey(StaticTextKey staticTextKey)
    {
        Mock<IPermissionChecker>? permissionCheckerMock = new Mock<IPermissionChecker>();
        Mock<IAccountingGateway>? accountingGatewayMock = new Mock<IAccountingGateway>();
        Mock<IStaticTextProvider>? staticTextProviderMock = new Mock<IStaticTextProvider>();
        Mock<IAccountTextsBuilder>? accountTextsBuilderMock = new Mock<IAccountTextsBuilder>();
        Mock<IEmptyRuleSetBuilder>? emptyRuleSetBuilderMock = new Mock<IEmptyRuleSetBuilder>();
        Fixture? fixture = new Fixture();
        Random? random = new Random(fixture!.Create<int>());

        permissionCheckerMock!.Setup(fixture!);
        staticTextProviderMock!.Setup(fixture!);
        accountTextsBuilderMock!.Setup(m => m.BuildAsync(It.IsAny<AccountModel>(), It.IsAny<IFormatProvider>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new Mock<IAccountTexts>().Object));
        emptyRuleSetBuilderMock!.Setup();
        accountingGatewayMock!.Setup(m => m.GetAccountAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(fixture!.CreateAccountModel(random!)));

        IQueryFeature<AccountSummaryRequest, AccountSummaryResponse> sut = new OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.Accounting.AccountSummary.AccountSummaryFeature(permissionCheckerMock!.Object, accountingGatewayMock!.Object, staticTextProviderMock!.Object, accountTextsBuilderMock!.Object, emptyRuleSetBuilderMock!.Object);

        AccountSummaryRequest accountSummaryRequest = CreateAccountSummaryRequest(fixture!);
        AccountSummaryResponse result = await sut.ExecuteAsync(accountSummaryRequest);

        Assert.That(result.StaticTexts.ContainsKey(staticTextKey), Is.True);
    }

    private static AccountSummaryRequest CreateAccountSummaryRequest(Fixture fixture, int? accountingNumber = null, string? accountNumber = null, DateTimeOffset? statusDate = null, IFormatProvider? formatProvider = null, ISecurityContext? securityContext = null)
    {
        return new AccountSummaryRequest(Guid.NewGuid(), accountingNumber ?? fixture.Create<int>(), accountNumber ?? fixture.Create<string>(), statusDate ?? fixture.Create<DateTimeOffset>(), formatProvider ?? System.Globalization.CultureInfo.InvariantCulture, securityContext ?? CreateSecurityContext(fixture));
    }
}