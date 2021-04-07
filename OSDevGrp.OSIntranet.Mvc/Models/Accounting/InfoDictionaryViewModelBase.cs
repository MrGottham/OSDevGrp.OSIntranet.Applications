using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Mvc.Models.Accounting
{
    public abstract class InfoDictionaryViewModelBase<TInfoCollectionModel, TInfoModel> : IReadOnlyDictionary<short, TInfoCollectionModel> where TInfoCollectionModel : InfoCollectionViewModelBase<TInfoModel> where TInfoModel : InfoViewModelBase
    {
        public IDictionary<short, TInfoCollectionModel> Items { get; set; } = new ConcurrentDictionary<short, TInfoCollectionModel>();

        public int Count => Items.Count;

        public IEnumerable<short> Keys => Items.Keys;

        public IEnumerable<TInfoCollectionModel> Values => Items.Values;

        public TInfoCollectionModel this[short year] => Items[year];

        public bool ContainsKey(short year) => Items.ContainsKey(year);

        public bool TryGetValue(short year, out TInfoCollectionModel infoCollectionModel) => Items.TryGetValue(year, out infoCollectionModel);

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<KeyValuePair<short, TInfoCollectionModel>> IEnumerable<KeyValuePair<short, TInfoCollectionModel>>.GetEnumerator() => Items.GetEnumerator();
    }
}