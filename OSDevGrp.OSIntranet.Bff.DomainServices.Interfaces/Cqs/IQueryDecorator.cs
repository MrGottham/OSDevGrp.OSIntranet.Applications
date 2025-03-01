namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs
{
    public interface IQueryDecorator<in TRequest, TResponse> : IDecorator where TRequest : IRequest where TResponse : IResponse
    {
        IQueryFeature<TRequest, TResponse> GetInnerFeature();
    }
}