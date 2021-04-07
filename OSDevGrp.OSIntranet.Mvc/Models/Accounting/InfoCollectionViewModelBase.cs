using System.Collections;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public abstract class InfoCollectionViewModelBase<TInfoViewModel> : IReadOnlyCollection<TInfoViewModel> where TInfoViewModel : InfoViewModelBase
    {
        public IList<TInfoViewModel> Items { get; set; } = new List<TInfoViewModel>();

        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<TInfoViewModel> IEnumerable<TInfoViewModel>.GetEnumerator() => Items.GetEnumerator();
    }
}