using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Security;
using OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenKeyGenerator;
using System.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security.TokenKeyProvider;

[TestFixture]
public class ResolveAsyncTests
{
    #region Private variables

    private Mock<ITokenKeyGenerator>? _tokenKeyGeneratorMock;
    private Mock<IOptions<TokenKeyProviderOptions>>? _tokenKeyProviderOptionsMock;
    private Fixture? _fixture;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _tokenKeyGeneratorMock = new Mock<ITokenKeyGenerator>();
        _tokenKeyProviderOptionsMock = new Mock<IOptions<TokenKeyProviderOptions>>();
        _fixture = new Fixture();
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithClaimsPrincipalWithoutClaimsIdentity_AssertValueWasCalledOnTokenKeyProviderOptions()
    {
        ITokenKeyProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: false);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyProviderOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithClaimsPrincipalWithoutClaimsIdentity_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForNonAuthenticatedClaimsPrincipal()
    {
        string anonymousUserIdentifier = _fixture!.Create<string>();
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            AnonymousUserIdentifier = anonymousUserIdentifier,
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: false);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 4 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == anonymousUserIdentifier &&
                    value.ElementAt(3) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithClaimsPrincipalWithoutClaimsIdentity_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithGivenCancellationToken()
    {
        ITokenKeyProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: false);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ResolveAsync(claimsPrincipal, cancellationToken);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.IsAny<IReadOnlyCollection<string>>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithClaimsPrincipalWithoutClaimsIdentity_ReturnsTokenKeyGeneratedByTokenKeyGenerator()
    {
        string generatedTokenKey = _fixture.Create<string>();
        ITokenKeyProvider sut = CreateSut(generatedTokenKey: generatedTokenKey);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: false);
        string result = await sut.ResolveAsync(claimsPrincipal);

