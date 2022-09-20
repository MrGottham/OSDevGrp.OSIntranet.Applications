using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal class BudgetAccountToCsvConverter : DomainObjectToCsvConverterBase<IBudgetAccount>, IBudgetAccountToCsvConverter
    {
        #region Private variables

        private readonly IStatusDateProvider _statusDateProvider;

        #endregion

        #region Constructor

        public BudgetAccountToCsvConverter(IStatusDateProvider statusDateProvider) 
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
                "Kontonummer",
                "Kontonavn",
                "Beskrivelse",
                "Note",
                "Kontogruppe, nr.",
                "Kontogruppe, navn",
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

        public override Task<IEnumerable<string>> ConvertAsync(IBudgetAccount budgetAccount)
        {
            NullGuard.NotNull(budgetAccount, nameof(budgetAccount));

            ResetColumns()
                .AddColumnValue(budgetAccount.AccountNumber)
                .AddColumnValue(budgetAccount.AccountName)
                .AddColumnValue(budgetAccount.Description)
                .AddColumnValue(budgetAccount.Note);

            AddColumnValues(budgetAccount.BudgetAccountGroup);
            AddColumnValues(budgetAccount.ValuesForMonthOfStatusDate);
            AddColumnValues(budgetAccount.ValuesForLastMonthOfStatusDate);
            AddColumnValues(budgetAccount.ValuesForYearToDateOfStatusDate);
            AddColumnValues(budgetAccount.ValuesForLastYearOfStatusDate);

            return Task.FromResult(ColumnValues);
        }

        private void AddColumnValues(IBudgetAccountGroup budgetAccountGroup)
        {
            NullGuard.NotNull(budgetAccountGroup, nameof(budgetAccountGroup));

            AddColumnValue(budgetAccountGroup.Number)
                .AddColumnValue(budgetAccountGroup.Name);
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