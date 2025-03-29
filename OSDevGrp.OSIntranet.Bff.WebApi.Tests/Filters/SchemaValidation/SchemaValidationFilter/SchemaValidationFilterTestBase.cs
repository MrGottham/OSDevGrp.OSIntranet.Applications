using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.SchemaValidation.SchemaValidationFilter;

public abstract class SchemaValidationFilterTestBase
{
    #region Methods

    protected static ActionContext CreateActionContext(ModelStateDictionary? modelState = null)
    {
        return new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), modelState ?? CreateModelState());
    }

    protected static ModelStateDictionary CreateModelState()
    {
        return new ModelStateDictionary();
    }

    protected static Controller CreateController()
    {
        return CreateControllerMock().Object;
    }

    private static Mock<Controller> CreateControllerMock()
    {
        return new Mock<Controller>();
    }

    #endregion
}