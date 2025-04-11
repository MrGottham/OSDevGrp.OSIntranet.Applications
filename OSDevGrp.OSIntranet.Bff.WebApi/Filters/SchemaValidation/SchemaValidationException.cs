using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;

public class SchemaValidationException : Exception
{
    #region Constructors

    public SchemaValidationException(string message) 
        : base(message)
    {
    }

    public SchemaValidationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }

    internal SchemaValidationException(IEnumerable<ValidationResult> validationResults) 
        : base(ToMessage(validationResults.ToArray()))
    {
    }

    internal SchemaValidationException(IEnumerable<ValidationResult> validationResults, Exception innerException) 
        : base(ToMessage(validationResults.ToArray()), innerException)
    {
    }

    #endregion

    #region Methods

    private static string ToMessage(ValidationResult[] validationResults)
    {
        string[] errorMessages = validationResults.Where(validationResult => string.IsNullOrWhiteSpace(validationResult.ErrorMessage) == false)
            .Select(validationResult => validationResult.ErrorMessage!)
            .ToArray();

        return ToMessage(errorMessages);
    }

    private static string ToMessage(string[] errorMessages)
    {
        if (errorMessages.Length == 0)
        {
            return "One or more values does not satisfy the specified requirements.";
        }

        if (errorMessages.Length == 1)
        {
            return errorMessages[0];
        }

        StringBuilder messageBuilder = new StringBuilder();
        messageBuilder.AppendLine("Multiple values does not satisfy the specified requirements:");
        foreach (string errorMessage in errorMessages)
        {
            messageBuilder.AppendLine($"- {errorMessage}");
        }

        return messageBuilder.ToString();;
    }

    #endregion
}