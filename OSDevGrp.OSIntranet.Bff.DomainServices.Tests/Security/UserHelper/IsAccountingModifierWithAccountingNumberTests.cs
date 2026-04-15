using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class IsAccountingModifierWithAccountingNumberTests : UserHelperTestBase
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
    [TestCase(1, "1")]
    [TestCase(2, "1,2")]
    [TestCase(3, "1,2,3")]
    [TestCase(4, "1,2,3,*")]
    [TestCase(5, "*")]
    public void IsAccountingModifier_WhenUserHasAccountingClaimAndAccountingModifierClaimWithValueAllowingAccountingNumber_ReturnsTrue(int accountingNumber, string accountingModifierClaimValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingModifierClaim: true, hasAccountingModifierClaimValue: true, accountingModifierClaimValue: accountingModifierClaimValue);
        bool result = sut.IsAccountingModifier(user, accountingNumber);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1, "2,3,4,5")]
    [TestCase(2, "1,3,4,5")]
    [TestCase(3, "1,2,4,5")]
    [TestCase(4, "x,y")]
    [TestCase(5, "")]
    public void IsAccountingModifier_WhenUserHasAccountingClaimAndAccountingModifierClaimWithValueNotAllowingAccountingNumber_ReturnsFalse(int accountingNumber, string accountingModifierClaimValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingModifierClaim: true, hasAccountingModifierClaimValue: true, accountingModifierClaimValue: accountingModifierClaimValue);
        bool result = sut.IsAccountingModifier(user, accountingNumber);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingModifier_WhenUserHasAccountingClaimButNoAccountingModifierClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: true, hasAccountingModifierClaim: false);
        bool result = sut.IsAccountingModifier(user, _fixture.Create<int>());

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1, "1")]
    [TestCase(2, "1,2")]
    [TestCase(3, "1,2,3")]
    [TestCase(4, "1,2,3,*")]
    [TestCase(5, "*")]
    public void IsAccountingModifier_WhenUserDoesNotHaveccountingClaimButAccountingModifierClaimWithValueAllowingAccountingNumber_ReturnsFalse(int accountingNumber, string accountingModifierClaimValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingModifierClaim: true, hasAccountingModifierClaimValue: true, accountingModifierClaimValue: accountingModifierClaimValue);
        bool result = sut.IsAccountingModifier(user, accountingNumber);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(1, "2,3,4,5")]
    [TestCase(2, "1,3,4,5")]
    [TestCase(3, "1,2,4,5")]
    [TestCase(4, "x,y")]
    [TestCase(5, "")]
    public void IsAccountingModifier_WhenUserDoesNotHaveccountingClaimButAccountingModifierClaimWithValueNotAllowingAccountingNumber_ReturnsFalse(int accountingNumber, string accountingModifierClaimValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingModifierClaim: true, hasAccountingModifierClaimValue: true, accountingModifierClaimValue: accountingModifierClaimValue);
        bool result = sut.IsAccountingModifier(user, accountingNumber);

        Assert.That(result, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public void IsAccountingModifier_WhenUserDoesNotHaveccountingClaimAndNoAccountingModifierClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasAccountingClaim: false, hasAccountingModifierClaim: false);
        bool result = sut.IsAccountingModifier(user, _fixture.Create<int>());

        Assert.That(result, Is.False);
    }

    private IPermissionChecker CreateSut() => CreatePermissionChecker();
}