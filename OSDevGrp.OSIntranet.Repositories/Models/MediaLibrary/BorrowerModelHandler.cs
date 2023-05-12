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
	internal class BorrowerModelHandler : ModelHandlerBase<IBorrower, RepositoryContext, BorrowerModel, Guid>
	{
		#region Private variables

		private readonly bool _includeLendings;
		private readonly LendingModelHandler _lendingModelHandler;

		#endregion

		#region Constructor

		public BorrowerModelHandler(RepositoryContext dbContext, IConverter modelConverter, bool includeLendings) 
			: base(dbContext, modelConverter)
		{
			_includeLendings = includeLendings;
			_lendingModelHandler = new LendingModelHandler(dbContext, modelConverter, true, true);
		}

		#endregion

		#region Properties

		protected sealed override DbSet<BorrowerModel> Entities => DbContext.Borrowers;

		protected sealed override Func<IBorrower, Guid> PrimaryKey => borrower => borrower.BorrowerIdentifier;

		protected sealed override IQueryable<BorrowerModel> Reader => CreateReader();

		#endregion

		#region Methods

		protected sealed override Expression<Func<BorrowerModel, bool>> EntitySelector(Guid primaryKey) => borrowerModel => borrowerModel.ExternalBorrowerIdentifier == ValueConverter.GuidToString(primaryKey);

		protected sealed override Task<IEnumerable<IBorrower>> SortAsync(IEnumerable<IBorrower> borrowerCollection)
		{
			NullGuard.NotNull(borrowerCollection, nameof(borrowerCollection));

			return Task.FromResult(borrowerCollection.OrderBy(borrower => borrower.ToString()).AsEnumerable());
		}

		protected sealed override void OnDispose()
		{
			_lendingModelHandler.Dispose();
		}

		protected sealed override async Task<BorrowerModel> OnReadAsync(BorrowerModel borrowerModel)
		{
			NullGuard.NotNull(borrowerModel, nameof(borrowerModel));

			if (borrowerModel.Lendings != null)
			{
				borrowerModel.Lendings = (await _lendingModelHandler.ReadAsync(borrowerModel.Lendings)).ToList();
			}

			borrowerModel.Deletable = await CanDeleteAsync(borrowerModel);

			return borrowerModel;
		}

		protected override Task OnUpdateAsync(IBorrower borrower, BorrowerModel borrowerModel)
		{
			NullGuard.NotNull(borrower, nameof(borrower))
				.NotNull(borrowerModel, nameof(borrowerModel));

			borrowerModel.ExternalIdentifier = borrower.ExternalIdentifier;
			borrowerModel.FullName = borrower.FullName;
			borrowerModel.MailAddress = borrower.MailAddress;
			borrowerModel.PrimaryPhone = borrower.PrimaryPhone;
			borrowerModel.SecondaryPhone = borrower.SecondaryPhone;
			borrowerModel.LendingLimit = borrower.LendingLimit;

			return Task.CompletedTask;
		}

		protected sealed override async Task<bool> CanDeleteAsync(BorrowerModel borrowerModel)
		{
			NullGuard.NotNull(borrowerModel, nameof(borrowerModel));

			if (borrowerModel.Lendings != null)
			{
				return borrowerModel.Lendings.All(_lendingModelHandler.IsDeletable.Compile());
			}

			return await DbContext.Lendings
				.Where(lendingModel => lendingModel.BorrowerIdentifier == borrowerModel.BorrowerIdentifier)
				.AllAsync(_lendingModelHandler.IsDeletable);
		}

		protected sealed override async Task<BorrowerModel> OnDeleteAsync(BorrowerModel borrowerModel)
		{
			NullGuard.NotNull(borrowerModel, nameof(borrowerModel));

			borrowerModel.Lendings = await _lendingModelHandler.DeleteAsync(borrowerModel.Lendings);

			return borrowerModel;
		}

		private IQueryable<BorrowerModel> CreateReader()
		{
			IQueryable<BorrowerModel> reader = Entities;

			if (_includeLendings)
			{
				reader = reader.Include(m => m.Lendings).ThenInclude(m => m.Movie).ThenInclude(m => m.CoreData).ThenInclude(m => m.MediaType)
					.Include(m => m.Lendings).ThenInclude(m => m.Movie).ThenInclude(m => m.MovieGenre)
					.Include(m => m.Lendings).ThenInclude(m => m.Movie).ThenInclude(m => m.SpokenLanguage)
					.Include(m => m.Lendings).ThenInclude(m => m.Music).ThenInclude(m => m.CoreData).ThenInclude(m => m.MediaType)
					.Include(m => m.Lendings).ThenInclude(m => m.Music).ThenInclude(m => m.MusicGenre)
					.Include(m => m.Lendings).ThenInclude(m => m.Book).ThenInclude(m => m.CoreData).ThenInclude(m => m.MediaType)
					.Include(m => m.Lendings).ThenInclude(m => m.Book).ThenInclude(m => m.BookGenre)
					.Include(m => m.Lendings).ThenInclude(m => m.Book).ThenInclude(m => m.WrittenLanguage);
			}

			return reader;
		}

		#endregion
	}
}