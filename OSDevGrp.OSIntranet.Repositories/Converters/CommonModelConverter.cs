using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Common;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
	internal class CommonModelConverter : ConverterBase
    {
        #region Constructor

        private CommonModelConverter(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
            : base(licensesOptions, loggerFactory)
        {
        }

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<LetterHeadModel, ILetterHead>()
                .ConvertUsing(letterHeadModel => letterHeadModel.ToDomain());

            mapperConfiguration.CreateMap<ILetterHead, LetterHeadModel>()
                .ForMember(dest => dest.LetterHeadIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.Accountings, opt => opt.Ignore());

            mapperConfiguration.CreateMap<KeyValueEntryModel, IKeyValueEntry>()
                .ConvertUsing(keyValueEntryModel => keyValueEntryModel.ToDomain());

            mapperConfiguration.CreateMap<IKeyValueEntry, KeyValueEntryModel>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ToBase64()))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<NationalityModel, INationality>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<INationality, NationalityModel>()
	            .ForMember(dest => dest.NationalityIdentifier, opt => opt.MapFrom(src => src.Number))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.MediaPersonalities, opt => opt.Ignore());

            mapperConfiguration.CreateMap<LanguageModel, ILanguage>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<ILanguage, LanguageModel>()
	            .ForMember(dest => dest.LanguageIdentifier, opt => opt.MapFrom(src => src.Number))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.Movies, opt => opt.Ignore())
	            .ForMember(dest => dest.Books, opt => opt.Ignore());
        }

        internal static IConverter Create(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(licensesOptions, nameof(licensesOptions))
                .NotNull(loggerFactory, nameof(loggerFactory));

            return new CommonModelConverter(licensesOptions, loggerFactory);
        }

        #endregion
    }
}