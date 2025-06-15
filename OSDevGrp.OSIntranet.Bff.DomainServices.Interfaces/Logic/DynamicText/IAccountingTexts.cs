namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IAccountingTexts : IDynamicTexts
{
    IValueDisplayer BalanceBelowZero { get; }

    IValueDisplayer BackDating { get; }
}