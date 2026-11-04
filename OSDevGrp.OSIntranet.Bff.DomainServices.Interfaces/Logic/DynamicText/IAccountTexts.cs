namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IAccountTexts : IDynamicTexts
{
    IValueDisplayer StatusDate { get; }

    IAccountValuesDisplayer ValuesAtStatusDate { get; }

    IAccountValuesDisplayer ValuesAtEndOfLastMonthFromStatusDate { get; }

    IAccountValuesDisplayer ValuesAtEndOfLastYearFromStatusDate { get; }
}