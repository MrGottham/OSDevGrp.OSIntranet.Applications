using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public abstract class AccountCollectionModelBase<TAccountModel> : IReadOnlyCollection<TAccountModel> where TAccountModel : AccountIdentificationModel
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public IReadOnlyCollection<TAccountModel> Items { get; set; } = Array.Empty<TAccountModel>();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<TAccountModel> IEnumerable<TAccountModel>.GetEnumerator() => Items.GetEnumerator();
    }
}