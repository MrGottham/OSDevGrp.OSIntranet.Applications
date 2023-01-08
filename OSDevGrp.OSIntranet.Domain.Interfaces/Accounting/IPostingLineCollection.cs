using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingLineCollection : IEnumerable<IPostingLine>, ICalculable<IPostingLineCollection>, IProtectable
    {
        void Add(IPostingLine postingLine);

        void Add(IEnumerable<IPostingLine> postingLineCollection);

        IPostingLineCollection Between(DateTime fromDate, DateTime toDate);

        IPostingLineCollection Ordered();

        IPostingLineCollection Top(int numberOfPostingLines);

        decimal CalculatePostingValue(DateTime fromDate, DateTime toDate, int? sortOrder = null);

        Task<IPostingLineCollection> ApplyCalculationAsync(IAccounting calculatedAccounting);

        Task<IPostingLineCollection> ApplyCalculationAsync(IAccount calculatedAccount);

        Task<IPostingLineCollection> ApplyCalculationAsync(IBudgetAccount calculatedBudgetAccount);

        Task<IPostingLineCollection> ApplyCalculationAsync(IContactAccount calculatedContactAccount);
    }
}