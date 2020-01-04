using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface ICalculable<T>
    {
        DateTime StatusDate { get; }

        Task<T> CalculateAsync(DateTime statusDate);
    }
}