using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Queries.MyFirstQuery;

internal class MyFirstQueryFeature : IQueryFeature<MyFirstQueryRequest, MyFirstQueryResponse>
{
    #region Methods

    public Task<MyFirstQueryResponse> ExecuteAsync(MyFirstQueryRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion
}