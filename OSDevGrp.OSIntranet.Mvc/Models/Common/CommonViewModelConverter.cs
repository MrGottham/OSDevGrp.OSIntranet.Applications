using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Common
{
    internal class CommonViewModelConverter : ConverterBase
    {
        #region Constructor

        private CommonViewModelConverter(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
            : base(licensesOptions, loggerFactory)
        {
        }

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<ILetterHead, LetterHeadViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<LetterHeadViewModel, CreateLetterHeadCommand>();
            mapperConfiguration.CreateMap<LetterHeadViewModel, UpdateLetterHeadCommand>();
            mapperConfiguration.CreateMap<LetterHeadViewModel, DeleteLetterHeadCommand>();

            mapperConfiguration.CreateMap<INationality, GenericCategoryViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<ILanguage, GenericCategoryViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));
        }

        internal static IConverter Create(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(licensesOptions, nameof(licensesOptions))
                .NotNull(loggerFactory, nameof(loggerFactory));

            return new CommonViewModelConverter(licensesOptions, loggerFactory);
        }

        #endregion
    }
}