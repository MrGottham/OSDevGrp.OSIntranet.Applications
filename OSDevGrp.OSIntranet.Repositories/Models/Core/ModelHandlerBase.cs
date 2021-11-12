using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Models.Core
{
    internal abstract class ModelHandlerBase<TDomainModel, TDbContext, TEntityModel, TPrimaryKey> : ModelHandlerBase<TDomainModel, TDbContext, TEntityModel, TPrimaryKey, object> where TDomainModel : class where TDbContext : DbContext where TEntityModel : class, new()
    {
        #region Constructor

        protected ModelHandlerBase(TDbContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion
    }

    internal abstract class ModelHandlerBase<TDomainModel, TDbContext, TEntityModel, TPrimaryKey, TPrepareReadState> : IDisposable where TDomainModel : class where TDbContext : DbContext where TEntityModel : class, new() where TPrepareReadState : class
    {
        #region Constructor

        protected ModelHandlerBase(TDbContext dbContext, IConverter modelConverter)
        {
            NullGuard.NotNull(dbContext, nameof(dbContext))
                .NotNull(modelConverter, nameof(modelConverter));

            DbContext = dbContext;
            ModelConverter = modelConverter;
        }

        #endregion

        #region Properties

        protected TDbContext DbContext { get; }

        protected IConverter ModelConverter { get; }

        protected abstract DbSet<TEntityModel> Entities { get; }

        protected abstract Func<TDomainModel, TPrimaryKey> PrimaryKey { get; }

        protected virtual IQueryable<TEntityModel> Reader => Entities;

        protected virtual IQueryable<TEntityModel> UpdateReader => Reader;

        protected virtual IQueryable<TEntityModel> DeleteReader => Reader;

        #endregion

        #region Methods

        public void Dispose()
        {
            DbContext.Dispose();

            OnDispose();
        }

        internal async Task<TDomainModel> CreateAsync(TDomainModel domainModel, TPrepareReadState prepareReadState = null)
        {
            NullGuard.NotNull(domainModel, nameof(domainModel));

            return (await CreateAsync(new[] {domainModel}, prepareReadState)).SingleOrDefault();
        }

        internal async Task<IEnumerable<TDomainModel>> CreateAsync(IEnumerable<TDomainModel> domainModelCollection, TPrepareReadState prepareReadState = null, bool saveOnForEach = false)
        {
            NullGuard.NotNull(domainModelCollection, nameof(domainModelCollection));

            IDictionary<TDomainModel, EntityEntry<TEntityModel>> createdDomainModelEntityEntryDictionary = new ConcurrentDictionary<TDomainModel, EntityEntry<TEntityModel>>();
            foreach (TDomainModel domainModel in domainModelCollection)
            {
                TEntityModel entityModel = ModelConverter.Convert<TDomainModel, TEntityModel>(domainModel);
                EntityEntry<TEntityModel> entityEntry = await Entities.AddAsync(await OnCreateAsync(domainModel, entityModel));

                if (saveOnForEach)
                {
                    await DbContext.SaveChangesAsync();
                }

                createdDomainModelEntityEntryDictionary.Add(domainModel, entityEntry);
            }

            if (saveOnForEach == false)
            {
                await DbContext.SaveChangesAsync();
            }

            if (createdDomainModelEntityEntryDictionary.Count > 0 && prepareReadState != null)
            {
                await PrepareReadAsync(prepareReadState);
            }

            IList<TDomainModel> createdDomainModelCollection = new List<TDomainModel>();
            foreach (KeyValuePair<TDomainModel, EntityEntry<TEntityModel>> item in createdDomainModelEntityEntryDictionary)
            {
                TDomainModel riddenDomainModel = await OnReloadAsync(item.Key, item.Value.Entity, prepareReadState);
                if (riddenDomainModel == null)
                {
                    continue;
                }

                createdDomainModelCollection.Add(riddenDomainModel);
            }

            return createdDomainModelCollection;
        }

        internal async Task<TDomainModel> ReadAsync(TPrimaryKey primaryKey, TPrepareReadState prepareReadState = null)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey));

            if (prepareReadState != null)
            {
                await PrepareReadAsync(primaryKey, prepareReadState);
            }

            TEntityModel entityModel = await Reader.SingleOrDefaultAsync(EntitySelector(primaryKey));
            if (entityModel == null)
            {
                return null;
            }

            return ModelConverter.Convert<TEntityModel, TDomainModel>(await OnReadAsync(entityModel));
        }

        internal async Task<IEnumerable<TDomainModel>> ReadAsync(Expression<Func<TEntityModel, bool>> filterPredicate = null, TPrepareReadState prepareReadState = null)
        {
            if (prepareReadState != null)
            {
                await PrepareReadAsync(prepareReadState);
            }

            IQueryable<TEntityModel> entityModelReader = Reader;
            if (filterPredicate != null)
            {
                entityModelReader = entityModelReader.Where(filterPredicate);
            }

            TEntityModel[] entityModelCollection = await Task.FromResult(entityModelReader.ToArray());

            Task<TEntityModel>[] readEntityModelTaskCollection = entityModelCollection.Select(OnReadAsync).ToArray();
            TEntityModel[] riddenEntityModelCollection = await Task.WhenAll(readEntityModelTaskCollection);

            TDomainModel[] domainModelCollection = riddenEntityModelCollection.AsParallel()
                .Select(riddenEntityModel => ModelConverter.Convert<TEntityModel, TDomainModel>(riddenEntityModel))
                .ToArray();

            return (await SortAsync(domainModelCollection)).ToList();
        }

        internal async Task<TDomainModel> UpdateAsync(TDomainModel domainModel, TPrepareReadState prepareReadState = null)
        {
            NullGuard.NotNull(domainModel, nameof(domainModel));

            return (await UpdateAsync(new[] {domainModel}, prepareReadState)).SingleOrDefault();
        }

        internal async Task<IEnumerable<TDomainModel>> UpdateAsync(IEnumerable<TDomainModel> domainModelCollection, TPrepareReadState prepareReadState = null, bool saveOnForEach = false)
        {
            NullGuard.NotNull(domainModelCollection, nameof(domainModelCollection));

            IDictionary<TPrimaryKey, TEntityModel> updatedPrimaryKeyEntityModelDictionary = new ConcurrentDictionary<TPrimaryKey, TEntityModel>();
            foreach (TDomainModel domainModel in domainModelCollection)
            {
                TPrimaryKey primaryKey = PrimaryKey(domainModel);

                TEntityModel entityModel = await UpdateReader.SingleOrDefaultAsync(EntitySelector(primaryKey));
                if (entityModel == null)
                {
                    continue;
                }

                await OnUpdateAsync(domainModel, entityModel);

                if (saveOnForEach)
                {
                    await DbContext.SaveChangesAsync();
                }

                updatedPrimaryKeyEntityModelDictionary.Add(primaryKey, entityModel);
            }

            if (saveOnForEach == false)
            {
                await DbContext.SaveChangesAsync();
            }

            if (updatedPrimaryKeyEntityModelDictionary.Count > 0 && prepareReadState != null)
            {
                await PrepareReadAsync(prepareReadState);
            }

            IList<TDomainModel> updatedDomainModelCollection = new List<TDomainModel>();
            foreach (KeyValuePair<TPrimaryKey, TEntityModel> item in updatedPrimaryKeyEntityModelDictionary)
            {
                TDomainModel riddenDomainModel = await OnReloadAsync(item.Key, item.Value, prepareReadState);
                if (riddenDomainModel == null)
                {
                    continue;
                }

                updatedDomainModelCollection.Add(riddenDomainModel);
            }

            return updatedDomainModelCollection;
        }

        internal async Task<TDomainModel> DeleteAsync(TPrimaryKey primaryKey, TPrepareReadState prepareReadState = null)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey));

            return (await DeleteAsync(new[] {primaryKey}, prepareReadState)).SingleOrDefault();
        }

        internal async Task<IEnumerable<TDomainModel>> DeleteAsync(IEnumerable<TPrimaryKey> primaryKeyCollection, TPrepareReadState prepareReadState = null)
        {
            NullGuard.NotNull(primaryKeyCollection, nameof(primaryKeyCollection));

            if (prepareReadState != null)
            {
                await PrepareReadAsync(prepareReadState);
            }

            IList<TDomainModel> nonDeletedDomainModelCollection = new List<TDomainModel>();
            foreach (TPrimaryKey primaryKey in primaryKeyCollection)
            {
                if (prepareReadState != null)
                {
                    await PrepareReadAsync(primaryKey, prepareReadState);
                }

                TEntityModel entityModel = await DeleteReader.SingleOrDefaultAsync(EntitySelector(primaryKey));
                if (entityModel == null)
                {
                    continue;
                }

                TEntityModel riddenEntityModel = await OnReadAsync(entityModel);
                if (riddenEntityModel == null)
                {
                    continue;
                }

                if (await CanDeleteAsync(riddenEntityModel) == false)
                {
                    nonDeletedDomainModelCollection.Add(await OnReloadAsync(primaryKey, riddenEntityModel, prepareReadState));
                }

                Entities.Remove(await OnDeleteAsync(riddenEntityModel));
            }

            await DbContext.SaveChangesAsync();

            return nonDeletedDomainModelCollection;
        }

        protected abstract Expression<Func<TEntityModel, bool>> EntitySelector(TPrimaryKey primaryKey);

        protected abstract Task<IEnumerable<TDomainModel>> SortAsync(IEnumerable<TDomainModel> domainModelCollection);

        protected virtual void OnDispose()
        {
        }

        protected virtual Task<TEntityModel> OnCreateAsync(TDomainModel domainModel, TEntityModel entityModel)
        {
            NullGuard.NotNull(domainModel, nameof(domainModel))
                .NotNull(entityModel, nameof(entityModel));

            return Task.FromResult(entityModel);
        }

        protected virtual Task PrepareReadAsync(TPrepareReadState prepareReadState)
        {
            NullGuard.NotNull(prepareReadState, nameof(prepareReadState));

            return Task.CompletedTask;
        }

        protected virtual Task PrepareReadAsync(TPrimaryKey primaryKey, TPrepareReadState prepareReadState)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey))
                .NotNull(prepareReadState, nameof(prepareReadState));

            return Task.CompletedTask;
        }

        protected virtual Task<TEntityModel> OnReadAsync(TEntityModel entityModel)
        {
            NullGuard.NotNull(entityModel, nameof(entityModel));

            return Task.FromResult(entityModel);
        }

        protected abstract Task OnUpdateAsync(TDomainModel domainModel, TEntityModel entityModel);

        protected virtual Task<bool> CanDeleteAsync(TEntityModel entityModel)
        {
            NullGuard.NotNull(entityModel, nameof(entityModel));

            return Task.FromResult(true);
        }

        protected virtual Task<TEntityModel> OnDeleteAsync(TEntityModel entityModel)
        {
            NullGuard.NotNull(entityModel, nameof(entityModel));

            return Task.FromResult(entityModel);
        }

        protected virtual Task<TDomainModel> OnReloadAsync(TDomainModel domainModel, TEntityModel entityModel, TPrepareReadState prepareReadState)
        {
            NullGuard.NotNull(domainModel, nameof(domainModel))
                .NotNull(entityModel, nameof(entityModel));

            return OnReloadAsync(PrimaryKey(domainModel), entityModel, prepareReadState);
        }

        protected virtual Task<TDomainModel> OnReloadAsync(TPrimaryKey primaryKey, TEntityModel entityModel, TPrepareReadState prepareReadState)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey))
                .NotNull(entityModel, nameof(entityModel));

            return ReadAsync(primaryKey, prepareReadState);
        }

        #endregion
    }
}