using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Models.Common;
using OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Repositories.Converters
{
	internal class MediaLibraryModelConverter : ConverterBase
    {
	    #region Private variables

	    private readonly MapperCache _mapperCache = new();
	    private readonly IConverter _commonModelConverter = CommonModelConverter.Create();
	    private readonly IValueConverter<IMedia, MovieModel> _mediaToMovieModelConverter = new MediaToMovieModelConverter();
	    private readonly IValueConverter<IMedia, MusicModel> _mediaToMusicModelConverter = new MediaToMusicModelConverter();
	    private readonly IValueConverter<IMedia, BookModel> _mediaToBookModelConverter = new MediaToBookModelConverter();

		#endregion

		#region Properties

		protected override IDictionary<string, object> StateDictionary
		{
			get
			{
				IDictionary<string, object> stateDictionary = new ConcurrentDictionary<string, object>();
				stateDictionary.Add("MapperCache", _mapperCache);
				stateDictionary.Add("MediaLibraryModelConverter", this);
				return stateDictionary;
			}
		}

		#endregion

        #region Methods

		protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<MovieModel, IMovie>()
	            .ConvertUsing((src, _, context) => src.ToDomain(ResolveMapperCacheFromContext(context.Items), ResolveMediaLibraryModelConverterFromContext(context.Items), _commonModelConverter));

            mapperConfiguration.CreateMap<IMovie, MovieModel>()
	            .ForMember(dest => dest.MovieIdentifier, opt => opt.MapFrom(src => default(int)))
	            .ForMember(dest => dest.ExternalMediaIdentifier, opt => opt.MapFrom(src => src.MediaIdentifier))
	            .ForMember(dest => dest.CoreData, opt => opt.MapFrom(src => (IMedia)src))
	            .ForMember(dest => dest.MovieGenreIdentifier, opt => opt.MapFrom(src => src.MovieGenre.Number))
	            .ForMember(dest => dest.SpokenLanguageIdentifier, opt => opt.MapFrom(src => src.SpokenLanguage != null ? src.SpokenLanguage.Number : (int?)null))
	            .ForMember(dest => dest.SpokenLanguage, opt => opt.MapFrom(src => src.SpokenLanguage != null ? _commonModelConverter.Convert<ILanguage, LanguageModel>(src.SpokenLanguage) : null))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.MovieBindings, opt => opt.MapFrom(src => new List<MovieBindingModel>()))
	            .ForMember(dest => dest.Lendings, opt => opt.MapFrom(src => new List<LendingModel>()));

            mapperConfiguration.CreateMap<MusicModel, IMusic>()
	            .ConvertUsing((src, _, context) => src.ToDomain(ResolveMapperCacheFromContext(context.Items), ResolveMediaLibraryModelConverterFromContext(context.Items), _commonModelConverter));

            mapperConfiguration.CreateMap<IMusic, MusicModel>()
	            .ForMember(dest => dest.MusicIdentifier, opt => opt.MapFrom(src => default(int)))
	            .ForMember(dest => dest.ExternalMediaIdentifier, opt => opt.MapFrom(src => src.MediaIdentifier))
	            .ForMember(dest => dest.CoreData, opt => opt.MapFrom(src => (IMedia)src))
	            .ForMember(dest => dest.MusicGenreIdentifier, opt => opt.MapFrom(src => src.MusicGenre.Number))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.MusicBindings, opt => opt.MapFrom(src => new List<MusicBindingModel>()))
	            .ForMember(dest => dest.Lendings, opt => opt.MapFrom(src => new List<LendingModel>()));

			mapperConfiguration.CreateMap<BookModel, IBook>()
				.ConvertUsing((src, _, context) => src.ToDomain(ResolveMapperCacheFromContext(context.Items), ResolveMediaLibraryModelConverterFromContext(context.Items), _commonModelConverter));

			mapperConfiguration.CreateMap<IBook, BookModel>()
				.ForMember(dest => dest.BookIdentifier, opt => opt.MapFrom(src => default(int)))
				.ForMember(dest => dest.ExternalMediaIdentifier, opt => opt.MapFrom(src => src.MediaIdentifier))
				.ForMember(dest => dest.CoreData, opt => opt.MapFrom(src => (IMedia)src))
				.ForMember(dest => dest.BookGenreIdentifier, opt => opt.MapFrom(src => src.BookGenre.Number))
				.ForMember(dest => dest.WrittenLanguageIdentifier, opt => opt.MapFrom(src => src.WrittenLanguage != null ? src.WrittenLanguage.Number : (int?)null))
				.ForMember(dest => dest.WrittenLanguage, opt => opt.MapFrom(src => src.WrittenLanguage != null ? _commonModelConverter.Convert<ILanguage, LanguageModel>(src.WrittenLanguage) : null))
				.ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
				.ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
				.ForMember(dest => dest.BookBindings, opt => opt.MapFrom(src => new List<BookBindingModel>()))
				.ForMember(dest => dest.Lendings, opt => opt.MapFrom(src => new List<LendingModel>()));

			mapperConfiguration.CreateMap<IMedia, MediaCoreDataModel>()
				.ForMember(dest => dest.MediaCoreDataIdentifier, opt => opt.MapFrom(src => default(int)))
				.ForMember(dest => dest.MediaTypeIdentifier, opt => opt.MapFrom(src => src.MediaType.Number))
				.ForMember(dest => dest.Url, opt => opt.MapFrom(src => ValueConverter.UriToString(src.Url)))
				.ForMember(dest => dest.Image, opt => opt.MapFrom(src => ValueConverter.ByteArrayToString(src.Image)))
				.ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
				.ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
				.ForMember(dest => dest.MovieIdentifier, opt => opt.Ignore())
				.ForMember(dest => dest.Movie, opt => opt.Ignore())
				.ForMember(dest => dest.MusicIdentifier, opt => opt.Ignore())
				.ForMember(dest => dest.Music, opt => opt.Ignore())
				.ForMember(dest => dest.BookIdentifier, opt => opt.Ignore())
				.ForMember(dest => dest.Book, opt => opt.Ignore());

			mapperConfiguration.CreateMap<MediaPersonalityModel, IMediaPersonality>()
	            .ConvertUsing((src, _, context) => src.ToDomain(ResolveMapperCacheFromContext(context.Items), ResolveMediaLibraryModelConverterFromContext(context.Items), _commonModelConverter));

            mapperConfiguration.CreateMap<IMediaPersonality, MediaPersonalityModel>()
	            .ForMember(dest => dest.MediaPersonalityIdentifier, opt => opt.MapFrom(src => default(int)))
	            .ForMember(dest => dest.ExternalMediaPersonalityIdentifier, opt => opt.MapFrom(src => src.MediaPersonalityIdentifier))
	            .ForMember(dest => dest.NationalityIdentifier, opt => opt.MapFrom(src => src.Nationality.Number))
	            .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => _commonModelConverter.Convert<INationality, NationalityModel>(src.Nationality)))
	            .ForMember(dest => dest.Url, opt => opt.MapFrom(src => ValueConverter.UriToString(src.Url)))
	            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => ValueConverter.ByteArrayToString(src.Image)))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.MovieBindings, opt => opt.Ignore())
	            .ForMember(dest => dest.MusicBindings, opt => opt.Ignore())
	            .ForMember(dest => dest.BookBindings, opt => opt.Ignore());

            mapperConfiguration.CreateMap<BorrowerModel, IBorrower>()
	            .ConvertUsing((src, _, context) => src.ToDomain(ResolveMapperCacheFromContext(context.Items), ResolveMediaLibraryModelConverterFromContext(context.Items), _commonModelConverter));

            mapperConfiguration.CreateMap<IBorrower, BorrowerModel>()
	            .ForMember(dest => dest.BorrowerIdentifier, opt => opt.MapFrom(src => default(int)))
	            .ForMember(dest => dest.ExternalBorrowerIdentifier, opt => opt.MapFrom(src => src.BorrowerIdentifier))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.Lendings, opt => opt.MapFrom(src => new List<LendingModel>()));

			mapperConfiguration.CreateMap<LendingModel, ILending>()
				.ConvertUsing((src, _, context) => src.ToDomain(ResolveMapperCacheFromContext(context.Items), ResolveMediaLibraryModelConverterFromContext(context.Items), _commonModelConverter));

			mapperConfiguration.CreateMap<ILending, LendingModel>()
	            .ForMember(dest => dest.LendingIdentifier, opt => opt.MapFrom(src => default(int)))
	            .ForMember(dest => dest.ExternalLendingIdentifier, opt => opt.MapFrom(src => src.LendingIdentifier))
	            .ForMember(dest => dest.BorrowerIdentifier, opt => opt.MapFrom(src => default(int)))
	            .ForMember(dest => dest.MovieIdentifier, opt => opt.MapFrom(src => default(int?)))
	            .ForMember(dest => dest.Movie, opt => opt.ConvertUsing(_mediaToMovieModelConverter, src => src.Media))
	            .ForMember(dest => dest.MusicIdentifier, opt => opt.MapFrom(src => default(int?)))
	            .ForMember(dest => dest.Music, opt => opt.ConvertUsing(_mediaToMusicModelConverter, src => src.Media))
	            .ForMember(dest => dest.BookIdentifier, opt => opt.MapFrom(src => default(int?)))
	            .ForMember(dest => dest.Book, opt => opt.ConvertUsing(_mediaToBookModelConverter, src => src.Media))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()));

			mapperConfiguration.CreateMap<MovieGenreModel, IMovieGenre>()
                .ConvertUsing(src => src.ToDomain());

			mapperConfiguration.CreateMap<IMovieGenre, MovieGenreModel>()
				.ForMember(dest => dest.MovieGenreIdentifier, opt => opt.MapFrom(src => src.Number))
				.ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
				.ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
				.ForMember(dest => dest.Movies, opt => opt.Ignore());

            mapperConfiguration.CreateMap<MusicGenreModel, IMusicGenre>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<IMusicGenre, MusicGenreModel>()
	            .ForMember(dest => dest.MusicGenreIdentifier, opt => opt.MapFrom(src => src.Number))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.Music, opt => opt.Ignore());

            mapperConfiguration.CreateMap<BookGenreModel, IBookGenre>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<IBookGenre, BookGenreModel>()
	            .ForMember(dest => dest.BookGenreIdentifier, opt => opt.MapFrom(src => src.Number))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.Books, opt => opt.Ignore());

            mapperConfiguration.CreateMap<MediaTypeModel, IMediaType>()
                .ConvertUsing(src => src.ToDomain());

            mapperConfiguration.CreateMap<IMediaType, MediaTypeModel>()
	            .ForMember(dest => dest.MediaTypeIdentifier, opt => opt.MapFrom(src => src.Number))
	            .ForMember(dest => dest.CreatedUtcDateTime, opt => opt.MapFrom(src => src.CreatedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.ModifiedUtcDateTime, opt => opt.MapFrom(src => src.ModifiedDateTime.ToUniversalTime()))
	            .ForMember(dest => dest.CoreData, opt => opt.Ignore());
        }

        internal static IConverter Create()
        {
            return new MediaLibraryModelConverter();
        }

        private static MapperCache ResolveMapperCacheFromContext(IDictionary<string, object> stateDictionary)
        {
	        NullGuard.NotNull(stateDictionary, nameof(stateDictionary));

	        return stateDictionary["MapperCache"] as MapperCache;
        }

        private static IConverter ResolveMediaLibraryModelConverterFromContext(IDictionary<string, object> stateDictionary)
        {
	        NullGuard.NotNull(stateDictionary, nameof(stateDictionary));

	        return stateDictionary["MediaLibraryModelConverter"] as IConverter;
        }

        private class MediaToMovieModelConverter : IValueConverter<IMedia, MovieModel>
        {
			#region Methods

			public MovieModel Convert(IMedia media, ResolutionContext context)
			{
				NullGuard.NotNull(media, nameof(media))
					.NotNull(context, nameof(context));

				return media is IMovie movie ? context.Mapper.Map<IMovie, MovieModel>(movie) : null;
			}

			#endregion
		}

        private class MediaToMusicModelConverter : IValueConverter<IMedia, MusicModel>
        {
	        #region Methods

	        public MusicModel Convert(IMedia media, ResolutionContext context)
	        {
		        NullGuard.NotNull(media, nameof(media))
			        .NotNull(context, nameof(context));

		        return media is IMusic music ? context.Mapper.Map<IMusic, MusicModel>(music) : null;
	        }

	        #endregion
        }

        private class MediaToBookModelConverter : IValueConverter<IMedia, BookModel>
        {
	        #region Methods

	        public BookModel Convert(IMedia media, ResolutionContext context)
	        {
		        NullGuard.NotNull(media, nameof(media))
			        .NotNull(context, nameof(context));

		        return media is IBook book ? context.Mapper.Map<IBook, BookModel>(book) : null;
	        }

	        #endregion
        }

		#endregion
	}
}