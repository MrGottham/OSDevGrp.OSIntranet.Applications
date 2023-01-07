using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    internal class Validator : IValidator
    {
        #region Private variables

        private static IIntegerValidator _integerValidator;
        private static IDecimalValidator _decimalValidator;
        private static IStringValidator _stringValidator;
        private static IDateTimeValidator _dateTimeValidator;
        private static IObjectValidator _objectValidator;
        private static IEnumerableValidator _enumerableValidator;
        private static IPermissionValidator _permissionValidator;
        private static readonly object SyncRoot = new();

        #endregion

        #region Constructors

        public Validator(IIntegerValidator integerValidator, IDecimalValidator decimalValidator, IStringValidator stringValidator, IDateTimeValidator dateTimeValidator, IObjectValidator objectValidator, IEnumerableValidator enumerableValidator, IPermissionValidator permissionValidator)
        {
            NullGuard.NotNull(integerValidator, nameof(integerValidator))
                .NotNull(decimalValidator, nameof(decimalValidator))
                .NotNull(stringValidator, nameof(stringValidator))
                .NotNull(dateTimeValidator, nameof(dateTimeValidator))
                .NotNull(objectValidator, nameof(objectValidator))
                .NotNull(enumerableValidator, nameof(enumerableValidator))
                .NotNull(permissionValidator, nameof(permissionValidator));

            lock (SyncRoot)
            {
                _integerValidator = integerValidator;
                _decimalValidator = decimalValidator;
                _stringValidator = stringValidator;
                _dateTimeValidator = dateTimeValidator;
                _objectValidator = objectValidator;
                _enumerableValidator = enumerableValidator;
                _permissionValidator = permissionValidator;
            }
        }

        protected Validator()
        {
        }

        #endregion

        #region Methods

        public IIntegerValidator Integer => _integerValidator;

        public IDecimalValidator Decimal => _decimalValidator;

        public IStringValidator String => _stringValidator;

        public IDateTimeValidator DateTime => _dateTimeValidator;

        public IObjectValidator Object => _objectValidator;

        public IEnumerableValidator Enumerable => _enumerableValidator;

        public IPermissionValidator Permission => _permissionValidator;

        #endregion
    }
}