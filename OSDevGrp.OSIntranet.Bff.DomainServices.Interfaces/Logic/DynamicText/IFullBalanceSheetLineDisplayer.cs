namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IFullBalanceSheetLineDisplayer
{
    string? Identification { get; }

    string Description { get; }

    string? CreditAtStatusDate { get; }

    string? BalanceAtStatusDate { get; }

    string? AvailableAtStatusDate { get; }

    string? CreditAtEndOfLastMonthFromStatusDate { get; }

    string? BalanceAtEndOfLastMonthFromStatusDate { get; }

    string? AvailableAtEndOfLastMonthFromStatusDate { get; }

    string? CreditAtEndOfLastYearFromStatusDate { get; }

    string? BalanceAtEndOfLastYearFromStatusDate { get; }

    string? AvailableAtEndOfLastYearFromStatusDate { get; }
}