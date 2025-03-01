using AutoFixture;
using Moq;
using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Cqs;
using OSDevGrp.OSIntranet.Bff.DomainServices.Tests.SecurityContext;
using OSDevGrp.OSIntranet.Bff.ServiceGateways.Interfaces.SecurityContext;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Tests.Cqs.PipelineExtensions;

public abstract class PipelineExtensionTestBase
{
    #region Methods

    protected static ICommandFeature<IRequest> CreateCommandFeature(bool isPermissionVerifiable = true, bool permissionGranted = true)
    {
        return CreateCommandFeatureMock(isPermissionVerifiable, permissionGranted).Object;
    }

    protected static Mock<ICommandFeature<IRequest>> CreateCommandFeatureMock(bool isPermissionVerifiable = true, bool permissionGranted = true)
    {
        Mock<ICommandFeature<IRequest>> commandFeatureMock = new Mock<ICommandFeature<IRequest>>();
        commandFeatureMock.Setup(m => m.ExecuteAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        if (isPermissionVerifiable)
        {
            Mock<IPermissionVerifiable> permissionVerifiableMock = commandFeatureMock.As<IPermissionVerifiable>();
            permissionVerifiableMock.Setup(m => m.VerifyPermissionAsync(It.IsAny<ISecurityContext>(), It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(permissionGranted));
        }

        return commandFeatureMock;
    }

    protected static ICommandFeature<IRequest> CreateFailingCommandFeature<TException>(Func<TException> exceptionGetter) where TException : Exception
    {
        return CreateFailingCommandFeatureMock(exceptionGetter).Object;
    }

    protected static Mock<ICommandFeature<IRequest>> CreateFailingCommandFeatureMock<TException>(Func<TException> exceptionGetter) where TException : Exception
    {
        Mock<ICommandFeature<IRequest>> commandFeatureMock = new Mock<ICommandFeature<IRequest>>();
        commandFeatureMock.Setup(m => m.ExecuteAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
            .Throws(exceptionGetter());
        return commandFeatureMock;
    }

    protected static IQueryFeature<IRequest, IResponse> CreateQueryFeature(IResponse? response = null, bool isPermissionVerifiable = true, bool permissionGranted = true)
    {
        return CreateQueryFeatureMock(response, isPermissionVerifiable, permissionGranted).Object;
    }

    protected static Mock<IQueryFeature<IRequest, IResponse>> CreateQueryFeatureMock(IResponse? response = null, bool isPermissionVerifiable = true, bool permissionGranted = true)
    {
        Mock<IQueryFeature<IRequest, IResponse>> queryFeatureMock = new Mock<IQueryFeature<IRequest, IResponse>>();
        queryFeatureMock.Setup(m => m.ExecuteAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(response ?? CreateResponse()));

        if (isPermissionVerifiable)
        {
            Mock<IPermissionVerifiable> permissionVerifiableMock = queryFeatureMock.As<IPermissionVerifiable>();
            permissionVerifiableMock.Setup(m => m.VerifyPermissionAsync(It.IsAny<ISecurityContext>(), It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(permissionGranted));
        }

        return queryFeatureMock;
    }

    protected static IQueryFeature<IRequest, IResponse> CreateFailingQueryFeature<TException>(Func<TException> exceptionGetter) where TException : Exception
    {
        return CreateFailingQueryFeatureMock(exceptionGetter).Object;
    }

    protected static Mock<IQueryFeature<IRequest, IResponse>> CreateFailingQueryFeatureMock<TException>(Func<TException> exceptionGetter) where TException : Exception
    {
        Mock<IQueryFeature<IRequest, IResponse>> queryFeatureMock = new Mock<IQueryFeature<IRequest, IResponse>>();
        queryFeatureMock.Setup(m => m.ExecuteAsync(It.IsAny<IRequest>(), It.IsAny<CancellationToken>()))
            .Throws(exceptionGetter());
        return queryFeatureMock;
    }

    protected static IRequest CreateRequest(Func<Fixture> fixtureGetter, Guid? requestId = null, ISecurityContext? securityContext = null)
    {
        return CreateRequestMock(fixtureGetter, requestId, securityContext).Object;
    }

    protected static Mock<IRequest> CreateRequestMock(Func<Fixture> fixtureGetter, Guid? requestId = null, ISecurityContext? securityContext = null)
    {
        Mock<IRequest> requestMock = new Mock<IRequest>();
        requestMock.Setup(m => m.RequestId)
            .Returns(requestId ?? Guid.NewGuid());
        requestMock.Setup(m => m.SecurityContext)
            .Returns(securityContext ?? fixtureGetter().CreateSecurityContext());
        return requestMock;
    }

    protected static IResponse CreateResponse()
    {
        return CreateResponseMock().Object;
    }

    protected static Mock<IResponse> CreateResponseMock()
    {
        return new Mock<IResponse>();
    }

    #endregion
}