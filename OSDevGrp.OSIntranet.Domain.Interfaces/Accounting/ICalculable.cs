using System;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface ICalculable
    {
        DateTime StatusDate { get; }
    }

    public interface ICalculable<T> : ICalculable where T : ICalculable
    {
        Task<T> CalculateAsync(DateTime statusDate);
    }
}