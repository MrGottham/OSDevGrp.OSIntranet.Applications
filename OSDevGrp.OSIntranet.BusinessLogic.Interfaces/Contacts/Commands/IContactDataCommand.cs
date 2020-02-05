using System;
using OSDevGrp.OSIntranet.Domain.Interfaces.Contacts;
using OSDevGrp.OSIntranet.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Commands
{
    public interface IContactDataCommand : IContactCommand
    {
        INameCommand Name { get; set; }

        IAddressCommand Address { get; set; }

        string HomePhone { get; set; }

        string MobilePhone { get; set; }

        DateTime? Birthday { get; set; }

        string MailAddress { get; set; }

        ICompanyCommand Company { get; set; }

        int ContactGroupIdentifier { get; set; }

        string Acquaintance { get; set; }

        string PersonalHomePage { get; set; }

        int LendingLimit { get; set; }

        int PaymentTermIdentifier { get; set; }

        IContact ToDomain(IContactRepository contactRepository, IAccountingRepository accountingRepository);
    }
}