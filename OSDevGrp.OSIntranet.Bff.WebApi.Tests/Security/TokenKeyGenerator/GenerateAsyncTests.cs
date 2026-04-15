using System.Text;
using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenKeyGenerator;

[TestFixture]
public class GenerateAsyncTests
{
    #region Private variables

    private Mock<IHashGenerator>? _hashGeneratorMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _hashGeneratorMock = new Mock<IHashGenerator>();
        _fixture = new Fixture();
        _random = new Random(_fixture!.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenValuesIsEmpty_ThrowsArgumentException()
    {
        ITokenKeyGenerator sut = CreateSut();

        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<string>()));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenValuesIsEmpty_ThrowsArgumentExceptionWhereParamNameIsEqualToValues()
    {
        ITokenKeyGenerator sut = CreateSut();

        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<string>()));

        Assert.That(result!.ParamName, Is.EqualTo("values"));
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenValuesIsEmpty_ThrowsArgumentExceptionWithSpecificMessage()
    {
        ITokenKeyGenerator sut = CreateSut();

        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<string>()));

        Assert.That(result!.Message, Does.StartWith("The given collection does not contain any items."));
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenValuesIsEmpty_ThrowsArgumentExceptionWhereInnerExceptionIsNull()
    {
        ITokenKeyGenerator sut = CreateSut();

        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<string>()));

        Assert.That(result!.InnerException, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenValuesIsNotEmpty_AssertGenerateAsyncWasCalledOnHashGeneratorWithByteArrayForValues()
    {
        ITokenKeyGenerator sut = CreateSut();

        string[] values = _fixture!.CreateMany<string>(_random!.Next(5, 10)).ToArray();
        await sut.GenerateAsync(values);

        _hashGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<byte[]>(value => Encoding.UTF8.GetString(value) == string.Join("|", values)),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenValuesIsNotEmpty_AssertGenerateAsyncWasCalledOnHashGeneratorWithGivenCancellationToken()
    {
        ITokenKeyGenerator sut = CreateSut();

        string[] values = _fixture!.CreateMany<string>(_random!.Next(5, 10)).ToArray();
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GenerateAsync(values, cancellationToken);

        _hashGeneratorMock!.Verify(m => m.GenerateAsync(
                It.IsAny<byte[]>(),
                It.Is<CancellationToken>(value => value == cancellationToken)),
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenValuesIsNotEmpty_ReturnsGeneratedHashFromHashGenerator()
    {
        string hash = _fixture!.Create<string>();
        ITokenKeyGenerator sut = CreateSut(hash: hash);

        string[] values = _fixture!.CreateMany<string>(_random!.Next(5, 10)).ToArray();
        string result = await sut.GenerateAsync(values);

        Assert.That(result, Is.EqualTo(hash));
    }

    private ITokenKeyGenerator CreateSut(string? hash = null)
    {
        _hashGeneratorMock!.Setup(m => m.GenerateAsync(It.IsAny<byte[]>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(hash ?? _fixture!.Create<string>()));

        return new WebApi.Security.TokenKeyGenerator(_hashGeneratorMock.Object);
    }
}