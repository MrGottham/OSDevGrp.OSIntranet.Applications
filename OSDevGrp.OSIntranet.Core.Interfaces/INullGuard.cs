namespace OSDevGrp.OSIntranet.Core.Interfaces
{
    public interface INullGuard
    {
        INullGuard NotNull(object value, string argumentName);

        INullGuard NotNullOrEmpty(string value, string argumentName);

        INullGuard NotNullOrWhiteSpace(string value, string argumentName);
    }
}
