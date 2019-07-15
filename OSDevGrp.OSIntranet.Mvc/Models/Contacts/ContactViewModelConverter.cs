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
        }

        #endregion
    }
}
