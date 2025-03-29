using System.ComponentModel.DataAnnotations;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;

internal class SchemaValidator : ISchemaValidator
{
    #region Private variables

    private readonly DataAnnotationsValidator.DataAnnotationsValidator _dataAnnotationsValidator = new DataAnnotationsValidator.DataAnnotationsValidator();

    #endregion

    #region Methods

    public void Validate(object obj)
    {
        List<ValidationResult> validationResultCollection = new List<ValidationResult>();

        bool isValid = _dataAnnotationsValidator.TryValidateObjectRecursive(obj, validationResultCollection);
        if (isValid)
        {
            return;
        }

        throw new SchemaValidationException(validationResultCollection);
    }

    #endregion
}