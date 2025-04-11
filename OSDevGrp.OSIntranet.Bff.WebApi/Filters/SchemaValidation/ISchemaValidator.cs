namespace OSDevGrp.OSIntranet.Bff.WebApi.Filters.SchemaValidation;

public interface ISchemaValidator
{
    void Validate(object obj);
}