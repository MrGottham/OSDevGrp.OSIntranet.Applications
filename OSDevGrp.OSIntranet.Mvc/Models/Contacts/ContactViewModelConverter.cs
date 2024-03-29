﻿using System;
using System.Collections.Generic;
using AutoMapper;
using OSDevGrp.OSIntranet.BusinessLogic.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Accounting;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Contacts
{
    internal class ContactViewModelConverter : ConverterBase
    {
        #region Private variables

        private readonly IValueConverter<ContactViewModel, INameCommand> _nameCommandConverter = new NameCommandConverter();
        private readonly IValueConverter<CompanyViewModel, ICompanyNameCommand> _companyNameCommandConverter = new CompanyNameCommandConverter();
        private readonly IValueConverter<CompanyViewModel, ICompanyCommand> _companyCommandConverter = new CompanyCommandConverter();
        private readonly IValueConverter<IAddress, AddressViewModel> _addressViewModelConverter = new AddressViewModelConverter();
        private readonly IValueConverter<AddressViewModel, IAddressCommand> _addressCommandConverter = new AddressCommandConverter();
        private readonly IValueConverter<IContactGroup, ContactGroupViewModel> _contactGroupViewModelConverter = new ContactGroupViewModelConverter();
        private readonly IValueConverter<IPaymentTerm, PaymentTermViewModel> _paymentTermViewModelConverter = new PaymentTermViewModelConverter();

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IContact, ContactIdentificationViewModel>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name.DisplayName))
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => src.ToContactType()))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContact, ContactInfoViewModel>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name.DisplayName))
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => src.ToContactType()))
                .ForMember(dest => dest.HomePhone, opt => opt.Ignore())
                .ForMember(dest => dest.MobilePhone, opt => opt.Ignore())
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IContact, ContactViewModel>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name.DisplayName))
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => src.ToContactType()))
                .ForMember(dest => dest.HomePhone, opt => opt.Ignore())
                .ForMember(dest => dest.MobilePhone, opt => opt.Ignore())
                .ForMember(dest => dest.GivenName, opt => opt.MapFrom(src => src.Name is IPersonName ? ((IPersonName) src.Name).GivenName : null))
                .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Name is IPersonName ? ((IPersonName) src.Name).MiddleName : null))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Name is IPersonName ? ((IPersonName) src.Name).Surname : src.Name is ICompanyName ? ((ICompanyName) src.Name).FullName : src.Name.DisplayName))
                .ForMember(opt => opt.CompanyName, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.ConvertUsing(_addressViewModelConverter))
                .ForMember(dest => dest.ContactGroup, opt => opt.ConvertUsing(_contactGroupViewModelConverter))
                .ForMember(dest => dest.HomePage, opt => opt.MapFrom(src => src.PersonalHomePage))
                .ForMember(dest => dest.PaymentTerm, opt => opt.ConvertUsing(_paymentTermViewModelConverter))
                .ForMember(dest => dest.Country, opt => opt.Ignore())
                .ForMember(dest => dest.Countries, opt => opt.MapFrom(src => new List<CountryViewModel>(0)))
                .ForMember(dest => dest.ContactGroups, opt => opt.MapFrom(src => new List<ContactGroupViewModel>(0)))
                .ForMember(dest => dest.PaymentTerms, opt => opt.MapFrom(src => new List<PaymentTermViewModel>(0)))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<ContactViewModel, CreateContactCommand>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.ConvertUsing(_addressCommandConverter))
                .ForMember(dest => dest.Company, opt => opt.ConvertUsing(_companyCommandConverter))
                .ForMember(dest => dest.ContactGroupIdentifier, opt => opt.MapFrom(src => src.ContactGroup.Number))
                .ForMember(dest => dest.PersonalHomePage, opt => opt.MapFrom(src => src.HomePage))
                .ForMember(dest => dest.PaymentTermIdentifier, opt => opt.MapFrom(src => src.PaymentTerm.Number))
                .ForMember(dest => dest.TokenType, opt => opt.Ignore())
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.Expires, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.Name = _nameCommandConverter.Convert(src, context);
                });

            mapperConfiguration.CreateMap<ContactViewModel, UpdateContactCommand>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.ConvertUsing(_addressCommandConverter))
                .ForMember(dest => dest.Company, opt => opt.ConvertUsing(_companyCommandConverter))
                .ForMember(dest => dest.ContactGroupIdentifier, opt => opt.MapFrom(src => src.ContactGroup.Number))
                .ForMember(dest => dest.PersonalHomePage, opt => opt.MapFrom(src => src.HomePage))
                .ForMember(dest => dest.PaymentTermIdentifier, opt => opt.MapFrom(src => src.PaymentTerm.Number))
                .ForMember(dest => dest.TokenType, opt => opt.Ignore())
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.Expires, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    dest.Name = _nameCommandConverter.Convert(src, context);
                });

            mapperConfiguration.CreateMap<ContactViewModel, PersonNameCommand>();

            mapperConfiguration.CreateMap<ContactViewModel, CompanyNameCommand>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.CompanyName));

            mapperConfiguration.CreateMap<ICompany, CompanyViewModel>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Name.FullName))
                .ForMember(dest => dest.Address, opt => opt.ConvertUsing(_addressViewModelConverter));

            mapperConfiguration.CreateMap<CompanyViewModel, CompanyCommand>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Address, opt => opt.ConvertUsing(_addressCommandConverter))
                .AfterMap((src, dest, context) =>
                {
                    dest.Name = _companyNameCommandConverter.Convert(src, context);
                });

            mapperConfiguration.CreateMap<CompanyViewModel, CompanyNameCommand>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.CompanyName));

            mapperConfiguration.CreateMap<IAddress, AddressViewModel>();

            mapperConfiguration.CreateMap<AddressViewModel, AddressCommand>();

            mapperConfiguration.CreateMap<IContactGroup, ContactGroupViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<ContactGroupViewModel, CreateContactGroupCommand>();
            mapperConfiguration.CreateMap<ContactGroupViewModel, UpdateContactGroupCommand>();
            mapperConfiguration.CreateMap<ContactGroupViewModel, DeleteContactGroupCommand>();

            mapperConfiguration.CreateMap<ICountry, CountryViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<CountryViewModel, CreateCountryCommand>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<CountryViewModel, UpdateCountryCommand>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<CountryViewModel, DeleteCountryCommand>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<IPostalCode, PostalCodeViewModel>()
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<PostalCodeViewModel, CreatePostalCodeCommand>()
                .ForMember(dest => dest.CountryCode, opt =>
                {
                    opt.Condition(src => src.Country != null && string.IsNullOrWhiteSpace(src.Country.Code) == false);
                    opt.MapFrom(src => src.Country.Code);
                })
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<PostalCodeViewModel, UpdatePostalCodeCommand>()
                .ForMember(dest => dest.CountryCode, opt =>
                {
                    opt.Condition(src => src.Country != null && string.IsNullOrWhiteSpace(src.Country.Code) == false);
                    opt.MapFrom(src => src.Country.Code);
                })
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Code));

            mapperConfiguration.CreateMap<PostalCodeViewModel, DeletePostalCodeCommand>()
                .ForMember(dest => dest.CountryCode, opt =>
                {
                    opt.Condition(src => src.Country != null && string.IsNullOrWhiteSpace(src.Country.Code) == false);
                    opt.MapFrom(src => src.Country.Code);
                })
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.Code));
        }

        #endregion

        #region Private classes

        private class NameCommandConverter : IValueConverter<ContactViewModel, INameCommand>
        {
            #region Methods

            public INameCommand Convert(ContactViewModel sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(sourceMember, nameof(sourceMember))
                    .NotNull(context, nameof(context));

                switch (sourceMember.ContactType)
                {
                    case ContactType.Person:
                        return context.Mapper.Map<ContactViewModel, PersonNameCommand>(sourceMember);

                    case ContactType.Company:
                        return context.Mapper.Map<ContactViewModel, CompanyNameCommand>(sourceMember);
                }

                throw new NotSupportedException($"Unable to convert {sourceMember.ContactType} to an INameCommand.");
            }

            #endregion
        }

        private class CompanyNameCommandConverter : IValueConverter<CompanyViewModel, ICompanyNameCommand>
        {
            #region Methods

            public ICompanyNameCommand Convert(CompanyViewModel sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(sourceMember, nameof(sourceMember))
                    .NotNull(context, nameof(context));

                return context.Mapper.Map<CompanyViewModel, CompanyNameCommand>(sourceMember);
            }

            #endregion
        }

        private class CompanyCommandConverter : IValueConverter<CompanyViewModel, ICompanyCommand>
        {
            #region Methods

            public ICompanyCommand Convert(CompanyViewModel sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(context, nameof(context));

                return sourceMember == null ? null : context.Mapper.Map<CompanyViewModel, CompanyCommand>(sourceMember);
            }

            #endregion
        }

        private class AddressViewModelConverter : IValueConverter<IAddress, AddressViewModel>
        {
            #region Methods

            public AddressViewModel Convert(IAddress sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(context, nameof(context));

                return sourceMember == null ? new AddressViewModel() : context.Mapper.Map<IAddress, AddressViewModel>(sourceMember);
            }

            #endregion
        }

        private class AddressCommandConverter : IValueConverter<AddressViewModel, IAddressCommand>
        {
            #region Methods

            public IAddressCommand Convert(AddressViewModel sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(context, nameof(context));

                if (sourceMember == null || sourceMember.IsEmpty)
                {
                    return new AddressCommand
                    {
                        StreetLine1 = string.Empty,
                        StreetLine2 = string.Empty,
                        PostalCode = string.Empty,
                        City = string.Empty,
                        State = string.Empty,
                        Country = string.Empty
                    };
                }

                return context.Mapper.Map<AddressViewModel, AddressCommand>(sourceMember);
            }

            #endregion
        }

        private class ContactGroupViewModelConverter : IValueConverter<IContactGroup, ContactGroupViewModel>
        {
            #region Methods

            public ContactGroupViewModel Convert(IContactGroup sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(context, nameof(context));

                return sourceMember == null ? new ContactGroupViewModel() : context.Mapper.Map<IContactGroup, ContactGroupViewModel>(sourceMember);
            }

            #endregion
        }

        private class PaymentTermViewModelConverter : IValueConverter<IPaymentTerm, PaymentTermViewModel>
        {
            #region Private variables

            private readonly IConverter _accountingViewModelConverter = new AccountingViewModelConverter();

            #endregion
            
            #region Methods

            public PaymentTermViewModel Convert(IPaymentTerm sourceMember, ResolutionContext context)
            {
                NullGuard.NotNull(context, nameof(context));

                return sourceMember == null ? new PaymentTermViewModel() : _accountingViewModelConverter.Convert<IPaymentTerm, PaymentTermViewModel>(sourceMember);
            }

            #endregion
        }

        #endregion
    }
}