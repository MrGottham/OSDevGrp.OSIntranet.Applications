using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Models.Common
{
    internal class NationalityModelHandler : GenericCategoryModelHandlerBase<INationality, NationalityModel>
    {
        #region Constructor

        public NationalityModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<NationalityModel> Entities => DbContext.Nationalities;

        #endregion

        #region Methods

        protected override Expression<Func<NationalityModel, bool>> EntitySelector(int primaryKey) => nationalityModel => nationalityModel.NationalityIdentifier == primaryKey;

        protected override Task<bool> CanDeleteAsync(NationalityModel nationalityModel)
        {
            NullGuard.NotNull(nationalityModel, nameof(nationalityModel));

            return Task.FromResult(true);
        }

        #endregion
    }
}