using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;
using OSDevGrp.OSIntranet.WebApi.ClientApi;
using System.Security.Claims;

namespace OSDevGrp.OSIntranet.Bff.ServiceGateways.TestData;

public static class FixtureExtensions
{
    #region Methods

    public static AccountingModel[] CreateAccountingModels(this Fixture fixture, Random random)
    {
        return
        [
            fixture.CreateAccountingModel(random),
            fixture.CreateAccountingModel(random),
            fixture.CreateAccountingModel(random)
        ];
    }

    public static AccountingModel CreateAccountingModel(this Fixture fixture, Random random, LetterHeadIdentificationModel? letterHeadIdentificationModel = null, int? backDating = null, BalanceBelowZeroType? balanceBelowZeroType = null)
    {
        AccountingIdentificationModel accountingIdentificationModel = fixture.CreateAccountingIdentificationModel(random);

        return new AccountingModel(
            fixture.CreateAccountModels(random, accountingIdentificationModel: accountingIdentificationModel),
            backDating ?? random.Next(7, 365),
            balanceBelowZeroType ?? fixture.CreateBalanceBelowZeroType(),
            fixture.CreateBudgetAccountModels(random, accountingIdentificationModel: accountingIdentificationModel),
            fixture.CreateContactAccountModels(random, accountingIdentificationModel: accountingIdentificationModel),
            fixture.Create<bool>(),
            letterHeadIdentificationModel ?? fixture.CreateLetterHeadIdentificationModel(random),
            fixture.Create<bool>(),
            accountingIdentificationModel.Name,
            accountingIdentificationModel.Number,
            DateTimeOffset.UtcNow.Date);
    }

    public static AccountingIdentificationModel CreateAccountingIdentificationModel(this Fixture fixture, Random random)
    {
        return new AccountingIdentificationModel(
            fixture.Create<string>(),
            random.Next(1, 99));
    }

    public static LetterHeadModel[] CreateLetterHeadModels(this Fixture fixture, Random random)
    {
        return
        [
            fixture.CreateLetterHeadModel(random),
            fixture.CreateLetterHeadModel(random),
            fixture.CreateLetterHeadModel(random)
        ];
    }

    public static LetterHeadModel CreateLetterHeadModel(this Fixture fixture, Random random)
    {
        return new LetterHeadModel(
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            fixture.Create<string>(),
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            fixture.Create<string>(),
            random.Next(1, 99));
    }

    public static LetterHeadIdentificationModel CreateLetterHeadIdentificationModel(this Fixture fixture, Random random)
    {
        return new LetterHeadIdentificationModel(
            fixture.Create<string>(),
            random.Next(1, 99));
    }

    public static BalanceBelowZeroType CreateBalanceBelowZeroType(this Fixture fixture)
    {
        return fixture.Create<BalanceBelowZeroType>();
    }

