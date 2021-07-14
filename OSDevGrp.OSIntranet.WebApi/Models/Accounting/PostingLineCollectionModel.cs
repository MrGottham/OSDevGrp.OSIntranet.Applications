using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingLineCollectionModel : IReadOnlyCollection<PostingLineModel>
    {
        [JsonIgnore]
        public IReadOnlyCollection<PostingLineModel> Items { get; set; } = Array.Empty<PostingLineModel>();

        [JsonIgnore]
        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<PostingLineModel> IEnumerable<PostingLineModel>.GetEnumerator() => Items.GetEnumerator();
    }
}