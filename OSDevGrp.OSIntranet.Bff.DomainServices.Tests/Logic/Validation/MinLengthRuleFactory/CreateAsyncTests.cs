using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.Validation;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.Validation.MinLengthRuleFactory;

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
        IMinLengthRuleFactory sut = CreateSut();

        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), CultureInfo.InvariantCulture);

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
        IMinLengthRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), formatProvider);

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
        IMinLengthRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), CultureInfo.InvariantCulture, cancellationToken);

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
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMinLengthValidationError(StaticTextKey staticTextKey)
    {
        IMinLengthRuleFactory sut = CreateSut();

        int minLength = _fixture!.Create<int>();
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, minLength, CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MinLengthValidationError),
                It.Is<IEnumerable<object>>(value => value.Count() == 2 && ((string) value.ElementAt(0)).StartsWith($"{staticTextKey}:") && ((int) value.ElementAt(1)) == minLength),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMinLengthValidationErrorAndGivenFormatProvider(StaticTextKey staticTextKey)
    {
        IMinLengthRuleFactory sut = CreateSut();

        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MinLengthValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextForMinLengthValidationErrorAndGivenCancellationToken(StaticTextKey staticTextKey)
    {
        IMinLengthRuleFactory sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture!.Create<int>(), CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.MinLengthValidationError),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinLengthRule(StaticTextKey staticTextKey)
    {
        IMinLengthRuleFactory sut = CreateSut();

        IMinLengthRule result = (IMinLengthRule)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result, Is.TypeOf<MinLengthRule>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinLengthRuleWhereNameIsEqualToGivenName(StaticTextKey staticTextKey)
    {
        IMinLengthRuleFactory sut = CreateSut();

        string name = _fixture!.Create<string>();
        IMinLengthRule result = (IMinLengthRule)await sut.CreateAsync(name, staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.Name, Is.EqualTo(name));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinLengthRuleWhereRuleTypeIsEqualToMinLengthRule(StaticTextKey staticTextKey)
    {
        IMinLengthRuleFactory sut = CreateSut();

        IMinLengthRule result = (IMinLengthRule)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.RuleType, Is.EqualTo(ValidationRuleType.MinLengthRule));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinLengthRuleWhereMinLengthIsEqualToGivenMinLength(StaticTextKey staticTextKey)
    {
        IMinLengthRuleFactory sut = CreateSut();

        int minLength = _fixture!.Create<int>();
        IMinLengthRule result = (IMinLengthRule)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, minLength, CultureInfo.InvariantCulture);

        Assert.That(result.MinLength, Is.EqualTo(minLength));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.AccountingNumber)]
    [TestCase(StaticTextKey.AccountingName)]
    public async Task CreateAsync_WhenCalled_ReturnsMinLengthRuleWhereValidationErrorIsEqualToStaticTextResolvedByStaticTextProvider(StaticTextKey staticTextKey)
    {
        IMinLengthRuleFactory sut = CreateSut();

        IMinLengthRule result = (IMinLengthRule)await sut.CreateAsync(_fixture!.Create<string>(), staticTextKey, _fixture.Create<int>(), CultureInfo.InvariantCulture);

        Assert.That(result.ValidationError, Does.StartWith($"{StaticTextKey.MinLengthValidationError}:"));
    }

    private IMinLengthRuleFactory CreateSut()
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new DomainServices.Logic.Validation.MinLengthRuleFactory(_staticTextProviderMock!.Object);
    }
}