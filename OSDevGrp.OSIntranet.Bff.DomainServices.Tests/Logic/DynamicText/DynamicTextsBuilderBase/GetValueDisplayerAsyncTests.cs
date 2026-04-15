using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.DynamicTextsBuilderBase;

[TestFixture]
public class GetValueDisplayerAsyncTests
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
    [TestCase(StaticTextKey.BalanceBelowZero)]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    public async Task GetValueDisplayerAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextKey(StaticTextKey staticTextKey)
    {
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(staticTextKey: staticTextKey);

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == staticTextKey),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenFormatProvider()
    {
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut();

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(model, formatProvider);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.Is<IFormatProvider>(value => value == formatProvider),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenCancellationToken()
    {
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut();

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.BuildAsync(model, CultureInfo.InvariantCulture, cancellationToken);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.IsAny<StaticTextKey>(),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_AssertValueFormatterWasCalledWithValueFromModel()
    {
        decimal? valueFormatterCalledWith = null;
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(valueFormatter: (value, formatProvider) =>
        {
            valueFormatterCalledWith = value;
            return value.ToString("C", formatProvider);
        });

        decimal value = _fixture!.Create<decimal>();
        MyModel<decimal> model = new MyModel<decimal>(value);
        await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(valueFormatterCalledWith, Is.EqualTo(value));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_AssertValueFormatterWasCalledWithGivenFormatProvider()
    {
        IFormatProvider? valueFormatterCalledWith = null;
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(valueFormatter: (value, fp) =>
        {
            valueFormatterCalledWith = fp;
            return value.ToString("C", fp);
        });

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        await sut.BuildAsync(model, formatProvider);

        Assert.That(valueFormatterCalledWith, Is.EqualTo(formatProvider));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.BalanceBelowZero)]
    [TestCase(StaticTextKey.Debtors)]
    [TestCase(StaticTextKey.Creditors)]
    public async Task GetValueDisplayerAsync_WhenCalled_ReturnsValueDisplayerWhereLabelIsEqualToStaticTextFromStaticTextProvider(StaticTextKey staticTextKey)
    {
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(staticTextKey: staticTextKey);

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        MyValueDisplayerText result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValueDisplayer.Label, Does.StartWith($"{staticTextKey}:"));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_ReturnsValueDisplayerWhereValueIsEqualToValueFromValueFormatter()
    {
        string formattedValue = _fixture!.Create<string>(); 
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(valueFormatter: (_, _) => formattedValue);

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        MyValueDisplayerText result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValueDisplayer.Value, Is.EqualTo(formattedValue));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_ReturnsValueDisplayerWhereValueIsNullWhenValueFormatterReturnsNull()
    {
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(valueFormatter: (_, _) => null);

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        MyValueDisplayerText result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValueDisplayer.Value, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_ReturnsValueDisplayerWhereValueIsNullWhenValueFormatterReturnsEmpty()
    {
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(valueFormatter: (_, _) => string.Empty);

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        MyValueDisplayerText result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValueDisplayer.Value, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_ReturnsValueDisplayerWhereValueIsNullWhenValueFormatterReturnsWhiteSpace()
    {
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(valueFormatter: (_, _) => " ");

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        MyValueDisplayerText result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValueDisplayer.Value, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetValueDisplayerAsync_WhenCalled_ReturnsValueDisplayerWhereValueIsNullWhenValueFormatterReturnsWhiteSpaces()
    {
        IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> sut = CreateSut(valueFormatter: (_, _) => "  ");

        MyModel<decimal> model = new MyModel<decimal>(_fixture!.Create<decimal>());
        MyValueDisplayerText result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValueDisplayer.Value, Is.Null);
    }

    private IDynamicTextsBuilder<MyModel<decimal>, MyValueDisplayerText> CreateSut(StaticTextKey staticTextKey = StaticTextKey.BalanceBelowZero, Func<decimal, IFormatProvider, string?>? valueFormatter = null)
    {
        valueFormatter ??= (value, formatProvider) => value.ToString("C", formatProvider);

        _staticTextProviderMock!.Setup(_fixture!);

        return new MyDynamicTextsBuilder(staticTextKey, valueFormatter, _staticTextProviderMock!.Object);
    }

    private class MyDynamicTextsBuilder : DomainServices.Logic.DynamicText.DynamicTextsBuilderBase<MyModel<decimal>, MyValueDisplayerText>
    {
        #region Private variables

        private readonly StaticTextKey _staticTextKey;
        private readonly Func<decimal, IFormatProvider, string?> _valueFormatter;

        #endregion

        #region Constructor

        public MyDynamicTextsBuilder(StaticTextKey staticTextKey, Func<decimal, IFormatProvider, string?> valueFormatter, IStaticTextProvider staticTextProvider)
            : base(staticTextProvider)
        {
            _staticTextKey = staticTextKey;
            _valueFormatter = valueFormatter;
        }

        #endregion

        #region Methods

        public override async Task<MyValueDisplayerText> BuildAsync(MyModel<decimal> model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
        {
            IValueDisplayer valueDisplayer = await GetValueDisplayerAsync(_staticTextKey, _staticTextKey.DefaultArguments(), model.Value, formatProvider, _valueFormatter, cancellationToken);

            return new MyValueDisplayerText(valueDisplayer);
        }

        #endregion
    }
}