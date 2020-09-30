namespace OSDevGrp.OSIntranet.Core.Interfaces
{
    public interface IConverter
    {
        IConverterCache Cache { get; }

        TTarget Convert<TSource, TTarget>(TSource source);
    }
}