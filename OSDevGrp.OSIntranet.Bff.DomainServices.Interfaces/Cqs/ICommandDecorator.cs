namespace OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs
{
    public interface ICommandDecorator<TRequest> : IDecorator where TRequest : IRequest
    {
        ICommandFeature<TRequest> GetInnerFeature();
    }
}