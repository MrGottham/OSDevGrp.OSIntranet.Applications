using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class ApplyPostingLineCollectionViewModel : List<ApplyPostingLineViewModel>
    {
        internal void ApplyProtection()
        {
            foreach (ApplyPostingLineViewModel applyPostingLineViewModel in this)
            {
                applyPostingLineViewModel.ApplyProtection();
            }
        }
    }
}