﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Models.Core;

namespace OSDevGrp.OSIntranet.Repositories.Models.Contacts
{
    internal class PostalCodeModelHandler : ModelHandlerBase<IPostalCode, RepositoryContext, PostalCodeModel, Tuple<string, string>>
    {
        #region Constructor

        public PostalCodeModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<PostalCodeModel> Entities => DbContext.PostalCodes;

        protected override Func<IPostalCode, Tuple<string, string>> PrimaryKey => postalCode => new Tuple<string, string>(postalCode.Country.Code, postalCode.Code);

        protected override IQueryable<PostalCodeModel> Reader => Entities.Include(m => m.Country);

        #endregion

        #region Methods

        protected override Expression<Func<PostalCodeModel, bool>> EntitySelector(Tuple<string, string> primaryKey) => postalCodeModel => postalCodeModel.CountryCode == primaryKey.Item1 && postalCodeModel.PostalCode == primaryKey.Item2;

        protected override Task<IEnumerable<IPostalCode>> SortAsync(IEnumerable<IPostalCode> postalCodeCollection)
        {
            NullGuard.NotNull(postalCodeCollection, nameof(postalCodeCollection));

            return Task.FromResult(postalCodeCollection.OrderBy(m => m.City).AsEnumerable());
        }

        protected override async Task<PostalCodeModel> OnCreateAsync(IPostalCode postalCode, PostalCodeModel postalCodeModel)
        {
            NullGuard.NotNull(postalCode, nameof(postalCode))
                .NotNull(postalCodeModel, nameof(postalCodeModel));

            CountryModel countryModel = await DbContext.Countries.SingleAsync(m => m.Code == postalCode.Country.Code);
            postalCodeModel.CountryCode = countryModel.Code;
            postalCodeModel.Country = countryModel;

            return postalCodeModel;
        }

        protected override async Task<PostalCodeModel> OnReadAsync(PostalCodeModel postalCodeModel)
        {
            NullGuard.NotNull(postalCodeModel, nameof(postalCodeModel));

            postalCodeModel.Deletable = await CanDeleteAsync(postalCodeModel);

            return postalCodeModel;
        }

        protected override Task OnUpdateAsync(IPostalCode postalCode, PostalCodeModel postalCodeModel)
        {
            NullGuard.NotNull(postalCode, nameof(postalCode))
                .NotNull(postalCodeModel, nameof(postalCodeModel));

            postalCodeModel.City = postalCode.City;
            postalCodeModel.State = postalCode.State;

            return Task.CompletedTask;
        }

        #endregion
    }
}