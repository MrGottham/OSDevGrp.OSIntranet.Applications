using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Common
{
    internal class LetterHeadModelHandler : ModelHandlerBase<ILetterHead, CommonContext, LetterHeadModel, int>
    {
        #region Constructor

        public LetterHeadModelHandler(CommonContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<LetterHeadModel> Entities => DbContext.LetterHeads;

        protected override Func<ILetterHead, int> PrimaryKey => letterHead => letterHead.Number;

        #endregion

        #region Methods

        protected override Expression<Func<LetterHeadModel, bool>> EntitySelector(int primaryKey) => letterHeadModel => letterHeadModel.LetterHeadIdentifier == primaryKey;

        protected override Task<IEnumerable<ILetterHead>> SortAsync(IEnumerable<ILetterHead> letterHeadCollection)
        {
            NullGuard.NotNull(letterHeadCollection, nameof(letterHeadCollection));

            return Task.FromResult(letterHeadCollection.OrderBy(letterHead => letterHead.Number).AsEnumerable());
        }

        protected override async Task<LetterHeadModel> OnReadAsync(LetterHeadModel letterHeadModel)
        {
            NullGuard.NotNull(letterHeadModel, nameof(letterHeadModel));

            letterHeadModel.Deletable = await CanDeleteAsync(letterHeadModel);

            return letterHeadModel;
        }

        protected override Task OnUpdateAsync(ILetterHead letterHead, LetterHeadModel letterHeadModel)
        {
            NullGuard.NotNull(letterHead, nameof(letterHead))
                .NotNull(letterHeadModel, nameof(letterHeadModel));

            letterHeadModel.Name = letterHead.Name;
            letterHeadModel.Line1 = letterHead.Line1;
            letterHeadModel.Line2 = letterHead.Line2;
            letterHeadModel.Line3 = letterHead.Line3;
            letterHeadModel.Line4 = letterHead.Line4;
            letterHeadModel.Line5 = letterHead.Line5;
            letterHeadModel.Line6 = letterHead.Line6;
            letterHeadModel.Line7 = letterHead.Line7;
            letterHeadModel.CompanyIdentificationNumber = letterHead.CompanyIdentificationNumber;

            return Task.CompletedTask;
        }

        protected override async Task<bool> CanDeleteAsync(LetterHeadModel letterHeadModel)
        {
            NullGuard.NotNull(letterHeadModel, nameof(letterHeadModel));

            return await DbContext.Accountings.FirstOrDefaultAsync(accountingModel => accountingModel.LetterHeadIdentifier == letterHeadModel.LetterHeadIdentifier) == null;
        }

        #endregion
    }
}