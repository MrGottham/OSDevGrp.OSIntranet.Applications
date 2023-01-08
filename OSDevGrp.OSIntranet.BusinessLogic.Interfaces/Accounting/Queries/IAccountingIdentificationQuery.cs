using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core.Interfaces.QueryBus;
using OSDevGrp.OSIntranet.Repositories.Interfaces;
using System;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Accounting.Queries
{
    public interface IAccountingIdentificationQuery : IQuery
    {
        int AccountingNumber { get; set; }

        DateTime StatusDate { get; set; }

        IValidator Validate(IValidator validator, IAccountingRepository accountingRepository);
    }
}