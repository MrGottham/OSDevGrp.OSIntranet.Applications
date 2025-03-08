using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;

internal static class SecurityContextMocker
{
    internal static ISecurityContext CreateSecurityContext(this Fixture fixture, ClaimsPrincipal? user = null, IToken? token = null)
    {
        return fixture.CreateSecurityContextMock(user, token).Object;
    }

    internal static Mock<ISecurityContext> CreateSecurityContextMock(this Fixture fixture, ClaimsPrincipal? user = null, IToken? token = null)
    {
        Mock<ISecurityContext> securityContextMock = new Mock<ISecurityContext>();
        securityContextMock.Setup(m => m.User)
            .Returns(user ?? fixture.CreateAuthenticatedClaimsPrincipal());
        securityContextMock.Setup(m => m.AccessToken)
            .Returns(token ?? fixture.CreateToken());
        return securityContextMock;
    }

    internal static ClaimsPrincipal CreateAuthenticatedClaimsPrincipal(this Fixture fixture, ClaimsIdentity? claimsIdentity = null)
    {
        return new ClaimsPrincipal(claimsIdentity ?? fixture.CreateAuthenticatedClaimsIdentity());
    }

    internal static ClaimsPrincipal CreateNonAuthenticatedClaimsPrincipal(this Fixture fixture)
    {
        return new ClaimsPrincipal(fixture.CreateNonAuthenticatedClaimsIdentity());
    }

    internal static ClaimsIdentity CreateAuthenticatedClaimsIdentity(this Fixture fixture, bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string? nameIdentifierClaimValue = null, bool hasNameClaim = true, bool hasNameClaimValue = true, string? nameClaimValue = null, params Claim[] extraClaims)
    {
        return new ClaimsIdentity(fixture.CreateClaimCollection(hasNameIdentifierClaim: hasNameIdentifierClaim, hasNameIdentifierClaimValue: hasNameIdentifierClaimValue, nameIdentifierClaimValue: nameIdentifierClaimValue, hasNameClaim: hasNameClaim, hasNameClaimValue: hasNameClaimValue, nameClaimValue: nameClaimValue, extraClaims: extraClaims), fixture.Create<string>());
    }

    internal static ClaimsIdentity CreateNonAuthenticatedClaimsIdentity(this Fixture _)
    {
        return new ClaimsIdentity(Array.Empty<Claim>());
    }

    internal static IEnumerable<Claim> CreateClaimCollection(this Fixture fixture, bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string? nameIdentifierClaimValue = null, bool hasNameClaim = true, bool hasNameClaimValue = true, string? nameClaimValue = null, params Claim[] extraClaims)
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
        claimCollection.AddRange(extraClaims);
        return claimCollection;
    }

    internal static IToken CreateToken(this Fixture fixture, string? tokenType = null, string? token = null, DateTimeOffset? expires = null)
    {
        return fixture.CreateTokenMock(tokenType, token, expires).Object;
    }

    internal static Mock<IToken> CreateTokenMock(this Fixture fixture, string? tokenType = null, string? token = null, DateTimeOffset? expires = null)
    {
        expires ??= DateTimeOffset.UtcNow.AddMinutes(60);

        Mock<IToken> tokenMock = new Mock<IToken>();
        tokenMock.Setup(m => m.TokenType)
            .Returns(tokenType ?? fixture.Create<string>());
        tokenMock.Setup(m => m.Token)
            .Returns(token ?? fixture.Create<string>());
        tokenMock.Setup(m => m.Expires)
            .Returns(expires.Value);
        tokenMock.Setup(m => m.Expired)
            .Returns(expires.Value < DateTimeOffset.UtcNow);
        return tokenMock;
    }
}