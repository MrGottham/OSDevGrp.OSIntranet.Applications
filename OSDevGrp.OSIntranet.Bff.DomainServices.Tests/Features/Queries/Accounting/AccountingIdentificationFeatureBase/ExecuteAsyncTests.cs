using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MaxLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.RequiredValueRuleFactory;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.Accounting.AccountingIdentificationFeatureBase;

[TestFixture]
public class ExecuteAsyncTests : AccountingIdentificationFeatureTestBase
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
    public async Task ExecuteAsync_WhenCalled_AssertGetModelAsyncWasCalledOnAccountingIdentificationFeatureBaseWithGivenAccountingIdentificationRequest()
    {
        MyAccountingIdentificationRequest? getModelAsyncWasCalledWith = null;
        Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>> modelGetter = (req, _) =>
        {
            getModelAsyncWasCalledWith = req;
            return Task.FromResult(new object());
        };
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(modelGetter: modelGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(getModelAsyncWasCalledWith, Is.EqualTo(request));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetModelAsyncWasCalledOnAccountingIdentificationFeatureBaseWithGivenCancellationToken()
    {
        CancellationToken? getModelAsyncWasCalledWith = null;
        Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, ct) =>
        {
            getModelAsyncWasCalledWith = ct;
            return Task.FromResult(new object());
        };
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(modelGetter: modelGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        Assert.That(getModelAsyncWasCalledWith, Is.EqualTo(cancellationToken));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetStaticTextSpecificationsWasCalledOnAccountingIdentificationFeatureBaseWithGivenAccountingIdentificationRequest()
    {
        MyAccountingIdentificationRequest? getStaticTextSpecificationsWasCalledWith = null;
        Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (req, _) =>
        {
            getStaticTextSpecificationsWasCalledWith = req;
            return new Dictionary<StaticTextKey, IEnumerable<object>>();
        };
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(getStaticTextSpecificationsWasCalledWith, Is.EqualTo(request));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertGetStaticTextSpecificationsWasCalledOnAccountingIdentificationFeatureBaseWithModelReturnedByGetModelAsync()
    {
        object model = new object();
        Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, _) => Task.FromResult(model);
        object? getStaticTextSpecificationsWasCalledWith = null;
        Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, m) =>
        {
            getStaticTextSpecificationsWasCalledWith = m;
            return new Dictionary<StaticTextKey, IEnumerable<object>>();
        };
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(modelGetter: modelGetter, staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(getStaticTextSpecificationsWasCalledWith, Is.EqualTo(model));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AsserGetStaticTextAsyncWasCalledOnStaticTextProviderForEachStaticTextSpecificationReturnedByGetStaticTextSpecifications()
    {
        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Accountings, new object[] { 1, 2, 3 } },
            { StaticTextKey.Debtors, new object[] { 4, 5, 6 } },
            { StaticTextKey.Creditors, new object[] { 7, 8, 9 } }
        };
        Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, _) => staticTextSpecifications;
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
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
    public async Task ExecuteAsync_WhenCalled_AsserGetStaticTextAsyncWasCalledOnStaticTextProviderWithFormatProviderFromAccountingIdentificationRequest()
    {
        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Accountings, new object[] { 1, 2, 3 } },
            { StaticTextKey.Debtors, new object[] { 4, 5, 6 } },
            { StaticTextKey.Creditors, new object[] { 7, 8, 9 } }
        };
        Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, _) => staticTextSpecifications;
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!, formatProvider: formatProvider);
        await sut.ExecuteAsync(request);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Exactly(staticTextSpecifications.Count));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AsserGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenCancellationToken()
    {
        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Accountings, new object[] { 1, 2, 3 } },
            { StaticTextKey.Debtors, new object[] { 4, 5, 6 } },
            { StaticTextKey.Creditors, new object[] { 7, 8, 9 } }
        };
        Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, _) => staticTextSpecifications;
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Exactly(staticTextSpecifications.Count));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnDynamicTextsBuilderWithModelReturnedByGetModelAsync()
    {
        object model = new object();
        Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, _) => Task.FromResult(model);
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(modelGetter: modelGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        _dynamicTextsBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<object>(value => value == model),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnDynamicTextsBuilderWithFormatProviderFromAccountingIdentificationRequest()
    {
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!, formatProvider: formatProvider);
        await sut.ExecuteAsync(request);

        _dynamicTextsBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<object>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnDynamicTextsBuilderWithGivenCancellationToken()
    {
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut();

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        _dynamicTextsBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<object>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnValidationRuleSetBuilderWithFormatProviderFromAccountingIdentificationRequest()
    {
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!, formatProvider: formatProvider);
        await sut.ExecuteAsync(request);

        _validationRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AssertBuildAsyncWasCalledOnValidationRuleSetBuilderWithGivenCancellationToken()
    {
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut();

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        _validationRuleSetBuilderMock!.Verify(m => m.BuildAsync(
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AsserBuildResponseAsyncWasCalledOnAccountingIdentificationFeatureBaseWithModelReturnedByGetModelAsync()
    {
        object model = new object();
        Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, _) => Task.FromResult(model);
        object? buildResponseAsyncWasCalledWith = null;
        Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountingIdentificationResponse>> responseBuilder = (m, st, dt, vrs, _) =>
        {
            buildResponseAsyncWasCalledWith = m;
            return Task.FromResult(new MyAccountingIdentificationResponse(m, dt, st, vrs));
        };
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(modelGetter: modelGetter, responseBuilder: responseBuilder);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(buildResponseAsyncWasCalledWith, Is.EqualTo(model));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AsserBuildResponseAsyncWasCalledOnAccountingIdentificationFeatureBaseWithStaticTextsReturnedByStaticTextProvider()
    {
        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Accountings, new object[] { 1, 2, 3 } },
            { StaticTextKey.Debtors, new object[] { 4, 5, 6 } },
            { StaticTextKey.Creditors, new object[] { 7, 8, 9 } }
        };
        Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, _) => staticTextSpecifications;
        IReadOnlyDictionary<StaticTextKey, string>? buildResponseAsyncWasCalledWith = null;
        Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountingIdentificationResponse>> responseBuilder = (m, st, dt, vrs, _) =>
        {
            buildResponseAsyncWasCalledWith = st;
            return Task.FromResult(new MyAccountingIdentificationResponse(m, dt, st, vrs));
        };
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(staticTextSpecificationsGetter: staticTextSpecificationsGetter, responseBuilder: responseBuilder);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        foreach (KeyValuePair<StaticTextKey, IEnumerable<object>> staticTextSpecification in staticTextSpecifications)
        {
            Assert.That(buildResponseAsyncWasCalledWith!.ContainsKey(staticTextSpecification.Key), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AsserBuildResponseAsyncWasCalledOnAccountingIdentificationFeatureBaseWithDynamicTextReturnedByDynamicTextBuilder()
    {
        IDynamicTexts? buildResponseAsyncWasCalledWith = null;
        Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountingIdentificationResponse>> responseBuilder = (m, st, dt, vrs, _) =>
        {
            buildResponseAsyncWasCalledWith = dt;
            return Task.FromResult(new MyAccountingIdentificationResponse(m, dt, st, vrs));
        };
        IDynamicTexts dynamicTexts = new Mock<IDynamicTexts>().Object;
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(responseBuilder: responseBuilder, dynamicTexts: dynamicTexts);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(buildResponseAsyncWasCalledWith!, Is.EqualTo(dynamicTexts));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_AsserBuildResponseAsyncWasCalledOnAccountingIdentificationFeatureBaseWithValidationRuleSetReturnedByValidationRuleSetValidationRuleSetBuilder()
    {
        IReadOnlyCollection<IValidationRule>? buildResponseAsyncWasCalledWith = null;
        Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountingIdentificationResponse>> responseBuilder = (m, st, dt, vrs, _) =>
        {
            buildResponseAsyncWasCalledWith = vrs;
            return Task.FromResult(new MyAccountingIdentificationResponse(m, dt, st, vrs));
        };
        IReadOnlyCollection<IValidationRule> validationRuleSet =
        [
            _fixture!.CreateRequiredValueRule(),
            _fixture!.CreateMinLengthRule(),
            _fixture!.CreateMaxLengthRule()
        ];
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(responseBuilder: responseBuilder, validationRuleSet: validationRuleSet);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        await sut.ExecuteAsync(request);

        Assert.That(buildResponseAsyncWasCalledWith!, Is.EqualTo(validationRuleSet));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingIdentificationResponseWhereModelIsEqualToModelReturnedByGetModelAsync()
    {
        object model = new object();
        Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>> modelGetter = (_, _) => Task.FromResult(model);
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(modelGetter: modelGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        MyAccountingIdentificationResponse result = await sut.ExecuteAsync(request);

        Assert.That(result.Model, Is.EqualTo(model));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingIdentificationResponseWhereDynamicTextsIsEqualToDynamicTextsReturnedByDynamicTextBuilder()
    {
        IDynamicTexts dynamicTexts = new Mock<IDynamicTexts>().Object;
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(dynamicTexts: dynamicTexts);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        MyAccountingIdentificationResponse result = await sut.ExecuteAsync(request);

        Assert.That(result.DynamicTexts, Is.EqualTo(dynamicTexts));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingIdentificationResponseWhereStaticTextContainsStaticTextsReturnedByStaticTextProvider()
    {
        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            { StaticTextKey.Accountings, new object[] { 1, 2, 3 } },
            { StaticTextKey.Debtors, new object[] { 4, 5, 6 } },
            { StaticTextKey.Creditors, new object[] { 7, 8, 9 } }
        };
        Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>> staticTextSpecificationsGetter = (_, _) => staticTextSpecifications;
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(staticTextSpecificationsGetter: staticTextSpecificationsGetter);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        MyAccountingIdentificationResponse result = await sut.ExecuteAsync(request);

        foreach (KeyValuePair<StaticTextKey, IEnumerable<object>> staticTextSpecification in staticTextSpecifications)
        {
            Assert.That(result.StaticTexts.ContainsKey(staticTextSpecification.Key), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ExecuteAsync_WhenCalled_ReturnsAccountingIdentificationResponseWhereValidationRuleSetIsEqualToValidationRuleSetReturnedByValidationRuleSetBuilder()
    {
        IReadOnlyCollection<IValidationRule> validationRuleSet =
        [
            _fixture!.CreateRequiredValueRule(),
            _fixture!.CreateMinLengthRule(),
            _fixture!.CreateMaxLengthRule()
        ];
        IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> sut = CreateSut(validationRuleSet: validationRuleSet);

        MyAccountingIdentificationRequest request = CreateAccountingIdentificationRequest(_fixture!);
        MyAccountingIdentificationResponse result = await sut.ExecuteAsync(request);

        Assert.That(result.ValidationRuleSet, Is.EqualTo(validationRuleSet));
    }

    private IQueryFeature<MyAccountingIdentificationRequest, MyAccountingIdentificationResponse> CreateSut(Func<MyAccountingIdentificationRequest, CancellationToken, Task<object>>? modelGetter = null, Func<object, IReadOnlyDictionary<StaticTextKey, string>, IDynamicTexts, IReadOnlyCollection<IValidationRule>, CancellationToken, Task<MyAccountingIdentificationResponse>>? responseBuilder = null, Func<MyAccountingIdentificationRequest, object, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>>>? staticTextSpecificationsGetter = null, IDynamicTexts? dynamicTexts = null, IReadOnlyCollection<IValidationRule>? validationRuleSet = null)
    {
        return CreateSut(_fixture!, _permissionCheckerMock!, _accountingGatewayMock!, _staticTextProviderMock!, _dynamicTextsBuilderMock!, _validationRuleSetBuilderMock!, modelGetter: modelGetter, responseBuilder: responseBuilder, staticTextSpecificationsGetter: staticTextSpecificationsGetter, dynamicTexts: dynamicTexts, validationRuleSet: validationRuleSet);
    }
}