﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        private readonly IConverter _accountingModelConverter = new AccountingModelConverter();

        #endregion

        #region Constructor

        public ContactRepository(IConfiguration configuration, IPrincipalResolver principalResolver, ILoggerFactory loggerFactory)
            : base(configuration, principalResolver, loggerFactory)
        {
        }

        #endregion

        #region Methods

        public Task<IContact> ApplyContactSupplementAsync(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return Task.Run(() => ApplyContactSupplement(contact));
        }

        public Task<IEnumerable<IContact>> ApplyContactSupplementAsync(IEnumerable<IContact> contacts)
        {
            NullGuard.NotNull(contacts, nameof(contacts));

            return Task.Run(() => ApplyContactSupplement(contacts));
        }

        public Task<IContact> CreateOrUpdateContactSupplementAsync(IContact contact, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return Task.Run(() => CreateOrUpdateContactSupplement(contact, existingExternalIdentifier));
        }

        public Task<IContact> DeleteContactSupplementAsync(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return Task.Run(() => DeleteContactSupplement(contact));
        }

        public Task<IEnumerable<IContactGroup>> GetContactGroupsAsync()
        {
            return Task.Run(GetContactGroups);
        }

        public Task<IContactGroup> GetContactGroupAsync(int number)
        {
            return Task.Run(() => GetContactGroup(number));
        }

        public Task<IContactGroup> CreateContactGroupAsync(IContactGroup contactGroup)
        {
            NullGuard.NotNull(contactGroup, nameof(contactGroup));

            return Task.Run(() => CreateContactGroup(contactGroup));
        }

        public Task<IContactGroup> UpdateContactGroupAsync(IContactGroup contactGroup)
        {
            NullGuard.NotNull(contactGroup, nameof(contactGroup));

            return Task.Run(() => UpdateContactGroup(contactGroup));
        }

        public Task<IContactGroup> DeleteContactGroupAsync(int number)
        {
            return Task.Run(() => DeleteContactGroup(number));
        }

        public Task<IEnumerable<ICountry>> GetCountriesAsync()
        {
            return Task.Run(GetCountries);
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

        public Task<IEnumerable<IPostalCode>> GetPostalCodesAsync(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            return Task.Run(() => GetPostalCodes(countryCode));
        }

        public Task<IPostalCode> GetPostalCodeAsync(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            return Task.Run(() => GetPostalCode(countryCode, postalCode));
        }

        public Task<IPostalCode> CreatePostalCodeAsync(IPostalCode postalCode)
        {
            NullGuard.NotNull(postalCode, nameof(postalCode));

            return Task.Run(() => CreatePostalCode(postalCode));
        }

        public Task<IPostalCode> UpdatePostalCodeAsync(IPostalCode postalCode)
        {
            NullGuard.NotNull(postalCode, nameof(postalCode));

            return Task.Run(() => UpdatePostalCode(postalCode));
        }

        public Task<IPostalCode> DeletePostalCodeAsync(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            return Task.Run(() => DeletePostalCode(countryCode, postalCode));
        }

        private IContact ApplyContactSupplement(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);

                    ContactSupplementModel contactSupplementModel = GetContactSupplementModel(contact, context);
                    if (contactSupplementModel != null)
                    {
                        return contactSupplementModel.ApplyContactSupplement(contact, _contactModelConverter, _accountingModelConverter);
                    }
                    
                    return contact;
                },
                MethodBase.GetCurrentMethod());
        }

        private IEnumerable<IContact> ApplyContactSupplement(IEnumerable<IContact> contacts)
        {
            NullGuard.NotNull(contacts, nameof(contacts));

            return Execute(() => contacts.AsParallel()
                    .Select(ApplyContactSupplement)
                    .ToList(),
                MethodBase.GetCurrentMethod());
        }

        private IContact CreateOrUpdateContactSupplement(IContact contact, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);

                    ContactSupplementModel contactSupplementModel = GetContactSupplementModel(contact, context, existingExternalIdentifier);
                    if (contactSupplementModel == null)
                    {
                        contactSupplementModel = _contactModelConverter.Convert<IContact, ContactSupplementModel>(contact);
                        contactSupplementModel.ContactGroup = context.ContactGroups.Single(contactGroupModel => contactGroupModel.ContactGroupIdentifier == contact.ContactGroup.Number);
                        contactSupplementModel.PaymentTerm = context.PaymentTerms.Single(paymentTermModel => paymentTermModel.PaymentTermIdentifier == contact.PaymentTerm.Number);

                        context.ContactSupplements.Add(contactSupplementModel);

                        context.SaveChanges();

                        return ApplyContactSupplement(contact);
                    }

                    contactSupplementModel.Birthday = contact.Birthday;
                    contactSupplementModel.ContactGroupIdentifier = contact.ContactGroup.Number;
                    contactSupplementModel.ContactGroup = context.ContactGroups.Single(contactGroupModel => contactGroupModel.ContactGroupIdentifier == contact.ContactGroup.Number);
                    contactSupplementModel.Acquaintance = contact.Acquaintance;
                    contactSupplementModel.PersonalHomePage = contact.PersonalHomePage;
                    contactSupplementModel.LendingLimit = contact.LendingLimit;
                    contactSupplementModel.PaymentTermIdentifier = contact.PaymentTerm.Number;
                    contactSupplementModel.PaymentTerm = context.PaymentTerms.Single(paymentTermModel => paymentTermModel.PaymentTermIdentifier == contact.PaymentTerm.Number);

                    string[] externalIdentifierCollection = GetExternalIdentifierCollection(contact);
                    foreach (ContactSupplementBindingModel contactSupplementBindingModel in contactSupplementModel.ContactSupplementBindings.Where(m => externalIdentifierCollection.Any(externalIdentifier => externalIdentifier == m.ExternalIdentifier) == false).ToArray())
                    {
                        contactSupplementModel.ContactSupplementBindings.Remove(contactSupplementBindingModel);
                    }
                    foreach (string externalIdentifier in externalIdentifierCollection.Where(m => contactSupplementModel.ContactSupplementBindings.Any(contactSupplementBindingModel => contactSupplementBindingModel.ExternalIdentifier == m) == false))
                    {
                        ContactSupplementBindingModel contactSupplementBindingModel = new ContactSupplementBindingModel
                        {
                            ContactSupplementIdentifier = contactSupplementModel.ContactSupplementIdentifier,
                            ContactSupplement = contactSupplementModel,
                            ExternalIdentifier = externalIdentifier
                        };
                        contactSupplementModel.ContactSupplementBindings.Add(contactSupplementBindingModel);
                    }

                    context.SaveChanges();

                    return ApplyContactSupplement(contact);
                },
                MethodBase.GetCurrentMethod());
        }

        private IContact DeleteContactSupplement(IContact contact)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);

                    ContactSupplementModel contactSupplementModel = GetContactSupplementModel(contact, context);
                    if (contactSupplementModel == null)
                    {
                        return null;
                    }

                    context.ContactSupplements.Remove(contactSupplementModel);

                    context.SaveChanges();

                    return (IContact) null;
                },
                MethodBase.GetCurrentMethod());
        }

        private ContactSupplementModel GetContactSupplementModel(IContact contact, ContactContext context, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact))
                .NotNull(context, nameof(context));

            if (string.IsNullOrWhiteSpace(contact.InternalIdentifier) == false && int.TryParse(contact.InternalIdentifier, out int internalIdentifier))
            {
                ContactSupplementModel contactSupplementModel = context.ContactSupplements
                    .Include(model => model.ContactGroup)
                    .Include(model => model.PaymentTerm)
                    .Include(model => model.ContactSupplementBindings)
                    .SingleOrDefault(model => model.ContactSupplementIdentifier == internalIdentifier);
                if (contactSupplementModel != null)
                {
                    return contactSupplementModel;
                }
            }

            foreach (string externalIdentifier in GetExternalIdentifierCollection(contact, existingExternalIdentifier))
            {
                ContactSupplementBindingModel contactSupplementBindingModel = context.ContactSupplementBindings
                    .Include(model => model.ContactSupplement).ThenInclude(model => model.ContactGroup)
                    .Include(model => model.ContactSupplement).ThenInclude(model => model.PaymentTerm)
                    .Include(model => model.ContactSupplement).ThenInclude(model => model.ContactSupplementBindings)
                    .SingleOrDefault(model => model.ExternalIdentifier == externalIdentifier);
                if (contactSupplementBindingModel != null)
                {
                    return contactSupplementBindingModel.ContactSupplement;
                }
            }

            return null;
        }

        private string[] GetExternalIdentifierCollection(IContact contact, string existingExternalIdentifier = null)
        {
            NullGuard.NotNull(contact, nameof(contact));

            return new[] {contact.CalculateIdentifier(), existingExternalIdentifier ?? contact.ExternalIdentifier}
                .Where(m => string.IsNullOrWhiteSpace(m) == false)
                .ToArray();
        }

        private IEnumerable<IContactGroup> GetContactGroups()
        {
            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    return context.ContactGroups.AsParallel()
                        .Select(contactGroupModel =>
                        {
                            using (ContactContext subContext = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                            {
                                contactGroupModel.Deletable = CanDeleteContactGroup(subContext, contactGroupModel.ContactGroupIdentifier);
                            }

                            return _contactModelConverter.Convert<ContactGroupModel, IContactGroup>(contactGroupModel);
                        })
                        .OrderBy(contactGroup => contactGroup.Number)
                        .ToList();
                },
                MethodBase.GetCurrentMethod());
        }

        private IContactGroup GetContactGroup(int number)
        {
            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    ContactGroupModel contactGroupModel = context.ContactGroups.Find(number);
                    if (contactGroupModel == null)
                    {
                        return null;
                    }

                    contactGroupModel.Deletable = CanDeleteContactGroup(context, contactGroupModel.ContactGroupIdentifier);

                    return _contactModelConverter.Convert<ContactGroupModel, IContactGroup>(contactGroupModel);
                },
                MethodBase.GetCurrentMethod());
        }

        private IContactGroup CreateContactGroup(IContactGroup contactGroup)
        {
            NullGuard.NotNull(contactGroup, nameof(contactGroup));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    ContactGroupModel contactGroupModel = _contactModelConverter.Convert<IContactGroup, ContactGroupModel>(contactGroup);

                    context.ContactGroups.Add(contactGroupModel);

                    context.SaveChanges();

                    return GetContactGroup(contactGroupModel.ContactGroupIdentifier);
                },
                MethodBase.GetCurrentMethod());
        }

        private IContactGroup UpdateContactGroup(IContactGroup contactGroup)
        {
            NullGuard.NotNull(contactGroup, nameof(contactGroup));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    ContactGroupModel contactGroupModel = context.ContactGroups.Find(contactGroup.Number);
                    if (contactGroupModel == null)
                    {
                        return null;
                    }

                    contactGroupModel.Name = contactGroup.Name;

                    context.SaveChanges();

                    return GetContactGroup(contactGroupModel.ContactGroupIdentifier);
                },
                MethodBase.GetCurrentMethod());
        }

        private IContactGroup DeleteContactGroup(int number)
        {
            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    ContactGroupModel contactGroupModel = context.ContactGroups.Find(number);
                    if (contactGroupModel == null)
                    {
                        return null;
                    }

                    if (CanDeleteContactGroup(context, contactGroupModel.ContactGroupIdentifier) == false)
                    {
                        return GetContactGroup(contactGroupModel.ContactGroupIdentifier);
                    }

                    context.ContactGroups.Remove(contactGroupModel);

                    context.SaveChanges();

                    return null;
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeleteContactGroup(ContactContext context, int contactGroupIdentifier)
        {
            NullGuard.NotNull(context, nameof(context));

            return context.ContactSupplements.FirstOrDefault(contactSupplementModel => contactSupplementModel.ContactGroupIdentifier == contactGroupIdentifier) == null;
        }

        private IEnumerable<ICountry> GetCountries()
        {
            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    return context.Countries.AsParallel()
                        .Select(countryModel =>
                        {
                            using (ContactContext subContext = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                            {
                                countryModel.Deletable = CanDeleteCountry(subContext, countryModel.Code);
                            }

                            return _contactModelConverter.Convert<CountryModel, ICountry>(countryModel);
                        })
                        .OrderBy(country => country.Name)
                        .ToList();
                },
                MethodBase.GetCurrentMethod());
        }

        private ICountry GetCountry(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    CountryModel countryModel = context.Countries.Find(code);
                    if (countryModel == null)
                    {
                        return null;
                    }

                    countryModel.Deletable = CanDeleteCountry(context, countryModel.Code);

                    return _contactModelConverter.Convert<CountryModel, ICountry>(countryModel);
                },
                MethodBase.GetCurrentMethod());
        }

        private ICountry CreateCountry(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    CountryModel countryModel = _contactModelConverter.Convert<ICountry, CountryModel>(country);

                    context.Countries.Add(countryModel);

                    context.SaveChanges();

                    return GetCountry(countryModel.Code);
                },
                MethodBase.GetCurrentMethod());
        }

        private ICountry UpdateCountry(ICountry country)
        {
            NullGuard.NotNull(country, nameof(country));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
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
                },
                MethodBase.GetCurrentMethod());
        }

        private ICountry DeleteCountry(string code)
        {
            NullGuard.NotNullOrWhiteSpace(code, nameof(code));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
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
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeleteCountry(ContactContext context, string code)
        {
            NullGuard.NotNull(context, nameof(context))
                .NotNullOrWhiteSpace(code, nameof(code));

            return context.PostalCodes.FirstOrDefault(postalCode => postalCode.CountryCode == code) == null;
        }

        private IEnumerable<IPostalCode> GetPostalCodes(string countryCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    return context.PostalCodes
                        .Include(postalCodeModel => postalCodeModel.Country)
                        .Where(postalCodeModel => postalCodeModel.CountryCode == countryCode)
                        .AsParallel()
                        .Select(postalCodeModel =>
                        {
                            using (ContactContext subContext = new ContactContext(Configuration, PrincipalResolver, LoggerFactory))
                            {
                                postalCodeModel.Deletable = CanDeletePostalCode(subContext, postalCodeModel.CountryCode, postalCodeModel.PostalCode);
                            }

                            return _contactModelConverter.Convert<PostalCodeModel, IPostalCode>(postalCodeModel);
                        })
                        .OrderBy(postalCode => postalCode.City)
                        .ToList();
                },
                MethodBase.GetCurrentMethod());
        }

        private IPostalCode GetPostalCode(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    PostalCodeModel postalCodeModel = context.PostalCodes
                        .Include(m => m.Country)
                        .SingleOrDefault(m => m.CountryCode == countryCode && m.PostalCode == postalCode);
                    if (postalCodeModel == null)
                    {
                        return null;
                    }

                    postalCodeModel.Deletable = CanDeletePostalCode(context, postalCodeModel.CountryCode, postalCodeModel.PostalCode);

                    return _contactModelConverter.Convert<PostalCodeModel, IPostalCode>(postalCodeModel);
                },
                MethodBase.GetCurrentMethod());
        }

        public IPostalCode CreatePostalCode(IPostalCode postalCode)
        {
            NullGuard.NotNull(postalCode, nameof(postalCode));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    CountryModel countryModel = context.Countries.Find(postalCode.Country.Code);
                    if (countryModel == null)
                    {
                        return null;
                    }

                    PostalCodeModel postalCodeModel = _contactModelConverter.Convert<IPostalCode, PostalCodeModel>(postalCode);
                    postalCodeModel.CountryCode = countryModel.Code;
                    postalCodeModel.Country = countryModel;

                    context.PostalCodes.Add(postalCodeModel);

                    context.SaveChanges();

                    return GetPostalCode(postalCodeModel.CountryCode, postalCodeModel.PostalCode);
                },
                MethodBase.GetCurrentMethod());
        }

        public IPostalCode UpdatePostalCode(IPostalCode postalCode)
        {
            NullGuard.NotNull(postalCode, nameof(postalCode));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    PostalCodeModel postalCodeModel = context.PostalCodes
                        .Include(m => m.Country)
                        .SingleOrDefault(m => m.CountryCode == postalCode.Country.Code && m.PostalCode == postalCode.Code);
                    if (postalCodeModel == null)
                    {
                        return null;
                    }

                    postalCodeModel.City = postalCode.City;
                    postalCodeModel.State = postalCode.State;

                    context.SaveChanges();

                    return GetPostalCode(postalCodeModel.CountryCode, postalCodeModel.PostalCode);
                },
                MethodBase.GetCurrentMethod());
        }

        public IPostalCode DeletePostalCode(string countryCode, string postalCode)
        {
            NullGuard.NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            return Execute(() =>
                {
                    using ContactContext context = new ContactContext(Configuration, PrincipalResolver, LoggerFactory);
                    PostalCodeModel postalCodeModel = context.PostalCodes
                        .Include(m => m.Country)
                        .SingleOrDefault(m => m.CountryCode == countryCode && m.PostalCode == postalCode);
                    if (postalCodeModel == null)
                    {
                        return null;
                    }

                    if (CanDeletePostalCode(context, postalCodeModel.CountryCode, postalCodeModel.PostalCode) == false)
                    {
                        return GetPostalCode(postalCodeModel.CountryCode, postalCodeModel.PostalCode);
                    }

                    context.PostalCodes.Remove(postalCodeModel);

                    context.SaveChanges();

                    return null;
                },
                MethodBase.GetCurrentMethod());
        }

        private bool CanDeletePostalCode(ContactContext context, string countryCode, string postalCode)
        {
            NullGuard.NotNull(context, nameof(context))
                .NotNullOrWhiteSpace(countryCode, nameof(countryCode))
                .NotNullOrWhiteSpace(postalCode, nameof(postalCode));

            return true;
        }

        #endregion
    }
}
