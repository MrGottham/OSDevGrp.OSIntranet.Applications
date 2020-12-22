using System;
using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Exceptions;

namespace OSDevGrp.OSIntranet.WebApi.Models.Core
{
    internal class CoreModelConverter : ConverterBase
    {
        #region Private variales

        private static readonly IDictionary<Type, string> ErrorTypeDictionary = new Dictionary<Type, string>
        {
            {typeof(IntranetRepositoryException), "RepositoryError"},
            {typeof(IntranetSystemException), "SystemError"},
            {typeof(IntranetCommandBusException), "CommandBusError"},
            {typeof(IntranetQueryBusException), "QueryBusError"},
            {typeof(IntranetBusinessException), "BusinessError"},
            {typeof(IntranetValidationException), "ValidationError"}
        };

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IntranetRepositoryException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => src.GetType()))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt =>
                {
                    opt.Condition(src => src.MethodBase != null && string.IsNullOrWhiteSpace(src.MethodBase.Name) == false);
                    opt.MapFrom(src => src.MethodBase.Name);
                })
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetSystemException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => src.GetType()))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetCommandBusException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => src.GetType()))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetQueryBusException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => src.GetType()))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetBusinessException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => src.GetType()))
                .ForMember(dest => dest.ErrorMessage, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.Method, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingType, opt => opt.Ignore())
                .ForMember(dest => dest.ValidatingField, opt => opt.Ignore());

            mapperConfiguration.CreateMap<IntranetValidationException, ErrorModel>()
                .ForMember(dest => dest.ErrorType, opt => opt.MapFrom(src => src.GetType()))
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

            mapperConfiguration.CreateMap<Type, string>()
                .ConvertUsing(src => ErrorTypeDictionary[src]);
        }

        #endregion
    }
}