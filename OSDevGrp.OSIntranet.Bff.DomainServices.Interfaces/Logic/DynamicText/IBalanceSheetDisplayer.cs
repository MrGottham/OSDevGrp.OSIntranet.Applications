namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IBalanceSheetDisplayer
{
    string Header { get; }

    IValueDisplayer Assets { get; }

    IValueDisplayer Liabilities { get; }
}