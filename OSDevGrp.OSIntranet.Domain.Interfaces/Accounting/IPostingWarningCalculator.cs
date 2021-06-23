using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Domain.Interfaces.Accounting
{
    public interface IPostingWarningCalculator
    {
        Task<IPostingWarningCollection> CalculateAsync(IPostingLine postingLine);

        Task<IPostingWarningCollection> CalculateAsync(IPostingLineCollection postingLineCollection);
    }
}