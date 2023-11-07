using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands
{
    public interface ICreateGenericCategoryCommand<TGenericCategory> : IGenericCategoryDataCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
    }
}