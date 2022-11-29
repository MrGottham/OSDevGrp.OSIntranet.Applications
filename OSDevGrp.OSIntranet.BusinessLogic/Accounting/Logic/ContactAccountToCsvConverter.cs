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
    internal class ContactAccountToCsvConverter : DomainObjectToCsvConverterBase<IContactAccount>, IContactAccountToCsvConverter
    {
        #region Private variables

        private readonly IStatusDateProvider _statusDateProvider;

        #endregion

        #region Constructor

        public ContactAccountToCsvConverter(IStatusDateProvider statusDateProvider)
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
                "Mailadresse",
                "Primær telefonnummer",
                "Sekundær telefonnummer",
                "Beskrivelse",
                "Note",
                "Betalingsbetingelse, nr.",
                "Betalingsbetingelse, navn",
                $"Saldo pr. {statusDate.ToDateText(FormatProvider)}",
                $"Saldo pr. {statusDate.GetEndDateOfLastMonth().ToDateText(FormatProvider)}",
                $"Saldo pr. {statusDate.GetEndDateOfLastYear().ToDateText(FormatProvider)}"
            };

            return Task.FromResult<IEnumerable<string>>(columnNames);
        }

        public override Task<IEnumerable<string>> ConvertAsync(IContactAccount contactAccount)
        {
            NullGuard.NotNull(contactAccount, nameof(contactAccount));

            ResetColumns()
                .AddColumnValue(contactAccount.AccountNumber)
                .AddColumnValue(contactAccount.AccountName)
                .AddColumnValue(contactAccount.MailAddress)
                .AddColumnValue(contactAccount.PrimaryPhone)
                .AddColumnValue(contactAccount.SecondaryPhone)
                .AddColumnValue(contactAccount.Description)
                .AddColumnValue(contactAccount.Note);

            AddColumnValues(contactAccount.PaymentTerm);
            AddColumnValues(contactAccount.ValuesAtStatusDate);
            AddColumnValues(contactAccount.ValuesAtEndOfLastMonthFromStatusDate);
            AddColumnValues(contactAccount.ValuesAtEndOfLastYearFromStatusDate);

            return Task.FromResult(ColumnValues);
        }

        private void AddColumnValues(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            AddColumnValue(paymentTerm.Number)
                .AddColumnValue(paymentTerm.Name);
        }

        private void AddColumnValues(IContactInfoValues contactInfoValues)
        {
            NullGuard.NotNull(contactInfoValues, nameof(contactInfoValues));

            AddColumnValue(contactInfoValues.Balance);
        }

        #endregion
    }
}