using Newtonsoft.Json;

namespace OSDevGrp.OSIntranet.WebApi.Models.Accounting
{
    public class ContactAccountModel : AccountCoreDataModel
    {
        [JsonProperty(Required = Required.Default)]
        public string MailAddress { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string PrimaryPhone { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string SecondaryPhone { get; set; }

        [JsonProperty(Required = Required.Always)]
        public PaymentTermModel PaymentTerm { get; set; }
    }
}