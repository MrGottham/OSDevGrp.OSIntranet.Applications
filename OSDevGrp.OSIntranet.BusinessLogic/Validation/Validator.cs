using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    public class Validator : IValidator
    {
        #region Private variables

        private static IIntegerValidator _integerValidator;
        private static IDecimalValidator _decimalValidator;
        private static IStringValidator _stringValidator;
        private static IDateTimeValidator _dateTimeValidator;
        private static IObjectValidator _objectValidator;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Constructors

        public Validator(IIntegerValidator integerValidator, IDecimalValidator decimalValidator, IStringValidator stringValidator, IDateTimeValidator dateTimeValidator, IObjectValidator objectValidator)
        {
            NullGuard.NotNull(integerValidator, nameof(integerValidator))
                .NotNull(decimalValidator, nameof(decimalValidator))
                .NotNull(stringValidator, nameof(stringValidator))
                .NotNull(dateTimeValidator, nameof(dateTimeValidator))
                .NotNull(objectValidator, nameof(objectValidator));

            lock (SyncRoot)
            {
                _integerValidator = integerValidator;
                _decimalValidator = decimalValidator;
                _stringValidator = stringValidator;
                _dateTimeValidator = dateTimeValidator;
                _objectValidator = objectValidator;
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

        #endregion
    }
}
