﻿using Microsoft.Extensions.Logging;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Contexts;
using OSDevGrp.OSIntranet.Repositories.Converters;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using OSDevGrp.OSIntranet.Repositories.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories
{
    internal class ContactRepository(RepositoryContext repositoryContext, ILoggerFactory loggerFactory) : DatabaseRepositoryBase<RepositoryContext>(repositoryContext, loggerFactory), IContactRepository
    {
        #region Methods

        public Task<IContact> ApplyContactSupplementAsync(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return ExecuteAsync(async () =>
                {
                    using ContactSupplementModelHandler contactSupplementModelHandler = new ContactSupplementModelHandler(DbContext, ContactModelConverter.Create(), AccountingModelConverter.Create());
                    return await contactSupplementModelHandler.ApplyContactSupplementAsync(contact);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IContact>> ApplyContactSupplementAsync(IEnumerable<IContact> contacts)
        {
            NullGuard.NotNull(contacts, nameof(contacts));

            return ExecuteAsync(async () =>
                {
                    using ContactSupplementModelHandler contactSupplementModelHandler = new ContactSupplementModelHandler(DbContext, ContactModelConverter.Create(), AccountingModelConverter.Create());
                    return await contactSupplementModelHandler.ApplyContactSupplementAsync(contacts);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContact> CreateOrUpdateContactSupplementAsync(IContact contact, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return ExecuteAsync(async () =>
                {
                    using ContactSupplementModelHandler contactSupplementModelHandler = new ContactSupplementModelHandler(DbContext, ContactModelConverter.Create(), AccountingModelConverter.Create());
                    return await contactSupplementModelHandler.CreateOrUpdateContactSupplementAsync(contact, existingExternalIdentifier);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContact> DeleteContactSupplementAsync(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return ExecuteAsync(async () =>
                {
                    using ContactSupplementModelHandler contactSupplementModelHandler = new ContactSupplementModelHandler(DbContext, ContactModelConverter.Create(), AccountingModelConverter.Create());
                    return await contactSupplementModelHandler.DeleteAsync(contact);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IContactGroup>> GetContactGroupsAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using ContactGroupModelHandler contactGroupModelHandler = new ContactGroupModelHandler(DbContext, ContactModelConverter.Create());
                    return await contactGroupModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactGroup> GetContactGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using ContactGroupModelHandler contactGroupModelHandler = new ContactGroupModelHandler(DbContext, ContactModelConverter.Create());
                    return await contactGroupModelHandler.ReadAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactGroup> CreateContactGroupAsync(IContactGroup contactGroup)
        {
            NullGuard.NotNull(contactGroup, nameof(contactGroup));

            return ExecuteAsync(async () =>
                {
                    using ContactGroupModelHandler contactGroupModelHandler = new ContactGroupModelHandler(DbContext, ContactModelConverter.Create());
                    return await contactGroupModelHandler.CreateAsync(contactGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactGroup> UpdateContactGroupAsync(IContactGroup contactGroup)
        {
            NullGuard.NotNull(contactGroup, nameof(contactGroup));

            return ExecuteAsync(async () =>
                {
                    using ContactGroupModelHandler contactGroupModelHandler = new ContactGroupModelHandler(DbContext, ContactModelConverter.Create());
                    return await contactGroupModelHandler.UpdateAsync(contactGroup);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IContactGroup> DeleteContactGroupAsync(int number)
        {
            return ExecuteAsync(async () =>
                {
                    using ContactGroupModelHandler contactGroupModelHandler = new ContactGroupModelHandler(DbContext, ContactModelConverter.Create());
                    return await contactGroupModelHandler.DeleteAsync(number);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<ICountry>> GetCountriesAsync()
        {
            return ExecuteAsync(async () =>
                {
                    using CountryModelHandler countryModelHandler = new CountryModelHandler(DbContext, ContactModelConverter.Create());
                    return await countryModelHandler.ReadAsync();
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ICountry> GetCountryAsync(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            return ExecuteAsync(async () =>
                {
                    using CountryModelHandler countryModelHandler = new CountryModelHandler(DbContext, ContactModelConverter.Create());
                    return await countryModelHandler.ReadAsync(code);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ICountry> CreateCountryAsync(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            return ExecuteAsync(async () =>
                {
                    using CountryModelHandler countryModelHandler = new CountryModelHandler(DbContext, ContactModelConverter.Create());
                    return await countryModelHandler.CreateAsync(country);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ICountry> UpdateCountryAsync(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            return ExecuteAsync(async () =>
                {
                    using CountryModelHandler countryModelHandler = new CountryModelHandler(DbContext, ContactModelConverter.Create());
                    return await countryModelHandler.UpdateAsync(country);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<ICountry> DeleteCountryAsync(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            return ExecuteAsync(async () =>
                {
                    using CountryModelHandler countryModelHandler = new CountryModelHandler(DbContext, ContactModelConverter.Create());
                    return await countryModelHandler.DeleteAsync(code);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IEnumerable<IPostalCode>> GetPostalCodesAsync(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            return ExecuteAsync(async () =>
                {
                    using PostalCodeModelHandler postalCodeModelHandler = new PostalCodeModelHandler(DbContext, ContactModelConverter.Create());
                    return await postalCodeModelHandler.ReadAsync(postalCodeModel => postalCodeModel.CountryCode == countryCode);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPostalCode> GetPostalCodeAsync(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            return ExecuteAsync(async () =>
                {
                    using PostalCodeModelHandler postalCodeModelHandler = new PostalCodeModelHandler(DbContext, ContactModelConverter.Create());
                    return await postalCodeModelHandler.ReadAsync(new Tuple<string, string>(countryCode, postalCode));
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPostalCode> CreatePostalCodeAsync(IPostalCode postalCode)
        {
            NullGuard.NotNull(postalCode, nameof(postalCode));

            return ExecuteAsync(async () =>
                {
                    using PostalCodeModelHandler postalCodeModelHandler = new PostalCodeModelHandler(DbContext, ContactModelConverter.Create());
                    return await postalCodeModelHandler.CreateAsync(postalCode);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPostalCode> UpdatePostalCodeAsync(IPostalCode postalCode)
        {
            NullGuard.NotNull(postalCode, nameof(postalCode));

            return ExecuteAsync(async () =>
                {
                    using PostalCodeModelHandler postalCodeModelHandler = new PostalCodeModelHandler(DbContext, ContactModelConverter.Create());
                    return await postalCodeModelHandler.UpdateAsync(postalCode);
                },
                MethodBase.GetCurrentMethod());
        }

        public Task<IPostalCode> DeletePostalCodeAsync(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            return ExecuteAsync(async () =>
                {
                    using PostalCodeModelHandler postalCodeModelHandler = new PostalCodeModelHandler(DbContext, ContactModelConverter.Create());
                    return await postalCodeModelHandler.DeleteAsync(new Tuple<string, string>(countryCode, postalCode));
                },
                MethodBase.GetCurrentMethod());
        }

        #endregion
    }
}