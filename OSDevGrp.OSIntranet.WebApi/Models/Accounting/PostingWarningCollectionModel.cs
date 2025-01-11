using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingWarningCollectionModel : IReadOnlyCollection<PostingWarningModel>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public IReadOnlyCollection<PostingWarningModel> Items { get; set; } = Array.Empty<PostingWarningModel>();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<PostingWarningModel> IEnumerable<PostingWarningModel>.GetEnumerator() => Items.GetEnumerator();
    }
}