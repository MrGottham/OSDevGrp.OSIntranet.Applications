using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.DynamicText.DynamicTextsBuilderBase;

[TestFixture]
public class BuildAsyncTests
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
    public async Task BuildAsync_WhenCalledWithMultipleModels_AssertBuildAsyncWasCalledForEachModel()
    {
        IList<object> buildAsyncCalledWith = new List<object>();
        IDynamicTextsBuilder<object, IDynamicTexts> sut = CreateSut(onBuildAsync: (model, _, _) =>
        {
            buildAsyncCalledWith.Add(model);

            return Task.FromResult(CreateDynamicTexts());
        });

        object[] models = _fixture!.CreateMany<object>(_random!.Next(5, 10)).ToArray();
        await sut.BuildAsync(models, CultureInfo.InvariantCulture);

        Assert.That(models.All(buildAsyncCalledWith.Contains), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalledWithMultipleModels_AssertBuildAsyncWasCalledForEachModelWithGivenFormatProvider()
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;

        int buildAsyncCalled = 0;
        IDynamicTextsBuilder<object, IDynamicTexts> sut = CreateSut(onBuildAsync: (_, fp, _) =>
        {
            Assert.That(fp, Is.EqualTo(formatProvider));

            buildAsyncCalled++;

            return Task.FromResult(CreateDynamicTexts());
        });

        object[] models = _fixture!.CreateMany<object>(_random!.Next(5, 10)).ToArray();
        await sut.BuildAsync(models, CultureInfo.InvariantCulture);

        Assert.That(buildAsyncCalled, Is.EqualTo(models.Length));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalledWithMultipleModels_AssertBuildAsyncWasCalledForEachModelWithGivenCancellationToken()
    {
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        int buildAsyncCalled = 0;
        IDynamicTextsBuilder<object, IDynamicTexts> sut = CreateSut(onBuildAsync: (_, _, ct) =>
        {
            Assert.That(ct, Is.EqualTo(cancellationToken));

            buildAsyncCalled++;

            return Task.FromResult(CreateDynamicTexts());
        });

        object[] models = _fixture!.CreateMany<object>(_random!.Next(5, 10)).ToArray();
        await sut.BuildAsync(models, CultureInfo.InvariantCulture, cancellationToken);

        Assert.That(buildAsyncCalled, Is.EqualTo(models.Length));
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCalledWithMultipleModels_ReturnsCollectionWithDynamicTextsForAllModels()
    {
        IDictionary<object, IDynamicTexts> dictionary = _fixture!.CreateMany<object>(_random!.Next(5, 10)).ToDictionary(model => model, _ => CreateDynamicTexts());
        IDynamicTextsBuilder<object, IDynamicTexts> sut = CreateSut(onBuildAsync: (model, _, _) => Task.FromResult(dictionary[model]));

        IReadOnlyCollection<IDynamicTexts> result = await sut.BuildAsync(dictionary.Keys, CultureInfo.InvariantCulture);

        Assert.That(dictionary.Values.All(dynamicText => result.Contains(dynamicText)), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task BuildAsync_WhenCancellationIsRequested_ThrowsOperationCanceledException()
    {
        IDynamicTextsBuilder<object, IDynamicTexts> sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        try
        {
            object[] models = _fixture!.CreateMany<object>(_random!.Next(5, 10)).ToArray();
            await sut.BuildAsync(models, CultureInfo.InvariantCulture, cancellationTokenSource.Token);

            Assert.Fail($"An {typeof(OperationCanceledException)} was expected");
        }
        catch (OperationCanceledException)
        {
        }
        catch
        {
            Assert.Fail($"An {typeof(OperationCanceledException)} was expected");
        }
    }

    private IDynamicTextsBuilder<object, IDynamicTexts> CreateSut(Func<object, IFormatProvider, CancellationToken, Task<IDynamicTexts>>? onBuildAsync = null)
    {
        _staticTextProviderMock!.Setup(_fixture!);

        onBuildAsync ??= (_, _, _) => Task.FromResult(CreateDynamicTexts());

        return new MyDynamicTextsBuilder(_staticTextProviderMock!.Object, onBuildAsync);
    }

    private static IDynamicTexts CreateDynamicTexts()
    {
        return new Mock<IDynamicTexts>().Object;
    }

    private class MyDynamicTextsBuilder : DomainServices.Logic.DynamicText.DynamicTextsBuilderBase<object, IDynamicTexts>
    {
        #region Private variables

        private readonly Func<object, IFormatProvider, CancellationToken, Task<IDynamicTexts>> _onBuildAsync;

        #endregion

        #region Constructor

        public MyDynamicTextsBuilder(IStaticTextProvider staticTextProvider, Func<object, IFormatProvider, CancellationToken, Task<IDynamicTexts>> onBuildAsync)
            : base(staticTextProvider)
        {
            _onBuildAsync = onBuildAsync;
        }

        #endregion

        #region Methods

        public override Task<IDynamicTexts> BuildAsync(object model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
        {
            return _onBuildAsync(model, formatProvider, cancellationToken);
        }

        #endregion
    }
}