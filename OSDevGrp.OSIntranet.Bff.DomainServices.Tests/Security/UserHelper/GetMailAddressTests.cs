using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class GetMailAddressTests : UserHelperTestBase
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
    public void GetMailAddress_WhenUserHasEmailClaimWithValue_ReturnsNotNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasEmailClaim: true, hasEmailClaimValue: true);
        string? result = sut.GetMailAddress(user);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetMailAddress_WhenUserHasEmailClaimWithValue_ReturnsNonEmptyValue()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasEmailClaim: true, hasEmailClaimValue: true);
        string? result = sut.GetMailAddress(user);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public void GetMailAddress_WhenUserHasEmailClaimWithValue_ReturnsValueFromEmailClaim()
    {
        IUserHelper sut = CreateSut();

        string emailClaimValue = $"{_fixture!.Create<string>()}@{_fixture!.Create<string>()}.local";
        ClaimsPrincipal user = CreateUser(_fixture!, hasEmailClaim: true, hasEmailClaimValue: true, emailClaimValue: emailClaimValue);
        string? result = sut.GetMailAddress(user);

        Assert.That(result, Is.EqualTo(emailClaimValue));
    }

    [Test]
    [Category("UnitTest")]
    public void GetMailAddress_WhenUserHasEmailClaimWithoutValue_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasEmailClaim: true, hasEmailClaimValue: false);
        string? result = sut.GetMailAddress(user);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetMailAddress_WhenUserDoesNotHaveEmailClaim_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasEmailClaim: false);
        string? result = sut.GetMailAddress(user);

        Assert.That(result, Is.Null);
    }

    private IUserHelper CreateSut() => CreateUserHelper();
}