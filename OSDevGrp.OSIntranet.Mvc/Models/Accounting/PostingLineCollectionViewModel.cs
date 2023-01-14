using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PostingLineCollectionViewModel : List<PostingLineViewModel>
    {
        public PostingLineCollectionViewMode ViewMode { get; set; } = PostingLineCollectionViewMode.WithDebitAndCredit;

        public bool IsProtected { get; set; }
    }
}