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
        accountingTextsMock.Setup(m => m.IncomeStatement)
            .Returns(fixture.CreateIncomeStatementDisplayer(random));
        accountingTextsMock.Setup(m => m.BalanceSheet)
            .Returns(fixture.CreateFullBalanceSheetDisplayer(random));
        accountingTextsMock.Setup(m => m.ChartOfAccounts)
            .Returns(fixture.CreateChartOfAccountsDisplayer(random));
        accountingTextsMock.Setup(m => m.ChartOfBudgetAccounts)
            .Returns(fixture.CreateChartOfBudgetAccountsDisplayer(random));
        accountingTextsMock.Setup(m => m.ChartOfContactAccounts)
            .Returns(fixture.CreateChartOfContactAccountsDisplayer(random));
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

    internal static IIncomeStatementDisplayer CreateIncomeStatementDisplayer(this Fixture fixture, Random random)
    {
        Mock<IIncomeStatementDisplayer> incomeStatementDisplayerMock = new Mock<IIncomeStatementDisplayer>();
        incomeStatementDisplayerMock.Setup(m => m.IncomeStatementLabel)
            .Returns(fixture.Create<string>());
        incomeStatementDisplayerMock.Setup(m => m.MonthOfStatusDateLabel)
            .Returns(fixture.Create<string>());
        incomeStatementDisplayerMock.Setup(m => m.LastMonthOfStatusDateLabel)
            .Returns(fixture.Create<string>());
        incomeStatementDisplayerMock.Setup(m => m.YearToDateOfStatusDateLabel)
            .Returns(fixture.Create<string>());
        incomeStatementDisplayerMock.Setup(m => m.LastYearOfStatusDateLabel)
            .Returns(fixture.Create<string>());
        incomeStatementDisplayerMock.Setup(m => m.BudgetLabel)
            .Returns(fixture.Create<string>());
        incomeStatementDisplayerMock.Setup(m => m.PostedLabel)
            .Returns(fixture.Create<string>());
        incomeStatementDisplayerMock.Setup(m => m.AvailableLabel)
            .Returns(fixture.Create<string>());
        incomeStatementDisplayerMock.Setup(m => m.StatusDate)
            .Returns(fixture.CreateValueDisplayer(random));
        List<IIncomeStatementLineDisplayer> lines = new List<IIncomeStatementLineDisplayer>();
        for (int i = 0; i < random.Next(5, 10); i++)
        {
            lines.Add(fixture.CreateIncomeStatementLineDisplayer(random));
        }
        incomeStatementDisplayerMock.Setup(m => m.Lines)
            .Returns(lines);
        return incomeStatementDisplayerMock.Object;
    }

    internal static IIncomeStatementLineDisplayer CreateIncomeStatementLineDisplayer(this Fixture fixture, Random random)
    {
        Mock<IIncomeStatementLineDisplayer> incomeStatementLineDisplayerMock = new Mock<IIncomeStatementLineDisplayer>();
        incomeStatementLineDisplayerMock.Setup(m => m.Identification)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.Description)
            .Returns(fixture.Create<string>());
        incomeStatementLineDisplayerMock.Setup(m => m.BudgetAtMonthOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.PostedAtMonthOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.AvailableAtMonthOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.BudgetAtLastMonthOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.PostedAtLastMonthOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.AvailableAtLastMonthOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.BudgetAtYearToDateOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.PostedAtYearToDateOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.AvailableAtYearToDateOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.BudgetAtLastYearOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.PostedAtLastYearOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        incomeStatementLineDisplayerMock.Setup(m => m.AvailableAtLastYearOfStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        return incomeStatementLineDisplayerMock.Object;
    }

    internal static IFullBalanceSheetDisplayer CreateFullBalanceSheetDisplayer(this Fixture fixture, Random random)
    {
        Mock<IFullBalanceSheetDisplayer> fullBalanceSheetDisplayerMock = new Mock<IFullBalanceSheetDisplayer>();
        fullBalanceSheetDisplayerMock.Setup(m => m.BalanceSheetLabel)
            .Returns(fixture.Create<string>());
        fullBalanceSheetDisplayerMock.Setup(m => m.BalanceSheetAtStatusDateLabel)
            .Returns(fixture.Create<string>());
        fullBalanceSheetDisplayerMock.Setup(m => m.BalanceSheetAtEndOfLastMonthFromStatusDateLabel)
            .Returns(fixture.Create<string>());
        fullBalanceSheetDisplayerMock.Setup(m => m.BalanceSheetAtEndOfLastYearFromStatusDateLabel)
            .Returns(fixture.Create<string>());
        fullBalanceSheetDisplayerMock.Setup(m => m.AssetsLabel)
            .Returns(fixture.Create<string>());
        fullBalanceSheetDisplayerMock.Setup(m => m.LiabilitiesLabel)
            .Returns(fixture.Create<string>());
        fullBalanceSheetDisplayerMock.Setup(m => m.StatusDate)
            .Returns(fixture.CreateValueDisplayer(random));
        List<IFullBalanceSheetLineDisplayer> assetsLines = new List<IFullBalanceSheetLineDisplayer>();
        for (int i = 0; i < random.Next(5, 10); i++)
        {
            assetsLines.Add(fixture.CreateFullBalanceSheetLineDisplayer(random));
        }
        fullBalanceSheetDisplayerMock.Setup(m => m.AssetsLines)
            .Returns(assetsLines);
        List<IFullBalanceSheetLineDisplayer> liabilitiesLines = new List<IFullBalanceSheetLineDisplayer>();
        for (int i = 0; i < random.Next(5, 10); i++)
        {
            liabilitiesLines.Add(fixture.CreateFullBalanceSheetLineDisplayer(random));
        }
        fullBalanceSheetDisplayerMock.Setup(m => m.LiabilitiesLines)
            .Returns(liabilitiesLines);
        return fullBalanceSheetDisplayerMock.Object;
    }

    internal static IFullBalanceSheetLineDisplayer CreateFullBalanceSheetLineDisplayer(this Fixture fixture, Random random)
    {
        Mock<IFullBalanceSheetLineDisplayer> fullBalanceSheetLineDisplayerMock = new Mock<IFullBalanceSheetLineDisplayer>();
        fullBalanceSheetLineDisplayerMock.Setup(m => m.Identification)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.Description)
            .Returns(fixture.Create<string>());
        fullBalanceSheetLineDisplayerMock.Setup(m => m.CreditAtStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.BalanceAtStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.AvailableAtStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.CreditAtEndOfLastMonthFromStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.BalanceAtEndOfLastMonthFromStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.AvailableAtEndOfLastMonthFromStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.CreditAtEndOfLastYearFromStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.BalanceAtEndOfLastYearFromStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        fullBalanceSheetLineDisplayerMock.Setup(m => m.AvailableAtEndOfLastYearFromStatusDate)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        return fullBalanceSheetLineDisplayerMock.Object;
    }

    internal static IChartOfAccountsDisplayer CreateChartOfAccountsDisplayer(this Fixture fixture, Random random)
    {
        List<IChartOfAccountsSectionDisplayer> sections = new List<IChartOfAccountsSectionDisplayer>();
        for (int i = 0; i < random.Next(5, 10); i++)
        {
            sections.Add(fixture.CreateChartOfAccountsSectionDisplayer(random));
        }

        Mock<IChartOfAccountsDisplayer> chartOfAccountsDisplayerMock = new Mock<IChartOfAccountsDisplayer>();
        chartOfAccountsDisplayerMock.Setup(m => m.ChartOfAccountsLabel)
            .Returns(fixture.Create<string>());
        chartOfAccountsDisplayerMock.Setup(m => m.AccountNumberLabel)
            .Returns(fixture.Create<string>());
        chartOfAccountsDisplayerMock.Setup(m => m.AccountNameLabel)
            .Returns(fixture.Create<string>());
        chartOfAccountsDisplayerMock.Setup(m => m.CreditLabel)
            .Returns(fixture.Create<string>());
        chartOfAccountsDisplayerMock.Setup(m => m.BalanceLabel)
            .Returns(fixture.Create<string>());
        chartOfAccountsDisplayerMock.Setup(m => m.AvailableLabel)
            .Returns(fixture.Create<string>());
        chartOfAccountsDisplayerMock.Setup(m => m.StatusDate)
            .Returns(fixture.CreateValueDisplayer(random));
        chartOfAccountsDisplayerMock.Setup(m => m.AccountCreationPossible)
            .Returns(random.Next(100) > 50);
        chartOfAccountsDisplayerMock.Setup(m => m.Sections)
            .Returns(sections);
        return chartOfAccountsDisplayerMock.Object;
    }

    internal static IChartOfAccountsSectionDisplayer CreateChartOfAccountsSectionDisplayer(this Fixture fixture, Random random)
    {
        List<IChartOfAccountsLineDisplayer> lines = new List<IChartOfAccountsLineDisplayer>();
        for (int i = 0; i < random.Next(10, 15); i++)
        {
            lines.Add(fixture.CreateChartOfAccountsLineDisplayer(random));
        }

        Mock<IChartOfAccountsSectionDisplayer> chartOfAccountsSectionDisplayerMock = new Mock<IChartOfAccountsSectionDisplayer>();
        chartOfAccountsSectionDisplayerMock.Setup(m => m.Identification)
            .Returns(fixture.Create<string>());
        chartOfAccountsSectionDisplayerMock.Setup(m => m.Description)
            .Returns(fixture.Create<string>());
        chartOfAccountsSectionDisplayerMock.Setup(m => m.Lines)
            .Returns(lines);
        return chartOfAccountsSectionDisplayerMock.Object;
    }

    internal static IChartOfAccountsLineDisplayer CreateChartOfAccountsLineDisplayer(this Fixture fixture, Random random)
    {
        Mock<IChartOfAccountsLineDisplayer> chartOfAccountsLineDisplayerMock = new Mock<IChartOfAccountsLineDisplayer>();
        chartOfAccountsLineDisplayerMock.Setup(m => m.AccountNumber)
            .Returns(fixture.Create<string>());
        chartOfAccountsLineDisplayerMock.Setup(m => m.AccountName)
            .Returns(fixture.Create<string>());
        chartOfAccountsLineDisplayerMock.Setup(m => m.Credit)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        chartOfAccountsLineDisplayerMock.Setup(m => m.Balance)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        chartOfAccountsLineDisplayerMock.Setup(m => m.Available)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        chartOfAccountsLineDisplayerMock.Setup(m => m.Modifiable)
            .Returns(random.Next(100) > 50);
        chartOfAccountsLineDisplayerMock.Setup(m => m.Deletable)
            .Returns(random.Next(100) > 50);
        return chartOfAccountsLineDisplayerMock.Object;
    }

    internal static IChartOfBudgetAccountsDisplayer CreateChartOfBudgetAccountsDisplayer(this Fixture fixture, Random random)
    {
        List<IChartOfBudgetAccountsSectionDisplayer> sections = new List<IChartOfBudgetAccountsSectionDisplayer>();
        for (int i = 0; i < random.Next(5, 10); i++)
        {
            sections.Add(fixture.CreateChartOfBudgetAccountsSectionDisplayer(random));
        }

        Mock<IChartOfBudgetAccountsDisplayer> chartOfBudgetAccountsDisplayerMock = new Mock<IChartOfBudgetAccountsDisplayer>();
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.ChartOfBudgetAccountsLabel)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.AccountNumberLabel)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.AccountNameLabel)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.BudgetLabel)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.PostedLabel)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.AvailableLabel)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.StatusDate)
            .Returns(fixture.CreateValueDisplayer(random));
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.BudgetAccountCreationPossible)
            .Returns(random.Next(100) > 50);
        chartOfBudgetAccountsDisplayerMock.Setup(m => m.Sections)
            .Returns(sections);
        return chartOfBudgetAccountsDisplayerMock.Object;
    }

    internal static IChartOfBudgetAccountsSectionDisplayer CreateChartOfBudgetAccountsSectionDisplayer(this Fixture fixture, Random random)
    {
        List<IChartOfBudgetAccountsLineDisplayer> lines = new List<IChartOfBudgetAccountsLineDisplayer>();
        for (int i = 0; i < random.Next(10, 15); i++)
        {
            lines.Add(fixture.CreateChartOfBudgetAccountsLineDisplayer(random));
        }

        Mock<IChartOfBudgetAccountsSectionDisplayer> chartOfBudgetAccountsSectionDisplayerMock = new Mock<IChartOfBudgetAccountsSectionDisplayer>();
        chartOfBudgetAccountsSectionDisplayerMock.Setup(m => m.Identification)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsSectionDisplayerMock.Setup(m => m.Description)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsSectionDisplayerMock.Setup(m => m.Lines)
            .Returns(lines);
        return chartOfBudgetAccountsSectionDisplayerMock.Object;
    }

    internal static IChartOfBudgetAccountsLineDisplayer CreateChartOfBudgetAccountsLineDisplayer(this Fixture fixture, Random random)
    {
        Mock<IChartOfBudgetAccountsLineDisplayer> chartOfBudgetAccountsLineDisplayerMock = new Mock<IChartOfBudgetAccountsLineDisplayer>();
        chartOfBudgetAccountsLineDisplayerMock.Setup(m => m.AccountNumber)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsLineDisplayerMock.Setup(m => m.AccountName)
            .Returns(fixture.Create<string>());
        chartOfBudgetAccountsLineDisplayerMock.Setup(m => m.Budget)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        chartOfBudgetAccountsLineDisplayerMock.Setup(m => m.Posted)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        chartOfBudgetAccountsLineDisplayerMock.Setup(m => m.Available)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        chartOfBudgetAccountsLineDisplayerMock.Setup(m => m.Modifiable)
            .Returns(random.Next(100) > 50);
        chartOfBudgetAccountsLineDisplayerMock.Setup(m => m.Deletable)
            .Returns(random.Next(100) > 50);
        return chartOfBudgetAccountsLineDisplayerMock.Object;
    }

    internal static IChartOfContactAccountsDisplayer CreateChartOfContactAccountsDisplayer(this Fixture fixture, Random random)
    {
        List<IChartOfContactAccountsLineDisplayer> lines = new List<IChartOfContactAccountsLineDisplayer>();
        for (int i = 0; i < random.Next(10, 15); i++)
        {
            lines.Add(fixture.CreateChartOfContactAccountsLineDisplayer(random));
        }

        Mock<IChartOfContactAccountsDisplayer> chartOfContactAccountsDisplayerMock = new Mock<IChartOfContactAccountsDisplayer>();
        chartOfContactAccountsDisplayerMock.Setup(m => m.ChartOfContactAccountsLabel)
            .Returns(fixture.Create<string>());
        chartOfContactAccountsDisplayerMock.Setup(m => m.AccountNumberLabel)
            .Returns(fixture.Create<string>());
        chartOfContactAccountsDisplayerMock.Setup(m => m.AccountNameLabel)
            .Returns(fixture.Create<string>());
        chartOfContactAccountsDisplayerMock.Setup(m => m.BalanceLabel)
            .Returns(fixture.Create<string>());
        chartOfContactAccountsDisplayerMock.Setup(m => m.StatusDate)
            .Returns(fixture.CreateValueDisplayer(random));
        chartOfContactAccountsDisplayerMock.Setup(m => m.ContactAccountCreationPossible)
            .Returns(random.Next(100) > 50);
        chartOfContactAccountsDisplayerMock.Setup(m => m.Lines)
            .Returns(lines);
        return chartOfContactAccountsDisplayerMock.Object;
    }

    internal static IChartOfContactAccountsLineDisplayer CreateChartOfContactAccountsLineDisplayer(this Fixture fixture, Random random)
    {
        Mock<IChartOfContactAccountsLineDisplayer> chartOfContactAccountsLineDisplayerMock = new Mock<IChartOfContactAccountsLineDisplayer>();
        chartOfContactAccountsLineDisplayerMock.Setup(m => m.AccountNumber)
            .Returns(fixture.Create<string>());
        chartOfContactAccountsLineDisplayerMock.Setup(m => m.AccountName)
            .Returns(fixture.Create<string>());
        chartOfContactAccountsLineDisplayerMock.Setup(m => m.Balance)
            .Returns(random.Next(100) > 50 ? fixture.Create<string>() : null);
        chartOfContactAccountsLineDisplayerMock.Setup(m => m.Modifiable)
            .Returns(random.Next(100) > 50);
        chartOfContactAccountsLineDisplayerMock.Setup(m => m.Deletable)
            .Returns(random.Next(100) > 50);
        return chartOfContactAccountsLineDisplayerMock.Object;
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