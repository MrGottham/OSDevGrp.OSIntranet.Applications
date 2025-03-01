using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.PermissionValidator;

[TestFixture]
public class HasClaimTests
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
    public void HasClaim_WhenClaimsPrincipalHasMatchingClaim_ReturnsTrue()
    {
        IPermissionValidator sut = CreateSut();

        Claim[] extraClaims = CreateExtraClaims();
        string expectedClaimType = extraClaims[_random!.Next(0, extraClaims.Length - 1)].Type;
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(extraClaims: extraClaims);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        bool result = sut.HasClaim(claimsPrincipal, claim => claim.Type == expectedClaimType);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void HasClaim_WhenClaimsPrincipalDoesNotHaveMatchingClaim_ReturnsFalse()
    {
        IPermissionValidator sut = CreateSut();

        Claim[] extraClaims = CreateExtraClaims();
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(extraClaims: extraClaims);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity: claimsIdentity);
        bool result = sut.HasClaim(claimsPrincipal, claim => claim.Type == _fixture.Create<string>());

        Assert.That(result, Is.False);
    }

    private static IPermissionValidator CreateSut()
    {
        return new DomainServices.Security.PermissionValidator();
    }

    private Claim[] CreateExtraClaims()
    {
        return _fixture!.CreateMany<string>(_random!.Next(10, 15))
            .Select(claimType => new Claim(claimType, _fixture.Create<string>()))
            .ToArray();
    }
}