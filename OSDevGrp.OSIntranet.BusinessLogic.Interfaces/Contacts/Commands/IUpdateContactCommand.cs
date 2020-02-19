using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IUpdateContactCommand : IContactDataCommand
    {
        string ExternalIdentifier { get; set; }

        Task<IContact> GetExistingContactAsync(IMicrosoftGraphRepository microsoftGraphRepository, IContactRepository contactRepository);
    }
}