using AutoFixture;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.AccountingController;

public abstract class AccountingControllerTestBase
{
    #region Methods

    protected static AccountingModel[] CreateAccountingModels(Fixture fixture, Random random)
    {
        return
        [
            CreateAccountingModel(fixture, random),
            CreateAccountingModel(fixture, random),
            CreateAccountingModel(fixture, random)
        ];
    }

    protected static AccountingModel CreateAccountingModel(Fixture fixture, Random random)
    {
        return new AccountingModel(
            CreateAccountModels(fixture, random),
            random.Next(7, 365),
            CreateBalanceBelowZeroType(fixture),
            CreateBudgetAccountModels(fixture, random),
            CreateContactAccountModels(fixture, random),
            fixture.Create<bool>(),
            CreateLetterHeadIdentificationModel(fixture, random),
            fixture.Create<bool>(),
            fixture.Create<string>(),
            random.Next(1, 99),
            DateTimeOffset.UtcNow.Date
        );
    }

    protected static LetterHeadIdentificationModel CreateLetterHeadIdentificationModel(Fixture fixture, Random random)
    {
        return new LetterHeadIdentificationModel(
            fixture.Create<string>(),
            random.Next(1, 99));
    }

    protected static BalanceBelowZeroType CreateBalanceBelowZeroType(Fixture fixture)
    {
        return fixture.Create<BalanceBelowZeroType>();
    }

    protected static AccountModel[] CreateAccountModels(Fixture fixture, Random random)
    {
        return [];
    }

    protected static BudgetAccountModel[] CreateBudgetAccountModels(Fixture fixture, Random random)
    {
        return [];
    }

    protected static ContactAccountModel[] CreateContactAccountModels(Fixture fixture, Random random)
    {
        return [];
    }

    #endregion
}