using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Models.Common;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class CommonModelConverter : ConverterBase
    {
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
        }

        internal static IConverter Create()
        {
            return new CommonModelConverter();
        }

        #endregion
    }
}