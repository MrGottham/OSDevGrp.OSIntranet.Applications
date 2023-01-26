using AutoMapper;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Common
{
    internal class CommonViewModelConverter : ConverterBase
    {
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

        #endregion
    }
}