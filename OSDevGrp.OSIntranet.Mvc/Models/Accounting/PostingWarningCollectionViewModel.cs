using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PostingWarningCollectionViewModel : List<PostingWarningViewModel>
    {
        internal void ApplyProtection()
        {
            foreach (PostingWarningViewModel postingWarningViewModel in this)
            {
                postingWarningViewModel.ApplyProtection();
            }
        }
    }
}