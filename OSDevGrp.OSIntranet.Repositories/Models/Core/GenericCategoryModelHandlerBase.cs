using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.Core
{
    internal abstract class GenericCategoryModelHandlerBase<TGenericCategory, TGenericCategoryModel> : ModelHandlerBase<TGenericCategory, RepositoryContext, TGenericCategoryModel, int> where TGenericCategory : class, IGenericCategory where TGenericCategoryModel : GenericCategoryModelBase, new()
    {
        #region Constructor

        protected GenericCategoryModelHandlerBase(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected sealed override Func<TGenericCategory, int> PrimaryKey => genericCategory => genericCategory.Number;

        #endregion

        #region Methods

        protected sealed override Task<IEnumerable<TGenericCategory>> SortAsync(IEnumerable<TGenericCategory> genericCategoryCollection)
        {
            NullGuard.NotNull(genericCategoryCollection, nameof(genericCategoryCollection));

            return Task.FromResult(genericCategoryCollection.OrderBy(genericCategory => genericCategory.Number).AsEnumerable());
        }

        protected sealed override Task<TGenericCategoryModel> OnCreateAsync(TGenericCategory genericCategory, TGenericCategoryModel genericCategoryModel)
        {
            NullGuard.NotNull(genericCategory, nameof(genericCategory))
                .NotNull(genericCategoryModel, nameof(genericCategoryModel));

            return Task.FromResult(genericCategoryModel);
        }

        protected sealed override async Task<TGenericCategoryModel> OnReadAsync(TGenericCategoryModel genericCategoryModel)
        {
            NullGuard.NotNull(genericCategoryModel, nameof(genericCategoryModel));

            genericCategoryModel.Deletable = await CanDeleteAsync(genericCategoryModel);

            return genericCategoryModel;
        }

        protected override Task OnUpdateAsync(TGenericCategory genericCategory, TGenericCategoryModel genericCategoryModel)
        {
            NullGuard.NotNull(genericCategory, nameof(genericCategory))
                .NotNull(genericCategoryModel, nameof(genericCategoryModel));

            genericCategoryModel.Name = genericCategory.Name;

            return Task.CompletedTask;
        }

        protected override Task<bool> CanDeleteAsync(TGenericCategoryModel genericCategoryModel)
        {
            NullGuard.NotNull(genericCategoryModel, nameof(genericCategoryModel));

            return Task.FromResult(false);
        }

        protected sealed override Task<TGenericCategoryModel> OnDeleteAsync(TGenericCategoryModel genericCategoryModel)
        {
            NullGuard.NotNull(genericCategoryModel, nameof(genericCategoryModel));

            return Task.FromResult(genericCategoryModel);
        }

        #endregion
    }
}