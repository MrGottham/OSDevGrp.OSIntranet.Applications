using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public abstract class InfoCollectionModelBase<TInfoModel> : IReadOnlyCollection<TInfoModel> where TInfoModel : InfoModelBase
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)] 
        public IReadOnlyCollection<TInfoModel> Items { get; set; } = Array.Empty<TInfoModel>();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<TInfoModel> IEnumerable<TInfoModel>.GetEnumerator() => Items.GetEnumerator();
    }
}