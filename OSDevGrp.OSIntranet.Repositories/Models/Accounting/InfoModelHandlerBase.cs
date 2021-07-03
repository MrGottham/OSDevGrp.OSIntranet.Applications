using System;
using System.Collections.Generic;
using System.Linq;
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
    internal abstract class InfoModelHandlerBase<TInfo, TInfoModel> : ModelHandlerBase<TInfo, RepositoryContext, TInfoModel, Tuple<int, string, short, short>, AccountingIdentificationState> where TInfo : class, IInfo where TInfoModel : InfoModelBase, new()
    {
        #region Constructor

        protected InfoModelHandlerBase(RepositoryContext dbContext, IConverter modelConverter)
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Methods

        internal async Task<IEnumerable<TInfoModel>> ReadAsync(IEnumerable<TInfoModel> infoModelCollection)
        {
            NullGuard.NotNull(infoModelCollection, nameof(infoModelCollection));

            return await Task.WhenAll(infoModelCollection.Select(OnReadAsync));
        }

        internal async Task<bool> IsDeletable(IEnumerable<TInfoModel> infoModelCollection)
        {
            NullGuard.NotNull(infoModelCollection, nameof(infoModelCollection));

            bool[] isDeletableCollection = await Task.WhenAll(infoModelCollection.Select(CanDeleteAsync));
            return isDeletableCollection.All(isDeletable => isDeletable);
        }

        internal async Task DeleteAsync(IList<TInfoModel> infoModelCollection)
        {
            NullGuard.NotNull(infoModelCollection, nameof(infoModelCollection));

            TInfoModel infoModelToDelete = infoModelCollection.FirstOrDefault();
            while (infoModelToDelete != null)
            {
                if (await CanDeleteAsync(infoModelToDelete) == false)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.UnableToDeleteOneOrMoreObjects, nameof(TInfoModel))
                        .WithMethodBase(MethodBase.GetCurrentMethod())
                        .Build();
                }

                if (await DeleteAsync(infoModelToDelete) != null)
                {
                    throw new IntranetExceptionBuilder(ErrorCode.UnableToDeleteOneOrMoreObjects, nameof(TInfoModel))
                        .WithMethodBase(MethodBase.GetCurrentMethod())
                        .Build();
                }

                infoModelToDelete = infoModelCollection.FirstOrDefault();
            }
        }

        internal abstract Task<TInfo> DeleteAsync(TInfoModel infoModel);

        protected override async Task<TInfoModel> OnCreateAsync(TInfo info, TInfoModel infoModel)
        {
            NullGuard.NotNull(info, nameof(info))
                .NotNull(infoModel, nameof(infoModel));

            YearMonthModel yearMonthModel = await DbContext.YearMonths.SingleOrDefaultAsync(m => m.Year == info.Year && m.Month == info.Month);
            if (yearMonthModel != null)
            {
                infoModel.YearMonthIdentifier = yearMonthModel.YearMonthIdentifier;
                infoModel.YearMonth = yearMonthModel;

                return infoModel;
            }

            EntityEntry<YearMonthModel> yearMonthModelEntityEntry = await DbContext.YearMonths.AddAsync(ModelConverter.Convert<TInfo, YearMonthModel>(info));
            infoModel.YearMonthIdentifier = yearMonthModelEntityEntry.Entity.YearMonthIdentifier;
            infoModel.YearMonth = yearMonthModelEntityEntry.Entity;

            return infoModel;
        }

        protected override async Task<TInfoModel> OnReadAsync(TInfoModel infoModel)
        {
            NullGuard.NotNull(infoModel, nameof(infoModel));

            infoModel.Deletable = await CanDeleteAsync(infoModel);

            return infoModel;
        }

        protected override async Task OnUpdateAsync(TInfo info, TInfoModel infoModel)
        {
            NullGuard.NotNull(info, nameof(info))
                .NotNull(infoModel, nameof(infoModel));

            YearMonthModel yearMonthModel = await DbContext.YearMonths.SingleAsync(m => m.Year == info.Year && m.Month == info.Month);
            infoModel.YearMonthIdentifier = yearMonthModel.YearMonthIdentifier;
            infoModel.YearMonth = yearMonthModel;
        }

        protected override Task<bool> CanDeleteAsync(TInfoModel infoModel)
        {
            NullGuard.NotNull(infoModel, nameof(infoModel));

            DateTime today = DateTime.Today;

            return Task.FromResult(infoModel.YearMonth.Year > today.Year || infoModel.YearMonth.Year == today.Year && infoModel.YearMonth.Month > today.Month);
        }

        protected override async Task<TInfoModel> OnDeleteAsync(TInfoModel infoModel)
        {
            NullGuard.NotNull(infoModel, nameof(infoModel));

            YearMonthModel yearMonthModel = await DbContext.YearMonths
                .Include(m => m.CreditInfos)
                .Include(m => m.BudgetInfos)
                .SingleAsync(m => m.YearMonthIdentifier == infoModel.YearMonthIdentifier);
            if (yearMonthModel.CreditInfos.Count + yearMonthModel.BudgetInfos.Count > 1)
            {
                return infoModel;
            }

            DbContext.YearMonths.Remove(yearMonthModel);

            return infoModel;
        }

        #endregion
    }
}