using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.OneOfRuleFactory;

[TestFixture]
public class CreateAsyncTests
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
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenStaticTextKey(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.Is<IEnumerable<object>>(value => value.Any() == false),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenStaticTextKeyAndGivenFormatProvider(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenStaticTextKeyAndGivenCancellationToken(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForOneOfValidationError(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        int[] validValues = _fixture!.CreateMany<int>(5).ToArray();
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, validValues, CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.OneOfValidationError),
                It.Is<IEnumerable<object>>(value => value.Count() == 2 && ((string)value.ElementAt(0)).StartsWith($"{staticTextKey}:") && ((string)value.ElementAt(1)) == string.Join(", ", validValues)),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForOneOfValidationErrorAndGivenFormatProvider(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.OneOfValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForOneOfValidationErrorAndGivenCancellationToken(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.OneOfValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsOneOfRule(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        IOneOfRule<int> result = (IOneOfRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<OneOfRule<int>>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsOneOfRuleWhereNameIsEqualToGivenName(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        string name = _fixture!.Create<string>();
        IOneOfRule<int> result = (IOneOfRule<int>)await sut.CreateAsync(name, staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), CultureInfo.InvariantCulture);

        Assert.That(result.Name, Is.EqualTo(name));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsOneOfRuleWhereRuleTypeIsEqualToOneOfRule(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        IOneOfRule<int> result = (IOneOfRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), CultureInfo.InvariantCulture);

        Assert.That(result.RuleType, Is.EqualTo(ValidationRuleType.OneOfRule));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsOneOfRuleWhereValidValuesIsEqualToGivenValidValues(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        int[] validValues = _fixture!.CreateMany<int>(5).ToArray();
        IOneOfRule<int> result = (IOneOfRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, validValues, CultureInfo.InvariantCulture);

        Assert.That(result.ValidValues, Is.EqualTo(validValues));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsOneOfRuleWhereValidationErrorIsEqualToStaticTextResolvedByStaticTextProvider(StaticTextKey staticTextKey)
    {
        IOneOfRuleFactory sut = CreateSut();

        IOneOfRule<int> result = (IOneOfRule<int>)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.CreateMany<int>(5).ToArray(), CultureInfo.InvariantCulture);

        Assert.That(result.ValidationError, Does.StartWith($"{StaticTextKey.OneOfValidationError}:"));
    }

    private IOneOfRuleFactory CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.Validation.OneOfRuleFactory(_staticTextProviderMock!.Object);
    }
}