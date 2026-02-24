using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.WebApi.Shared.Dtos;
using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Controllers.Accounting.Dtos;

public class AccountingTextsDto
{
    [Required]
    public required ValueDisplayerDto StatusDate { get; init; }

    [Required]
    public required ValueDisplayerDto BalanceBelowZero { get; init; }

    [Required]
    public required ValueDisplayerDto BackDating { get; init; }

    [Required]
    public required BalanceSheetDisplayerDto BalanceSheetAtStatusDate { get; init; }

    [Required]
    public required BalanceSheetDisplayerDto BalanceSheetAtEndOfLastMonthFromStatusDate { get; init;  }

    [Required]
    public required BalanceSheetDisplayerDto BalanceSheetAtEndOfLastYearFromStatusDate { get; init;  }

    [Required]
    public required BudgetStatementDisplayerDto BudgetStatementForMonthOfStatusDate { get; init; }

    [Required]
    public required BudgetStatementDisplayerDto BudgetStatementForLastMonthOfStatusDate { get; init; }

    [Required]
    public required BudgetStatementDisplayerDto BudgetStatementForYearToDateOfStatusDate { get; init; }

    [Required]
    public required BudgetStatementDisplayerDto BudgetStatementForLastYearOfStatusDate { get; init; }

    [Required]
    public required ObligeePartiesDisplayerDto ObligeePartiesAtStatusDate { get; init; }

    [Required]
    public required ObligeePartiesDisplayerDto ObligeePartiesAtEndOfLastMonthFromStatusDate { get; init;  }

    [Required]
    public required ObligeePartiesDisplayerDto ObligeePartiesAtEndOfLastYearFromStatusDate { get; init;  }

    [Required]
    public required IncomeStatementDisplayerDto IncomeStatement { get; init;  }

    internal static AccountingTextsDto Map(IAccountingTexts accountingTexts)
    {
        return new AccountingTextsDto
        {
            StatusDate = ValueDisplayerDto.Map(accountingTexts.StatusDate),
            BalanceBelowZero = ValueDisplayerDto.Map(accountingTexts.BalanceBelowZero),
            BackDating = ValueDisplayerDto.Map(accountingTexts.BackDating),
            BalanceSheetAtStatusDate = BalanceSheetDisplayerDto.Map(accountingTexts.BalanceSheetAtStatusDate),
            BalanceSheetAtEndOfLastMonthFromStatusDate = BalanceSheetDisplayerDto.Map(accountingTexts.BalanceSheetAtEndOfLastMonthFromStatusDate),
            BalanceSheetAtEndOfLastYearFromStatusDate = BalanceSheetDisplayerDto.Map(accountingTexts.BalanceSheetAtEndOfLastYearFromStatusDate),
            BudgetStatementForMonthOfStatusDate = BudgetStatementDisplayerDto.Map(accountingTexts.BudgetStatementForMonthOfStatusDate),
            BudgetStatementForLastMonthOfStatusDate = BudgetStatementDisplayerDto.Map(accountingTexts.BudgetStatementForLastMonthOfStatusDate),
            BudgetStatementForYearToDateOfStatusDate = BudgetStatementDisplayerDto.Map(accountingTexts.BudgetStatementForYearToDateOfStatusDate),
            BudgetStatementForLastYearOfStatusDate = BudgetStatementDisplayerDto.Map(accountingTexts.BudgetStatementForLastYearOfStatusDate),
            ObligeePartiesAtStatusDate = ObligeePartiesDisplayerDto.Map(accountingTexts.ObligeePartiesAtStatusDate),
            ObligeePartiesAtEndOfLastMonthFromStatusDate = ObligeePartiesDisplayerDto.Map(accountingTexts.ObligeePartiesAtEndOfLastMonthFromStatusDate),
            ObligeePartiesAtEndOfLastYearFromStatusDate = ObligeePartiesDisplayerDto.Map(accountingTexts.ObligeePartiesAtEndOfLastYearFromStatusDate),
            IncomeStatement = IncomeStatementDisplayerDto.Map(accountingTexts.IncomeStatement)
        };
    }
}