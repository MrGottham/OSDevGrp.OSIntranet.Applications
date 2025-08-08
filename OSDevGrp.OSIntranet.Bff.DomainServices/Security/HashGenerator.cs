using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Security.Cryptography;
using System.Text;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Security;

internal class HashGenerator : IHashGenerator
{
    #region Private variables

    private readonly SHA512 _sha512 = SHA512.Create();
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    #endregion

    #region Methods

    public void Dispose()
    {
        _sha512.Dispose();
        _semaphore.Dispose();
    }

    public async Task<string> GenerateAsync(byte[] buffer, CancellationToken cancellationToken = default)
    {
        if (buffer.Length == 0)
        {
            throw new ArgumentException("The buffer is empty.", nameof(buffer));
        }

        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            using MemoryStream memoryStream = new MemoryStream(buffer);

            memoryStream.Seek(0, SeekOrigin.Begin);
            byte[] hash = await _sha512.ComputeHashAsync(memoryStream, cancellationToken);

            StringBuilder keyBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                keyBuilder.Append(b.ToString("x2"));
            }

            return keyBuilder.ToString();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    #endregion
}