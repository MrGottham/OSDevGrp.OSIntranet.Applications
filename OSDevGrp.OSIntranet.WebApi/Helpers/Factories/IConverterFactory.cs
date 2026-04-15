using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Factories;

public interface IConverterFactory
{
    IConverter CreateCoreModelConverter();

    IConverter CreateAccountingModelConverter();

    IConverter CreateCommonModelConverter();

    IConverter CreateSecurityModelConverter();
}