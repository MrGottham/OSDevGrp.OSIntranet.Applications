namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IAccountValuesDisplayer
{
    string Header { get; }

    IValueDisplayer Credit { get; }

    IValueDisplayer Balance { get; }

    IValueDisplayer Available { get; }
}