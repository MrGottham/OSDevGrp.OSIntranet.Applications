using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.ModelHandlers.Core
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

        protected abstract Func<TDbContext, DbSet<TEntityModel>> Entities { get; }

        protected abstract Func<TDomainModel, TPrimaryKey> PrimaryKey { get; }

        protected abstract Expression<Func<TEntityModel, bool>> EntitySelector(TPrimaryKey primaryKey);

        protected virtual IQueryable<TEntityModel> Reader => Entities(DbContext);

        protected virtual IQueryable<TEntityModel> UpdateReader => Reader;

        protected virtual IQueryable<TEntityModel> DeleteReader => Reader;

        #endregion

        #region Methods

        public void Dispose()
        {
            DbContext.Dispose();
        }

        internal async Task<TDomainModel> CreateAsync(TDomainModel domainModel)
        {
            NullGuard.NotNull(domainModel, nameof(domainModel));

            TEntityModel entityModel = ModelConverter.Convert<TDomainModel, TEntityModel>(domainModel);
            await Entities(DbContext).AddAsync(await OnCreateAsync(domainModel, entityModel));

            await DbContext.SaveChangesAsync();

            return await ReadAsync(PrimaryKey(domainModel));
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

            TDomainModel[] domainModelCollection = await Task.FromResult(entityModelCollection
                .Select(entityModel => ModelConverter.Convert<TEntityModel, TDomainModel>(OnReadAsync(entityModel).GetAwaiter().GetResult()))
                .ToArray());

            return (await Sort(domainModelCollection)).ToList();
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

            return await ReadAsync(primaryKey);
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
                return await ReadAsync(primaryKey);
            }

            Entities(DbContext).Remove(await OnDeleteAsync(entityModel));

            await DbContext.SaveChangesAsync();

            return null;
        }

        protected abstract Task<IEnumerable<TDomainModel>> Sort(IEnumerable<TDomainModel> domainModelCollection);

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

        #endregion
    }
}