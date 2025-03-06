using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using System.Globalization;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.StaticText.StaticTextProvider;

[TestFixture]
public class GetStaticTextAsyncTests
{
    #region Private variables

    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(StaticTextKey.MrGotthamsHomepage, "Mr. Gottham's Homepage", 0)]
    [TestCase(StaticTextKey.OSDevelopmentGroup, "OS Development Group", 0)]
    [TestCase(StaticTextKey.Copyright, "OS Development Group © {0}", 1)]
    [TestCase(StaticTextKey.BuildInfo, "Build {0}", 1)]
    [TestCase(StaticTextKey.Start, "Start", 0)]
    [TestCase(StaticTextKey.Login, "Log ind", 0)]
    [TestCase(StaticTextKey.Logout, "Log ud", 0)]
    public async Task GetStaticTextAsync_WhenCalledWithSpecificStaticTextKey_ReturnsExpectedStaticTesxt(StaticTextKey staticTextKey, string expectedStaticText, int numberOfArguments)
    {
        IStaticTextProvider sut = CreateSut();

        IEnumerable<object> arguments = CreateArguments(numberOfArguments);
        IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        string result = await sut.GetStaticTextAsync(staticTextKey, arguments, formatProvider);

        Assert.That(result, Is.EqualTo(string.Format(expectedStaticText, arguments, formatProvider)));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextAsync_WhenCalledForAllStaticTextKeys_ReturnsStaticText()
    {
        IStaticTextProvider sut = CreateSut();

        foreach (StaticTextKey staticTextKey in Enum.GetValues<StaticTextKey>())
        {
            string result = await sut.GetStaticTextAsync(staticTextKey, CreateArguments(), CultureInfo.InvariantCulture);

            Assert.That(string.IsNullOrWhiteSpace(result), Is.False, $"The static text for {staticTextKey} is not defined");
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetStaticTextAsync_WhenCancellationIsRequestedOnCancellationToken_ThrowsOperationCanceledException()
    {
        IStaticTextProvider sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.Cancel();

        try
        {
            await sut.GetStaticTextAsync(StaticTextKey.Start, CreateArguments(), CultureInfo.InvariantCulture, cancellationToken);

            Assert.Fail("An OperationCanceledException was expected.");
        }
        catch (OperationCanceledException)
        {
        }
    }

    private static IStaticTextProvider CreateSut()
    {
        return new DomainServices.Logic.StaticText.StaticTextProvider();
    }

    private IEnumerable<object> CreateArguments(int? numberOfArguments = null)
    {
        if (numberOfArguments.HasValue && numberOfArguments.Value == 0)
        {
            return Array.Empty<object>();
        }

        return _fixture!.CreateMany<string>(numberOfArguments ?? _random!.Next(5, 10)).ToArray();
    }
}