using System;
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
    internal abstract class ModelHandlerBase<TDomainModel, TDbContext, TEntityModel, TPrimaryKey> : IDisposable where TDomainModel : class where TDbContext : DbContext where TEntityModel : class, new()
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

        internal async Task<TDomainModel> CreateAsync(TDomainModel domainModel)
        {
            NullGuard.NotNull(domainModel, nameof(domainModel));

            TEntityModel entityModel = ModelConverter.Convert<TDomainModel, TEntityModel>(domainModel);
            EntityEntry<TEntityModel> entityEntry = await Entities.AddAsync(await OnCreateAsync(domainModel, entityModel));

            await DbContext.SaveChangesAsync();

            return await OnReloadAsync(domainModel, entityEntry.Entity);
        }

        internal async Task<TDomainModel> ReadAsync(TPrimaryKey primaryKey)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey));

            TEntityModel entityModel = await Reader.SingleOrDefaultAsync(EntitySelector(primaryKey));
            if (entityModel == null)
            {
                return null;
            }

            return ModelConverter.Convert<TEntityModel, TDomainModel>(await OnReadAsync(entityModel));
        }

        internal async Task<IEnumerable<TDomainModel>> ReadAsync(Func<TEntityModel, bool> filterPredicate = null)
        {
            TEntityModel[] entityModelCollection = await Task.FromResult((filterPredicate != null ? Reader.Where(filterPredicate) : Reader).ToArray());

            Task<TEntityModel>[] readEntityModelTaskCollection = entityModelCollection.Select(OnReadAsync).ToArray();
            TEntityModel[] riddenEntityModelCollection = await Task.WhenAll(readEntityModelTaskCollection);

            TDomainModel[] domainModelCollection = riddenEntityModelCollection.AsParallel()
                .Select(riddenEntityModel => ModelConverter.Convert<TEntityModel, TDomainModel>(riddenEntityModel))
                .ToArray();

            return (await SortAsync(domainModelCollection)).ToList();
        }

        internal async Task<TDomainModel> UpdateAsync(TDomainModel domainModel)
        {
            NullGuard.NotNull(domainModel, nameof(domainModel));

            TPrimaryKey primaryKey = PrimaryKey(domainModel);

            TEntityModel entityModel = await UpdateReader.SingleOrDefaultAsync(EntitySelector(primaryKey));
            if (entityModel == null)
            {
                return null;
            }

            await OnUpdateAsync(domainModel, entityModel);

            await DbContext.SaveChangesAsync();

            return await OnReloadAsync(primaryKey, entityModel);
        }

        internal async Task<TDomainModel> DeleteAsync(TPrimaryKey primaryKey)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey));

            TEntityModel entityModel = await DeleteReader.SingleOrDefaultAsync(EntitySelector(primaryKey));
            if (entityModel == null)
            {
                return null;
            }

            if (await CanDeleteAsync(entityModel) == false)
            {
                return await OnReloadAsync(primaryKey, entityModel);
            }

            Entities.Remove(await OnDeleteAsync(entityModel));

            await DbContext.SaveChangesAsync();

            return null;
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

        protected virtual Task<TDomainModel> OnReloadAsync(TDomainModel domainModel, TEntityModel entityModel)
        {
            NullGuard.NotNull(domainModel, nameof(domainModel))
                .NotNull(entityModel, nameof(entityModel));

            return OnReloadAsync(PrimaryKey(domainModel), entityModel);
        }

        protected virtual Task<TDomainModel> OnReloadAsync(TPrimaryKey primaryKey, TEntityModel entityModel)
        {
            NullGuard.NotNull(primaryKey, nameof(primaryKey))
                .NotNull(entityModel, nameof(entityModel));

            return ReadAsync(primaryKey);
        }

        #endregion
    }
}