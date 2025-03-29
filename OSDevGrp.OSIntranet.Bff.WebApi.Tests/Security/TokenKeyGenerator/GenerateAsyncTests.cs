using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenKeyGenerator;

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
    public void GenerateAsync_WhenValuesIsEmpty_ThrowsArgumentException()
    {
        using ITokenKeyGenerator sut = CreateSut();

        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<string>()));

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenValuesIsEmpty_ThrowsArgumentExceptionWhereParamNameIsEqualToValues()
    {
        using ITokenKeyGenerator sut = CreateSut();

        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<string>()));

        Assert.That(result!.ParamName, Is.EqualTo("values"));
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenValuesIsEmpty_ThrowsArgumentExceptionWithSpecificMessage()
    {
        using ITokenKeyGenerator sut = CreateSut();

        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<string>()));

        Assert.That(result!.Message, Does.StartWith("The given collection does not contain any items."));
    }

    [Test]
    [Category("UnitTest")]
    public void GenerateAsync_WhenValuesIsEmpty_ThrowsArgumentExceptionWhereInnerExceptionIsNull()
    {
        using ITokenKeyGenerator sut = CreateSut();

        ArgumentException? result = Assert.ThrowsAsync<ArgumentException>(async () => await sut.GenerateAsync(Array.Empty<string>()));

        Assert.That(result!.InnerException, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase("8e76d072-1ce4-439d-bbe9-faef270f799c", "3019b96e-50fe-4e90-8513-3fabd5e48526", "250cc815-27bc-4c4a-b66b-3f2624537675", "cc506143e6d7162de1f10bbd474e3ae1fda95c954664c1c4c82ff79f91172acae63aed3b117ebe2f63385265b2c3e2b83a673bc5a783a0df886ef3476a1b449a")]
    [TestCase("b76db5a5-d17d-41b6-8881-250874b3e4c9", "b7a2e83c-4392-4ac3-a8e6-db1f97bd17d4", "5b87dc74-1db6-4488-b85c-e6b4cc6af857", "7554c16288e7c7eaa814728a027bfcda447201e53dd73824f7640dc7ddb9ada9b323e623ad28f3a80788080df92ebdf20c008cb772a69f33798ba25fc5ee3bc9")]
    [TestCase("1bbcc2b8-9aa0-453d-a482-7fae8ca560be", "381c9a28-6fd1-46c9-aa02-19b82a7f4a6c", "68b70075-965b-401d-8083-b2f674908564", "1d9477d7131052d307f75fd41a3edc8b57f8556d2fea50f691bdaca4e39e737dcd88af9f4e9ad6944bfd7b406faf7e7d2aed5546adb171bb6527fb9a74d1803a")]
    [TestCase("3cec54e5-a24f-43d7-82d1-69d759b71751", "c3c587a9-8782-449d-86bb-1d051c8a34d5", "dcd98a60-de79-49ef-b9a8-38f5dc452ecc", "e386bb50f04b69282a9b8134801dfe0127309f3201ab137dea0c3a356e9ceb50164f95d9e61cf1302d6d9bd01975bc283a5f3dca05e4d10d6d349f28a0d46662")]
    [TestCase("6d664a41-8241-435b-bb5a-e6e70df462ac", "457b9791-a547-42a6-aef3-02f3b043e0e4", "00e41522-872a-4df6-a823-157753b3ed72", "1f0a86a9d1c4965df235c0307b0f80943586cb0aa7039b426cc4bde122772d30a829692d05c2c54d6f32863632da14badf50d69edebff14b9da93526fc5c6ef6")]
    public async Task GenerateAsync_WhenValuesIsNonEmpty_ReturnsCalculatedTokenKey(string value1, string value2, string value3, string expectedValue)
    {
        using ITokenKeyGenerator sut = CreateSut();

        string result = await sut.GenerateAsync([value1, value2, value3]);

        Assert.That(result, Is.EqualTo(expectedValue));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenCalledMultipleTimes_ExpectNoError()
    {
        using ITokenKeyGenerator sut = CreateSut();

        await sut.GenerateAsync(_fixture!.CreateMany<string>(_random!.Next(5, 10)).ToArray());
        await sut.GenerateAsync(_fixture!.CreateMany<string>(_random!.Next(5, 10)).ToArray());
        await sut.GenerateAsync(_fixture!.CreateMany<string>(_random!.Next(5, 10)).ToArray());
    }

    private ITokenKeyGenerator CreateSut()
    {
        return new WebApi.Security.TokenKeyGenerator();
    }
}