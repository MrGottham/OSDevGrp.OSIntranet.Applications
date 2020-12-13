﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Accounting
{
    internal abstract class AccountModelHandlerBase<TAccount, TAccountModel> : ModelHandlerBase<TAccount, RepositoryContext, TAccountModel, Tuple<int, string>> where TAccount : class, IAccountBase where TAccountModel : AccountModelBase, new() 
    {
        #region Private variables

        private readonly DateTime _statusDate;

        #endregion

        #region Constructor

        protected AccountModelHandlerBase(RepositoryContext dbContext, IConverter modelConverter, DateTime statusDate, bool includePostingLines) 
            : base(dbContext, modelConverter)
        {
            _statusDate = statusDate.Date;

            IncludePostingLines = includePostingLines;
        }

        #endregion

        #region Properties

        protected bool IncludePostingLines { get; }

        protected override Func<TAccount, Tuple<int, string>> PrimaryKey => account => new Tuple<int, string>(account.Accounting.Number, account.AccountNumber);

        #endregion

        #region Methods

        internal async Task<bool> ExistsAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return await ReadAsync(accountingNumber, accountNumber) != null;
        }

        internal Task<TAccount> ReadAsync(int accountingNumber, string accountNumber)
        {
            NullGuard.NotNullOrWhiteSpace(accountNumber, nameof(accountNumber));

            return ReadAsync(new Tuple<int, string>(accountingNumber, accountNumber));
        }

        internal async Task<IEnumerable<TAccountModel>> ReadAsync(IEnumerable<TAccountModel> accountModelCollection)
        {
            NullGuard.NotNull(accountModelCollection, nameof(accountModelCollection));

            return await Task.WhenAll(accountModelCollection.Select(OnReadAsync));
        }

        internal async Task DeleteAsync(IList<TAccountModel> accountModelCollection)
        {
            NullGuard.NotNull(accountModelCollection, nameof(accountModelCollection));

            TAccountModel accountModelToDelete = accountModelCollection.FirstOrDefault();
            while (accountModelToDelete != null)
            {
                if (await CanDeleteAsync(accountModelToDelete) == false)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.UnableToDeleteOneOrMoreObjects, nameof(TAccountModel))
                        .WithMethodBase(MethodBase.GetCurrentMethod())
                        .Build();
                }

                if (await DeleteAsync(new Tuple<int, string>(accountModelToDelete.AccountingIdentifier, accountModelToDelete.AccountNumber)) != null)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.UnableToDeleteOneOrMoreObjects, nameof(TAccountModel))
                        .WithMethodBase(MethodBase.GetCurrentMethod())
                        .Build();
                }

                accountModelToDelete = accountModelCollection.FirstOrDefault();
            }
        }

        protected override Expression<Func<TAccountModel, bool>> EntitySelector(Tuple<int, string> primaryKey) => accountModel => accountModel.AccountingIdentifier == primaryKey.Item1 && accountModel.AccountNumber == primaryKey.Item2;

        protected override Task<IEnumerable<TAccount>> SortAsync(IEnumerable<TAccount> accountCollection)
        {
            NullGuard.NotNull(accountCollection, nameof(accountCollection));

            return Task.FromResult(accountCollection.OrderBy(account => account.Accounting.Number).ThenBy(account => account.AccountNumber).AsEnumerable());
        }

        protected override async Task<TAccountModel> OnCreateAsync(TAccount account, TAccountModel accountModel)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(accountModel, nameof(accountModel));

            EntityEntry<BasicAccountModel> basicAccountEntityEntry = await DbContext.BasicAccounts.AddAsync(accountModel.BasicAccount);

            accountModel.Accounting = await DbContext.Accountings.SingleAsync(accountingModel => accountingModel.AccountingIdentifier == account.Accounting.Number);
            accountModel.BasicAccount = basicAccountEntityEntry.Entity;

            return accountModel;
        }

        protected override async Task<TAccountModel> OnReadAsync(TAccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            accountModel.StatusDate = _statusDate;
            accountModel.Deletable = await CanDeleteAsync(accountModel);

            // TODO: Call OnReadAsync on each posting line.

            return accountModel;
        }

        protected override async Task OnUpdateAsync(TAccount account, TAccountModel accountModel)
        {
            NullGuard.NotNull(account, nameof(account))
                .NotNull(accountModel, nameof(accountModel));

            accountModel.AccountingIdentifier = account.Accounting.Number;
            accountModel.Accounting = await DbContext.Accountings.SingleAsync(accountingModel => accountingModel.AccountingIdentifier == account.Accounting.Number);
            accountModel.AccountNumber = account.AccountNumber;
            accountModel.BasicAccount.AccountName = account.AccountName;
            accountModel.BasicAccount.Description = account.Description;
            accountModel.BasicAccount.Note = account.Note;
        }

        protected override async Task<TAccountModel> OnDeleteAsync(TAccountModel accountModel)
        {
            NullGuard.NotNull(accountModel, nameof(accountModel));

            BasicAccountModel basicAccountModel = await DbContext.BasicAccounts.SingleAsync(model => model.BasicAccountIdentifier == accountModel.BasicAccountIdentifier);
            DbContext.BasicAccounts.Remove(basicAccountModel);

            return accountModel;
        }

        #endregion
    }
}