using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.SchemaValidation.SchemaValidationFilter;

[TestFixture]
public class OnActionExecutingTests : SchemaValidationFilterTestBase
{
    #region Private variables

    private Mock<ISchemaValidator>? _schemaValidatorMock;

    #endregion

    [SetUp]
    public void SetUp()
    {
        _schemaValidatorMock = new Mock<ISchemaValidator>();
    }

    [Test]
    [Category("UnitTest")]
    public void OnActionExecuting_WhenCalled_AssertValidateWasCalledOnSchemaValidatorWithModelStateFromActionExecutingContext()
    {
        IActionFilter sut = CreateSut();

        ModelStateDictionary modelState = new ModelStateDictionary();
        ActionContext actionContext = CreateActionContext(modelState: modelState);
        ActionExecutingContext actionExecutingContext = CreateActionExecutingContext(actionContext: actionContext);
        sut.OnActionExecuting(actionExecutingContext);

        _schemaValidatorMock!.Verify(m => m.Validate(It.Is<ModelStateDictionary>(value => value == modelState)), Times.Once);
    }

    private IActionFilter CreateSut()
    {
        return new WebApi.Filters.SchemaValidation.SchemaValidationFilter(_schemaValidatorMock!.Object);
    }

    private static ActionExecutingContext CreateActionExecutingContext(ActionContext? actionContext = null)
    {
        return new ActionExecutingContext(actionContext ?? CreateActionContext(), new List<IFilterMetadata>(), new Dictionary<string, object?>(), CreateController());
    }
}