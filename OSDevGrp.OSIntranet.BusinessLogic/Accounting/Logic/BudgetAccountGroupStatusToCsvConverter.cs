using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal class BudgetAccountGroupStatusToCsvConverter : DomainObjectToCsvConverterBase<IBudgetAccountGroupStatus>, IBudgetAccountGroupStatusToCsvConverter
    {
        #region Private variables

        private readonly IStatusDateProvider _statusDateProvider;

        #endregion

        #region Constructor

        public BudgetAccountGroupStatusToCsvConverter(IStatusDateProvider statusDateProvider)
            : base(DefaultFormatProvider.Create())
        {
            NullGuard.NotNull(statusDateProvider, nameof(statusDateProvider));

            _statusDateProvider = statusDateProvider;
        }

        #endregion

        #region Methods

        public override Task<IEnumerable<string>> GetColumnNamesAsync()
        {
            DateTime statusDate = _statusDateProvider.GetStatusDate();

            string[] columnNames =
            {
                "Nummer",
                "Navn",
                $"Budget pr. {statusDate.ToDateText(FormatProvider)}",
                $"Bogført pr. {statusDate.ToDateText(FormatProvider)}",
                $"Budget {statusDate.GetEndDateOfLastMonth().ToMonthYearText(FormatProvider)}",
                $"Bogført {statusDate.GetEndDateOfLastMonth().ToMonthYearText(FormatProvider)}",
                $"Budget {statusDate.GetFirstDateOfYear().ToDateText(FormatProvider)} til {statusDate.ToDateText(FormatProvider)}",
                $"Bogført {statusDate.GetFirstDateOfYear().ToDateText(FormatProvider)} til {statusDate.ToDateText(FormatProvider)}",
                $"Budget {statusDate.GetEndDateOfLastYear().ToYearText(FormatProvider)}",
                $"Bogført {statusDate.GetEndDateOfLastYear().ToYearText(FormatProvider)}"
            };

            return Task.FromResult<IEnumerable<string>>(columnNames);
        }

        public override Task<IEnumerable<string>> ConvertAsync(IBudgetAccountGroupStatus budgetAccountGroupStatus)
        {
            NullGuard.NotNull(budgetAccountGroupStatus, nameof(budgetAccountGroupStatus));

            ResetColumns()
                .AddColumnValue(budgetAccountGroupStatus.Number)
                .AddColumnValue(budgetAccountGroupStatus.Name);

            AddColumnValues(budgetAccountGroupStatus.ValuesForMonthOfStatusDate);
            AddColumnValues(budgetAccountGroupStatus.ValuesForLastMonthOfStatusDate);
            AddColumnValues(budgetAccountGroupStatus.ValuesForYearToDateOfStatusDate);
            AddColumnValues(budgetAccountGroupStatus.ValuesForLastYearOfStatusDate);

            return Task.FromResult(ColumnValues);
        }

        private void AddColumnValues(IBudgetInfoValues budgetInfoValues)
        {
            NullGuard.NotNull(budgetInfoValues, nameof(budgetInfoValues));

            AddColumnValue(budgetInfoValues.Budget)
                .AddColumnValue(budgetInfoValues.Posted);
        }

        #endregion
    }
}