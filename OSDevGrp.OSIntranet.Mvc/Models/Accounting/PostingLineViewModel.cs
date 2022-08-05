using System;
using System.ComponentModel.DataAnnotations;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PostingLineViewModel : AuditableViewModelBase
    {
        public Guid Identifier { get; set; }

        [DataType(DataType.Text)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime PostingDate { get; set; }

        public string Reference { get; set; }

        public AccountIdentificationViewModel Account { get; set; }

        public CreditInfoValuesViewModel AccountValuesAtPostingDate { get; set; }

        public string Details { get; set; }

        public AccountIdentificationViewModel BudgetAccount { get; set; }

        public BudgetInfoValuesViewModel BudgetAccountValuesAtPostingDate { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }

        public decimal PostingValue => Debit ?? 0M - Credit ?? 0M;

        public AccountIdentificationViewModel ContactAccount { get; set; }

        public BalanceInfoValuesViewModel ContactAccountValuesAtPostingDate { get; set; }

        public int SortOrder { get; set; }
    }
}