using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Validation
{
    public class Validator : IValidator
    {
        #region Constructor

        public Validator(IIntegerValidator integerValidator, IDecimalValidator decimalValidator, IStringValidator stringValidator, IDateTimeValidator dateTimeValidator, IObjectValidator objectValidator)
        {
            NullGuard.NotNull(integerValidator, nameof(integerValidator))
                .NotNull(decimalValidator, nameof(decimalValidator))
                .NotNull(stringValidator, nameof(stringValidator))
                .NotNull(dateTimeValidator, nameof(dateTimeValidator))
                .NotNull(objectValidator, nameof(objectValidator));

            Integer = integerValidator;
            Decimal = decimalValidator;
            String = stringValidator;
            DateTime = dateTimeValidator;
            Object = objectValidator;
        }

        protected Validator()
        {
        }

        #endregion

        #region Methods

        public IIntegerValidator Integer { get; }

        public IDecimalValidator Decimal { get; }

        public IStringValidator String { get; }

        public IDateTimeValidator DateTime { get; }

        public IObjectValidator Object { get; }

        #endregion
    }
}
