namespace OSDevGrp.OSIntranet.Domain.Interfaces.Core
{
    public interface IGenericCategory : IAuditable, IDeletable
    {
        public int Number { get; }

        public string Name { get; }
    }
}