    public static AccountModel[] CreateAccountModels(this Fixture fixture, Random random, AccountingIdentificationModel? accountingIdentificationModel = null)
    {
        accountingIdentificationModel ??= fixture.CreateAccountingIdentificationModel(random);

        AccountGroupModel[] accountGroupModels = fixture.CreateAccountGroupModels(random);

        return
        [
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)]),
            fixture.CreateAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, accountGroupModel: accountGroupModels[random.Next(0, accountGroupModels.Length - 1)])
        ];
    }

    public static AccountModel CreateAccountModel(this Fixture fixture, Random random, AccountingIdentificationModel? accountingIdentificationModel = null, AccountGroupModel? accountGroupModel = null)
    {
        AccountIdentificationModel accountIdentificationModel = fixture.CreateAccountIdentificationModel(random, accountingIdentificationModel);

        return new AccountModel(
            accountGroupModel ?? fixture.CreateAccountGroupModel(random),
            accountIdentificationModel.Accounting,
            accountIdentificationModel.AccountName,
            accountIdentificationModel.AccountNumber,
            fixture.CreateCreditInfoModels(random),
            fixture.Create<bool>(),
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            fixture.Create<bool>(),
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            DateTimeOffset.UtcNow.Date,
            fixture.CreateCreditInfoValuesModel(),
            fixture.CreateCreditInfoValuesModel(),
            fixture.CreateCreditInfoValuesModel());
    }

    public static AccountIdentificationModel CreateAccountIdentificationModel(this Fixture fixture, Random random, AccountingIdentificationModel? accountingIdentificationModel = null, string? accountNumber = null)
    {
        accountingIdentificationModel ??= fixture.CreateAccountingIdentificationModel(random);
        accountNumber ??= fixture.CreateAccountNumber();

        return new AccountIdentificationModel(
            accountingIdentificationModel,
            fixture.Create<string>(),
            accountNumber);
    }

    public static string CreateAccountNumber(this Fixture fixture)
    {
        return fixture.Create<string>().ToUpper();
    }

    public static AccountGroupModel[] CreateAccountGroupModels(this Fixture fixture, Random random)
    {
        return
        [
            fixture.CreateAccountGroupModel(random, number: 1, accountGroupType: AccountGroupType.Assets),
            fixture.CreateAccountGroupModel(random, number: 2, accountGroupType: AccountGroupType.Assets),
            fixture.CreateAccountGroupModel(random, number: 3, accountGroupType: AccountGroupType.Assets),
            fixture.CreateAccountGroupModel(random, number: 4, accountGroupType: AccountGroupType.Liabilities),
            fixture.CreateAccountGroupModel(random, number: 5, accountGroupType: AccountGroupType.Liabilities)
        ];
    }

    public static AccountGroupModel CreateAccountGroupModel(this Fixture fixture, Random random, int? number = null, AccountGroupType? accountGroupType = null)
    {
        return new AccountGroupModel(
            accountGroupType ?? fixture.CreateAccountGroupType(),
            fixture.Create<string>(),
            number ?? random.Next(1, 99));
    }

    public static AccountGroupType CreateAccountGroupType(this Fixture fixture)
    {
        return fixture.Create<AccountGroupType>();
    }

    public static CreditInfoModel[] CreateCreditInfoModels(this Fixture fixture, Random random)
    {
        return [];
    }

    public static CreditInfoValuesModel CreateCreditInfoValuesModel(this Fixture fixture)
    {
        return new CreditInfoValuesModel(
            fixture.Create<double>(),
            fixture.Create<double>(),
            fixture.Create<double>());
    }

    public static BudgetAccountModel[] CreateBudgetAccountModels(this Fixture fixture, Random random, AccountingIdentificationModel? accountingIdentificationModel = null)
    {
        accountingIdentificationModel ??= fixture.CreateAccountingIdentificationModel(random);

        BudgetAccountGroupModel[] budgetAccountGroupModels = fixture.CreateBudgetAccountGroupModels(random);

        return
        [
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)]),
            fixture.CreateBudgetAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, budgetAccountGroupModel: budgetAccountGroupModels[random.Next(0, budgetAccountGroupModels.Length - 1)])
        ];
    }

    public static BudgetAccountModel CreateBudgetAccountModel(this Fixture fixture, Random random, AccountingIdentificationModel? accountingIdentificationModel = null, BudgetAccountGroupModel? budgetAccountGroupModel = null)
    {
        AccountIdentificationModel accountIdentificationModel = fixture.CreateAccountIdentificationModel(random, accountingIdentificationModel);

        return new BudgetAccountModel(
            accountIdentificationModel.Accounting,
            accountIdentificationModel.AccountName,
            accountIdentificationModel.AccountNumber,
            budgetAccountGroupModel ?? fixture.CreateBudgetAccountGroupModel(random),
            fixture.CreateBudgetInfoModels(random),
            fixture.Create<bool>(),
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            fixture.Create<bool>(),
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            DateTimeOffset.UtcNow.Date,
            fixture.CreateBudgetInfoValuesModel(),
            fixture.CreateBudgetInfoValuesModel(),
            fixture.CreateBudgetInfoValuesModel(),
            fixture.CreateBudgetInfoValuesModel());
    }

    public static BudgetAccountGroupModel[] CreateBudgetAccountGroupModels(this Fixture fixture, Random random)
    {
        return
        [
            fixture.CreateBudgetAccountGroupModel(random, number: 1),
            fixture.CreateBudgetAccountGroupModel(random, number: 2),
            fixture.CreateBudgetAccountGroupModel(random, number: 3),
            fixture.CreateBudgetAccountGroupModel(random, number: 4),
            fixture.CreateBudgetAccountGroupModel(random, number: 5)
        ];
    }

    public static BudgetAccountGroupModel CreateBudgetAccountGroupModel(this Fixture fixture, Random random, int? number = null)
    {
        return new BudgetAccountGroupModel(
            fixture.Create<string>(),
            number ?? random.Next(1, 99));
    }

    public static BudgetInfoModel[] CreateBudgetInfoModels(this Fixture fixture, Random random)
    {
        return [];
    }

    public static BudgetInfoValuesModel CreateBudgetInfoValuesModel(this Fixture fixture)
    {
        return new BudgetInfoValuesModel(
            fixture.Create<double>(),
            fixture.Create<double>(),
            fixture.Create<double>());
    }

    public static ContactAccountModel[] CreateContactAccountModels(this Fixture fixture, Random random, AccountingIdentificationModel? accountingIdentificationModel = null)
    {
        accountingIdentificationModel ??= fixture.CreateAccountingIdentificationModel(random);

        PaymentTermModel[] paymentTermModels = fixture.CreatePaymentTermModels(random);

        return
        [
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)]),
            fixture.CreateContactAccountModel(random, accountingIdentificationModel: accountingIdentificationModel, paymentTermModel: paymentTermModels[random.Next(0, paymentTermModels.Length - 1)])
        ];
    }

    public static ContactAccountModel CreateContactAccountModel(this Fixture fixture, Random random, AccountingIdentificationModel? accountingIdentificationModel = null, PaymentTermModel? paymentTermModel = null)
    {
        AccountIdentificationModel accountIdentificationModel = fixture.CreateAccountIdentificationModel(random, accountingIdentificationModel);

        return new ContactAccountModel(
            accountIdentificationModel.Accounting,
            accountIdentificationModel.AccountName,
            accountIdentificationModel.AccountNumber,
            fixture.CreateBalanceInfoModels(random),
            fixture.Create<bool>(),
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            random.Next(100) > 50 ? fixture.CreateEmail() : null,
            fixture.Create<bool>(),
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            paymentTermModel ?? fixture.CreatePaymentTermModel(random),
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            random.Next(100) > 50 ? fixture.Create<string>() : null,
            DateTimeOffset.UtcNow.Date,
            fixture.CreateBalanceInfoValuesModel(),
            fixture.CreateBalanceInfoValuesModel(),
            fixture.CreateBalanceInfoValuesModel());
    }

    public static BalanceInfoModel[] CreateBalanceInfoModels(this Fixture fixture, Random random)
    {
        return [];
    }

    public static BalanceInfoValuesModel CreateBalanceInfoValuesModel(this Fixture fixture)
    {
        return new BalanceInfoValuesModel(fixture.Create<double>());
    }

    public static PaymentTermModel[] CreatePaymentTermModels(this Fixture fixture, Random random)
    {
        return
        [
            fixture.CreatePaymentTermModel(random, number: 1),
            fixture.CreatePaymentTermModel(random, number: 2),
            fixture.CreatePaymentTermModel(random, number: 3),
            fixture.CreatePaymentTermModel(random, number: 4),
            fixture.CreatePaymentTermModel(random, number: 5)
        ];
    }

    public static PaymentTermModel CreatePaymentTermModel(this Fixture fixture, Random random, int? number = null)
    {
        return new PaymentTermModel(
            fixture.Create<string>(),
            number ?? random.Next(1, 99));
    }

    public static ISecurityContext CreateSecurityContext(this Fixture fixture, ClaimsPrincipal? user = null, IToken? token = null)
    {
        return fixture.CreateSecurityContextMock(user, token).Object;
    }

    public static Mock<ISecurityContext> CreateSecurityContextMock(this Fixture fixture, ClaimsPrincipal? user = null, IToken? token = null)
    {
        Mock<ISecurityContext> securityContextMock = new Mock<ISecurityContext>();
        securityContextMock.Setup(m => m.User)
            .Returns(user ?? fixture.CreateAuthenticatedClaimsPrincipal());
        securityContextMock.Setup(m => m.AccessToken)
            .Returns(token ?? fixture.CreateToken());
        return securityContextMock;
    }

    public static ClaimsPrincipal CreateAuthenticatedClaimsPrincipal(this Fixture fixture, ClaimsIdentity? claimsIdentity = null)
    {
        return new ClaimsPrincipal(claimsIdentity ?? fixture.CreateAuthenticatedClaimsIdentity());
    }

    public static ClaimsPrincipal CreateNonAuthenticatedClaimsPrincipal(this Fixture fixture, bool hasClaimsIdentity = true)
    {
        return hasClaimsIdentity 
            ? new ClaimsPrincipal(fixture.CreateNonAuthenticatedClaimsIdentity()) 
            : new ClaimsPrincipal();
    }

    public static ClaimsIdentity CreateAuthenticatedClaimsIdentity(this Fixture fixture, bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string? nameIdentifierClaimValue = null, bool hasNameClaim = true, bool hasNameClaimValue = true, string? nameClaimValue = null, bool hasEmailClaim = true, bool hasEmailClaimValue = true, string? emailClaimValue = null, params Claim[] extraClaims)
    {
        return new ClaimsIdentity(fixture.CreateClaimCollection(hasNameIdentifierClaim: hasNameIdentifierClaim, hasNameIdentifierClaimValue: hasNameIdentifierClaimValue, nameIdentifierClaimValue: nameIdentifierClaimValue, hasNameClaim: hasNameClaim, hasNameClaimValue: hasNameClaimValue, nameClaimValue: nameClaimValue, hasEmailClaim: hasEmailClaim, hasEmailClaimValue: hasEmailClaimValue, emailClaimValue: emailClaimValue, extraClaims: extraClaims), fixture.Create<string>());
    }

    public static ClaimsIdentity CreateNonAuthenticatedClaimsIdentity(this Fixture _)
    {
        return new ClaimsIdentity(Array.Empty<Claim>());
    }

    public static IEnumerable<Claim> CreateClaimCollection(this Fixture fixture, bool hasNameIdentifierClaim = true, bool hasNameIdentifierClaimValue = true, string? nameIdentifierClaimValue = null, bool hasNameClaim = true, bool hasNameClaimValue = true, string? nameClaimValue = null, bool hasEmailClaim = true, bool hasEmailClaimValue = true, string? emailClaimValue = null, params Claim[] extraClaims)
    {
        List<Claim> claimCollection = new List<Claim>();
        if (hasNameIdentifierClaim)
        {
            claimCollection.Add(new Claim(ClaimTypes.NameIdentifier, hasNameIdentifierClaimValue ? nameIdentifierClaimValue ?? fixture.Create<string>() : string.Empty));
        }
        if (hasNameClaim)
        {
            claimCollection.Add(new Claim(ClaimTypes.Name, hasNameClaimValue ? nameClaimValue ?? fixture.Create<string>() : string.Empty));
        }
        if (hasEmailClaim)
        {
            claimCollection.Add(new Claim(ClaimTypes.Email, hasEmailClaimValue ? emailClaimValue ?? fixture.CreateEmail() : string.Empty));
        }
        claimCollection.AddRange(extraClaims);
        return claimCollection;
    }

    public static string CreateEmail(this Fixture fixture)
    {
        return $"{fixture.Create<string>()}@{fixture.Create<string>()}.local";
    }   

    public static IToken CreateToken(this Fixture fixture, string? tokenType = null, string? token = null, DateTimeOffset? expires = null, bool? expired = null)
    {
        return fixture.CreateTokenMock(tokenType, token, expires, expired).Object;
    }

    public static Mock<IToken> CreateTokenMock(this Fixture fixture, string? tokenType = null, string? token = null, DateTimeOffset? expires = null, bool? expired = null)
    {
        expires ??= DateTimeOffset.UtcNow.AddMinutes(60);

        Mock<IToken> tokenMock = new Mock<IToken>();
        tokenMock.Setup(m => m.TokenType)
            .Returns(tokenType ?? fixture.Create<string>());
        tokenMock.Setup(m => m.Token)
            .Returns(token ?? fixture.Create<string>());
        tokenMock.Setup(m => m.Expires)
            .Returns(expires.Value);
        tokenMock.Setup(m => m.Expired)
            .Returns(expires.Value < DateTimeOffset.UtcNow);
        tokenMock.Setup(m => m.Expired)
            .Returns(expired ?? expires.Value < DateTimeOffset.UtcNow);
        return tokenMock;
    }

    #endregion
}