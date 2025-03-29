using System.Security.Cryptography;
using System.Text;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class TokenKeyGenerator : ITokenKeyGenerator
{
    #region Private variables

    private readonly SHA512 _sha512 = SHA512.Create();

    #endregion

    #region Methods

    public void Dispose()
    {
        _sha512.Dispose();
    }

    public async Task<string> GenerateAsync(IReadOnlyCollection<string> values, CancellationToken cancellationToken = default)
    {
        if (values.Count == 0)
        {
            throw new ArgumentException("The given collection does not contain any items.", nameof(values));
        }

        using MemoryStream memoryStream = new MemoryStream();
        using StreamWriter streamWriter = new StreamWriter(memoryStream);
        for (int i = 0; i < values.Count; i++)
        {
            if (i > 0)
            {
                await streamWriter.WriteAsync('|');
            }
            await streamWriter.WriteAsync(values.ElementAt(i));
        }
        await streamWriter.FlushAsync();

        memoryStream.Seek(0, SeekOrigin.Begin);
        byte[] hash = await _sha512.ComputeHashAsync(memoryStream, cancellationToken);

        StringBuilder keyBuilder = new StringBuilder();
        foreach (byte b in hash)
        {
            keyBuilder.Append(b.ToString("x2"));
        }

        return keyBuilder.ToString();
    }

    #endregion
}