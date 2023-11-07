using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Core.Commands
{
    public interface IDeleteGenericCategoryCommand<TGenericCategory> : IGenericCategoryIdentificationCommand<TGenericCategory> where TGenericCategory : IGenericCategory
    {
    }
}