using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;

namespace OSDevGrp.OSIntranet.Repositories
{
    public class ContactRepository : RepositoryBase, IContactRepository
    {
        #region Private variables

        private readonly IConverter _contactModelConverter = new ContactModelConverter();

        #endregion

        #region Constructor

        public ContactRepository(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IEnumerable<ICountry>> GetCountriesAsync()
        {
            return Task.Run(() => GetCountries());
        }

        public Task<ICountry> GetCountryAsync(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            return Task.Run(() => GetCountry(code));
        }

        public Task<ICountry> CreateCountryAsync(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            return Task.Run(() => CreateCountry(country));
        }

        public Task<ICountry> UpdateCountryAsync(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            return Task.Run(() => UpdateCountry(country));
        }

        public Task<ICountry> DeleteCountryAsync(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            return Task.Run(() => DeleteCountry(code));
        }

        private IEnumerable<ICountry> GetCountries()
        {
            return Execute(() =>
                {
                    using (ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        return context.Countries.AsParallel()
                            .Select(countryModel =>
                            {
                                using (ContactContext subContext = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                                {
                                    countryModel.Deletable = CanDeleteCountry(subContext, countryModel.Code);
                                }

                                return _contactModelConverter.Convert<CountryModel, ICountry>(countryModel);
                            })
                            .OrderBy(countryModel => countryModel.Name)
                            .ToList();
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private ICountry GetCountry(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            return Execute(() =>
                {
                    using (ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        CountryModel countryModel = context.Countries.Find(code);
                        if (countryModel == null)
                        {
                            return null;
                        }

                        countryModel.Deletable = CanDeleteCountry(context, countryModel.Code);

                        return _contactModelConverter.Convert<CountryModel, ICountry>(countryModel);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private ICountry CreateCountry(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            return Execute(() =>
                {
                    using (ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        CountryModel countryModel = _contactModelConverter.Convert<ICountry, CountryModel>(country);

                        context.Countries.Add(countryModel);

                        context.SaveChanges();

                        return GetCountry(countryModel.Code);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private ICountry UpdateCountry(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            return Execute(() =>
                {
                    using (ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        CountryModel countryModel = context.Countries.Find(country.Code);
                        if (countryModel == null)
                        {
                            return null;
                        }

                        countryModel.Name = country.Name;
                        countryModel.UniversalName = country.UniversalName;
                        countryModel.PhonePrefix = country.PhonePrefix;

                        context.SaveChanges();

                        return GetCountry(countryModel.Code);
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private ICountry DeleteCountry(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            return Execute(() =>
                {
                    using (ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                    {
                        CountryModel countryModel = context.Countries.Find(code);
                        if (countryModel == null)
                        {
                            return null;
                        }

                        if (CanDeleteCountry(context, countryModel.Code) == false)
                        {
                            return GetCountry(countryModel.Code);
                        }

                        context.Countries.Remove(countryModel);

                        context.SaveChanges();

                        return null;
                    }
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeleteCountry(ContactContext context, string code)
        {
            NullGuard.NotNull(context, nameof(context))
                .NotNullOrWhiteSpace(code, nameof(code));

            return false;
        }

        #endregion
    }
}
