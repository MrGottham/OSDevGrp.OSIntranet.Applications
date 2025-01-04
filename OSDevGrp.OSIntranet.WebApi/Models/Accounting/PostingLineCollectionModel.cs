using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingLineCollectionModel : IReadOnlyCollection<PostingLineModel>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public IReadOnlyCollection<PostingLineModel> Items { get; set; } = Array.Empty<PostingLineModel>();

        [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<PostingLineModel> IEnumerable<PostingLineModel>.GetEnumerator() => Items.GetEnumerator();
    }
}