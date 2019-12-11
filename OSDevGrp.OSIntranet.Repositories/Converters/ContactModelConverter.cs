using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class ContactModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<ContactGroupModel, IContactGroup>()
                .ConvertUsing(contactGroupModel => contactGroupModel.ToDomain());

            mapperConfiguration.CreateMap<IContactGroup, ContactGroupModel>()
                .ForMember(dest => dest.ContactGroupIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<CountryModel, ICountry>()
                .ConvertUsing(countryModel => countryModel.ToDomain());

            mapperConfiguration.CreateMap<ICountry, CountryModel>()
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<PostalCodeModel, IPostalCode>()
                .ConvertUsing(postalCodeModel => postalCodeModel.ToDomain(this));

            mapperConfiguration.CreateMap<IPostalCode, PostalCodeModel>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Country.Code))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Code))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));
        }

        #endregion
    }
}
