using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces;

public interface IConverterFactory
{
    IConverter CreateContactModelConverter();

    IConverter CreateAccountingModelConverter();

    IConverter CreateMediaLibraryModelConverter();

    IConverter CreateCommonModelConverter();

    IConverter CreateSecurityModelConverter();

    IConverter CreateMicrosoftGraphModelConverter();

    IConverter CreateExternalDashboardConverter();
}