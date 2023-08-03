using Moq;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;
using OSDevGrp.OSIntranet.Domain.Interfaces.Common;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.BusinessLogic.Tests.Validation
{
	internal class ValidatorMockContext
    {
        #region Constructor

        internal ValidatorMockContext()
        {            
            ValidatorMock = new Mock<IValidator>();

            IntegerValidatorMock = BuildIntegerValidatorMock(ValidatorMock);
            DecimalValidatorMock = BuildDecimalValidatorMock(ValidatorMock);
            StringValidatorMock = BuildStringValidatorMock(ValidatorMock);
            DateTimeValidatorMock = BuildDateTimeValidatorMock(ValidatorMock);
            ObjectValidatorMock = BuildObjectValidatorMock(ValidatorMock);
            EnumerableValidatorMock = BuildEnumerableValidatorMock(ValidatorMock);
            PermissionValidatorMock = BuildPermissionValidatorMock(ValidatorMock);

            ValidatorMock.Setup(m => m.Integer)
                .Returns(IntegerValidatorMock.Object);
            ValidatorMock.Setup(m => m.Decimal)
                .Returns(DecimalValidatorMock.Object);
            ValidatorMock.Setup(m => m.String)
                .Returns(StringValidatorMock.Object);
            ValidatorMock.Setup(m => m.DateTime)
                .Returns(DateTimeValidatorMock.Object);
            ValidatorMock.Setup(m => m.Object)
                .Returns(ObjectValidatorMock.Object);
            ValidatorMock.Setup(m => m.Enumerable)
                .Returns(EnumerableValidatorMock.Object);
            ValidatorMock.Setup(m => m.Permission)
                .Returns(PermissionValidatorMock.Object);
        }

        #endregion

        #region Properties

        internal Mock<IValidator> ValidatorMock;

        internal Mock<IIntegerValidator> IntegerValidatorMock;

        internal Mock<IDecimalValidator> DecimalValidatorMock;

        internal Mock<IStringValidator> StringValidatorMock;

        internal Mock<IDateTimeValidator> DateTimeValidatorMock;

        internal Mock<IObjectValidator> ObjectValidatorMock;

        internal Mock<IEnumerableValidator> EnumerableValidatorMock;

        internal Mock<IPermissionValidator> PermissionValidatorMock;

        #endregion

        #region Methods

        private static Mock<IIntegerValidator> BuildIntegerValidatorMock(Mock<IValidator> validatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock));

            Mock<IIntegerValidator> integerValidatorMock = new Mock<IIntegerValidator>();
            integerValidatorMock.Setup(m => m.ShouldBeGreaterThanZero(It.IsAny<int>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            integerValidatorMock.Setup(m => m.ShouldBeGreaterThanOrEqualToZero(It.IsAny<int>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            integerValidatorMock.Setup(m => m.ShouldBeBetween(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            return integerValidatorMock;
        }

        private static Mock<IDecimalValidator> BuildDecimalValidatorMock(Mock<IValidator> validatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock));

            Mock<IDecimalValidator> decimalValidatorMock = new Mock<IDecimalValidator>();
            decimalValidatorMock.Setup(m => m.ShouldBeGreaterThanZero(It.IsAny<decimal>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            decimalValidatorMock.Setup(m => m.ShouldBeGreaterThanOrEqualToZero(It.IsAny<decimal>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            decimalValidatorMock.Setup(m => m.ShouldBeBetween(It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            return decimalValidatorMock;
        }

        private static Mock<IStringValidator> BuildStringValidatorMock(Mock<IValidator> validatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock));

            Mock<IStringValidator> stringValidatorMock = new Mock<IStringValidator>();
            stringValidatorMock.Setup(m => m.ShouldNotBeNull(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            stringValidatorMock.Setup(m => m.ShouldNotBeNullOrEmpty(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            stringValidatorMock.Setup(m => m.ShouldNotBeNullOrWhiteSpace(It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            stringValidatorMock.Setup(m => m.ShouldHaveMinLength(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(validatorMock.Object);
            stringValidatorMock.Setup(m => m.ShouldHaveMaxLength(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(validatorMock.Object);
            stringValidatorMock.Setup(m => m.ShouldMatchPattern(It.IsAny<string>(), It.IsAny<Regex>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(validatorMock.Object);
            return stringValidatorMock;
        }

        private static Mock<IDateTimeValidator> BuildDateTimeValidatorMock(Mock<IValidator> validatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock));

            Mock<IDateTimeValidator> dateTimeValidatorMock = new Mock<IDateTimeValidator>();
            dateTimeValidatorMock.Setup(m => m.ShouldBePastDate(It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBePastDateOrToday(It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBePastDateTime(It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeToday(It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeFutureDate(It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeFutureDateOrToday(It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeFutureDateTime(It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBePastDateWithinDaysFromOffsetDate(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeFutureDateWithinDaysFromOffsetDate(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeEarlierThanOffsetDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
	            .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeEarlierThanOrEqualToOffsetDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
	            .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeLaterThanOffsetDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            dateTimeValidatorMock.Setup(m => m.ShouldBeLaterThanOrEqualToOffsetDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);
            return dateTimeValidatorMock;
        }

        private static Mock<IObjectValidator> BuildObjectValidatorMock(Mock<IValidator> validatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock));

            Mock<IObjectValidator> objectValidatorMock = new Mock<IObjectValidator>();
            SetupGenericObjectValidatorMock<int>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<string>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<decimal?>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<object>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<BalanceBelowZeroType>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<AccountGroupType>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<IEnumerable<Claim>>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<INameCommand>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<IEnumerable<string>>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<IEnumerable<ICreditInfoCommand>>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<IEnumerable<IBudgetInfoCommand>>(validatorMock, objectValidatorMock);
            SetupGenericObjectValidatorMock<IEnumerable<IApplyPostingLineCommand>>(validatorMock, objectValidatorMock);
            return objectValidatorMock;
        }

        private static void SetupGenericObjectValidatorMock<T>(Mock<IValidator> validatorMock, Mock<IObjectValidator> objectValidatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock))
                .NotNull(objectValidatorMock, nameof(objectValidatorMock));

            objectValidatorMock.Setup(m => m.ShouldBeKnownValue(It.IsAny<T>(), It.IsAny<Func<T, Task<bool>>>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(validatorMock.Object);
            objectValidatorMock.Setup(m => m.ShouldBeUnknownValue(It.IsAny<T>(), It.IsAny<Func<T, Task<bool>>>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(validatorMock.Object);
            objectValidatorMock.Setup(m => m.ShouldNotBeNull(It.IsAny<T>(), It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(validatorMock.Object);

            SetupShouldBeDeletable<T, IContactGroup>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, ICountry>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IPostalCode>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IAccounting>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IAccount>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IBudgetAccount>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IContactAccount>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IAccountGroup>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IBudgetAccountGroup>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IPaymentTerm>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, ILetterHead>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IGenericCategory>(validatorMock, objectValidatorMock);
            SetupShouldBeDeletable<T, IKeyValueEntry>(validatorMock, objectValidatorMock);
        }

        private static void SetupShouldBeDeletable<TValue, TDeletable>(Mock<IValidator> validatorMock, Mock<IObjectValidator> objectValidatorMock) where TDeletable : IDeletable
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock))
                .NotNull(objectValidatorMock, nameof(objectValidatorMock));

            objectValidatorMock.Setup(m => m.ShouldBeDeletable(It.IsAny<TValue>(), It.IsAny<Func<TValue, Task<TDeletable>>>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(validatorMock.Object);
        }

        private static Mock<IEnumerableValidator> BuildEnumerableValidatorMock(Mock<IValidator> validatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock));

            Mock<IEnumerableValidator> enumerableValidatorMock = new Mock<IEnumerableValidator>();
            SetupGenericEnumerableValidatorMock<byte>(validatorMock, enumerableValidatorMock);
            SetupGenericEnumerableValidatorMock<string>(validatorMock, enumerableValidatorMock);
            SetupGenericEnumerableValidatorMock<IApplyPostingLineCommand>(validatorMock, enumerableValidatorMock);
            return enumerableValidatorMock;
        }

        private static void SetupGenericEnumerableValidatorMock<T>(Mock<IValidator> validatorMock, Mock<IEnumerableValidator> enumerableValidatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock))
                .NotNull(enumerableValidatorMock, nameof(enumerableValidatorMock));

            enumerableValidatorMock.Setup(m => m.ShouldContainItems(It.IsAny<IEnumerable<T>>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(validatorMock.Object);
            enumerableValidatorMock.Setup(m => m.ShouldHaveMinItems(It.IsAny<IEnumerable<T>>(), It.IsAny<int>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
	            .Returns(validatorMock.Object);
			enumerableValidatorMock.Setup(m => m.ShouldHaveMaxItems(It.IsAny<IEnumerable<T>>(), It.IsAny<int>(), It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<bool>()))
				.Returns(validatorMock.Object);
        }

		private static Mock<IPermissionValidator> BuildPermissionValidatorMock(Mock<IValidator> validatorMock)
        {
            NullGuard.NotNull(validatorMock, nameof(validatorMock));

            Mock<IPermissionValidator> permissionValidatorMock = new Mock<IPermissionValidator>();
            permissionValidatorMock.Setup(m => m.HasNecessaryPermission(It.IsAny<bool>()))
                .Returns(validatorMock.Object);
            return permissionValidatorMock;
        }

        #endregion
    }
}