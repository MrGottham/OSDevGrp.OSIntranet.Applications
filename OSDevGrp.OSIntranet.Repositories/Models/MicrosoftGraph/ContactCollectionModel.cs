using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph
{
    [DataContract]
    internal class ContactCollectionModel
    {
        [DataMember(Name = "@odata.context", IsRequired = true, EmitDefaultValue = true)]
        public string Context { get; set; }

        [DataMember(Name = "@odata.nextLink", IsRequired = false, EmitDefaultValue = false)]
        public string NextLink { get; set; }

        [DataMember(Name = "value", IsRequired = true, EmitDefaultValue = true)]
        public List<ContactModel> Collection { get; set; }
    }

    internal static class ContactCollectionModelExtensions
    {
        internal static IEnumerable<IContact> ToDomain(this ContactCollectionModel contactCollectionModel, IConverter microsoftGraphModelConverter)
        {
            NullGuard.NotNull(contactCollectionModel, nameof(contactCollectionModel))
                .NotNull(microsoftGraphModelConverter, nameof(microsoftGraphModelConverter));

            return contactCollectionModel.Collection.Select(microsoftGraphModelConverter.Convert<ContactModel, IContact>).ToList();
        }
    }
}
