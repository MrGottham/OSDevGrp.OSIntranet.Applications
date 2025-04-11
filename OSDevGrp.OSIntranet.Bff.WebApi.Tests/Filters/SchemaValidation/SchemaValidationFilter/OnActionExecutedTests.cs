using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Tests.Filters.SchemaValidation.SchemaValidationFilter;

[TestFixture]
public class OnActionExecutedTests : SchemaValidationFilterTestBase
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
    public void OnActionExecuted_WhenResultOnActionExecutedContextIsNull_AssertValidateWasNotCalledOnSchemaValidator()
    {
        IActionFilter sut = CreateSut();

        IActionResult? result = null;
        ActionExecutedContext actionExecutedContext = CreateActionExecutedContext(result: result);
        sut.OnActionExecuted(actionExecutedContext);

        _schemaValidatorMock!.Verify(m => m.Validate(It.IsAny<object>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public void OnActionExecuted_WhenResultOnActionExecutedContextIsNotTypeOfObjectResult_AssertValidateWasNotCalledOnSchemaValidator()
    {
        IActionFilter sut = CreateSut();

        IActionResult? result = new OkResult();;
        ActionExecutedContext actionExecutedContext = CreateActionExecutedContext(result: result);
        sut.OnActionExecuted(actionExecutedContext);

        _schemaValidatorMock!.Verify(m => m.Validate(It.IsAny<object>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public void OnActionExecuted_WhenResultOnActionExecutedContextIsTypeOfObjectResultWhereValueIsNull_AssertValidateWasNotCalledOnSchemaValidator()
    {
        IActionFilter sut = CreateSut();

        object? objectResultValue = null;
        IActionResult? result = new OkObjectResult(objectResultValue);
        ActionExecutedContext actionExecutedContext = CreateActionExecutedContext(result: result);
        sut.OnActionExecuted(actionExecutedContext);

        _schemaValidatorMock!.Verify(m => m.Validate(It.IsAny<object>()), Times.Never);
    }

    [Test]
    [Category("UnitTest")]
    public void OnActionExecuted_WhenResultOnActionExecutedContextIsTypeOfObjectResultWhereValueIsNotNull_AssertValidateWasCalledOnSchemaValidatorWithValueFromObjectResultAtActionExecutedContext()
    {
        IActionFilter sut = CreateSut();

        object objectResultValue = new object();
        IActionResult? result = new OkObjectResult(objectResultValue);
        ActionExecutedContext actionExecutedContext = CreateActionExecutedContext(result: result);
        sut.OnActionExecuted(actionExecutedContext);

        _schemaValidatorMock!.Verify(m => m.Validate(It.Is<object>(value => value == objectResultValue)), Times.Once);
    }

    private IActionFilter CreateSut()
    {
        return new WebApi.Filters.SchemaValidation.SchemaValidationFilter(_schemaValidatorMock!.Object);
    }

    private static ActionExecutedContext CreateActionExecutedContext(ActionContext? actionContext = null, IActionResult? result = null)
    {
        return new ActionExecutedContext(actionContext ?? CreateActionContext(), new List<IFilterMetadata>(), CreateController())
        {
            Result = result
        };
    }
}