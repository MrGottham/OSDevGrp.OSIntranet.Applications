using OSDevGrp.OSIntranet.Core.Interfaces;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Factories;

public interface IConverterFactory
{
    IConverter CreateHomeViewModelConverter();

    IConverter CreateContactViewModelConverter();

    IConverter CreateAccountingViewModelConverter();

    IConverter CreateMediaLibraryViewModelConverter();

    IConverter CreateCommonViewModelConverter();

    IConverter CreateSecurityViewModelConverter();
}