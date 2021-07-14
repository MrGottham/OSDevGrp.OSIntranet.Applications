using System.Collections;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public class PostingLineCollectionViewModel : IReadOnlyCollection<PostingLineViewModel>
    {
        public IList<PostingLineViewModel> Items { get; set; } = new List<PostingLineViewModel>();

        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<PostingLineViewModel> IEnumerable<PostingLineViewModel>.GetEnumerator() => Items.GetEnumerator();
    }
}