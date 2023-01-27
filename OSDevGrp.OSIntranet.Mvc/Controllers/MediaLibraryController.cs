﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OSDevGrp.OSIntranet.BusinessLogic.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.MediaLibrary.Queries;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.CommandBus;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Core.Queries;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Mvc.Models.Common;
using OSDevGrp.OSIntranet.Mvc.Models.Core;
using OSDevGrp.OSIntranet.Mvc.Models.MediaLibrary;
using OSDevGrp.OSIntranet.Mvc.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Mvc.Controllers
{
    [Authorize(Policy = Policies.MediaLibraryPolicy)]
    public class MediaLibraryController : Controller
    {
        #region Private variables

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IConverter _mediaLibraryViewModelConverter = new MediaLibraryViewModelConverter();
        private readonly IConverter _commonViewModelConverter = new CommonViewModelConverter();

        #endregion

        #region Constructor

        public MediaLibraryController(ICommandBus commandBus, IQueryBus queryBus)
        {
            NullGuard.NotNull(commandBus, nameof(commandBus))
                .NotNull(queryBus, nameof(queryBus));

            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> MovieGenres()
        {
            GenericCategoryCollectionViewModel movieGenreCollectionViewModel = await GetMovieGenreCollectionViewModelAsync();

            return movieGenreCollectionViewModel.AsView(this);
        }

        [HttpGet]
        public IActionResult CreateMovieGenre()
        {
            return GenericCategoryViewModel.Create("Opret filmgenre", "MediaLibrary", "CreateMovieGenre", "MovieGenres")
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateMovieGenre(GenericCategoryViewModel genericCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMovieGenre(int number)
        {
            IMovieGenre movieGenre = await GetMovieGenreAsync(number);
            if (movieGenre == null)
            {
                return BadRequest();
            }

            return GenericCategoryViewModel.Create("Redigér filmgenre", "MediaLibrary", nameof(UpdateMovieGenre), nameof(MovieGenres), movieGenre)
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> UpdateMovieGenre(GenericCategoryViewModel genericCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteMovieGenre(int number)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> MusicGenres()
        {
            GenericCategoryCollectionViewModel musicGenreCollectionViewModel = await GetMusicGenreCollectionViewModelAsync();

            return musicGenreCollectionViewModel.AsView(this);
        }

        [HttpGet]
        public IActionResult CreateMusicGenre()
        {
            return GenericCategoryViewModel.Create("Opret musikgenre", "MediaLibrary", "CreateMusicGenre", "MusicGenres")
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateMusicGenre(GenericCategoryViewModel genericCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMusicGenre(int number)
        {
            IMusicGenre musicGenre = await GetMusicGenreAsync(number);
            if (musicGenre == null)
            {
                return BadRequest();
            }

            return GenericCategoryViewModel.Create("Redigér musikgenre", "MediaLibrary", nameof(UpdateMusicGenre), nameof(MusicGenres), musicGenre)
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> UpdateMusicGenre(GenericCategoryViewModel genericCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteMusicGenre(int number)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> BookGenres()
        {
	        GenericCategoryCollectionViewModel bookGenreCollectionViewModel = await GetBookGenreCollectionViewModelAsync();

	        return bookGenreCollectionViewModel.AsView(this);
        }

		[HttpGet]
        public IActionResult CreateBookGenre()
        {
            return GenericCategoryViewModel.Create("Opret litterær genre", "MediaLibrary", "CreateBookGenre", "BookGenres")
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateBookGenre(GenericCategoryViewModel genericCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBookGenre(int number)
        {
            IBookGenre bookGenre = await GetBookGenreAsync(number);
            if (bookGenre == null)
            {
                return BadRequest();
            }

            return GenericCategoryViewModel.Create("Redigér litterær genre", "MediaLibrary", nameof(UpdateBookGenre), nameof(BookGenres), bookGenre)
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> UpdateBookGenre(GenericCategoryViewModel genericCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteBookGenre(int number)
        {
	        throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> MediaTypes()
        {
            GenericCategoryCollectionViewModel mediaTypeCollectionViewModel = await GetMediaTypeCollectionViewModelAsync();

            return mediaTypeCollectionViewModel.AsView(this);
        }

        [HttpGet]
        public IActionResult CreateMediaType()
        {
            return GenericCategoryViewModel.Create("Opret medietype", "MediaLibrary", "CreateMediaType", "MediaTypes")
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> CreateMediaType(GenericCategoryViewModel genericCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMediaType(int number)
        {
            IMediaType mediaType = await GetMediaTypeAsync(number);
            if (mediaType == null)
            {
                return BadRequest();
            }

            return GenericCategoryViewModel.Create("Redigér medietype", "MediaLibrary", nameof(UpdateMediaType), nameof(MediaTypes), mediaType)
                .AsView(this);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> UpdateMediaType(GenericCategoryViewModel genericCategoryViewModel)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteMediaType(int number)
        {
            throw new NotImplementedException();
        }

        private async Task<GenericCategoryCollectionViewModel> GetMovieGenreCollectionViewModelAsync()
        {
            IEnumerable<IMovieGenre> movieGenres = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IMovieGenre>>(MediaLibraryQueryFactory.BuildEmptyQuery());

            return GenericCategoryCollectionViewModel.Create("Filmgenre", "MediaLibrary", nameof(CreateMovieGenre), nameof(UpdateMovieGenre), (genericCategoryViewModel, urlHelper) => genericCategoryViewModel.GetDeletionUrl("MediaLibrary", nameof(DeleteMovieGenre), urlHelper), movieGenres ?? Array.Empty<IMovieGenre>(), _mediaLibraryViewModelConverter);
        }

        private async Task<GenericCategoryCollectionViewModel> GetMusicGenreCollectionViewModelAsync()
        {
            IEnumerable<IMusicGenre> musicGenres = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IMusicGenre>>(MediaLibraryQueryFactory.BuildEmptyQuery());

            return GenericCategoryCollectionViewModel.Create("Musikgenre", "MediaLibrary", nameof(CreateMusicGenre), nameof(UpdateMusicGenre), (genericCategoryViewModel, urlHelper) => genericCategoryViewModel.GetDeletionUrl("MediaLibrary", nameof(DeleteMusicGenre), urlHelper), musicGenres ?? Array.Empty<IMusicGenre>(), _mediaLibraryViewModelConverter);
        }

        private async Task<GenericCategoryCollectionViewModel> GetBookGenreCollectionViewModelAsync()
        {
	        IEnumerable<IBookGenre> bookGenres = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IBookGenre>>(MediaLibraryQueryFactory.BuildEmptyQuery());

	        return GenericCategoryCollectionViewModel.Create("Litterære genre", "MediaLibrary", nameof(CreateBookGenre), nameof(UpdateBookGenre), (genericCategoryViewModel, urlHelper) => genericCategoryViewModel.GetDeletionUrl("MediaLibrary", nameof(DeleteBookGenre), urlHelper), bookGenres ?? Array.Empty<IBookGenre>(), _mediaLibraryViewModelConverter);
        }

        private async Task<GenericCategoryCollectionViewModel> GetMediaTypeCollectionViewModelAsync()
        {
            IEnumerable<IMediaType> mediaTypes = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<IMediaType>>(MediaLibraryQueryFactory.BuildEmptyQuery());

            return GenericCategoryCollectionViewModel.Create("Medietyper", "MediaLibrary", nameof(CreateMediaType), nameof(UpdateMediaType), (genericCategoryViewModel, urlHelper) => genericCategoryViewModel.GetDeletionUrl("MediaLibrary", nameof(DeleteMediaType), urlHelper), mediaTypes ?? Array.Empty<IMediaType>(), _mediaLibraryViewModelConverter);
        }

        private async Task<GenericCategoryCollectionViewModel> GetNationalityCollectionViewModelAsync()
        {
	        IEnumerable<INationality> nationalities = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<INationality>>(CommonQueryFactory.BuildEmptyQuery());

	        return GenericCategoryCollectionViewModel.Create("Nationaliteter", "Common", "CreateNationality", "UpdateNationality", (genericCategoryViewModel, urlHelper) => genericCategoryViewModel.GetDeletionUrl("Common", "DeleteNationality", urlHelper), nationalities ?? Array.Empty<INationality>(), _commonViewModelConverter);
        }

        private async Task<GenericCategoryCollectionViewModel> GetLanguageCollectionViewModelAsync()
        {
	        IEnumerable<ILanguage> languages = await _queryBus.QueryAsync<EmptyQuery, IEnumerable<ILanguage>>(CommonQueryFactory.BuildEmptyQuery());

	        return GenericCategoryCollectionViewModel.Create("Sprog", "Common", "CreateLanguage", "UpdateLanguage", (genericCategoryViewModel, urlHelper) => genericCategoryViewModel.GetDeletionUrl("Common", "DeleteLanguage", urlHelper), languages ?? Array.Empty<ILanguage>(), _commonViewModelConverter);
        }

        private Task<IMovieGenre> GetMovieGenreAsync(int number)
        {
            return _queryBus.QueryAsync<IGetMovieGenreQuery, IMovieGenre>(MediaLibraryQueryFactory.BuildGetMovieGenreQuery(number));
        }

        private Task<IMusicGenre> GetMusicGenreAsync(int number)
        {
            return _queryBus.QueryAsync<IGetMusicGenreQuery, IMusicGenre>(MediaLibraryQueryFactory.BuildGetMusicGenreQuery(number));
        }

        private Task<IBookGenre> GetBookGenreAsync(int number)
        {
            return _queryBus.QueryAsync<IGetBookGenreQuery, IBookGenre>(MediaLibraryQueryFactory.BuildGetBookGenreQuery(number));
        }

        private Task<IMediaType> GetMediaTypeAsync(int number)
        {
            return _queryBus.QueryAsync<IGetMediaTypeQuery, IMediaType>(MediaLibraryQueryFactory.BuildGetMediaTypeQuery(number));
        }

        private Task<INationality> GetNationalityAsync(int number)
        {
            return _queryBus.QueryAsync<IGetNationalityQuery, INationality>(CommonQueryFactory.BuildGetNationalityQuery(number));
        }

        private Task<ILanguage> GetLanguageAsync(int number)
        {
            return _queryBus.QueryAsync<IGetLanguageQuery, ILanguage>(CommonQueryFactory.BuildGetLanguageQuery(number));
        }

		#endregion
    }
}