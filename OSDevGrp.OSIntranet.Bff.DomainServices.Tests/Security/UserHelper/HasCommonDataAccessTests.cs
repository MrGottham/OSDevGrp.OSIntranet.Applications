using AutoFixture;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;

[TestFixture]
public class HasCommonDataAccessTests : UserHelperTestBase
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
    [TestCase(true)]
    [TestCase(false)]
    public void HasCommonDataAccess_WhenUserHasCommonDataClaim_ReturnsTrue(bool withValue)
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasCommonDataClaim: true, hasCommonDataClaimValue: withValue);
        bool result = sut.HasCommonDataAccess(user);

        Assert.That(result, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public void HasCommonDataAccess_WhenUserDoesNotHaveCommonDataClaim_ReturnsFalse()
    {
        IPermissionChecker sut = CreateSut();

        ClaimsPrincipal user = CreateUser(_fixture!, hasCommonDataClaim: false);
        bool result = sut.HasCommonDataAccess(user);

        Assert.That(result, Is.False);
    }

    private IPermissionChecker CreateSut() => CreatePermissionChecker();
}