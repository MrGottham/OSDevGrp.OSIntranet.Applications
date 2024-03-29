﻿using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingLine : IAuditable, ICalculable<IPostingLine>
    {
        Guid Identifier { get; }

        IAccounting Accounting { get; }

        DateTime PostingDate { get; }

        string Reference { get; }

        IAccount Account { get; }

        ICreditInfoValues AccountValuesAtPostingDate { get; }

        string Details { get; }

        IBudgetAccount BudgetAccount { get; }

        IBudgetInfoValues BudgetAccountValuesAtPostingDate { get; }

        decimal Debit { get; }

        decimal Credit { get; }

        decimal PostingValue { get; }

        IContactAccount ContactAccount { get; }

        IContactInfoValues ContactAccountValuesAtPostingDate { get; }

        int SortOrder { get; }

        Task<IPostingLine> ApplyCalculationAsync(IAccounting calculatedAccounting);

        Task<IPostingLine> ApplyCalculationAsync(IAccount calculatedAccount);

        Task<IPostingLine> ApplyCalculationAsync(IBudgetAccount calculatedBudgetAccount);

        Task<IPostingLine> ApplyCalculationAsync(IContactAccount calculatedContactAccount);
    }
}