﻿using System;
using AutoMapper;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Contacts;
using OSDevGrp.OSIntranet.Mvc.Models.Core;

namespace OSDevGrp.OSIntranet.Mvc.Models.Home
{
    internal class HomeViewModelConverter : ConverterBase
    {
        #region Private variables

        private readonly IValueConverter<IContact, DateTime> _upcomingBirthdayConverter = new UpcomingBirthdayConverter();
        private readonly IValueConverter<IContact, ushort> _ageOnUpcomingBirthdayConverter = new AgeOnUpcomingBirthdayConverter();

        #endregion

        #region Methods

        protected override void Initialize(IMapperConfigurationExpression mapperConfiguration)
        {
            NullGuard.NotNull(mapperConfiguration, nameof(mapperConfiguration));

            mapperConfiguration.CreateMap<IContact, ContactWithUpcomingBirthdayViewModel>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Name.DisplayName))
                .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => src.ToContactType()))
                .ForMember(dest => dest.HomePhone, opt => opt.Ignore())
                .ForMember(dest => dest.MobilePhone, opt => opt.Ignore())
                .ForMember(dest => dest.UpcomingBirthday, opt => opt.ConvertUsing(_upcomingBirthdayConverter, src => src))
                .ForMember(dest => dest.AgeOnUpcomingBirthday, opt => opt.ConvertUsing(_ageOnUpcomingBirthdayConverter, src => src))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));

            mapperConfiguration.CreateMap<IAccounting, AccountingPresentationViewModel>()
                .ForMember(dest => dest.AccountingNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.EditMode, opt => opt.MapFrom(src => EditMode.None));
        }

        #endregion

        #region Private classes

        private class UpcomingBirthdayConverter : UpcomingBirthdayConverterBase, IValueConverter<IContact, DateTime>
        {
            #region Methods

            public DateTime Convert(IContact contact, ResolutionContext context)
            {
                NullGuard.NotNull(contact, nameof(contact))
                    .NotNull(context, nameof(context));

                if (contact.Birthday.HasValue == false)
                {
                    throw new NotSupportedException("Unable to convert a contact without a birthday.");
                }

                return CalculateUpcomingBirthday(contact.Birthday.Value);
            }

            #endregion
        }

        private class AgeOnUpcomingBirthdayConverter : UpcomingBirthdayConverterBase, IValueConverter<IContact, ushort>
        {
            #region Methods

            public ushort Convert(IContact contact, ResolutionContext context)
            {
                NullGuard.NotNull(contact, nameof(contact))
                    .NotNull(context, nameof(context));

                if (contact.Birthday.HasValue == false || contact.Age.HasValue == false)
                {
                    throw new NotSupportedException("Unable to convert a contact without a birthday and without an age.");
                }

                if (CalculateUpcomingBirthday(contact.Birthday.Value) > DateTime.Today)
                {
                    return (ushort) (contact.Age.Value + 1);
                }

                return contact.Age.Value;
            }

            #endregion
        }

        private abstract class UpcomingBirthdayConverterBase
        {
            #region Methods

            protected static DateTime CalculateUpcomingBirthday(DateTime birthday)
            {
                NullGuard.NotNull(birthday, nameof(birthday));

                DateTime today = DateTime.Today;
                DateTime upcomingBirthday = new DateTime(today.Year, birthday.Month, Math.Min(birthday.Day, DateTime.DaysInMonth(today.Year, birthday.Month)));

                if (upcomingBirthday.Month > today.Month || upcomingBirthday.Month == today.Month && upcomingBirthday.Day >= today.Day)
                {
                    return upcomingBirthday.Date;
                }

                return upcomingBirthday.AddYears(1).Date;
            }

            #endregion
        }

        #endregion
    }
}