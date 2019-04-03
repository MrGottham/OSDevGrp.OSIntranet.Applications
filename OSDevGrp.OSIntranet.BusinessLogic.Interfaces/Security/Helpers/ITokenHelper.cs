using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Security.Helpers
{
    public interface ITokenHelper
    {
        IToken Generate(IClientSecretIdentity clientSecretIdentity);
    }
}
