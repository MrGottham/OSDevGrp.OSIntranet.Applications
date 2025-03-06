using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Features.Queries.PageFeatureBase;

[TestFixture]
public class GetStaticTextsAsyncTests
{
    #region Private variables

    private Mock<IStaticTextProvider>? _staticTextProviderMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _staticTextProviderMock = new Mock<IStaticTextProvider>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_AssertGetStaticTextSpecificationsWasCalledOnPageFeatureBase()
    {
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut();

        TestRequest request = CreateTestRequest();
        await sut.ExecuteAsync(request);

        Assert.That(((TestFeature) sut).GetStaticTextSpecificationsWasCalled, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_AssertGetStaticTextSpecificationsWasCalledOnPageFeatureBaseWithSameRequest()
    {
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut();

        TestRequest request = CreateTestRequest();
        await sut.ExecuteAsync(request);

        Assert.That(((TestFeature) sut).GetStaticTextSpecificationsWasCalledWithRequest, Is.EqualTo(request));;
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_AssertGetStaticTextSpecificationsWasCalledOnPageFeatureBaseWithSpecifiedStaticTextArgument()
    {
        object staticTextArgument = new object();
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut(staticTextArgument);

        TestRequest request = CreateTestRequest();
        await sut.ExecuteAsync(request);

        Assert.That(((TestFeature) sut).GetStaticTextSpecificationsWasCalledWithArgument, Is.EqualTo(staticTextArgument));;
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_AsserGetStaticTextAsyncWasCalledOnStaticTextProviderForEachStaticTextSpecification()
    {
        IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications = new Dictionary<StaticTextKey, IEnumerable<object>>
        {
            {StaticTextKey.Start, _fixture!.CreateMany<object>(_random!.Next(5, 10)).ToArray()},
            {StaticTextKey.Login, _fixture!.CreateMany<object>(_random!.Next(5, 10)).ToArray()},
            {StaticTextKey.Logout, _fixture!.CreateMany<object>(_random!.Next(5, 10)).ToArray()}
        };
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut(staticTextSpecifications);

        TestRequest request = CreateTestRequest();
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
    public async Task GetStaticTextsAsync_WhenCalled_AsserGetStaticTextAsyncWasCalledOnStaticTextProviderForEachStaticTextSpecificationWithFormatProviderFromRequest()
    {
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut(StaticTextKey.Start, StaticTextKey.Login, StaticTextKey.Logout);

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        TestRequest request = CreateTestRequest(formatProvider: formatProvider);
        await sut.ExecuteAsync(request);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(), 
                It.IsAny<IEnumerable<object>>(), 
                It.Is<IFormatProvider>(value => value != null && value == formatProvider), 
                It.IsAny<CancellationToken>()), 
            Times.Exactly(3));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_AsserGetStaticTextAsyncWasCalledOnStaticTextProviderForEachStaticTextSpecificationWithCancellationTokenFromFeatureExecution()
    {
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut(StaticTextKey.Start, StaticTextKey.Login, StaticTextKey.Logout);

        TestRequest request = CreateTestRequest();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ExecuteAsync(request, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(), 
                It.IsAny<IEnumerable<object>>(), 
                It.IsAny<IFormatProvider>(), 
                It.Is<CancellationToken>(value =>  value == cancellationToken)), 
            Times.Exactly(3));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_ReturnsNotNull()
    {
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut(StaticTextKey.Start, StaticTextKey.Login, StaticTextKey.Logout);

        TestRequest request = CreateTestRequest();
        TestResponse response = await sut.ExecuteAsync(request);

        Assert.That(response.StaticTexts, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_ReturnsNonEmptyDictionary()
    {
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut(StaticTextKey.Start, StaticTextKey.Login, StaticTextKey.Logout);

        TestRequest request = CreateTestRequest();
        TestResponse response = await sut.ExecuteAsync(request);

        Assert.That(response.StaticTexts, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_ReturnsNonEmptyDictionaryContainingExpectedStaticTextKeys()
    {
        StaticTextKey[] staticTextKeys = {StaticTextKey.Start, StaticTextKey.Login, StaticTextKey.Logout};
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut(staticTextKeys);

        TestRequest request = CreateTestRequest();
        TestResponse response = await sut.ExecuteAsync(request);

        foreach (StaticTextKey staticTextKey in staticTextKeys)
        {
            Assert.That(response.StaticTexts.ContainsKey(staticTextKey), Is.True);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextsAsync_WhenCalled_ReturnsNonEmptyDictionaryContainingExpectedStaticTextForEachStaticTextKeys()
    {
        StaticTextKey[] staticTextKeys = {StaticTextKey.Start, StaticTextKey.Login, StaticTextKey.Logout};
        IQueryFeature<TestRequest, TestResponse> sut = CreateSut(staticTextKeys);

        TestRequest request = CreateTestRequest();
        TestResponse response = await sut.ExecuteAsync(request);

        foreach (StaticTextKey staticTextKey in staticTextKeys)
        {
            Assert.That(response.StaticTexts[staticTextKey].StartsWith($"{staticTextKey}:"), Is.True);
        }
    }

    private IQueryFeature<TestRequest, TestResponse> CreateSut()
    {
        return CreateSut(StaticTextKey.Start, StaticTextKey.Login, StaticTextKey.Logout);
    }

    private IQueryFeature<TestRequest, TestResponse> CreateSut(params StaticTextKey[] staticTextKeys)
    {
        return CreateSut(new object(), staticTextKeys.ToDictionary(stk => stk, _ => Array.Empty<object>().AsEnumerable()));
    }

    private IQueryFeature<TestRequest, TestResponse> CreateSut(object staticTextArgument, params StaticTextKey[] staticTextKeys)
    {
        return CreateSut(staticTextArgument, staticTextKeys.ToDictionary(stk => stk, _ => Array.Empty<object>().AsEnumerable()));
    }

    private IQueryFeature<TestRequest, TestResponse> CreateSut(IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications)
    {
        return CreateSut(new object(), staticTextSpecifications);
    }

    private IQueryFeature<TestRequest, TestResponse> CreateSut(object staticTextArgument, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications)
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new TestFeature(staticTextArgument, staticTextSpecifications, _staticTextProviderMock!.Object);
    }

    private TestRequest CreateTestRequest(IFormatProvider? formatProvider = null)
    {
        return new TestRequest(Guid.NewGuid(), formatProvider ?? CultureInfo.InvariantCulture, _fixture!.CreateSecurityContext());
    }

    private class TestRequest : PageRequestBase
    {
        #region Constructor

        public TestRequest(Guid requestId, IFormatProvider formatProvider, ISecurityContext securityContext) 
            : base(requestId, formatProvider, securityContext)
        {
        }

        #endregion
    }

    private class TestResponse : PageResponseBase
    {
        #region Constructor

        public TestResponse(IReadOnlyDictionary<StaticTextKey, string> staticTexts) 
            : base(staticTexts)
        {
        }

        #endregion
    }

    private class TestFeature : PageFeatureBase<TestRequest, TestResponse, object>
    {
        #region Private variables

        private readonly object _staticTextArgument;
        private readonly IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> _staticTextSpecifications;

        #endregion

        #region Constructor

        public TestFeature(object staticTextArgument, IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> staticTextSpecifications, IStaticTextProvider staticTextProvider) 
            : base(staticTextProvider)
        {
            _staticTextArgument = staticTextArgument;
            _staticTextSpecifications = staticTextSpecifications;
        }

        #endregion

        #region Properties

        public bool GetStaticTextSpecificationsWasCalled { get; private set; }

        public TestRequest? GetStaticTextSpecificationsWasCalledWithRequest { get; private set; }

        public object? GetStaticTextSpecificationsWasCalledWithArgument { get; private set; }

        #endregion

        #region Methods

        public override async Task<TestResponse> ExecuteAsync(TestRequest request, CancellationToken cancellationToken = default)
        {
            IReadOnlyDictionary<StaticTextKey, string> _staticText = await GetStaticTextsAsync(request, _staticTextArgument, cancellationToken);

            return new TestResponse(_staticText);
        }

        protected override IReadOnlyDictionary<StaticTextKey, IEnumerable<object>> GetStaticTextSpecifications(TestRequest request, object argument)
        {
            GetStaticTextSpecificationsWasCalled = true;
            GetStaticTextSpecificationsWasCalledWithRequest = request;
            GetStaticTextSpecificationsWasCalledWithArgument = argument;

            return _staticTextSpecifications;
        }

        #endregion
    }
}