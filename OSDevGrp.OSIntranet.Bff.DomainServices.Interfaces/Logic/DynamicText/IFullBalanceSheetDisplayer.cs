namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IFullBalanceSheetDisplayer
{
    string BalanceSheetLabel { get; }

    string BalanceSheetAtStatusDateLabel { get;}

    string BalanceSheetAtEndOfLastMonthFromStatusDateLabel { get; }

    string BalanceSheetAtEndOfLastYearFromStatusDateLabel { get; }

    string AssetsLabel { get;}

    string LiabilitiesLabel { get; }

    IValueDisplayer StatusDate { get; }

    IReadOnlyCollection<IFullBalanceSheetLineDisplayer> AssetsLines { get; }

    IReadOnlyCollection<IFullBalanceSheetLineDisplayer> LiabilitiesLines { get; }
}