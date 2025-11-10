using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Controllers.Accounting.Dtos;

internal static class FixtureExtensions
{
    #region Methods


    internal static IAccountingTexts CreateAccountingTexts(this Fixture fixture, Random random)
    {
        Mock<IAccountingTexts> accountingTextsMock = new Mock<IAccountingTexts>();
        accountingTextsMock.Setup(m => m.StatusDate)
            .Returns(fixture.CreateValueDisplayer(random));
        accountingTextsMock.Setup(m => m.BalanceBelowZero)
            .Returns(fixture.CreateValueDisplayer(random));
        accountingTextsMock.Setup(m => m.BackDating)
            .Returns(fixture.CreateValueDisplayer(random));
        accountingTextsMock.Setup(m => m.BalanceSheetAtStatusDate)
            .Returns(fixture.CreateBalanceSheetDisplayer(random));
        accountingTextsMock.Setup(m => m.BalanceSheetAtEndOfLastMonthFromStatusDate)
            .Returns(fixture.CreateBalanceSheetDisplayer(random));
        accountingTextsMock.Setup(m => m.BalanceSheetAtEndOfLastYearFromStatusDate)
            .Returns(fixture.CreateBalanceSheetDisplayer(random));
        accountingTextsMock.Setup(m => m.BudgetStatementForMonthOfStatusDate)
            .Returns(fixture.CreateBudgetStatementDisplayer(random));
        accountingTextsMock.Setup(m => m.BudgetStatementForLastMonthOfStatusDate)
            .Returns(fixture.CreateBudgetStatementDisplayer(random));
        accountingTextsMock.Setup(m => m.BudgetStatementForYearToDateOfStatusDate)
            .Returns(fixture.CreateBudgetStatementDisplayer(random));
        accountingTextsMock.Setup(m => m.BudgetStatementForLastYearOfStatusDate)
            .Returns(fixture.CreateBudgetStatementDisplayer(random));
        accountingTextsMock.Setup(m => m.ObligeePartiesAtStatusDate)
            .Returns(fixture.CreateObligeePartiesDisplayer(random));
        accountingTextsMock.Setup(m => m.ObligeePartiesAtEndOfLastMonthFromStatusDate)
            .Returns(fixture.CreateObligeePartiesDisplayer(random));
        accountingTextsMock.Setup(m => m.ObligeePartiesAtEndOfLastYearFromStatusDate)
            .Returns(fixture.CreateObligeePartiesDisplayer(random));
        return accountingTextsMock.Object;
    }

    internal static IBalanceSheetDisplayer CreateBalanceSheetDisplayer(this Fixture fixture, Random random)
    {
        Mock<IBalanceSheetDisplayer> balanceSheetDisplayerMock = new Mock<IBalanceSheetDisplayer>();
        balanceSheetDisplayerMock.Setup(m => m.Header)
            .Returns(fixture.Create<string>());
        balanceSheetDisplayerMock.Setup(m => m.Assets)
            .Returns(fixture.CreateValueDisplayer(random));
        balanceSheetDisplayerMock.Setup(m => m.Liabilities)
            .Returns(fixture.CreateValueDisplayer(random));
        return balanceSheetDisplayerMock.Object;
    }

    internal static IBudgetStatementDisplayer CreateBudgetStatementDisplayer(this Fixture fixture, Random random)
    {
        Mock<IBudgetStatementDisplayer> budgetStatementDisplayerMock = new Mock<IBudgetStatementDisplayer>();
        budgetStatementDisplayerMock.Setup(m => m.Header)
            .Returns(fixture.Create<string>());
        budgetStatementDisplayerMock.Setup(m => m.Budget)
            .Returns(fixture.CreateValueDisplayer(random));
        budgetStatementDisplayerMock.Setup(m => m.Posted)
            .Returns(fixture.CreateValueDisplayer(random));
        budgetStatementDisplayerMock.Setup(m => m.Available)
            .Returns(fixture.CreateValueDisplayer(random));
        return budgetStatementDisplayerMock.Object;
    }

    internal static IObligeePartiesDisplayer CreateObligeePartiesDisplayer(this Fixture fixture, Random random)
    {
        Mock<IObligeePartiesDisplayer> obligeePartiesDisplayerMock = new Mock<IObligeePartiesDisplayer>();
        obligeePartiesDisplayerMock.Setup(m => m.Header)
            .Returns(fixture.Create<string>());
        obligeePartiesDisplayerMock.Setup(m => m.Debtors)
            .Returns(fixture.CreateValueDisplayer(random));
        obligeePartiesDisplayerMock.Setup(m => m.Creditors)
            .Returns(fixture.CreateValueDisplayer(random));
        return obligeePartiesDisplayerMock.Object;
    }

    internal static IValueDisplayer CreateValueDisplayer(this Fixture fixture, Random random)
    {
        Mock<IValueDisplayer> valueDisplayerMock = new Mock<IValueDisplayer>();
        valueDisplayerMock.Setup(m => m.Label)
            .Returns(fixture.Create<string>());
        valueDisplayerMock.Setup(m => m.Value)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        return valueDisplayerMock.Object;
    }

    #endregion
}