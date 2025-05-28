using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class GetFullNameTests : UserHelperTestBase
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
    public void GetFullName_WhenUserHasNameClaimWithValue_ReturnsNotNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: true, hasNameClaimValue: true);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserHasNameClaimWithValue_ReturnsNonEmptyValue()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: true, hasNameClaimValue: true);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserHasNameClaimWithValue_ReturnsValueFromNameClaim()
    {
        IUserHelper sut = CreateSut();

        string nameClaimValue = _fixture!.Create<string>();
        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: true, hasNameClaimValue: true, nameClaimValue: nameClaimValue);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.EqualTo(nameClaimValue));
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserHasNameClaimWithoutValueButEmailClaimWithValue_ReturnsNotNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: true, hasNameClaimValue: false, hasEmailClaim: true, hasEmailClaimValue: true);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserHasNameClaimWithoutValueButEmailClaimWithValue_ReturnsNonEmptyValue()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: true, hasNameClaimValue: false, hasEmailClaim: true, hasEmailClaimValue: true);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserHasNameClaimWithoutValueButEmailClaimWithValue_ReturnsValueFromNameClaim()
    {
        IUserHelper sut = CreateSut();

        string emailClaimValue = $"{_fixture!.Create<string>()}@{_fixture.Create<string>()}.local";
        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: true, hasNameClaimValue: false, hasEmailClaim: true, hasEmailClaimValue: true, emailClaimValue: emailClaimValue);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.EqualTo(emailClaimValue));
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserHasNameClaimWithoutValueButEmailClaimWithoutValue_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: true, hasNameClaimValue: false, hasEmailClaim: true, hasEmailClaimValue: false);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserHasNameClaimWithoutValueAndNoEmailClaim_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: true, hasNameClaimValue: false, hasEmailClaim: false);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserDoesNotHaveNameClaimButEmailClaimWithValue_ReturnsNotNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: false, hasEmailClaim: true, hasEmailClaimValue: true);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserDoesNotHaveNameClaimButEmailClaimWithValue_ReturnsNonEmptyValue()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: false, hasEmailClaim: true, hasEmailClaimValue: true);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserDoesNotHaveNameClaimButEmailClaimWithValue_ReturnsValueFromNameClaim()
    {
        IUserHelper sut = CreateSut();

        string emailClaimValue = $"{_fixture!.Create<string>()}@{_fixture.Create<string>()}.local";
        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: false, hasEmailClaim: true, hasEmailClaimValue: true, emailClaimValue: emailClaimValue);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.EqualTo(emailClaimValue));
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserDoesNotHaveNameClaimButEmailClaimWithoutValue_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: false, hasEmailClaim: true, hasEmailClaimValue: false);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetFullName_WhenUserDoesNotHaveNameClaimAndNoEmailClaim_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameClaim: false, hasEmailClaim: false);
        string? result = sut.GetFullName(user);

        Assert.That(result, Is.Null);
    }

    private IUserHelper CreateSut() => CreateUserHelper();
}