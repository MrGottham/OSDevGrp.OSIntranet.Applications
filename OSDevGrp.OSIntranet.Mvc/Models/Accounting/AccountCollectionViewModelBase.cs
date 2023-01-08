using System;
using System.Collections;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public abstract class AccountCollectionViewModelBase<TAccountViewModel> : IReadOnlyCollection<TAccountViewModel> where TAccountViewModel : AccountIdentificationViewModel
    {
        public IReadOnlyCollection<TAccountViewModel> Items { get; set; } = Array.Empty<TAccountViewModel>();

        public int Count => Items.Count;

        public bool IsProtected { get; set; }

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<TAccountViewModel> IEnumerable<TAccountViewModel>.GetEnumerator() => Items.GetEnumerator();
    }
}