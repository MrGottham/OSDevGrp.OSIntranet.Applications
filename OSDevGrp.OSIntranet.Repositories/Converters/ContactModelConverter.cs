using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Models.Accounting;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class ContactModelConverter : ConverterBase
    {
        #region Private variables

        private readonly IConverter _accountingConverter = new AccountingModelConverter();

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IContact, ContactSupplementModel>()
                .ForMember(dest => dest.ContactSupplementIdentifier, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.InternalIdentifier) ? default : int.Parse(src.InternalIdentifier)))
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(new NullableDateTimeResolver<IContact, ContactSupplementModel>(contact => contact.Birthday)))
                .ForMember(dest => dest.ContactGroupIdentifier, opt => opt.MapFrom(src => src.ContactGroup.Number))
                .ForMember(dest => dest.PaymentTermIdentifier, opt => opt.MapFrom(src => src.PaymentTerm.Number))
                .ForMember(dest => dest.PaymentTerm, opt => opt.MapFrom(src => _accountingConverter.Convert<IPaymentTerm, PaymentTermModel>(src.PaymentTerm)))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ContactSupplementBindings, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.ContactSupplementBindings = new List<ContactSupplementBindingModel>();

                    if (string.IsNullOrWhiteSpace(src.ExternalIdentifier) == false)
                    {
                        dest.ContactSupplementBindings.Add(new ContactSupplementBindingModel {ContactSupplement = dest, ExternalIdentifier = src.ExternalIdentifier});
                    }

                    string calculatedIdentifier = src.CalculateIdentifier();
                    if (string.IsNullOrWhiteSpace(calculatedIdentifier))
                    {
                        return;
                    }

                    dest.ContactSupplementBindings.Add(new ContactSupplementBindingModel {ContactSupplement = dest, ExternalIdentifier = calculatedIdentifier});
                });

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