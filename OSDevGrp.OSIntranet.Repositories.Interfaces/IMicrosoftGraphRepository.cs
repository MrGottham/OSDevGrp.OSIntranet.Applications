using System;
using System.Threading.Tasks;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IMicrosoftGraphRepository : IRepository
    {
        Task<Uri> GetAuthorizeUriAsync(Uri redirectUri, Guid stateIdentifier);

        Task<IRefreshableToken> AcquireTokenAsync(Uri redirectUri, string code);

        Task<IRefreshableToken> RefreshTokenAsync(Uri redirectUri, IRefreshableToken refreshableToken);

        Task GetContacts(IRefreshableToken refreshableToken);
    }
}
