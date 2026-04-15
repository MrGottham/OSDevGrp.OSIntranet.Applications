using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Options;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.MediaLibrary
{
    internal class MediaLibraryViewModelConverter : ConverterBase
    {
        #region Constructor

        private MediaLibraryViewModelConverter(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
            : base(licensesOptions, loggerFactory)
        {
        }

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IMovieGenre, GenericCategoryViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IMusicGenre, GenericCategoryViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IBookGenre, GenericCategoryViewModel>()
	            .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IMediaType, GenericCategoryViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));
        }

        internal static IConverter Create(IOptions<LicensesOptions> licensesOptions, ILoggerFactory loggerFactory)
        {
            NullGuard.NotNull(licensesOptions, nameof(licensesOptions))
                .NotNull(loggerFactory, nameof(loggerFactory));

            return new MediaLibraryViewModelConverter(licensesOptions, loggerFactory);
        }

        #endregion
    }
}