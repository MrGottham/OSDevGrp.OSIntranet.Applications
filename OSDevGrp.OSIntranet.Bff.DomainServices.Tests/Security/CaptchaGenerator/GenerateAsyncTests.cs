using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.CaptchaGenerator;

[TestFixture]
public class GenerateAsyncTests
{
    #region Private variables

    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenCalled_ReturnNonEmptyByteArray()
    {
        ICaptchaGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        byte[] result = await sut.GenerateAsync(CreateCode(), cancellationTokenSource.Token);

        Assert.That(result, Is.Not.Empty);
    }

    private static ICaptchaGenerator CreateSut()
    {
        return new DomainServices.Security.CaptchaGenerator();
    }

    private string CreateCode()
    {
        string code = _fixture!.Create<string>();
        return code[..(code.Length > 6 ? 6 : code.Length)];
    }
}