using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;
using OSDevGrp.OSIntranet.WebApi.ClientApi;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.DynamicText;

internal class AccountingTextsBuilder : DynamicTextsBuilderBase<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>>, IAccountingTexts>, IAccountingTextsBuilder
{
    #region Private variables

    private readonly IPostingLineCollectionTextsBuilder _postingLineCollectionTextsBuilder;

    #endregion

    #region Constructor

    public AccountingTextsBuilder(IPostingLineCollectionTextsBuilder postingLineCollectionTextsBuilder, IStaticTextProvider staticTextProvider)
        : base(staticTextProvider)
    {
        _postingLineCollectionTextsBuilder = postingLineCollectionTextsBuilder;
    }

    #endregion

    #region Methods

    public Task<IAccountingTexts> BuildAsync(Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>> model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return BuildAsync(Tuple.Create<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>>(model.Item1, model.Item2, [model.Item1.LetterHead]), formatProvider, cancellationToken);
    }

    public async Task<IReadOnlyCollection<IAccountingTexts>> BuildAsync(IEnumerable<Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>>> models, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        return await Task.WhenAll(models.Select(model => BuildAsync(model, formatProvider, cancellationToken)));
    }

    public override async Task<IAccountingTexts> BuildAsync(Tuple<AccountingModel, IReadOnlyCollection<PostingLineModel>, IReadOnlyCollection<LetterHeadIdentificationModel>> model, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
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
        IChartOfAccountsDisplayer? chartOfAccounts = null;
        IChartOfBudgetAccountsDisplayer? chartOfBudgetAccounts = null;
        IChartOfContactAccountsDisplayer? chartOfContactAccounts = null;
        IPostingLineCollectionTexts? postingLineCollection = null;

        Task buildStatusDateTask = GetStatusDateAsync(model.Item1.StatusDate, "d. MMMM yyyy", formatProvider, cancellationToken).ContinueWith(task => statusDate = task.Result, cancellationToken);
        Task buildBalanceBelowZeroTask = BuildBalanceBelowZeroAsync(model.Item1, formatProvider, cancellationToken).ContinueWith(task => balanceBelowZero = task.Result, cancellationToken);
        Task buildBackDatingTask = BuildBackDatingAsync(model.Item1, formatProvider, cancellationToken).ContinueWith(task => backDating = task.Result, cancellationToken);
        Task buildBalanceSheetAtStatusDateTask = BalanceSheetDisplayer.CreateAsync(StaticTextKey.BalanceSheetAtStatusDate, StaticTextKey.Assets, StaticTextKey.Liabilities, StaticTextProvider, model.Item1, m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Assets).Select(m => m.ValuesAtStatusDate).Sum(v => (decimal)v.Balance), m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Liabilities).Select(m => m.ValuesAtStatusDate).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => balanceSheetAtStatusDate = task.Result, cancellationToken);
        Task buildBalanceSheetAtEndOfLastMonthFromStatusDateTask = BalanceSheetDisplayer.CreateAsync(StaticTextKey.BalanceSheetAtEndOfLastMonthFromStatusDate, StaticTextKey.Assets, StaticTextKey.Liabilities, StaticTextProvider, model.Item1, m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Assets).Select(m => m.ValuesAtEndOfLastMonthFromStatusDate).Sum(v => (decimal)v.Balance), m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Liabilities).Select(m => m.ValuesAtEndOfLastMonthFromStatusDate).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => balanceSheetAtEndOfLastMonthFromStatusDate = task.Result, cancellationToken);
        Task buildBalanceSheetAtEndOfLastYearFromStatusDateTask = BalanceSheetDisplayer.CreateAsync(StaticTextKey.BalanceSheetAtEndOfLastYearFromStatusDate, StaticTextKey.Assets, StaticTextKey.Liabilities, StaticTextProvider, model.Item1, m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Assets).Select(m => m.ValuesAtEndOfLastYearFromStatusDate).Sum(v => (decimal)v.Balance), m => m.Accounts.Where(m => m.AccountGroup.AccountGroupType == AccountGroupType.Liabilities).Select(m => m.ValuesAtEndOfLastYearFromStatusDate).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => balanceSheetAtEndOfLastYearFromStatusDate = task.Result, cancellationToken);
        Task buildBudgetStatementForMonthOfStatusDateTask = BudgetStatementDisplayer.CreateAsync(StaticTextKey.BudgetStatementForMonthOfStatusDate, StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model.Item1, m => m.BudgetAccounts.Select(m => m.ValuesForMonthOfStatusDate).Sum(v => (decimal) v.Budget), m => m.BudgetAccounts.Select(m => m.ValuesForMonthOfStatusDate).Sum(v => (decimal) v.Posted), m => m.BudgetAccounts.Select(m => m.ValuesForMonthOfStatusDate).Sum(v => (decimal) v.Available), formatProvider, cancellationToken).ContinueWith(task => budgetStatementForMonthOfStatusDate = task.Result, cancellationToken);
        Task buildBudgetStatementForLastMonthOfStatusDateTask = BudgetStatementDisplayer.CreateAsync(StaticTextKey.BudgetStatementForLastMonthOfStatusDate, StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model.Item1, m => m.BudgetAccounts.Select(m => m.ValuesForLastMonthOfStatusDate).Sum(v => (decimal) v.Budget), m => m.BudgetAccounts.Select(m => m.ValuesForLastMonthOfStatusDate).Sum(v => (decimal) v.Posted), m => m.BudgetAccounts.Select(m => m.ValuesForLastMonthOfStatusDate).Sum(v => (decimal) v.Available), formatProvider, cancellationToken).ContinueWith(task => budgetStatementForLastMonthOfStatusDate = task.Result, cancellationToken);
        Task buildBudgetStatementForYearToDateOfStatusDateTask = BudgetStatementDisplayer.CreateAsync(StaticTextKey.BudgetStatementForYearToDateOfStatusDate, StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model.Item1, m => m.BudgetAccounts.Select(m => m.ValuesForYearToDateOfStatusDate).Sum(v => (decimal) v.Budget), m => m.BudgetAccounts.Select(m => m.ValuesForYearToDateOfStatusDate).Sum(v => (decimal) v.Posted), m => m.BudgetAccounts.Select(m => m.ValuesForYearToDateOfStatusDate).Sum(v => (decimal) v.Available), formatProvider, cancellationToken).ContinueWith(task => budgetStatementForYearToDateOfStatusDate = task.Result, cancellationToken);
        Task buildBudgetStatementForLastYearOfStatusDateTask = BudgetStatementDisplayer.CreateAsync(StaticTextKey.BudgetStatementForLastYearOfStatusDate, StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model.Item1, m => m.BudgetAccounts.Select(m => m.ValuesForLastYearOfStatusDate).Sum(v => (decimal) v.Budget), m => m.BudgetAccounts.Select(m => m.ValuesForLastYearOfStatusDate).Sum(v => (decimal) v.Posted), m => m.BudgetAccounts.Select(m => m.ValuesForLastYearOfStatusDate).Sum(v => (decimal) v.Available), formatProvider, cancellationToken).ContinueWith(task => budgetStatementForLastYearOfStatusDate = task.Result, cancellationToken);
        Task buildObligeePartiesAtStatusDateTask = ObligeePartiesDisplayer.CreateAsync( StaticTextKey.ObligeePartiesAtStatusDate, StaticTextKey.Debtors, StaticTextKey.Creditors, StaticTextProvider, model.Item1, m => m.ContactAccounts.Select(m => m.ValuesAtStatusDate).Where(v => v.IsDebtor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), m => m.ContactAccounts.Select(m => m.ValuesAtStatusDate).Where(v => v.IsCreditor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => obligeePartiesAtStatusDate = task.Result, cancellationToken);
        Task buildObligeePartiesAtEndOfLastMonthFromStatusDateTask = ObligeePartiesDisplayer.CreateAsync(StaticTextKey.ObligeePartiesAtEndOfLastMonthFromStatusDate, StaticTextKey.Debtors, StaticTextKey.Creditors, StaticTextProvider, model.Item1, m => m.ContactAccounts.Select(m => m.ValuesAtEndOfLastMonthFromStatusDate).Where(v => v.IsDebtor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), m => m.ContactAccounts.Select(m => m.ValuesAtEndOfLastMonthFromStatusDate).Where(v => v.IsCreditor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => obligeePartiesAtEndOfLastMonthFromStatusDate = task.Result, cancellationToken);
        Task buildObligeePartiesAtEndOfLastYearFromStatusDateTask = ObligeePartiesDisplayer.CreateAsync(StaticTextKey.ObligeePartiesAtEndOfLastYearFromStatusDate, StaticTextKey.Debtors, StaticTextKey.Creditors, StaticTextProvider, model.Item1, m => m.ContactAccounts.Select(m => m.ValuesAtEndOfLastYearFromStatusDate).Where(v => v.IsDebtor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), m => m.ContactAccounts.Select(m => m.ValuesAtEndOfLastYearFromStatusDate).Where(v => v.IsCreditor(m.BalanceBelowZero)).Sum(v => (decimal)v.Balance), formatProvider, cancellationToken).ContinueWith(task => obligeePartiesAtEndOfLastYearFromStatusDate = task.Result, cancellationToken);
        Task buildIncomeStatementTask = IncomeStatementDisplayer.CreateAsync(StaticTextKey.Budget, StaticTextKey.Result, StaticTextKey.Available, StaticTextProvider, model.Item1, formatProvider, cancellationToken).ContinueWith(task => incomeStatement = task.Result, cancellationToken);
        Task buildBalanceSheetTask = FullBalanceSheetDisplayer.CreateAsync(StaticTextProvider, model.Item1, formatProvider, cancellationToken).ContinueWith(task => balanceSheet = task.Result, cancellationToken);
        Task buildChartOfAccountsTask = ChartOfAccountsDisplayer.CreateAsync(StaticTextProvider, model.Item1, formatProvider, cancellationToken).ContinueWith(task => chartOfAccounts = task.Result, cancellationToken);
        Task buildChartOfBudgetAccountsTask = ChartOfBudgetAccountsDisplayer.CreateAsync(StaticTextProvider, model.Item1, formatProvider, cancellationToken).ContinueWith(task => chartOfBudgetAccounts = task.Result, cancellationToken);
        Task buildChartOfContactAccountsTask = ChartOfContactAccountsDisplayer.CreateAsync(StaticTextProvider, model.Item1, formatProvider, cancellationToken).ContinueWith(task => chartOfContactAccounts = task.Result);
        Task buildPostingLineCollectionTask = _postingLineCollectionTextsBuilder.BuildAsync(model.Item2, formatProvider, cancellationToken).ContinueWith(task => postingLineCollection = task.Result, cancellationToken);
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
            buildBalanceSheetTask,
            buildChartOfAccountsTask,
            buildChartOfBudgetAccountsTask,
            buildChartOfContactAccountsTask,
            buildPostingLineCollectionTask);

        return new AccountingTexts(
            model.Item1,
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
            chartOfAccounts!,
            chartOfBudgetAccounts!,
            chartOfContactAccounts!,
            postingLineCollection!,
            formatProvider);
    }

    private async Task<IValueDisplayer> BuildBalanceBelowZeroAsync(AccountingModel accountingModel, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        IDictionary<BalanceBelowZeroType, string> staticTexts = new Dictionary<BalanceBelowZeroType, string>
        {
            {BalanceBelowZeroType.Debtors, await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Debtors, StaticTextKey.Debtors.DefaultArguments(), formatProvider, cancellationToken)},
            {BalanceBelowZeroType.Creditors, await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Creditors, StaticTextKey.Creditors.DefaultArguments(), formatProvider, cancellationToken)}
        };

        return await GetValueDisplayerAsync(StaticTextKey.BalanceBelowZero, StaticTextKey.BalanceBelowZero.DefaultArguments(), accountingModel.BalanceBelowZero, formatProvider, (value, _) => Resolve(value, staticTexts), cancellationToken);
    }

    private async Task<IValueDisplayer> BuildBackDatingAsync(AccountingModel accountingModel, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        string days = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Days, StaticTextKey.Days.DefaultArguments(), formatProvider, cancellationToken);
        string day = await StaticTextProvider.GetStaticTextAsync(StaticTextKey.Day, StaticTextKey.Day.DefaultArguments(), formatProvider, cancellationToken);

        return await GetValueDisplayerAsync(StaticTextKey.BackDating, StaticTextKey.BackDating.DefaultArguments(), accountingModel.BackDating, formatProvider, (value, fp) => $"{value.ToString(fp)} {(value == 1 ? day : days).ToLower()}", cancellationToken);
    }

    #endregion
}