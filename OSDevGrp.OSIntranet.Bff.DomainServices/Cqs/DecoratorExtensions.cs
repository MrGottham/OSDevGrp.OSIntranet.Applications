using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Cqs;

internal static class DecoratorExtensions
{
    #region Methods

    internal static ICommandFeature<TRequest> GetInnerMostFeature<TRequest>(this ICommandDecorator<TRequest> commandDecorator) where TRequest : IRequest
    {
        ICommandFeature<TRequest> innerFeature = commandDecorator.GetInnerFeature();
        while (true)
        {
            if (innerFeature is not ICommandDecorator<TRequest> temp)
            {
                return innerFeature;
            }
            innerFeature = temp.GetInnerFeature();
        }
    }

    internal static IQueryFeature<TRequest, TResponse> GetInnerMostFeature<TRequest, TResponse>(this IQueryDecorator<TRequest, TResponse> queryDecorator) where TRequest : IRequest where TResponse : IResponse
    {
        IQueryFeature<TRequest, TResponse> innerFeature = queryDecorator.GetInnerFeature();
        while (true)
        {
            if (innerFeature is not IQueryDecorator<TRequest, TResponse> temp)
            {
                return innerFeature;
            }
            innerFeature = temp.GetInnerFeature();
        }
    }

    #endregion
}