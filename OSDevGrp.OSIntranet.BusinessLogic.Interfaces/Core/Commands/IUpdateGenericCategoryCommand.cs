using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands
{
    public interface IUpdateGenericCategoryCommand<TGenericCategory> : IGenericCategoryDataCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
    }
}