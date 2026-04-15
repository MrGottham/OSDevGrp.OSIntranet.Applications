using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountingTextsBuilder : DynamicTextsBuilderBase<AccountingModel, IAccountingTexts>, IAccountingTextsBuilder
{
    #region Constructor

    public AccountingTextsBuilder(IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
    }

    #endregion

    #region Methods

    public override async Task<IAccountingTexts> BuildAsync(AccountingModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        IValueDisplayer? statusDate = null;
        IValueDisplayer? balanceBelowZero = null;
        IValueDisplayer? backDating = null;
        IBalanceSheetDisplayer? balanceSheetAtStatusDate = null;
        IBalanceSheetDisplayer? balanceSheetAtEndOfLastMonthFromStatusDate = null;
        IBalanceSheetDisplayer? balanceSheetAtEndOfLastYearFromStatusDate = null;
        IBudgetStatementDisplayer? budgetStatementForMonthOfStatusDate = null;
        IBudgetStatementDisplayer? budgetStatementForLastMonthOfStatusDate = null;
        IBudgetStatementDisplayer? budgetStatementForYearToDateOfStatusDate = null;
        IBudgetStatementDisplayer? budgetStatementForLastYearOfStatusDate = null;
        IObligeePartiesDisplayer? obligeePartiesAtStatusDate = null;
        IObligeePartiesDisplayer? obligeePartiesAtEndOfLastMonthFromStatusDate = null;
        IObligeePartiesDisplayer? obligeePartiesAtEndOfLastYearFromStatusDate = null;
        IIncomeStatementDisplayer? incomeStatement = null;
        IFullBalanceSheetDisplayer? balanceSheet = null;

        Task buildStatusDateTask = GetStatusDateAsync(model.StatusDate, "d. MMMM yyyy", formatProvider, cancellationToken).ContinueWith(task => statusDate = task.Result, cancellationToken);
        Task buildBalanceBelowZeroTask = BuildBalanceBelowZeroAsync(model, formatProvider, cancellationToken).ContinueWith(task => balanceBelowZero = task.Result, cancellationToken);
        Task buildBackDatingTask = BuildBackDatingAsync(model, formatProvider, cancellationToken).ContinueWith(task => backDating = task.Result, cancellationToken);
        Task buildBalanceSheetAtStatusDateTask = BalanceSheetDisplayer.CreateAsync(StaticTextKey.BalanceSheetAtStatusDate, StaticTextKey.Assets, StaticTextKey.Liabilities, StaticTextProvider, model, m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Assets).Select(m => m.ValuesAtStatusDate).Sum(v => (decimal)v.Balance), m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Liabilities).Select(m => m.ValuesAtStatusDate).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => balanceSheetAtStatusDate = task.Result, cancellationToken);
        Task buildBalanceSheetAtEndOfLastMonthFromStatusDateTask = BalanceSheetDisplayer.CreateAsync(StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate, StaticTextKey.Assets, StaticTextKey.Liabilities, StaticTextProvider, model, m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Assets).Select(m => m.ValuesAtEndOfLastMonthFromStatusDate).Sum(v => (decimal)v.Balance), m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Liabilities).Select(m => m.ValuesAtEndOfLastMonthFromStatusDate).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => balanceSheetAtEndOfLastMonthFromStatusDate = task.Result, cancellationToken);        Task buildBalanceSheetAtEndOfLastYearFromStatusDateTask = BalanceSheetDisplayer.CreateAsync(StaticTextKey.BalanceSheetAtEndOfLastYearFromStatusDate, StaticTextKey.Assets, StaticTextKey.Liabilities, StaticTextProvider, model, m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Assets).Select(m => m.ValuesAtEndOfLastYearFromStatusDate).Sum(v => (decimal)v.Balance), m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Liabilities).Select(m => m.ValuesAtEndOfLastYearFromStatusDate).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => balanceSheetAtEndOfLastYearFromStatusDate = task.Result, cancellationToken);
        Task buildBudgetStatementForMonthOfStatusDateTask = BudgetStatementDisplayer.CreateAsync(StaticTextKey.BudgetStatementForMonthOfStatusDate, StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model, m => m.BudgetAccounts.Select(m => m.ValuesForMonthOfStatusDate).Sum(v => (decimal) v.Budget), m => m.BudgetAccounts.Select(m => m.ValuesForMonthOfStatusDate).Sum(v => (decimal) v.Posted), m => m.BudgetAccounts.Select(m => m.ValuesForMonthOfStatusDate).Sum(v => (decimal) v.Available), formatProvider, cancellationToken).ContinueWith(task => budgetStatementForMonthOfStatusDate = task.Result, cancellationToken);
        Task buildBudgetStatementForLastMonthOfStatusDateTask = BudgetStatementDisplayer.CreateAsync(StaticTextKey.BudgetStatementForLastMonthOfStatusDate, StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model, m => m.BudgetAccounts.Select(m => m.ValuesForLastMonthOfStatusDate).Sum(v => (decimal) v.Budget), m => m.BudgetAccounts.Select(m => m.ValuesForLastMonthOfStatusDate).Sum(v => (decimal) v.Posted), m => m.BudgetAccounts.Select(m => m.ValuesForLastMonthOfStatusDate).Sum(v => (decimal) v.Available), formatProvider, cancellationToken).ContinueWith(task => budgetStatementForLastMonthOfStatusDate = task.Result, cancellationToken);
        Task buildBudgetStatementForYearToDateOfStatusDateTask = BudgetStatementDisplayer.CreateAsync(StaticTextKey.BudgetStatementForYearToDateOfStatusDate, StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model, m => m.BudgetAccounts.Select(m => m.ValuesForYearToDateOfStatusDate).Sum(v => (decimal) v.Budget), m => m.BudgetAccounts.Select(m => m.ValuesForYearToDateOfStatusDate).Sum(v => (decimal) v.Posted), m => m.BudgetAccounts.Select(m => m.ValuesForYearToDateOfStatusDate).Sum(v => (decimal) v.Available), formatProvider, cancellationToken).ContinueWith(task => budgetStatementForYearToDateOfStatusDate = task.Result, cancellationToken);
        Task buildBudgetStatementForLastYearOfStatusDateTask = BudgetStatementDisplayer.CreateAsync(StaticTextKey.BudgetStatementForLastYearOfStatusDate, StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model, m => m.BudgetAccounts.Select(m => m.ValuesForLastYearOfStatusDate).Sum(v => (decimal) v.Budget), m => m.BudgetAccounts.Select(m => m.ValuesForLastYearOfStatusDate).Sum(v => (decimal) v.Posted), m => m.BudgetAccounts.Select(m => m.ValuesForLastYearOfStatusDate).Sum(v => (decimal) v.Available), formatProvider, cancellationToken).ContinueWith(task => budgetStatementForLastYearOfStatusDate = task.Result, cancellationToken);
        Task buildObligeePartiesAtStatusDateTask = ObligeePartiesDisplayer.CreateAsync( StaticTextKey.ObligeePartiesAtStatusDate, StaticTextKey.Debtors, StaticTextKey.Creditors, StaticTextProvider, model, m => m.ContactAccounts.Select(m => m.ValuesAtStatusDate).Where(v => v.IsDebtor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), m => m.ContactAccounts.Select(m => m.ValuesAtStatusDate).Where(v => v.IsCreditor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => obligeePartiesAtStatusDate = task.Result, cancellationToken);
        Task buildObligeePartiesAtEndOfLastMonthFromStatusDateTask = ObligeePartiesDisplayer.CreateAsync(StaticTextKey.ObligeePartiesAtEndOfLastMonthFromStatusDate, StaticTextKey.Debtors, StaticTextKey.Creditors, StaticTextProvider, model, m => m.ContactAccounts.Select(m => m.ValuesAtEndOfLastMonthFromStatusDate).Where(v => v.IsDebtor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), m => m.ContactAccounts.Select(m => m.ValuesAtEndOfLastMonthFromStatusDate).Where(v => v.IsCreditor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => obligeePartiesAtEndOfLastMonthFromStatusDate = task.Result, cancellationToken);
        Task buildObligeePartiesAtEndOfLastYearFromStatusDateTask = ObligeePartiesDisplayer.CreateAsync(StaticTextKey.ObligeePartiesAtEndOfLastYearFromStatusDate, StaticTextKey.Debtors, StaticTextKey.Creditors, StaticTextProvider, model, m => m.ContactAccounts.Select(m => m.ValuesAtEndOfLastYearFromStatusDate).Where(v => v.IsDebtor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), m => m.ContactAccounts.Select(m => m.ValuesAtEndOfLastYearFromStatusDate).Where(v => v.IsCreditor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => obligeePartiesAtEndOfLastYearFromStatusDate = task.Result, cancellationToken);
        Task buildIncomeStatementTask = IncomeStatementDisplayer.CreateAsync(StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model, formatProvider, cancellationToken).ContinueWith(task => incomeStatement = task.Result, cancellationToken);
        Task buildBalanceSheetTask = FullBalanceSheetDisplayer.CreateAsync(StaticTextProvider, model, formatProvider, cancellationToken).ContinueWith(task => balanceSheet = task.Result, cancellationToken);
        await Task.WhenAll(buildStatusDateTask,
            buildBalanceBelowZeroTask,
            buildBackDatingTask,
            buildBalanceSheetAtStatusDateTask,
            buildBalanceSheetAtEndOfLastMonthFromStatusDateTask,
            buildBalanceSheetAtEndOfLastYearFromStatusDateTask,
            buildBudgetStatementForMonthOfStatusDateTask,
            buildBudgetStatementForLastMonthOfStatusDateTask,
            buildBudgetStatementForYearToDateOfStatusDateTask,
            buildBudgetStatementForLastYearOfStatusDateTask,
            buildObligeePartiesAtStatusDateTask,
            buildObligeePartiesAtEndOfLastMonthFromStatusDateTask,
            buildObligeePartiesAtEndOfLastYearFromStatusDateTask,
            buildIncomeStatementTask,
            buildBalanceSheetTask);

        return new AccountingTexts(
            model,
            statusDate!,
            balanceBelowZero!,
            backDating!,
            balanceSheetAtStatusDate!,
            balanceSheetAtEndOfLastMonthFromStatusDate!,
            balanceSheetAtEndOfLastYearFromStatusDate!,
            budgetStatementForMonthOfStatusDate!,
            budgetStatementForLastMonthOfStatusDate!,
            budgetStatementForYearToDateOfStatusDate!,
            budgetStatementForLastYearOfStatusDate!,
            obligeePartiesAtStatusDate!,
            obligeePartiesAtEndOfLastMonthFromStatusDate!,
            obligeePartiesAtEndOfLastYearFromStatusDate!,
            incomeStatement!,
            balanceSheet!,
            formatProvider);
    }

    private async Task<IValueDisplayer> BuildBalanceBelowZeroAsync(AccountingModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        IDictionary<BalanceBelowZeroType, string> staticTexts = new Dictionary<BalanceBelowZeroType, string>
        {
            {BalanceBelowZeroType.Debtors, await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Debtors, StaticTextKey.Debtors.DefaultArguments(), formatProvider, cancellationToken)},
            {BalanceBelowZeroType.Creditors, await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Creditors, StaticTextKey.Creditors.DefaultArguments(), formatProvider, cancellationToken)}
        };

        return await GetValueDisplayerAsync(StaticTextKey.BalanceBelowZero, StaticTextKey.BalanceBelowZero.DefaultArguments(), model.BalanceBelowZero, formatProvider, (value, _) => Resolve(value, staticTexts), cancellationToken);
    }

    private async Task<IValueDisplayer> BuildBackDatingAsync(AccountingModel model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string days = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Days, StaticTextKey.Days.DefaultArguments(), formatProvider, cancellationToken);
        string day = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Day, StaticTextKey.Day.DefaultArguments(), formatProvider, cancellationToken);

        return await GetValueDisplayerAsync(StaticTextKey.BackDating, StaticTextKey.BackDating.DefaultArguments(), model.BackDating, formatProvider, (value, fp) => $"{value.ToString(fp)} {(value == 1 ? day : days).ToLower()}", cancellationToken);
    }

    #endregion
}