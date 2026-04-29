namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.DynamicText;

public interface IChartOfAccountsSectionDisplayer
{
    string Identification { get; }

    string Description { get; }

    IReadOnlyCollection<IChartOfAccountsLineDisplayer> Lines { get; }
}