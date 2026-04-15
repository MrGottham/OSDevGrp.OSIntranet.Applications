using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class GetNameIdentfierTests : UserHelperTestBase
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
    public void GetNameIdentifier_WhenUserHasNameIdentifierClaimWithValue_ReturnsNotNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true);
        string? result = sut.GetNameIdentifier(user);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetNameIdentifier_WhenUserHasNameIdentifierClaimWithValue_ReturnsNonEmptyValue()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true);
        string? result = sut.GetNameIdentifier(user);

        Assert.That(result, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public void GetNameIdentifier_WhenUserHasNameIdentifierClaimWithValue_ReturnsValueFromNameIdentifierClaim()
    {
        IUserHelper sut = CreateSut();

        string nameIdentifierClaimValue = _fixture!.Create<string>();
        ClaimsPrincipal user = CreateUser(_fixture!, hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: true, nameIdentifierClaimValue: nameIdentifierClaimValue);
        string? result = sut.GetNameIdentifier(user);

        Assert.That(result, Is.EqualTo(nameIdentifierClaimValue));
    }

    [Test]
    [Category("UnitTest")]
    public void GetNameIdentifier_WhenUserHasNameIdentifierClaimWithoutValue_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameIdentifierClaim: true, hasNameIdentifierClaimValue: false);
        string? result = sut.GetNameIdentifier(user);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetNameIdentifier_WhenUserDoesNotHaveNameIdentifierClaim_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasNameIdentifierClaim: false);
        string? result = sut.GetNameIdentifier(user);

        Assert.That(result, Is.Null);
    }

    private IUserHelper CreateSut() => CreateUserHelper();
}