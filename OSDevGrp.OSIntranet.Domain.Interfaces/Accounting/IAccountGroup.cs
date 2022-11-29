using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Accounting.Enums;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IAccountGroup : IAccountGroupBase
    {
        AccountGroupType AccountGroupType { get; }

        Task<IAccountGroupStatus> CalculateAsync(DateTime statusDate, IAccountCollection accountCollection);
    }
}