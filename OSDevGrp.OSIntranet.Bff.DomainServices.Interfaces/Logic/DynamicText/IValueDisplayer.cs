namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IValueDisplayer
{
    public string Label { get; }

    public string? Value { get; }
}