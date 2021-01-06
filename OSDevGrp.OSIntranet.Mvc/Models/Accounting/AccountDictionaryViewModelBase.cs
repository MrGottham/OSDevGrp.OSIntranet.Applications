using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public abstract class AccountDictionaryViewModelBase<TAccountGroupViewModel, TAccountCollectionViewModel, TAccount> : IReadOnlyDictionary<TAccountGroupViewModel, TAccountCollectionViewModel> where TAccountGroupViewModel : AccountGroupViewModelBase where TAccountCollectionViewModel : AccountCollectionViewModelBase<TAccount> where TAccount : AccountIdentificationViewModel
    {
        public IReadOnlyDictionary<TAccountGroupViewModel, TAccountCollectionViewModel> Items { get; set; } = new ReadOnlyDictionary<TAccountGroupViewModel, TAccountCollectionViewModel>(new Dictionary<TAccountGroupViewModel, TAccountCollectionViewModel>());

        public int Count => Items.Count;

        public IEnumerable<TAccountGroupViewModel> Keys => Items.Keys;

        public IEnumerable<TAccountCollectionViewModel> Values => Items.Values;

        public TAccountCollectionViewModel this[TAccountGroupViewModel accountGroupViewModel] => Items[accountGroupViewModel];

        public bool ContainsKey(TAccountGroupViewModel accountGroupViewModel) => Items.ContainsKey(accountGroupViewModel);

        public bool TryGetValue(TAccountGroupViewModel accountGroupViewModel, out TAccountCollectionViewModel accountCollectionViewModel) => Items.TryGetValue(accountGroupViewModel, out accountCollectionViewModel);

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<KeyValuePair<TAccountGroupViewModel, TAccountCollectionViewModel>> IEnumerable<KeyValuePair<TAccountGroupViewModel, TAccountCollectionViewModel>>.GetEnumerator() => Items.GetEnumerator();
    }
}