        Assert.That(result, Is.EqualTo(generatedTokenKey));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertValueWasCalledOnTokenKeyProviderOptions()
    {
        ITokenKeyProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: true);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyProviderOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForNonAuthenticatedClaimsPrincipal()
    {
        string anonymousUserIdentifier = _fixture!.Create<string>();
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            AnonymousUserIdentifier = anonymousUserIdentifier,
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: true);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 4 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == anonymousUserIdentifier &&
                    value.ElementAt(3) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithGivenCancellationToken()
    {
        ITokenKeyProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: true);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ResolveAsync(claimsPrincipal, cancellationToken);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.IsAny<IReadOnlyCollection<string>>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_ReturnsTokenKeyGeneratedByTokenKeyGenerator()
    {
        string generatedTokenKey = _fixture.Create<string>();
        ITokenKeyProvider sut = CreateSut(generatedTokenKey: generatedTokenKey);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal(hasClaimsIdentity: true);
        string result = await sut.ResolveAsync(claimsPrincipal);

        Assert.That(result, Is.EqualTo(generatedTokenKey));
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalWithoutNameIdentifierClaim_AssertValueWasCalledOnTokenKeyProviderOptions()
    {
        ITokenKeyProvider sut = CreateSut();

        try
        {
            ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: false);
            ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
            await sut.ResolveAsync(claimsPrincipal);

            Assert.Fail("An SecurityException was expected.");
        }
        catch (SecurityException)
        {
            _tokenKeyProviderOptionsMock!.Verify(m => m.Value, Times.Once);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalWithoutNameIdentifierClaim_ThrowsSecurityException()
    {
        ITokenKeyProvider sut = CreateSut();

        try
        {
            ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: false);
            ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
            await sut.ResolveAsync(claimsPrincipal);

            Assert.Fail("An SecurityException was expected.");
        }
        catch (SecurityException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalWithoutNameIdentifierClaim_ThrowsSecurityExceptionWithSpecificMessage()
    {
        ITokenKeyProvider sut = CreateSut();

        try
        {
            ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: false);
            ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
            await sut.ResolveAsync(claimsPrincipal);

            Assert.Fail("An SecurityException was expected.");
        }
        catch (SecurityException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Unable to generate a token key for the authenticated user."));
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalWithoutNameIdentifierClaim_ThrowsSecurityExceptionWhereInnerExceptionIsNull()
    {
        ITokenKeyProvider sut = CreateSut();

        try
        {
            ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: false);
            ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
            await sut.ResolveAsync(claimsPrincipal);

            Assert.Fail("An SecurityException was expected.");
        }
        catch (SecurityException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalWithNameIdentifierClaimWithoutValue_AssertValueWasCalledOnTokenKeyProviderOptions()
    {
        ITokenKeyProvider sut = CreateSut();

        try
        {
            ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
            await sut.ResolveAsync(claimsPrincipal);

            Assert.Fail("An SecurityException was expected.");
        }
        catch (SecurityException)
        {
            _tokenKeyProviderOptionsMock!.Verify(m => m.Value, Times.Once);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalWithNameIdentifierClaimWithoutValue_ThrowsSecurityException()
    {
        ITokenKeyProvider sut = CreateSut();

        try
        {
            ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
            await sut.ResolveAsync(claimsPrincipal);

            Assert.Fail("An SecurityException was expected.");
        }
        catch (SecurityException)
        {
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalWithNameIdentifierClaimWithoutValue_ThrowsSecurityExceptionWithSpecificMessage()
    {
        ITokenKeyProvider sut = CreateSut();

        try
        {
            ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
            await sut.ResolveAsync(claimsPrincipal);

            Assert.Fail("An SecurityException was expected.");
        }
        catch (SecurityException ex)
        {
            Assert.That(ex.Message, Is.EqualTo("Unable to generate a token key for the authenticated user."));
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalWithNameIdentifierClaimWithoutValue_ThrowsSecurityExceptionWhereInnerExceptionIsNull()
    {
        ITokenKeyProvider sut = CreateSut();

        try
        {
            ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
            ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
            await sut.ResolveAsync(claimsPrincipal);

            Assert.Fail("An SecurityException was expected.");
        }
        catch (SecurityException ex)
        {
            Assert.That(ex.InnerException, Is.Null);
        }
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true)]
    [TestCase(true, true, true, false)]
    [TestCase(true, true, false, true)]
    [TestCase(true, true, false, false)]
    [TestCase(true, false, true, true)]
    [TestCase(true, false, true, false)]
    [TestCase(true, false, false, true)]
    [TestCase(true, false, false, false)]
    [TestCase(false, true, true, true)]
    [TestCase(false, true, true, false)]
    [TestCase(false, true, false, true)]
    [TestCase(false, true, false, false)]
    [TestCase(false, false, true, true)]
    [TestCase(false, false, true, false)]
    [TestCase(false, false, false, true)]
    [TestCase(false, false, false, false)]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaim_AssertValueWasCalledOnTokenKeyProviderOptions(bool hasNameClaim, bool hasNameClaimValue, bool hasEmailClaim, bool hasEmailClaimValue)
    {
        ITokenKeyProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, hasNameClaim: hasNameClaim, hasNameClaimValue: hasNameClaimValue, hasEmailClaim: hasEmailClaim, hasEmailClaimValue: hasEmailClaimValue);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyProviderOptionsMock!.Verify(m => m.Value, Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithoutNameClaimNorEmailClaim_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: false, hasEmailClaim: false);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 4 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithNameClaimWithoutValueAndNoEmailClaim_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: true, hasNameClaimValue: false, hasEmailClaim: false);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 4 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithEmailClaimWithoutValueAndNoNameClaim_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: false, hasEmailClaim: true, hasEmailClaimValue: false);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 4 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithNameClaimWithoutValueAndEmailClaimWithoutValue_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: true, hasNameClaimValue: false, hasEmailClaim: true, hasEmailClaimValue: false);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 4 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithNameClaimWithValueAndNoEmailClaim_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        string name = _fixture!.Create<string>();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: true, hasNameClaimValue: true, nameClaimValue: name, hasEmailClaim: false);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 5 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == name &&
                    value.ElementAt(4) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithNameClaimWithValueAndEmailClaimWithoutValue_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        string name = _fixture!.Create<string>();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: true, hasNameClaimValue: true, nameClaimValue: name, hasEmailClaim: true, hasEmailClaimValue: false);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 5 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == name &&
                    value.ElementAt(4) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithEmailClaimWithValueAndNoNameClaim_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        string email = _fixture!.CreateEmail();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: false, hasEmailClaim: true, hasEmailClaimValue: true, emailClaimValue: email);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 5 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == email &&
                    value.ElementAt(4) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithEmailClaimWithValueAndNameClaimWihtoutValue_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        string email = _fixture!.CreateEmail();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: true, hasNameClaimValue: false, hasEmailClaim: true, hasEmailClaimValue: true, emailClaimValue: email);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 5 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == email &&
                    value.ElementAt(4) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaimAndWithNameClaimWithValueAndEmailClaimWithValue_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithValuesForAuthenticatedClaimsPrincipal()
    {
        string salt = _fixture!.Create<string>();
        TokenKeyProviderOptions tokenKeyProviderOptions = new TokenKeyProviderOptions
        {
            Salt = salt
        };
        ITokenKeyProvider sut = CreateSut(tokenKeyProviderOptions: tokenKeyProviderOptions);

        string nameIdentifier = _fixture!.Create<string>();
        string name = _fixture!.Create<string>();
        string email = _fixture!.CreateEmail();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifier, hasNameClaim: true, hasNameClaimValue: true, nameClaimValue: name, hasEmailClaim: true, hasEmailClaimValue: true, emailClaimValue: email);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        await sut.ResolveAsync(claimsPrincipal);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.Is<IReadOnlyCollection<string>>(value => value.Count == 6 &&
                    value.ElementAt(0) == typeof(WebApi.Security.TokenStorage).Namespace &&
                    value.ElementAt(1) == typeof(WebApi.Security.TokenStorage).Name &&
                    value.ElementAt(2) == nameIdentifier &&
                    value.ElementAt(3) == name &&
                    value.ElementAt(4) == email &&
                    value.ElementAt(5) == salt),
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true)]
    [TestCase(true, true, true, false)]
    [TestCase(true, true, false, true)]
    [TestCase(true, true, false, false)]
    [TestCase(true, false, true, true)]
    [TestCase(true, false, true, false)]
    [TestCase(true, false, false, true)]
    [TestCase(true, false, false, false)]
    [TestCase(false, true, true, true)]
    [TestCase(false, true, true, false)]
    [TestCase(false, true, false, true)]
    [TestCase(false, true, false, false)]
    [TestCase(false, false, true, true)]
    [TestCase(false, false, true, false)]
    [TestCase(false, false, false, true)]
    [TestCase(false, false, false, false)]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaim_AssertGenerateAsyncWasCalledOnTokenKeyGeneratorWithGivenCancellationToken(bool hasNameClaim, bool hasNameClaimValue, bool hasEmailClaim, bool hasEmailClaimValue)
    {
        ITokenKeyProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, hasNameClaim: hasNameClaim, hasNameClaimValue: hasNameClaimValue, hasEmailClaim: hasEmailClaim, hasEmailClaimValue: hasEmailClaimValue);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.ResolveAsync(claimsPrincipal, cancellationToken);

        _tokenKeyGeneratorMock!.Verify(m => m.GenerateAsync(
                It.IsAny<IReadOnlyCollection<string>>(),
                It.Is<CancellationToken>(value => value == cancellationToken)), 
            Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true, true, true, true)]
    [TestCase(true, true, true, false)]
    [TestCase(true, true, false, true)]
    [TestCase(true, true, false, false)]
    [TestCase(true, false, true, true)]
    [TestCase(true, false, true, false)]
    [TestCase(true, false, false, true)]
    [TestCase(true, false, false, false)]
    [TestCase(false, true, true, true)]
    [TestCase(false, true, true, false)]
    [TestCase(false, true, false, true)]
    [TestCase(false, true, false, false)]
    [TestCase(false, false, true, true)]
    [TestCase(false, false, true, false)]
    [TestCase(false, false, false, true)]
    [TestCase(false, false, false, false)]
    public async Task ResolveAsync_WhenCalledWithAuthenticatedClaimsPrincipalContainingValueInNameIdentifierClaim_ReturnsTokenKeyGeneratedByTokenKeyGenerator(bool hasNameClaim, bool hasNameClaimValue, bool hasEmailClaim, bool hasEmailClaimValue)
    {
        string generatedTokenKey = _fixture.Create<string>();
        ITokenKeyProvider sut = CreateSut(generatedTokenKey: generatedTokenKey);

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, hasNameClaim: hasNameClaim, hasNameClaimValue: hasNameClaimValue, hasEmailClaim: hasEmailClaim, hasEmailClaimValue: hasEmailClaimValue);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        string result = await sut.ResolveAsync(claimsPrincipal);

        Assert.That(result, Is.EqualTo(generatedTokenKey));
    }

    private ITokenKeyProvider CreateSut(string? generatedTokenKey = null, TokenKeyProviderOptions? tokenKeyProviderOptions = null)	
    {
        _tokenKeyGeneratorMock!.Setup(_fixture!, tokenKey: generatedTokenKey);

        _tokenKeyProviderOptionsMock!.Setup(m => m.Value)
            .Returns(tokenKeyProviderOptions ?? new TokenKeyProviderOptions());

        return new WebApi.Security.TokenKeyProvider(_tokenKeyGeneratorMock!.Object, _tokenKeyProviderOptionsMock!.Object);
    }
}