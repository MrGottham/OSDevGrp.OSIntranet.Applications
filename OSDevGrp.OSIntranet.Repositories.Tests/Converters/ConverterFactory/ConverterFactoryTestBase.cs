using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Repositories.Tests.Converters.ConverterFactory;

public abstract class ConverterFactoryTestBase : RepositoryTestBase
{
    #region Methods

    protected IConverterFactory CreateSut()
    {
        return new Repositories.Converters.ConverterFactory(CreateLicensesOptions(), CreateLoggerFactory());
    }

    #endregion
}