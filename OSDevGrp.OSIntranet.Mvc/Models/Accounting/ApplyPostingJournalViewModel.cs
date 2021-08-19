namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ApplyPostingJournalViewModel
    {
        public int AccountingNumber { get; set; }

        public ApplyPostingLineCollectionViewModel ApplyPostingLines { get; set; }
    }
}