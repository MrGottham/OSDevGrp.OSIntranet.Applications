using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.WebApi.Models.Core
{
    internal class CoreModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IntranetRepositoryException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => "RepositoryError"))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt =>
                {
                    opt.Condition(src => src.MethodBase != null && string.IsNullOrWhiteSpace(src.MethodBase.Name) == false);
                    opt.MapFrom(src => src.MethodBase.Name);
                })
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetSystemException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => "SystemError"))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetCommandBusException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => "CommandBusError"))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetQueryBusException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => "QueryBusError"))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetBusinessException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => "BusinessError"))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetValidationException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => "ValidationError"))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt =>
                {
                    opt.Condition(src => src.ValidatingType != null && string.IsNullOrWhiteSpace(src.ValidatingType.Name) == false);
                    opt.MapFrom(src => src.ValidatingType.Name);
                })
                .ForMember(dest => dest.ValidatingField, opt =>
                {
                    opt.Condition(src => string.IsNullOrWhiteSpace(src.ValidatingField) == false);
                });
        }

        #endregion
    }
}
