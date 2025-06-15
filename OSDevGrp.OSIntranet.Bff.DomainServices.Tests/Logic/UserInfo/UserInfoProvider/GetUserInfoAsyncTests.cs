using AutoFixture;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.UserInfo;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Security.UserHelper;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Logic.UserInfo.UserInfoProvider;

[TestFixture]
public class GetUserInfoAsyncTests
{
    #region Private variables

    private Mock<IUserHelper>? _userHelperMock;
    private Mock<IAccountingGateway>? _accountingGatewayMock;
    private Fixture? _fixture;
    private Random? _random;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _userHelperMock = new Mock<IUserHelper>();
        _accountingGatewayMock = new Mock<IAccountingGateway>();
        _fixture = new Fixture();
        _random = new Random(_fixture.Create<int>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalled_AssertIsAuthenticatedWasCalledOnUserHelperWithGivenClaimsPrincipal(bool isAuthenticated)
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: isAuthenticated);

        ClaimsPrincipal claimsPrincipal = isAuthenticated
            ? _fixture!.CreateAuthenticatedClaimsPrincipal()
            : _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAuthenticated(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertGetNameIdentifierWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.GetNameIdentifier(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertGetFullNameWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.GetFullName(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertGetMailAddressWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.GetMailAddress(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertHasAccountingAccessWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.HasAccountingAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertGetDefaultAccountingNumberWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.GetDefaultAccountingNumber(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertGetAccountingsAsyncWasNotCalledOnAccountingGateway()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _accountingGatewayMock!.Verify(m => m.GetAccountingsAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertIsAccountingAdministratorWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAccountingAdministrator(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertIsAccountingCreatorWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAccountingCreator(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertIsAccountingModifierWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAccountingModifier(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertIsAccountingViewerWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAccountingViewer(It.IsAny<ClaimsPrincipal>(), It.IsAny<int?>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_AssertHasCommonDataAccessWasNotCalledOnUserHelper()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.HasCommonDataAccess(It.IsAny<ClaimsPrincipal>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertGetNameIdentifierWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.GetNameIdentifier(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertGetFullNameWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.GetFullName(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertGetMailAddressWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.GetMailAddress(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertHasAccountingAccessWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.HasAccountingAccess(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertGetDefaultAccountingNumberWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.GetDefaultAccountingNumber(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichHasAccountingAccesss_AssertGetAccountingsAsyncWasCalledOnAccountingGatewayWithSameCancellationToken()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasAccountingAccess: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GetUserInfoAsync(claimsPrincipal, cancellationToken);

        _accountingGatewayMock!.Verify(m => m.GetAccountingsAsync(It.Is<CancellationToken>(value => value == cancellationToken)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichDoesNotHaveAccountingAccesss_AssertGetAccountingsAsyncWasNotCalledOnAccountingGateway()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasAccountingAccess: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = cancellationTokenSource.Token;
        await sut.GetUserInfoAsync(claimsPrincipal, cancellationToken);

        _accountingGatewayMock!.Verify(m => m.GetAccountingsAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertIsAccountingAdministratorWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAccountingAdministrator(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertIsAccountingCreatorWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAccountingCreator(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertIsAccountingModifierWasCalledOnUserHelperWithGivenClaimsPrincipalAndAccountingNumberEqualToNull()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAccountingModifier(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal), It.Is<int?>(value => value == null)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsAccountingModifier_AssertIsAccountingModifierWasCalledOnUserHelperWithGivenClaimsPrincipalAndEachAccountingNumberOnAccountingsFromAccoutingGateway()
    {
        AccountingModel[] accountings = _fixture!.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray();
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingModifier: true, accountings: accountings);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        foreach (AccountingModel accounting in accountings)
        {
            _userHelperMock!.Verify(m => m.IsAccountingModifier(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal), It.Is<int?>(value => value == accounting.Number)), Times.Once);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsNotAccountingModifier_AssertIsAccountingModifierWasCalledOnUserHelperWithGivenClaimsPrincipalAndEachAccountingNumberOnAccountingsFromAccoutingGateway()
    {
        AccountingModel[] accountings = _fixture!.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray();
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingModifier: false, accountings: accountings);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        foreach (AccountingModel accounting in accountings)
        {
            _userHelperMock!.Verify(m => m.IsAccountingModifier(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal), It.Is<int?>(value => value == accounting.Number)), Times.Never);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertIsAccountingViewerWasCalledOnUserHelperWithGivenClaimsPrincipalAndAccountingNumberEqualToNull()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.IsAccountingViewer(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal), It.Is<int?>(value => value == null)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsAccountingViewer_AssertIsAccountingViewerWasNotCalledOnUserHelperWithGivenClaimsPrincipalAndAnyAccountingNumberOnAccountingsFromAccoutingGateway()
    {
        AccountingModel[] accountings = _fixture!.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray();
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingViewer: true, accountings: accountings);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        foreach (AccountingModel accounting in accountings)
        {
            _userHelperMock!.Verify(m => m.IsAccountingViewer(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal), It.Is<int?>(value => value == accounting.Number)), Times.Once);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsNotAccountingViewer_AssertIsAccountingViewerWasNotCalledOnUserHelperWithGivenClaimsPrincipalAndAnyAccountingNumberOnAccountingsFromAccoutingGateway()
    {
        AccountingModel[] accountings = _fixture!.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray();
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingViewer: false, accountings: accountings);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        foreach (AccountingModel accounting in accountings)
        {
            _userHelperMock!.Verify(m => m.IsAccountingViewer(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal), It.Is<int?>(value => value == accounting.Number)), Times.Never);
        }
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_AssertHasCommonDataAccessWasCalledOnUserHelperWithGivenClaimsPrincipal()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        await sut.GetUserInfoAsync(claimsPrincipal);

        _userHelperMock!.Verify(m => m.HasCommonDataAccess(It.Is<ClaimsPrincipal>(value => value == claimsPrincipal)), Times.Once);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithNonAuthenticatedClaimsPrincipal_ReturnsNull()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateNonAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result, Is.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsNotNull()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModel()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result, Is.TypeOf<DomainServices.Logic.UserInfo.UserInfoModel>());
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereNameIdentifierIsEqualToNameIdentifierFromUserHelper(bool hasNameIdentifier)
    {
        string? nameIdentifier = hasNameIdentifier ? _fixture.Create<string>() : null;
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasNameIdentifier: hasNameIdentifier, nameIdentifier: nameIdentifier);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.NameIdentifier, Is.EqualTo(nameIdentifier));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereNameIsEqualToFullNameFromUserHelper(bool hasFullName)
    {
        string? fullName = hasFullName ? _fixture.Create<string>() : null;
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasFullName: hasFullName, fullName: fullName);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Name, Is.EqualTo(fullName));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereMailAddressIsEqualToMailAddressFromUserHelper(bool hasMailAddress)
    {
        string? mailAddress = hasMailAddress ? _fixture.Create<string>() : null;
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasMailAddress: hasMailAddress, mailAddress: mailAddress);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.MailAddress, Is.EqualTo(mailAddress));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereHasAccountingAccessIsEqualToHasAccountingAccessFromUserHelper(bool hasAccountingAccess)
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasAccountingAccess: hasAccountingAccess);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.HasAccountingAccess, Is.EqualTo(hasAccountingAccess));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereDefaultAccountingNumberIsEqualToDefaultAccountingNumberFromUserHelper(bool hasDefaultAccountingNumber)
    {
        int? defaultAccountingNumber = hasDefaultAccountingNumber ? _fixture!.Create<int>() : null;
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasDefaultAccountingNumber: hasDefaultAccountingNumber, defaultAccountingNumber: defaultAccountingNumber);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.DefaultAccountingNumber, Is.EqualTo(defaultAccountingNumber));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichHasAccountingAccesss_ReturnsUserInfoModelWhereAccountingsIsNotEmpty()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasAccountingAccess: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Accountings, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichHasAccountingAccesss_ReturnsUserInfoModelWhereAccountingsContainsAccountingsFromAccoutingGateway()
    {
        AccountingModel[] accountings = _fixture!.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray();
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasAccountingAccess: true, accountings: accountings);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(accountings.All(accounting => result!.Accountings.ContainsKey(accounting.Number)), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichDoesNotHaveAccountingAccesss_ReturnsUserInfoModelWhereAccountingsIsEmpty()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasAccountingAccess: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.Accountings, Is.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereIsAccountingAdministratorIsEqualToIsAccountingAdministratorFromUserHelper(bool isAccountingAdministrator)
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingAdministrator: isAccountingAdministrator);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.IsAccountingAdministrator, Is.EqualTo(isAccountingAdministrator));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereIsAccountingCreatorIsEqualToIsAccountingCreatorFromUserHelper(bool isAccountingCreator)
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingCreator: isAccountingCreator);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.IsAccountingCreator, Is.EqualTo(isAccountingCreator));
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereIsAccountingModifierIsEqualToIsAccountingModifierFromUserHelper(bool isAccountingModifier)
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingModifier: isAccountingModifier);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.IsAccountingModifier, Is.EqualTo(isAccountingModifier));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsAccountingModifier_ReturnsUserInfoModelWhereModifiableAccountingsIsNotEmpty()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingModifier: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.ModifiableAccountings, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsAccountingModifier_ReturnsUserInfoModelWhereModifiableAccountingsIsBaseOnAccountingsFromAccoutingGateway()
    {
        AccountingModel[] accountings = _fixture!.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray();
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingModifier: true, accountings: accountings);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(accountings.All(accounting => result!.ModifiableAccountings.ContainsKey(accounting.Number)), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsNotAccountingModifier_ReturnsUserInfoModelWhereModifiableAccountingsIsEmpty()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingModifier: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.ModifiableAccountings, Is.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereIsAccountingViewerIsEqualToIsAccountingViewerFromUserHelper(bool isAccountingViewer)
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingViewer: isAccountingViewer);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.IsAccountingViewer, Is.EqualTo(isAccountingViewer));
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsAccountingViewer_ReturnsUserInfoModelWhereViewableAccountingsIsNotEmpty()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingViewer: true);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.ViewableAccountings, Is.Not.Empty);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsAccountingViewer_ReturnsUserInfoModelWhereViewableAccountingsIsBaseOnAccountingsFromAccoutingGateway()
    {
        AccountingModel[] accountings = _fixture!.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray();
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingViewer: true, accountings: accountings);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(accountings.All(accounting => result!.ViewableAccountings.ContainsKey(accounting.Number)), Is.True);
    }

    [Test]
    [Category("UnitTest")]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipalWhichIsNotAccountingViewer_ReturnsUserInfoModelWhereViewableAccountingsIsEmpty()
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, isAccountingViewer: false);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.ViewableAccountings, Is.Empty);
    }

    [Test]
    [Category("UnitTest")]
    [TestCase(true)]
    [TestCase(false)]
    public async Task GetUserInfoAsync_WhenCalledWithAuthenticatedClaimsPrincipal_ReturnsUserInfoModelWhereHasCommonDataAccessIsEqualToHasCommonDataAccessFromUserHelper(bool hasCommonDataAccess)
    {
        IUserInfoProvider sut = CreateSut(isAuthenticated: true, hasCommonDataAccess: hasCommonDataAccess);

        ClaimsPrincipal claimsPrincipal = _fixture!.CreateAuthenticatedClaimsPrincipal();
        IUserInfoModel? result = await sut.GetUserInfoAsync(claimsPrincipal);

        Assert.That(result!.HasCommonDataAccess, Is.EqualTo(hasCommonDataAccess));
    }

    private IUserInfoProvider CreateSut(bool isAuthenticated = true, bool hasNameIdentifier = true, string? nameIdentifier = null, bool hasFullName = true, string? fullName = null, bool hasMailAddress = true, string? mailAddress = null, bool hasAccountingAccess = true, bool hasDefaultAccountingNumber = true, int? defaultAccountingNumber = null, IEnumerable<AccountingModel>? accountings = null, bool isAccountingAdministrator = true, bool isAccountingCreator = true, bool isAccountingModifier = true, bool isAccountingViewer = true, bool hasCommonDataAccess = true)
    {
        _userHelperMock!.Setup(_fixture!, isAuthenticated: isAuthenticated, hasNameIdentifier: hasNameIdentifier, nameIdentifier: nameIdentifier, hasFullName: hasFullName, fullName: fullName, hasMailAddress: hasMailAddress, mailAddress: mailAddress, hasAccountingAccess: hasAccountingAccess, hasDefaultAccountingNumber: hasDefaultAccountingNumber, defaultAccountingNumber: defaultAccountingNumber, isAccountingAdministrator: isAccountingAdministrator, isAccountingCreator: isAccountingCreator, isAccountingModifier: isAccountingModifier, isAccountingViewer: isAccountingViewer, hasCommonDataAccess: hasCommonDataAccess);

        _accountingGatewayMock!.Setup(m => m.GetAccountingsAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(accountings ?? _fixture.CreateMany<AccountingModel>(_random!.Next(5, 10)).ToArray()));

        return new DomainServices.Logic.UserInfo.UserInfoProvider(_userHelperMock!.Object, _accountingGatewayMock!.Object);
    }
}