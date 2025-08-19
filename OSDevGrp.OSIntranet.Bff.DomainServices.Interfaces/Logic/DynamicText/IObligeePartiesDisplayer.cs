namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IObligeePartiesDisplayer
{
    string Header { get; }

    IValueDisplayer Debtors  { get; }

    IValueDisplayer Creditors { get; }
}