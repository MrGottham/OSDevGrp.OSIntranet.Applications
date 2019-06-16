﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph
{
    [DataContract]
    internal class EmailAddressModel
    {
        [DataMember(Name = "address", IsRequired = false)]
        public string Address { get; set; }

        [DataMember(Name = "name", IsRequired = false)]
        public string Name { get; set; }
    }

    internal static class EmailAddressModelExtensions
    {
        internal static string ToDomain(this EmailAddressModel emailAddressModel)
        {
            NullGuard.NotNull(emailAddressModel, nameof(emailAddressModel));

            return string.IsNullOrWhiteSpace(emailAddressModel.Address) ? null : emailAddressModel.Address;
        }

        internal static string ToDomain(this IEnumerable<EmailAddressModel> emailAddressModelCollection, IConverter microsoftGraphModelConverter)
        {
            NullGuard.NotNull(emailAddressModelCollection, nameof(emailAddressModelCollection))
                .NotNull(microsoftGraphModelConverter, nameof(microsoftGraphModelConverter));

            EmailAddressModel emailAddressModel = emailAddressModelCollection.FirstOrDefault();
            return emailAddressModel == null ? null : microsoftGraphModelConverter.Convert<EmailAddressModel, string>(emailAddressModel);
        }
    }
}
