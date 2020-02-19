using System.Collections.Generic;
using System.Linq;
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

            mapperConfiguration.CreateMap<IContact, ContactModel>()
                .ForMember(dest => dest.Identifier, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.ExternalIdentifier) == false);
                    opt.MapFrom(src => src.ExternalIdentifier);
                })
                .ForMember(dest => dest.DisplayName, opt =>
                {
                    opt.Condition(src => src.Name != null && string.IsNullOrWhiteSpace(src.Name.DisplayName) == false);
                    opt.MapFrom(src => src.Name.DisplayName);
                })
                .ForMember(dest => dest.GivenName, opt =>
                {
                    opt.Condition(src => src.Name is IPersonName || src.Name is ICompanyName);
                    opt.MapFrom(src => src.Name is IPersonName ? string.IsNullOrWhiteSpace((src.Name as IPersonName).GivenName) ? string.Empty : (src.Name as IPersonName).GivenName : string.Empty);
                })
                .ForMember(dest => dest.MiddleName, opt =>
                {
                    opt.PreCondition(src => src.Name is IPersonName);
                    opt.Condition(src => src.Name is IPersonName personName && string.IsNullOrWhiteSpace(personName.MiddleName) == false);
                    opt.MapFrom(src => ((IPersonName) src.Name).MiddleName);
                })
                .ForMember(dest => dest.Surname, opt =>
                {
                    opt.Condition(src => src.Name is IPersonName || src.Name is ICompanyName);
                    opt.MapFrom(src => src.Name is IPersonName ? string.IsNullOrWhiteSpace(((IPersonName) src.Name).Surname) ? string.Empty : ((IPersonName) src.Name).Surname : string.IsNullOrWhiteSpace(((ICompanyName) src.Name).FullName) ? string.Empty : ((ICompanyName) src.Name).FullName);
                })
                .ForMember(dest => dest.HomeAddress, opt =>
                {
                    opt.Condition(src => src.Address != null && (string.IsNullOrWhiteSpace(src.Address.StreetLine1) == false || string.IsNullOrWhiteSpace(src.Address.StreetLine2) == false || string.IsNullOrWhiteSpace(src.Address.PostalCode) == false || string.IsNullOrWhiteSpace(src.Address.City) == false || string.IsNullOrWhiteSpace(src.Address.State) == false || string.IsNullOrWhiteSpace(src.Address.Country) == false));
                    opt.MapFrom(src => src.Address);
                })
                .ForMember(dest => dest.HomePhones, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.HomePhone) == false ? new[] {src.HomePhone} : new string[0]))
                .ForMember(dest => dest.MobilePhone, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.MobilePhone) == false);
                    opt.MapFrom(src => src.MobilePhone);
                })
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(new NullableDateTimeResolver<IContact, ContactModel>(contact => contact.Birthday)))
                .ForMember(dest => dest.EmailAddresses, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.MailAddress) == false ? new[] {src} : new IContact[0]))
                .ForMember(dest => dest.CompanyName, opt =>
                {
                    opt.Condition(src => src.Company?.Name != null && string.IsNullOrWhiteSpace(src.Company.Name.FullName) == false);
                    opt.MapFrom(src => src.Company.Name.FullName);
                })
                .ForMember(dest => dest.BusinessAddress, opt =>
                {
                    opt.Condition(src => src.Company?.Address != null);
                    opt.MapFrom(src => src.Company.Address);
                })
                .ForMember(dest => dest.BusinessPhones, opt => opt.MapFrom(src => src.Company != null ? new[] {src.Company.PrimaryPhone, src.Company.SecondaryPhone}.Where(value => string.IsNullOrWhiteSpace(value) == false) : new string[0]))
                .ForMember(dest => dest.BusinessHomePage, opt =>
                {
                    opt.Condition(src => src.Company != null && string.IsNullOrWhiteSpace(src.Company.HomePage) == false);
                    opt.MapFrom(src => src.Company.HomePage);
                })
                .ForMember(dest => dest.CreatedDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDateTime, opt => opt.Ignore());

            mapperConfiguration.CreateMap<ContactCollectionModel, IEnumerable<IContact>>()
                .ConvertUsing(contactCollectionModel => contactCollectionModel.ToDomain(this));

            mapperConfiguration.CreateMap<PhysicalAddressModel, IAddress>()
                .ConvertUsing(physicalAddressModel => physicalAddressModel.ToDomain());

            mapperConfiguration.CreateMap<IAddress, PhysicalAddressModel>()
                .ForMember(dest => dest.Street, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.StreetLine1) == false || string.IsNullOrWhiteSpace(src.StreetLine2) == false);
                    opt.MapFrom(src => string.Join("\n", new[] {src.StreetLine1, src.StreetLine2}.Where(value => string.IsNullOrWhiteSpace(value) == false)));
                })
                .ForMember(dest => dest.PostalCode, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.PostalCode) == false);
                    opt.MapFrom(src => src.PostalCode);
                })
                .ForMember(dest => dest.City, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.City) == false);
                    opt.MapFrom(src => src.City);
                })
                .ForMember(dest => dest.State, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.State) == false);
                    opt.MapFrom(src => src.State);
                })
                .ForMember(dest => dest.CountryOrRegion, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.Country) == false);
                    opt.MapFrom(src => src.Country);
                });

            mapperConfiguration.CreateMap<EmailAddressModel, string>()
                .ConvertUsing(emailAddressModel => emailAddressModel.ToDomain());

            mapperConfiguration.CreateMap<IContact, EmailAddressModel>()
                .ForMember(dest => dest.Address, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.MailAddress) == false);
                    opt.MapFrom(src => src.MailAddress);
                })
                .ForMember(dest => dest.Name, opt =>
                {
                    opt.Condition(src => src.Name != null && string.IsNullOrWhiteSpace(src.Name.DisplayName) == false);
                    opt.MapFrom(src => src.Name.DisplayName);
                });

            mapperConfiguration.CreateMap<IEnumerable<EmailAddressModel>, string>()
                .ConvertUsing(emailAddressModelCollection => emailAddressModelCollection.ToDomain(this));
        }

        #endregion
    }
}