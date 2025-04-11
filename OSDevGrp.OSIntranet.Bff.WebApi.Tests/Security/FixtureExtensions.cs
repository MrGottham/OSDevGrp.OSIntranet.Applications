using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Security;

internal static class FixtureExtensions
{
    #region Methods

    internal static ISecurityContext CreateSecurityContext(this Fixture fixture, Random random, ClaimsPrincipal? user = null, IToken? accessToken = null)
    {
        return fixture.CreateSecurityContextMock(random, user, accessToken).Object;
    }

    internal static Mock<ISecurityContext> CreateSecurityContextMock(this Fixture fixture, Random random, ClaimsPrincipal? user = null, IToken? accessToken = null)
    {
        Mock<ISecurityContext> securityContextMock = new Mock<ISecurityContext>();
        securityContextMock.Setup(m => m.User)
            .Returns(user ?? fixture.CreateAuthenticatedClaimsPrincipal());
        securityContextMock.Setup(m => m.AccessToken)
            .Returns(accessToken ?? fixture.CreateToken(random));
        return securityContextMock;
    }

    internal static ClaimsPrincipal CreateAuthenticatedClaimsPrincipal(this Fixture fixture, ClaimsIdentity? claimsIdentity = null)
    {
        return new ClaimsPrincipal(claimsIdentity ?? fixture.CreateAuthenticatedClaimsIdentity());
    }

    internal static ClaimsPrincipal CreateNonAuthenticatedClaimsPrincipal(this Fixture fixture, bool hasClaimsIdentity = true)
    {
        return hasClaimsIdentity 
            ? new ClaimsPrincipal(fixture.CreateNonAuthenticatedClaimsIdentity()) 
            : new ClaimsPrincipal();
    }

    internal static ClaimsIdentity CreateAuthenticatedClaimsIdentity(this Fixture fixture, bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string? nameIdentifierClaimValue = null, bool hasNameClaim = true, bool hasNameClaimValue = true, string? nameClaimValue = null, bool hasEmailClaim = true, bool hasEmailClaimValue = true, string? emailClaimValue = null)
    {
        return new ClaimsIdentity(fixture.CreateClaimCollection(hasNameIdentifierClaim: hasNameIdentifierClaim, hasNameIdentifierClaimValue: hasNameIdentifierClaimValue, nameIdentifierClaimValue: nameIdentifierClaimValue, hasNameClaim: hasNameClaim, hasNameClaimValue: hasNameClaimValue, nameClaimValue: nameClaimValue, hasEmailClaim: hasEmailClaim, hasEmailClaimValue: hasEmailClaimValue, emailClaimValue: emailClaimValue), fixture.Create<string>());
    }

    internal static ClaimsIdentity CreateNonAuthenticatedClaimsIdentity(this Fixture _)
    {
        return new ClaimsIdentity(Array.Empty<Claim>());
    }

    private static IEnumerable<Claim> CreateClaimCollection(this Fixture fixture, bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string? nameIdentifierClaimValue = null, bool hasNameClaim = true, bool hasNameClaimValue = true, string? nameClaimValue = null, bool hasEmailClaim = true, bool hasEmailClaimValue = true, string? emailClaimValue = null)
    {
        List<Claim> claimCollection = new List<Claim>();
        if (hasNameIdentifierClaim)
        {
            claimCollection.Add(new Claim(ClaimTypes.NameIdentifier, hasNameIdentifierClaimValue ? nameIdentifierClaimValue ?? fixture.Create<string>() : string.Empty));
        }
        if (hasNameClaim)
        {
            claimCollection.Add(new Claim(ClaimTypes.Name, hasNameClaimValue ? nameClaimValue ?? fixture.Create<string>() : string.Empty));
        }
        if (hasEmailClaim)
        {
            claimCollection.Add(new Claim(ClaimTypes.Email, hasEmailClaimValue ? emailClaimValue ?? fixture.CreateEmail() : string.Empty));
        }
        return claimCollection;
    }

    internal static string CreateEmail(this Fixture fixture)
    {
        return $"{fixture.Create<string>()}@{fixture.Create<string>()}.local";
    }   

    internal static IToken CreateToken(this Fixture fixture, Random random, string? tokenType = null, string? token = null, DateTimeOffset? expires = null, bool? expired = null)
    {
        return fixture.CreateTokenMock(random, tokenType, token, expires, expired).Object;
    }

    internal static Mock<IToken> CreateTokenMock(this Fixture fixture, Random random, string? tokenType = null, string? token = null, DateTimeOffset? expires = null, bool? expired = null)
    {
        expires ??= DateTimeOffset.UtcNow.AddMinutes(random.Next(5, 60));

        Mock<IToken> tokenMock = new Mock<IToken>();
        tokenMock.Setup(m => m.TokenType)
            .Returns(tokenType ?? fixture.Create<string>());
        tokenMock.Setup(m => m.Token)
            .Returns(token ?? fixture.Create<string>());
        tokenMock.Setup(m => m.Expires)
            .Returns(expires.Value);
        tokenMock.Setup(m => m.Expired)
            .Returns(expired ?? expires.Value < DateTimeOffset.UtcNow);
        return tokenMock;
    }

    #endregion
}