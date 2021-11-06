using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Common
{
    public interface IKeyValueEntry : IAuditable, IDeletable
    {
        string Key { get; }

        byte[] Value { get; }

        T ToObject<T>() where T : class;

        string ToBase64();
    }
}