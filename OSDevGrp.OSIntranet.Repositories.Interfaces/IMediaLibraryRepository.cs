using OSDevGrp.OSIntranet.Domain.Interfaces.MediaLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OSDevGrp.OSIntranet.Repositories.Interfaces
{
    public interface IMediaLibraryRepository : IRepository
    {
        Task<IEnumerable<IMediaType>> GetMediaTypesAsync();

        Task<IMediaType> GetMediaTypeAsync(int number);

        Task CreateMediaTypeAsync(IMediaType mediaType);

        Task UpdateMediaTypeAsync(IMediaType mediaType);

        Task DeleteMediaTypeAsync(int number);
    }
}