using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Features.Commands.MyFirstCommand;

internal class MyFirstCommandFeature : ICommandFeature<MyFirstCommandRequest>
{
    #region Methods

    public Task ExecuteAsync(MyFirstCommandRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion
}