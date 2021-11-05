using System;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PostingWarningViewModel
    {
        public Guid Identifier { get; set; }

        public PostingWarningReason Reason { get; set; }

        public AccountIdentificationViewModel Account { get; set; }

        public decimal Amount { get; set; }

        public PostingLineViewModel PostingLine { get; set; }
    }
}