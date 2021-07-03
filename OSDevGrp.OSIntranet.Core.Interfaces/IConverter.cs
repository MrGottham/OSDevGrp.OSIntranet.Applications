namespace OSDevGrp.OSIntranet.Core.Interfaces
{
    public interface IConverter
    {
        TTarget Convert<TSource, TTarget>(TSource source);
    }
}