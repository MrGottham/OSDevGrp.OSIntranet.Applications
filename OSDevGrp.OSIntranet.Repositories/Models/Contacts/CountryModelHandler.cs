using System;
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
    internal class CountryModelHandler : ModelHandlerBase<ICountry, RepositoryContext, CountryModel, string>
    {
        #region Constructor

        public CountryModelHandler(RepositoryContext dbContext, IConverter modelConverter) 
            : base(dbContext, modelConverter)
        {
        }

        #endregion

        #region Properties

        protected override DbSet<CountryModel> Entities => DbContext.Countries;

        protected override Func<ICountry, string> PrimaryKey => country => country.Code;

        #endregion

        #region Methods

        protected override Expression<Func<CountryModel, bool>> EntitySelector(string primaryKey) => countryModel => countryModel.Code == primaryKey;

        protected override Task<IEnumerable<ICountry>> SortAsync(IEnumerable<ICountry> countryCollection)
        {
            NullGuard.NotNull(countryCollection, nameof(countryCollection));

            return Task.FromResult(countryCollection.OrderBy(m => m.Name).AsEnumerable());
        }

        protected override async Task<CountryModel> OnReadAsync(CountryModel countryModel)
        {
            NullGuard.NotNull(countryModel, nameof(countryModel));

            countryModel.Deletable = await CanDeleteAsync(countryModel);

            return countryModel;
        }

        protected override Task OnUpdateAsync(ICountry country, CountryModel countryModel)
        {
            NullGuard.NotNull(country, nameof(country))
                .NotNull(countryModel, nameof(countryModel));

            countryModel.Name = country.Name;
            countryModel.UniversalName = country.UniversalName;
            countryModel.PhonePrefix = country.PhonePrefix;

            return Task.CompletedTask;
        }

        protected override async Task<bool> CanDeleteAsync(CountryModel countryModel)
        {
            NullGuard.NotNull(countryModel, nameof(countryModel));

            if (countryModel.PostalCodes != null)
            {
                return countryModel.PostalCodes.Any() == false;
            }

            return await DbContext.PostalCodes.FirstOrDefaultAsync(postalCodeModel => postalCodeModel.CountryCode == countryModel.Code) == null;
        }

        #endregion
    }
}