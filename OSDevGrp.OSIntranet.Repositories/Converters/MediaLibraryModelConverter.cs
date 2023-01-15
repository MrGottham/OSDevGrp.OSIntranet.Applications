using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
    internal class MediaLibraryModelConverter : ConverterBase
    {
        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<MovieGenreModel, IMovieGenre>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<IMovieGenre, MovieGenreModel>()
                .ForMember(dest => dest.MovieGenreIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<MusicGenreModel, IMusicGenre>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<IMusicGenre, MusicGenreModel>()
                .ForMember(dest => dest.MusicGenreIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<BookGenreModel, IBookGenre>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<IBookGenre, BookGenreModel>()
                .ForMember(dest => dest.BookGenreIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

            mapperConfiguration.CreateMap<MediaTypeModel, IMediaType>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<IMediaType, MediaTypeModel>()
                .ForMember(dest => dest.MediaTypeIdentifier, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
                .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));
        }

        internal static IConverter Create()
        {
            return new MediaLibraryModelConverter();
        }

        #endregion
    }
}