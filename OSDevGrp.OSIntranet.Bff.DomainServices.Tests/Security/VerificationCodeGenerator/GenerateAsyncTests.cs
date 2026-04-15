using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Text.RegularExpressions;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.VerificationCodeGenerator;

[TestFixture]
public class GenerateAsyncTests
{
    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenCalled_ReturnNonEmptyVerificationCode()
    {
        IVerificationCodeGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string result = await sut.GenerateAsync(cancellationTokenSource.Token);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenCalled_ReturnVerificationCodeWhichLengthIsEqualToSix()
    {
        IVerificationCodeGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string result = await sut.GenerateAsync(cancellationTokenSource.Token);

        Assert.That(result.Length, Is.EqualTo(6));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GenerateAsync_WhenCalled_ReturnVerificationCodeWhichMatchesPattern()
    {
        IVerificationCodeGenerator sut = CreateSut();

        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        string result = await sut.GenerateAsync(cancellationTokenSource.Token);

        Assert.That(Regex.IsMatch(result, "^[A-Za-z0-9]{6}$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(32)), Is.True);
    }

    private static IVerificationCodeGenerator CreateSut()
    {
        return new DomainServices.Security.VerificationCodeGenerator();
    }
}