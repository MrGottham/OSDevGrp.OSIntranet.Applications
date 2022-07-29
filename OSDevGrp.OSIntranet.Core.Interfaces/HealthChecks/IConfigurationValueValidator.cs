using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Core.Interfaces.HealthChecks
{
    public interface IConfigurationValueValidator
    {
        Task ValidateAsync();
    }
}