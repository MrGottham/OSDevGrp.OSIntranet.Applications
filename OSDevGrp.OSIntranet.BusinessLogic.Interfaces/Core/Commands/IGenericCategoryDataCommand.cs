using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands
{
    public interface IGenericCategoryDataCommand<TGenericCategory> : IGenericCategoryIdentificationCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
        string Name { get; }

        TGenericCategory ToDomain();
    }
}