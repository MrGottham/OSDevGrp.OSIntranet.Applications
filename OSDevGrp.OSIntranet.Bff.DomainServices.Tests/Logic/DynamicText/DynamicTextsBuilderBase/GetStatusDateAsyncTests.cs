using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.DynamicTextsBuilderBase;

[TestFixture]
public class GetStatusDateAsyncTests
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
    public async Task GetStatusDateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithStaticTextKeyForStatusDate()
    {
        IDynamicTextsBuilder<MyModel<DateTimeOffset>, MyValueDisplayerText> sut = CreateSut();

        MyModel<DateTimeOffset> model = new MyModel<DateTimeOffset>(DateTimeOffset.Now);
        await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        _staticTextProviderMock!.Verify(m => m.GetStaticTextAsync(
                It.Is<StaticTextKey>(value => value == StaticTextKey.StatusDate),
                It.IsAny<IEnumerable<object>>(),
                It.IsAny<IFormatProvider>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStatusDateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenFormatProvider()
    {
        IDynamicTextsBuilder<MyModel<DateTimeOffset>, MyValueDisplayerText> sut = CreateSut();

        MyModel<DateTimeOffset> model = new MyModel<DateTimeOffset>(DateTimeOffset.Now);
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
    public async Task GetStatusDateAsync_WhenCalled_AssertGetStaticTextAsyncWasCalledOnStaticTextProviderWithGivenCancellationToken()
    {
        IDynamicTextsBuilder<MyModel<DateTimeOffset>, MyValueDisplayerText> sut = CreateSut();

        MyModel<DateTimeOffset> model = new MyModel<DateTimeOffset>(DateTimeOffset.Now);
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
    public async Task GetStatusDateAsync_WhenCalled_ReturnsValueDisplayerWhereLabelIsEqualToStaticTextFromStaticTextProviderForStatusDate()
    {
        IDynamicTextsBuilder<MyModel<DateTimeOffset>, MyValueDisplayerText> sut = CreateSut();

        MyModel<DateTimeOffset> model = new MyModel<DateTimeOffset>(DateTimeOffset.Now);
        MyValueDisplayerText result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValueDisplayer.Label, Does.StartWith($"{StaticTextKey.StatusDate}:"));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("2024-01-31", "yyyy-MM-dd", "2024-01-31")]
    [TestCase("2024-01-31", "dd-MM-yyyy", "31-01-2024")]
    [TestCase("2024-01-31", "d. MMMM yyyy", "31. January 2024")]
    public async Task GetStatusDateAsync_WhenCalled_ReturnsValueDisplayerWhereValueIsEqualToFormattedValue(string value, string format, string expectedFormattedValue)
    {
        IDynamicTextsBuilder<MyModel<DateTimeOffset>, MyValueDisplayerText> sut = CreateSut(format);

        MyModel<DateTimeOffset> model = new MyModel<DateTimeOffset>(DateTimeOffset.ParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture));
        MyValueDisplayerText result = await sut.BuildAsync(model, CultureInfo.InvariantCulture);

        Assert.That(result.ValueDisplayer.Value, Is.EqualTo(expectedFormattedValue));
    }

    private IDynamicTextsBuilder<MyModel<DateTimeOffset>, MyValueDisplayerText> CreateSut(string format = "yyyy-MM-dd")
    {
        _staticTextProviderMock!.Setup(_fixture!);

        return new MyDynamicTextsBuilder(format, _staticTextProviderMock!.Object);
    }

    private class MyDynamicTextsBuilder : DomainServices.Logic.DynamicText.DynamicTextsBuilderBase<MyModel<DateTimeOffset>, MyValueDisplayerText>
    {
        #region Private variables

        private readonly string _format;

        #endregion

        #region Constructor

        public MyDynamicTextsBuilder(string format, IStaticTextProvider staticTextProvider)
            : base(staticTextProvider)
        {
            _format = format;
        }

        #endregion

        #region Methods

        public override async Task<MyValueDisplayerText> BuildAsync(MyModel<DateTimeOffset> model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
        {
            IValueDisplayer valueDisplayer = await GetStatusDateAsync(model.Value, _format, formatProvider, cancellationToken);

            return new MyValueDisplayerText(valueDisplayer);
        }

        #endregion
    }
}