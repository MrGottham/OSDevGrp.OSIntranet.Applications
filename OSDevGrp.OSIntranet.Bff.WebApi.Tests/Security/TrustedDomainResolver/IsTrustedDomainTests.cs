using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Options;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TrustedDomainResolver;

[TestFixture]
public class IsTrustedDomainTests
{
    #region Private variables

    private Mock<IOptions<TrustedDomainOptions>>? _trustedDomainOptionsMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _trustedDomainOptionsMock = new Mock<IOptions<TrustedDomainOptions>>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public void IsTrustedDomain_WhenCalled_AssertValueWasCalledOnTrustedDomainOptions()
    {
        ITrustedDomainResolver sut = CreateSut();

        Uri uri = CreateUri();
        sut.IsTrustedDomain(uri);

        _trustedDomainOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public void IsTrustedDomain_WhenUriIsTrustedDomain_ReturnsTrue()
    {
        string[] trustedDomainNames = CreateDomainNameCollection().ToArray();
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(trustedDomainNames: trustedDomainNames);
        ITrustedDomainResolver sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        Uri uri = CreateUri(domainName: trustedDomainNames[_random!.Next(0, trustedDomainNames.Length)]);
        bool result = sut.IsTrustedDomain(uri);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void IsTrustedDomain_WhenUriIsUntrustedDomain_ReturnsFalse()
    {
        IEnumerable<string> trustedDomainNames = CreateDomainNameCollection();
        TrustedDomainOptions trustedDomainOptions = CreateTrustedDomainOptions(trustedDomainNames: trustedDomainNames);
        ITrustedDomainResolver sut = CreateSut(trustedDomainOptions: trustedDomainOptions);

        Uri uri = CreateUri(domainName: CreateDomainName());
        bool result = sut.IsTrustedDomain(uri);

        Assert.That(result, Is.False);
    }

    private ITrustedDomainResolver CreateSut(TrustedDomainOptions? trustedDomainOptions = null)
    {
        _trustedDomainOptionsMock!.Setup(m => m.Value)
            .Returns(trustedDomainOptions ?? CreateTrustedDomainOptions());

        return new WebApi.Security.TrustedDomainResolver(_trustedDomainOptionsMock!.Object);
    }

    private TrustedDomainOptions CreateTrustedDomainOptions(IEnumerable<string>? trustedDomainNames = null)
    {
        return new TrustedDomainOptions
        {
            TrustedDomainCollection = string.Join(";", trustedDomainNames ?? CreateDomainNameCollection())
        };
    }

    private IEnumerable<string> CreateDomainNameCollection() => [CreateDomainName(), CreateDomainName(), CreateDomainName(), CreateDomainName(), CreateDomainName()];

    private string CreateDomainName() => $"{_fixture!.Create<string>()}.local";

    private Uri CreateUri(string? domainName = null)
    {
        return new Uri($"https://{domainName ?? CreateDomainName()}/{_fixture!.Create<string>()}", UriKind.Absolute); 
    }
}