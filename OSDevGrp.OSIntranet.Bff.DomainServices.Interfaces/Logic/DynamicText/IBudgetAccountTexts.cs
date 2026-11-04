namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IBudgetAccountTexts : IDynamicTexts
{
    IValueDisplayer StatusDate { get; }

    IBudgetAccountValuesDisplayer ValuesForMonthOfStatusDate { get; }

    IBudgetAccountValuesDisplayer ValuesForLastMonthOfStatusDate { get; }

    IBudgetAccountValuesDisplayer ValuesForYearToDateOfStatusDate { get; }

    IBudgetAccountValuesDisplayer ValuesForLastYearOfStatusDate { get; }
}