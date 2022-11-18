using System;
using System.Collections.Generic;
using Markdown;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Logic;
using OSDevGrp.OSIntranet.BusinessLogic.Providers;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;

namespace OSDevGrp.OSIntranet.BusinessLogic.Accounting.Logic
{
    internal class ContactAccountStatementToMarkdownConverter : AccountToMarkdownConverterBase<IContactAccount>, IContactAccountStatementToMarkdownConverter
    {
        #region Constructor

        public ContactAccountStatementToMarkdownConverter(IStatusDateProvider statusDateProvider, IClaimResolver claimResolver)
            : base(statusDateProvider, claimResolver, DefaultFormatProvider.Create())
        {
        }

        #endregion

        #region Methods

        protected override IEnumerable<IMarkdownBlockElement> GetHeaderMarkdownCollection(IAccounting accounting, IContactAccount contactAccount, DateTime statusDate)
        {
            NullGuard.NotNull(accounting, nameof(accounting))
                .NotNull(contactAccount, nameof(contactAccount));

            IList<IMarkdownBlockElement> headerMarkdownCollection = new List<IMarkdownBlockElement>
            {
                GetHeaderMarkdown(GetTextMarkdown(contactAccount.AccountName), 1),
                GetParagraphMarkdown(GetTextMarkdown($"Kontonr.: {contactAccount.AccountNumber}"))
            };

            IMarkdownBlockElement mailAddressMarkdown = GetMailAddressMarkdown(contactAccount.MailAddress);
            if (mailAddressMarkdown != null)
            {
                headerMarkdownCollection.Add(mailAddressMarkdown);
            }

            IMarkdownBlockElement primaryPhoneMarkdown = GetPhoneMarkdown("Primær telefonnummer", contactAccount.PrimaryPhone);
            if (primaryPhoneMarkdown != null)
            {
                headerMarkdownCollection.Add(primaryPhoneMarkdown);
            }

            IMarkdownBlockElement secondaryPhoneMarkdown = GetPhoneMarkdown("Sekundær telefonnummer", contactAccount.SecondaryPhone);
            if (secondaryPhoneMarkdown != null)
            {
                headerMarkdownCollection.Add(secondaryPhoneMarkdown);
            }

            headerMarkdownCollection.Add(GetPaymentTermMarkdown(contactAccount.PaymentTerm));
            headerMarkdownCollection.Add(GetContactInfoValuesMarkdown(statusDate, contactAccount.ValuesAtStatusDate));
            headerMarkdownCollection.Add(GetContactInfoValuesMarkdown(statusDate.GetEndDateOfLastMonth(), contactAccount.ValuesAtEndOfLastMonthFromStatusDate));
            headerMarkdownCollection.Add(GetContactInfoValuesMarkdown(statusDate.GetEndDateOfLastYear(), contactAccount.ValuesAtEndOfLastYearFromStatusDate));

            return headerMarkdownCollection;
        }

        protected override IMarkdownBlockElement GetContentHeaderMarkdown(IAccounting accounting, IContactAccount account, DateTime statusDate) => GetHeaderMarkdown(GetTextMarkdown($"Kontoudtog pr. {statusDate.ToString("d. MMMM yyyy", FormatProvider)}"), 2);

        protected override IEnumerable<IPostingLine> GetPostingLineCollectionForContent(IPostingLineCollection postingLineCollection, DateTime statusDate)
        {
            NullGuard.NotNull(postingLineCollection, nameof(postingLineCollection));

            return postingLineCollection.Top(30);
        }

        protected override decimal GetBalance(IPostingLine postingLine)
        {
            NullGuard.NotNull(postingLine, nameof(postingLine));

            return postingLine.ContactAccountValuesAtPostingDate?.Balance ?? 0M;
        }

        private IMarkdownBlockElement GetContactInfoValuesMarkdown(DateTime statusDateForContactInfoValues, IContactInfoValues contactInfoValues)
        {
            NullGuard.NotNull(contactInfoValues, nameof(contactInfoValues));

            return GetParagraphMarkdown(GetTextMarkdown($"Saldo pr. {statusDateForContactInfoValues.ToString("d. MMMM yyyy", FormatProvider)}: {GetCurrencyMarkdown(contactInfoValues.Balance, false)}"));
        }

        private static IMarkdownBlockElement GetMailAddressMarkdown(string mailAddress)
        {
            return string.IsNullOrWhiteSpace(mailAddress) == false
                ? GetParagraphMarkdown(GetTextMarkdown($"Mailadresse: {GetLinkMarkdown(mailAddress, new Uri($"mailto:{mailAddress}"))}"))
                : null;
        }

        private static IMarkdownBlockElement GetPhoneMarkdown(string phoneText, string phoneNumber)
        {
            NullGuard.NotNullOrWhiteSpace(phoneText, nameof(phoneText));

            return string.IsNullOrWhiteSpace(phoneNumber) == false
                ? GetParagraphMarkdown(GetTextMarkdown($"{phoneText}: {GetLinkMarkdown(phoneNumber, new Uri($"tel:{phoneNumber}"))}"))
                : null;
        }

        private static IMarkdownBlockElement GetPaymentTermMarkdown(IPaymentTerm paymentTerm)
        {
            NullGuard.NotNull(paymentTerm, nameof(paymentTerm));

            return GetParagraphMarkdown(GetTextMarkdown($"Betalingsbetingelse: {paymentTerm.Name}"));
        }

        #endregion
    }
}