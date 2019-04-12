namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation
{
    public interface IValidator
    {
        IIntegerValidator Integer { get; }

        IDecimalValidator Decimal { get; }

        IStringValidator String { get; }

        IDateTimeValidator DateTime { get; }

        IObjectValidator Object { get; }
    }
}
