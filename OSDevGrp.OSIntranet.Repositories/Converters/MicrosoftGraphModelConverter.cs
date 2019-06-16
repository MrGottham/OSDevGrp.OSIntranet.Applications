using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using OSDevGrp.OSIntranet.Repositories.Models.MicrosoftGraph;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class MicrosoftGraphModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<TokenModel, IRefreshableToken>()
                .ConvertUsing(tokenModel => tokenModel.ToDomain());

            mapperConfiguration.CreateMap<ContactModel, IContact>()
                .ConvertUsing(contactModel => contactModel.ToDomain(this));

            mapperConfiguration.CreateMap<ContactModel, ICompany>()
                .ConvertUsing(contactModel => contactModel.ToCompany(this));

            mapperConfiguration.CreateMap<ContactModel, IName>()
                .ConvertUsing(contactModel => contactModel.ToName());

            mapperConfiguration.CreateMap<ContactModel, ICompanyName>()
                .ConvertUsing(contactModel => contactModel.ToCompanyName());

            mapperConfiguration.CreateMap<ContactCollectionModel, IEnumerable<IContact>>()
                .ConvertUsing(contactCollectionModel => contactCollectionModel.ToDomain(this));

            mapperConfiguration.CreateMap<PhysicalAddressModel, IAddress>()
                .ConvertUsing(physicalAddressModel => physicalAddressModel.ToDomain());

            mapperConfiguration.CreateMap<EmailAddressModel, string>()
                .ConvertUsing(emailAddressModel => emailAddressModel.ToDomain());

            mapperConfiguration.CreateMap<IEnumerable<EmailAddressModel>, string>()
                .ConvertUsing(emailAddressModelCollection => emailAddressModelCollection.ToDomain(this));
        }

        #endregion
    }
}
