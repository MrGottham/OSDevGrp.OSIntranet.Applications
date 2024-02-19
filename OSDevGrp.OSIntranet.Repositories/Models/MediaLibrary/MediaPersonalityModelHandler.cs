using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.MediaLibrary
{
	internal class MediaPersonalityModelHandler : ModelHandlerBase<IMediaPersonality, RepositoryContext, MediaPersonalityModel, Guid>
	{
		#region Private variables

		private readonly bool _includeMediaBindings;

		#endregion

		#region Constructor

		public MediaPersonalityModelHandler(RepositoryContext dbContext, IConverter modelConverter, bool includeMediaBindings) 
			: base(dbContext, modelConverter)
		{
			_includeMediaBindings = includeMediaBindings;
		}

		#endregion

		#region Properties

		protected sealed override DbSet<MediaPersonalityModel> Entities => DbContext.MediaPersonalities;

		protected override IQueryable<MediaPersonalityModel> Reader => CreateReader();

		#endregion

		#region Methods

		internal Task<MediaPersonalityModel> ReadAsync(MediaPersonalityModel mediaPersonalityModel)
		{
			NullGuard.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			return OnReadAsync(mediaPersonalityModel);
		}

		protected sealed override Func<IMediaPersonality, Guid> PrimaryKey => mediaPersonality => mediaPersonality.MediaPersonalityIdentifier;

		protected sealed override Expression<Func<MediaPersonalityModel, bool>> EntitySelector(Guid primaryKey) => mediaPersonalityModel => mediaPersonalityModel.ExternalMediaPersonalityIdentifier == primaryKey;

		protected sealed override Task<IEnumerable<IMediaPersonality>> SortAsync(IEnumerable<IMediaPersonality> mediaPersonalityCollection)
		{
			NullGuard.NotNull(mediaPersonalityCollection, nameof(mediaPersonalityCollection));

			return Task.FromResult(mediaPersonalityCollection.OrderBy(mediaPersonality => mediaPersonality.ToString()).AsEnumerable());
		}

		protected sealed override async Task<MediaPersonalityModel> OnCreateAsync(IMediaPersonality mediaPersonality, MediaPersonalityModel mediaPersonalityModel)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality))
				.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			mediaPersonalityModel.Nationality = await DbContext.Nationalities.SingleAsync(m => m.NationalityIdentifier == mediaPersonality.Nationality.Number);

			return mediaPersonalityModel;
		}

		protected sealed override async Task<MediaPersonalityModel> OnReadAsync(MediaPersonalityModel mediaPersonalityModel)
		{
			NullGuard.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			mediaPersonalityModel.Deletable = await CanDeleteAsync(mediaPersonalityModel);

			return mediaPersonalityModel;
		}

		protected sealed override async Task OnUpdateAsync(IMediaPersonality mediaPersonality, MediaPersonalityModel mediaPersonalityModel)
		{
			NullGuard.NotNull(mediaPersonality, nameof(mediaPersonality))
				.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			mediaPersonalityModel.GivenName = mediaPersonality.GivenName;
			mediaPersonalityModel.MiddleName = mediaPersonality.MiddleName;
			mediaPersonalityModel.Surname = mediaPersonality.Surname;
			mediaPersonalityModel.NationalityIdentifier = mediaPersonality.Nationality.Number;
			mediaPersonalityModel.Nationality = await DbContext.Nationalities.SingleAsync(m => m.NationalityIdentifier == mediaPersonality.Nationality.Number);
			mediaPersonalityModel.BirthDate = mediaPersonality.BirthDate;
			mediaPersonalityModel.DateOfDead = mediaPersonality.DateOfDead;
			mediaPersonalityModel.Url = ValueConverter.UriToString(mediaPersonality.Url);
			mediaPersonalityModel.Image = ValueConverter.ByteArrayToString(mediaPersonality.Image);
		}

		protected sealed override async Task<bool> CanDeleteAsync(MediaPersonalityModel mediaPersonalityModel)
		{
			NullGuard.NotNull(mediaPersonalityModel, nameof(mediaPersonalityModel));

			if (mediaPersonalityModel.MovieBindings != null && mediaPersonalityModel.MovieBindings.Any())
			{
				return false;
			}

			if (mediaPersonalityModel.MusicBindings != null && mediaPersonalityModel.MusicBindings.Any())
			{
				return false;
			}

			if (mediaPersonalityModel.BookBindings != null && mediaPersonalityModel.BookBindings.Any())
			{
				return false;
			}

			if (await DbContext.MovieBindings.FirstOrDefaultAsync(movieBindingModel => movieBindingModel.MediaPersonalityIdentifier == mediaPersonalityModel.MediaPersonalityIdentifier) != null)
			{
				return false;
			}

			if (await DbContext.MusicBindings.FirstOrDefaultAsync(musicBindingModel => musicBindingModel.MediaPersonalityIdentifier == mediaPersonalityModel.MediaPersonalityIdentifier) != null)
			{
				return false;
			}

			return await DbContext.BookBindings.FirstOrDefaultAsync(bookBindingModel => bookBindingModel.MediaPersonalityIdentifier == mediaPersonalityModel.MediaPersonalityIdentifier) == null;
		}

		private IQueryable<MediaPersonalityModel> CreateReader()
		{
			IQueryable<MediaPersonalityModel> reader = Entities.Include(m => m.Nationality);

			if (_includeMediaBindings == false)
			{
				return reader;
			}

			return reader.Include(m => m.MovieBindings)
				.Include(m => m.MusicBindings)
				.Include(m => m.BookBindings);
		}

		#endregion
	}
}