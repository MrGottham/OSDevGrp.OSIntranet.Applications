namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IContactAccountTexts : IDynamicTexts
{
    IValueDisplayer StatusDate { get; }

    IContactAccountValuesDisplayer ValuesAtStatusDate { get; }

    IContactAccountValuesDisplayer ValuesAtEndOfLastMonthFromStatusDate { get; }

    IContactAccountValuesDisplayer ValuesAtEndOfLastYearFromStatusDate { get; }
}