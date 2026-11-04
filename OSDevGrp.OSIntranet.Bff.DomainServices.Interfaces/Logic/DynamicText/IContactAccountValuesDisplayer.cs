namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IContactAccountValuesDisplayer
{
    string Header { get; }

    IValueDisplayer Balance { get; }
}