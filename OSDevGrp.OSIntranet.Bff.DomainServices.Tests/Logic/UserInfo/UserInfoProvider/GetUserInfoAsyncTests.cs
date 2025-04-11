using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Models.Home;
using OSDevGrp.OSIntranet.Bff.DomainServices.Models.Home;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Globalization;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;

[TestFixture]
public class GetUserInfoAsyncTests
{
    #region Private variables

    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenIdentityOnClaimsPrincipalIsNull_AssertGetAccountingsAsyncWasNotCalledOnAccountingGateway()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        _accountingGatewayMock!.Verify(m => m.GetAccountingsAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenIdentityOnClaimsPrincipalIsNonAuthenticatedClaimsIdentity_AssertGetAccountingsAsyncWasNotCalledOnAccountingGateway()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateNonAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        _accountingGatewayMock!.Verify(m => m.GetAccountingsAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasAccountingClaim_AssertGetAccountingsAsyncWasCalledOnAccountingGatewayWithSameCancellationToken(bool hasClaimValue)
    {
        IUserInfoProvider sut = CreateSut();

        Claim accountingClaim = CreateAccountingClaim(hasClaimValue: hasClaimValue);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(extraClaims: accountingClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal, cancellationToken);

        _accountingGatewayMock!.Verify(m => m.GetAccountingsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalDoesNotHaveAccountingClaim_AssertGetAccountingsAsyncWasNotCalledOnAccountingGateway()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        _accountingGatewayMock!.Verify(m => m.GetAccountingsAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenIdentityOnClaimsPrincipalIsNull_ReturnsNull()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenIdentityOnClaimsPrincipalIsNonAuthenticatedClaimsIdentity_ReturnsNull()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateNonAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenIdentityOnClaimsPrincipalIsAuthenticatedClaimsIdentity_ReturnsNotNull()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenIdentityOnClaimsPrincipalIsAuthenticatedClaimsIdentity_ReturnsUserInfoModel()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result, Is.TypeOf<UserInfoModel>());
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasNameClaimWithValue_ReturnsUserInfoModelWhereNameIsEqualToValueFromNameClaim()
    {
        IUserInfoProvider sut = CreateSut();

        string nameClaimValue = _fixture!.Create<string>();
        Claim emailClaim = CreateEmailClaim(hasClaimValue: true);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameClaim: true, hasNameClaimValue: true, nameClaimValue: nameClaimValue, extraClaims: emailClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Name, Is.EqualTo(nameClaimValue));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasNameClaimWithoutValueAndEmailClaimWithValue_ReturnsUserInfoModelWhereNameIsEqualToValueFromEmailClaim()
    {
        IUserInfoProvider sut = CreateSut();

        string email = CreateEmail();
        Claim emailClaim = CreateEmailClaim(hasClaimValue: true, claimValue: email);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameClaim: true, hasNameClaimValue: false, extraClaims: emailClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Name, Is.EqualTo(email));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasNameClaimWithoutValueAndEmailClaimWithoutValue_ReturnsUserInfoModelWhereNameIsNull()
    {
        IUserInfoProvider sut = CreateSut();

        Claim emailClaim = CreateEmailClaim(hasClaimValue: false);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameClaim: true, hasNameClaimValue: false, extraClaims: emailClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Name, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalDoesNotHaveNameClaimButEmailClaimWithValue_ReturnsUserInfoModelWhereNameIsEqualToValueFromEmailClaim()
    {
        IUserInfoProvider sut = CreateSut();

        string email = CreateEmail();
        Claim emailClaim = CreateEmailClaim(hasClaimValue: true, claimValue: email);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameClaim: false, extraClaims: emailClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Name, Is.EqualTo(email));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalDoesNotHaveNameClaimButEmailClaimWithoutValue_ReturnsUserInfoModelWhereNameIsNull()
    {
        IUserInfoProvider sut = CreateSut();

        Claim emailClaim = CreateEmailClaim(hasClaimValue: false);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameClaim: false, extraClaims: emailClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Name, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalDoesNotHaveNameClaimNorEmailClaim_ReturnsUserInfoModelWhereNameIsNull()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(hasNameClaim: false);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Name, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasAccountingClaim_ReturnsUserInfoModelWhereHasAccountingAccessIsTrue(bool hasClaimValue)
    {
        IUserInfoProvider sut = CreateSut();

        Claim accountingClaim = CreateAccountingClaim(hasClaimValue: hasClaimValue);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(extraClaims: accountingClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.HasAccountingAccess, Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasAccountingClaimWithValue_ReturnsUserInfoModelWhereDefaultAccountingNumberIsEqualToValueFromAccountingClaim()
    {
        IUserInfoProvider sut = CreateSut();

        int accountingNumber = _fixture.Create<int>();
        Claim accountingClaim = CreateAccountingClaim(hasClaimValue: true, claimValue: accountingNumber);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(extraClaims: accountingClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.DefaultAccountingNumber, Is.EqualTo(accountingNumber));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasAccountingClaimWithoutValue_ReturnsUserInfoModelWhereDefaultAccountingNumberIsNull()
    {
        IUserInfoProvider sut = CreateSut();

        Claim accountingClaim = CreateAccountingClaim(hasClaimValue: false);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(extraClaims: accountingClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.DefaultAccountingNumber, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasAccountingClaim_ReturnsUserInfoModelWhereAccountingsIsNotEmpty(bool hasClaimValue)
    {
        IUserInfoProvider sut = CreateSut();

        Claim accountingClaim = CreateAccountingClaim(hasClaimValue: hasClaimValue);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(extraClaims: accountingClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Accountings, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalHasAccountingClaim_ReturnsUserInfoModelWhereAccountingsContainsAccountingsFromAccoutingGateway(bool hasClaimValue)
    {
        AccountingModel[] accountings = _fixture!.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray();
        IUserInfoProvider sut = CreateSut(accountings: accountings);

        Claim accountingClaim = CreateAccountingClaim(hasClaimValue: hasClaimValue);
        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity(extraClaims: accountingClaim);
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(accountings.All(accounting => result!.Accountings.ContainsKey(accounting.Number)), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalDoesNotHaveAccountingClaim_ReturnsUserInfoModelWhereHasAccountingAccessIsFalse()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.HasAccountingAccess, Is.False);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalDoesNotHaveAccountingClaim_ReturnsUserInfoModelWhereDefaultAccountingNumberIsNull()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.DefaultAccountingNumber, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenAuthenticatedClaimsPrincipalDoesNotHaveAccountingClaim_ReturnsUserInfoModelWhereAccountingsIsEmpty()
    {
        IUserInfoProvider sut = CreateSut();

        ClaimsIdentity claimsIdentity = _fixture!.CreateAuthenticatedClaimsIdentity();
        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal(claimsIdentity);
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Accountings, Is.Empty);
    }

    private IUserInfoProvider CreateSut(IEnumerable<AccountingModel>? accountings = null)
    {
        _accountingGatewayMock!.Setup(m => m.GetAccountingsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountings ?? _fixture.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray()));

        return new DomainServices.Logic.UserInfo.UserInfoProvider(_accountingGatewayMock!.Object);
    }

    private Claim CreateEmailClaim(bool hasClaimValue = true, string? claimValue = null)
    {
        return new Claim(ClaimTypes.Email, hasClaimValue ? claimValue ?? CreateEmail() : string.Empty);
    }

    private string CreateEmail()
    {
        return $"{_fixture.Create<string>()}@{_fixture.Create<string>()}.local";
    }

    private Claim CreateAccountingClaim(bool hasClaimValue = true, int? claimValue = null)
    {
        return new Claim(DomainServices.Security.ClaimTypes.AccountingClaimType, hasClaimValue ? (claimValue ?? _fixture.Create<int>()).ToString(CultureInfo.InvariantCulture) : string.Empty);
    }
}