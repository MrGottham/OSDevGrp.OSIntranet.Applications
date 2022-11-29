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
    internal class AccountToCsvConverter : DomainObjectToCsvConverterBase<IAccount>, IAccountToCsvConverter
    {
        #region Private variables

        private readonly IStatusDateProvider _statusDateProvider;

        #endregion

        #region Constructor

        public AccountToCsvConverter(IStatusDateProvider statusDateProvider)
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
                "Kontogruppe, type",
                $"Saldo pr. {statusDate.ToDateText(FormatProvider)}",
                $"Kredit pr. {statusDate.ToDateText(FormatProvider)}",
                $"Saldo pr. {statusDate.GetEndDateOfLastMonth().ToDateText(FormatProvider)}",
                $"Kredit pr. {statusDate.GetEndDateOfLastMonth().ToDateText(FormatProvider)}",
                $"Saldo pr. {statusDate.GetEndDateOfLastYear().ToDateText(FormatProvider)}",
                $"Kredit pr. {statusDate.GetEndDateOfLastYear().ToDateText(FormatProvider)}"
            };

            return Task.FromResult<IEnumerable<string>>(columnNames);
        }

        public override Task<IEnumerable<string>> ConvertAsync(IAccount account)
        {
            NullGuard.NotNull(account, nameof(account));

            ResetColumns()
                .AddColumnValue(account.AccountNumber)
                .AddColumnValue(account.AccountName)
                .AddColumnValue(account.Description)
                .AddColumnValue(account.Note);

            AddColumnValues(account.AccountGroup);
            AddColumnValues(account.ValuesAtStatusDate);
            AddColumnValues(account.ValuesAtEndOfLastMonthFromStatusDate);
            AddColumnValues(account.ValuesAtEndOfLastYearFromStatusDate);

            return Task.FromResult(ColumnValues);
        }

        private void AddColumnValues(IAccountGroup accountGroup)
        {
            NullGuard.NotNull(accountGroup, nameof(accountGroup));

            AddColumnValue(accountGroup.Number)
                .AddColumnValue(accountGroup.Name)
                .AddColumnValue(accountGroup.AccountGroupType.Translate());
        }

        private void AddColumnValues(ICreditInfoValues creditInfoValues)
        {
            NullGuard.NotNull(creditInfoValues, nameof(creditInfoValues));

            AddColumnValue(creditInfoValues.Balance)
                .AddColumnValue(creditInfoValues.Credit);
        }

        #endregion
    }
}