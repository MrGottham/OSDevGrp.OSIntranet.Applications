using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.WebApi.Models.Shared;

namespace OSDevGrp.OSIntranet.WebApi.Models.Contacts;

internal class ContactModelConverter : ConverterBase
{
    private readonly IConverter _sharedModelConverter;

    private ContactModelConverter(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
        : base(licensesOptions, loggerFactory)
    {
        _sharedModelConverter = SharedModelConverter.Create(licensesOptions, loggerFactory);
    }

    protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
    {
        NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

        mapperConfiguration.CreateMap<IContactGroup, ContactGroupModel>();
        mapperConfiguration.CreateMap<ICountry, CountryCodeModel>();
        mapperConfiguration.CreateMap<ICountry, CountryCodeIdentificationModel>();
        mapperConfiguration.CreateMap<IPostalCode, PostalCodeModel>();

        mapperConfiguration.CreateMap<IName, NameModel>()
            .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src is IPersonName ? ((IPersonName) src).GivenName : null))
            .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src is IPersonName ? ((IPersonName) src).MiddleName : null))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src is IPersonName ? ((IPersonName) src).Surname : null))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src is ICompanyName ? ((ICompanyName) src).FullName : null));

        mapperConfiguration.CreateMap<IAddress, AddressModel>();

        mapperConfiguration.CreateMap<ICompany, CompanyModel>();

        mapperConfiguration.CreateMap<IContact, ContactModel>()
            .ForMember(dest => dest.PaymentTerm, opt => opt.MapFrom(src => _sharedModelConverter.Convert<IPaymentTerm, PaymentTermModel>(src.PaymentTerm)));
    }

    internal static IConverter Create(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
    {
        NullGuard.NotNull(licensesOptions, nameof(licensesOptions))
            .NotNull(loggerFactory, nameof(loggerFactory));

        return new ContactModelConverter(licensesOptions, loggerFactory);
    }
}