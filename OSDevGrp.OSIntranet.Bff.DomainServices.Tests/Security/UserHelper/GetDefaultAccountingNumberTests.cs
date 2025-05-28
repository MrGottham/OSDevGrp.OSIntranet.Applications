using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class GetDefaultAccountingNumberTests : UserHelperTestBase
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
    public void GetDefaultAccountingNumber_WhenUserHasAccountingClaimWithValue_ReturnsNotNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingClaimValue: true);
        int? result = sut.GetDefaultAccountingNumber(user);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetDefaultAccountingNumber_WhenUserHasAccountingClaimWithValue_ReturnsValueFromAccountingClaim()
    {
        IUserHelper sut = CreateSut();

        int accountingClaimValue = _fixture!.Create<int>();
        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingClaimValue: true, accountingClaimValue: accountingClaimValue);
        int? result = sut.GetDefaultAccountingNumber(user);

        Assert.That(result, Is.EqualTo(accountingClaimValue));
    }

    [Test]
    [Category("UnitTest")]
    public void GetDefaultAccountingNumber_WhenUserHasAccountingClaimWithoutValue_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingClaimValue: false);
        int? result = sut.GetDefaultAccountingNumber(user);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public void GetDefaultAccountingNumber_WhenUserDoesNotHaveAccountingClaim_ReturnsNull()
    {
        IUserHelper sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false);
        int? result = sut.GetDefaultAccountingNumber(user);

        Assert.That(result, Is.Null);
    }

    private IUserHelper CreateSut() => CreateUserHelper();
}