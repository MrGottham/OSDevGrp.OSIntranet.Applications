using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;

namespace OSDevGrp.OSIntranet.WebApi.Models.Contacts;

internal class ContactModelConverter : ConverterBase
{
    private ContactModelConverter(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
        : base(licensesOptions, loggerFactory)
    {
    }

    protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
    {
        NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

        mapperConfiguration.CreateMap<IContactGroup, ContactGroupModel>();
        mapperConfiguration.CreateMap<ICountry, CountryCodeModel>();
        mapperConfiguration.CreateMap<IPostalCode, PostalCodeModel>();
    }

    internal static IConverter Create(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
    {
        NullGuard.NotNull(licensesOptions, nameof(licensesOptions))
            .NotNull(loggerFactory, nameof(loggerFactory));

        return new ContactModelConverter(licensesOptions, loggerFactory);
    }
}