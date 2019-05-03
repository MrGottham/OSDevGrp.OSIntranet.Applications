namespace OSDevGrp.OSIntranet.Domain.Interfaces.Core
{
    public interface IDeletable
    {
        bool Deletable { get; }

        void AllowDeletion();

        void DisallowDeletion();
    }
}