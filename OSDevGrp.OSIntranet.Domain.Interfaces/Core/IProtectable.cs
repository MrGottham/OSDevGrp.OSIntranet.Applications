namespace OSDevGrp.OSIntranet.Domain.Interfaces.Core
{
    public interface IProtectable : IDeletable
    {
        bool IsProtected { get; }

        void ApplyProtection();
    }
}