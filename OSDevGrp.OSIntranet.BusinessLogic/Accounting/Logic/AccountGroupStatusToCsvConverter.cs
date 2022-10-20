using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Converters;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal class AccountGroupStatusToCsvConverter : DomainObjectToCsvConverterBase<IAccountGroupStatus>, IAccountGroupStatusToCsvConverter
    {
        #region Private variables

        private readonly IStatusDateProvider _statusDateProvider;

        #endregion

        #region Constructor

        public AccountGroupStatusToCsvConverter(IStatusDateProvider statusDateProvider)
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
                "Type",
                $"{AccountGroupType.Assets.Translate()} pr. {statusDate.ToDateText(FormatProvider)}",
                $"{AccountGroupType.Liabilities.Translate()} pr. {statusDate.ToDateText(FormatProvider)}",
                $"{AccountGroupType.Assets.Translate()} pr. {statusDate.GetEndDateOfLastMonth().ToDateText(FormatProvider)}",
                $"{AccountGroupType.Liabilities.Translate()} pr. {statusDate.GetEndDateOfLastMonth().ToDateText(FormatProvider)}",
                $"{AccountGroupType.Assets.Translate()} pr. {statusDate.GetEndDateOfLastYear().ToDateText(FormatProvider)}",
                $"{AccountGroupType.Liabilities.Translate()} pr. {statusDate.GetEndDateOfLastYear().ToDateText(FormatProvider)}"
            };

            return Task.FromResult<IEnumerable<string>>(columnNames);
        }

        public override Task<IEnumerable<string>> ConvertAsync(IAccountGroupStatus accountGroupStatus)
        {
            NullGuard.NotNull(accountGroupStatus, nameof(accountGroupStatus));

            ResetColumns()
                .AddColumnValue(accountGroupStatus.Number)
                .AddColumnValue(accountGroupStatus.Name)
                .AddColumnValue(accountGroupStatus.AccountGroupType.Translate());

            AddColumnValues(accountGroupStatus.ValuesAtStatusDate);
            AddColumnValues(accountGroupStatus.ValuesAtEndOfLastMonthFromStatusDate);
            AddColumnValues(accountGroupStatus.ValuesAtEndOfLastYearFromStatusDate);

            return Task.FromResult(ColumnValues);
        }

        private void AddColumnValues(IAccountCollectionValues accountCollectionValues)
        {
            NullGuard.NotNull(accountCollectionValues, nameof(accountCollectionValues));

            AddColumnValue(accountCollectionValues.Assets)
                .AddColumnValue(accountCollectionValues.Liabilities);
        }

        #endregion
    }
}