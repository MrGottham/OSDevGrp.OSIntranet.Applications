namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IBudgetStatementDisplayer
{
    string Header { get; }

    IValueDisplayer Budget { get; }

    IValueDisplayer Posted { get; }

    IValueDisplayer Available { get; }
}