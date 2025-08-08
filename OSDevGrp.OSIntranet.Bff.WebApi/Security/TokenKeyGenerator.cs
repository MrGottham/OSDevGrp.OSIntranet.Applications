using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using System.Text;

namespace OSDevGrp.OSIntranet.Bff.WebApi.Security;

internal class TokenKeyGenerator : ITokenKeyGenerator
{
    #region Private variables

    private readonly IHashGenerator _hashGenerator;

    #endregion

    #region Constructor

    public TokenKeyGenerator(IHashGenerator hashGenerator)
    {
        _hashGenerator = hashGenerator;
    }

    #endregion

    #region Methods

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

        return await _hashGenerator.GenerateAsync(memoryStream.ToArray(), cancellationToken);
    }

    #endregion
}