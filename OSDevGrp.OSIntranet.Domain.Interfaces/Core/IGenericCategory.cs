namespace OSDevGrp.OSIntranet.Domain.Interfaces.Core
{
    public interface IGenericCategory : IAuditable, IDeletable
    {
        int Number { get; }

        string Name { get; }
    }
}