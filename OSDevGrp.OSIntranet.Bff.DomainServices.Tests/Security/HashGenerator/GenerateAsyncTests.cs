using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Text;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.HashGenerator;

[TestFixture]
public class GenerateAsyncTests
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
    public void GenerateAsync_WhenBufferIsEmpty_ThrowsArgumentException()
    {
        using IHashGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<byte>(), cancellationTokenSource.Token));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenBufferIsEmpty_ThrowsArgumentExceptionWhereParamNameIsEqualToData()
    {
        using IHashGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<byte>(), cancellationTokenSource.Token));

        Assert.That(result!.ParamName, Is.EqualTo("buffer"));
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenBufferIsEmpty_ThrowsArgumentExceptionWithSpecificMessage()
    {
        using IHashGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<byte>(), cancellationTokenSource.Token));

        Assert.That(result!.Message, Does.StartWith("The buffer is empty."));
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenBufferIsEmpty_ThrowsArgumentExceptionWhereInnerExceptionIsNull()
    {
        using IHashGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<byte>(), cancellationTokenSource.Token));

        Assert.That(result!.InnerException, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("8e76d072-1ce4-439d-bbe9-faef270f799c", "b9d047dd7bd9e4946ef239ae4ad6d660e4758c04de82233b4c7f14f123a65700541bfb7328b1cbacc8449d7782fe36642289c759bd30f10600d14653bc5f728a")]
    [TestCase("b76db5a5-d17d-41b6-8881-250874b3e4c9", "ac440e1a36cd68ffd4cb58f54a8e7474855066c16a0c464b374f88f35f5c97c2e2acf335670da26824fd2b5065b8a5c9435f0e985a6fec490e0d15df058289b7")]
    [TestCase("1bbcc2b8-9aa0-453d-a482-7fae8ca560be", "5087f5915c8b9f16a823a23a3adb734e8eee0d06e2c2e381d61760dc2a95a3563ae2828e492df542ce4552af4a040f669ef6f03d2ae5c024dc97b8eb93998c00")]
    [TestCase("3cec54e5-a24f-43d7-82d1-69d759b71751", "ff1ab14f7efc841bd3a308aea574ed19b406e845aad2def108fc988862519fd5bdc11c03185290ad42525bc0fb891593cbcd6e2c0adb216f6c79e609ada30f04")]
    [TestCase("6d664a41-8241-435b-bb5a-e6e70df462ac", "a14e17e7af58bb1d4784fb5fa0fe5f9b0638268ed5b718547a889f72792b589bbf747f9f5c51e75586a18aab6a30fa917099affb8207e2120407de3726df8083")]
    public async Task GenerateAsync_WhenBufferIsEmpty_ReturnsGeneratedHash(string buffer, string expectedValue)
    {
        using IHashGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string result = await sut.GenerateAsync(Encoding.UTF8.GetBytes(buffer), cancellationTokenSource.Token);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenCalledMultipleTimes_ExpectNoError()
    {
        using IHashGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        await sut.GenerateAsync(_fixture!.CreateMany<byte>(_random!.Next(32, 64)).ToArray(), cancellationTokenSource.Token);
        await sut.GenerateAsync(_fixture!.CreateMany<byte>(_random!.Next(32, 64)).ToArray(), cancellationTokenSource.Token);
        await sut.GenerateAsync(_fixture!.CreateMany<byte>(_random!.Next(32, 64)).ToArray(), cancellationTokenSource.Token);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenCalledMultipleTimesSimultaneous_ExpectNoError()
    {
        using IHashGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        await Task.WhenAll(
            sut.GenerateAsync(_fixture!.CreateMany<byte>(_random!.Next(32, 64)).ToArray(), cancellationTokenSource.Token),
            sut.GenerateAsync(_fixture!.CreateMany<byte>(_random!.Next(32, 64)).ToArray(), cancellationTokenSource.Token),
            sut.GenerateAsync(_fixture!.CreateMany<byte>(_random!.Next(32, 64)).ToArray(), cancellationTokenSource.Token));
    }

    private IHashGenerator CreateSut()
    {
        return new DomainServices.Security.HashGenerator();
    }
}