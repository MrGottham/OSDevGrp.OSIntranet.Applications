using System;
using System.Collections;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PostingWarningCollectionViewModel : IReadOnlyCollection<PostingWarningViewModel>
    {
        public IReadOnlyCollection<PostingWarningViewModel> Items { get; set; } = Array.Empty<PostingWarningViewModel>();

        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<PostingWarningViewModel> IEnumerable<PostingWarningViewModel>.GetEnumerator() => Items.GetEnumerator();
    }
}