using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class PostingWarningCollectionModel : IReadOnlyCollection<PostingWarningModel>
    {
        [JsonIgnore]
        public IReadOnlyCollection<PostingWarningModel> Items { get; set; } = Array.Empty<PostingWarningModel>();

        [JsonIgnore]
        public int Count => Items.Count;

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        IEnumerator<PostingWarningModel> IEnumerable<PostingWarningModel>.GetEnumerator() => Items.GetEnumerator();
    }
}