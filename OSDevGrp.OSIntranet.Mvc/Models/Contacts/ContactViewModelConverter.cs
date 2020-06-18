using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    internal class ContactViewModelConverter : ConverterBase
    {
        #region Private variables

        private readonly IValueConverter<IAddress, AddressViewModel> _addressViewModelConverter = new AddressViewModelConverter();

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IContact, ContactIdentificationViewModel>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name.DisplayName))
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => src.ToContactType()))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContact, ContactInfoViewModel>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name.DisplayName))
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => src.ToContactType()))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContact, ContactViewModel>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name.DisplayName))
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => src.ToContactType()))
                .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.Name is IPersonName ? ((IPersonName) src.Name).GivenName : null))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Name is IPersonName ? ((IPersonName) src.Name).MiddleName : null))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Name is IPersonName ? ((IPersonName) src.Name).Surname : src.Name is ICompanyName ? ((ICompanyName) src.Name).FullName : src.Name.DisplayName))
                .ForMember(opt => opt.CompanyName, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.ConvertUsing(_addressViewModelConverter))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Countries, opt => opt.MapFrom(src => new List<CountryViewModel>(0)))
                .ForMember(dest => dest.ContactGroups, opt => opt.MapFrom(src => new List<ContactGroupViewModel>(0)))
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => new List<PaymentTermViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IAddress, AddressViewModel>();

            mapperConfiguration.CreateMap<IContactGroup, ContactGroupViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<ContactGroupViewModel, CreateContactGroupCommand>();
            mapperConfiguration.CreateMap<ContactGroupViewModel, UpdateContactGroupCommand>();
            mapperConfiguration.CreateMap<ContactGroupViewModel, DeleteContactGroupCommand>();

            mapperConfiguration.CreateMap<ICountry, CountryViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<CountryViewModel, CreateCountryCommand>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<CountryViewModel, UpdateCountryCommand>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<CountryViewModel, DeleteCountryCommand>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<IPostalCode, PostalCodeViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<PostalCodeViewModel, CreatePostalCodeCommand>()
                .ForMember(dest => dest.CountryCode, opt =>
                {
                    opt.Condition(src => src.Country != null && string.IsNullOrWhiteSpace(src.Country.Code) == false);
                    opt.MapFrom(src => src.Country.Code);
                })
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<PostalCodeViewModel, UpdatePostalCodeCommand>()
                .ForMember(dest => dest.CountryCode, opt =>
                {
                    opt.Condition(src => src.Country != null && string.IsNullOrWhiteSpace(src.Country.Code) == false);
                    opt.MapFrom(src => src.Country.Code);
                })
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<PostalCodeViewModel, DeletePostalCodeCommand>()
                .ForMember(dest => dest.CountryCode, opt =>
                {
                    opt.Condition(src => src.Country != null && string.IsNullOrWhiteSpace(src.Country.Code) == false);
                    opt.MapFrom(src => src.Country.Code);
                })
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Code));
        }

        #endregion

        #region Private classes

        private class AddressViewModelConverter : IValueConverter<IAddress, AddressViewModel>
        {
            #region Methods

            public AddressViewModel Convert(IAddress sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(context, nameof(context));

                return sourceMember == null ? new AddressViewModel() : context.Mapper.Map<IAddress, AddressViewModel>(sourceMember);
            }

            #endregion
        }

        #endregion
    }
}