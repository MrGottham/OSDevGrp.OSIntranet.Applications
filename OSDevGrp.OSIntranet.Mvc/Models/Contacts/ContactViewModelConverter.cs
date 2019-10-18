using AutoMapper;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    internal class ContactViewModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

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
    }
}
