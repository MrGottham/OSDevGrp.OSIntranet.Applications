using System;
using Newtonsoft.Json;
using OSDevGrp.OSIntranet.WebApi.Models.Common;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class AccountingModel : AccountingIdentificationModel
    {
        [JsonProperty(Required = Required.Always)]
        public LetterHeadIdentificationModel LetterHead { get; set; }

        [JsonProperty(Required = Required.Always)]
        public BalanceBelowZeroType BalanceBelowZero { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int BackDating { get; set; }

        [JsonProperty(Required = Required.Always)]
        public DateTime StatusDate { get; set; }
    }
}