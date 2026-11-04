namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IBudgetAccountValuesDisplayer
{
    string Header { get; }

    IValueDisplayer Budget { get; }

    IValueDisplayer Posted { get; }

    IValueDisplayer Available { get; }
}