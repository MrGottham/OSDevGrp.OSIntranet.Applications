using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public abstract class InfoCollectionModelBase<TInfoModel> : IReadOnlyCollection<TInfoModel> where TInfoModel : InfoModelBase
    {
        [JsonIgnore] 
        public IReadOnlyCollection<TInfoModel> Items { get; set; } = Array.Empty<TInfoModel>();

        [JsonIgnore]
        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<TInfoModel> IEnumerable<TInfoModel>.GetEnumerator() => Items.GetEnumerator();
    }
}