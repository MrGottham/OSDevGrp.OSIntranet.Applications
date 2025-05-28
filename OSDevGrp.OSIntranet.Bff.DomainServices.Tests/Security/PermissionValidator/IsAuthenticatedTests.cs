using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.PermissionValidator;

[TestFixture]
public class IsAuthenticatedTests
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
    public void IsAuthenticated_WhenIdentityOnClaimsPrincipalIsNull_ReturnsFalse()
    {
        IPermissionValidator sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
        bool result = sut.IsAuthenticated(claimsPrincipal);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAuthenticated_WhenIdentityOnClaimsPrincipalIsNonAuthenticatedClaimsIdentity_ReturnsFalse()
    {
        IPermissionValidator sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateNonAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        bool result = sut.IsAuthenticated(claimsPrincipal);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAuthenticated_WhenIdentityOnClaimsPrincipalIsAuthenticatedClaimsIdentity_ReturnsTrue()
    {
        IPermissionValidator sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        bool result = sut.IsAuthenticated(claimsPrincipal);

        Assert.That(result, Is.True);
    }

    private static IPermissionValidator CreateSut()
    {
        return new DomainServices.Security.PermissionValidator();
    }
}