using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ApplyPostingJournalResultViewModel
    {
        [JsonIgnore]
        public bool IsProtected { get; private set; }

        public PostingLineCollectionViewModel PostingLines { get; set; }

        public PostingWarningCollectionViewModel PostingWarnings { get; set; }

        internal void ApplyProtection()
        {
            IsProtected = true;
            PostingWarnings?.ApplyProtection();
        }
    }
}