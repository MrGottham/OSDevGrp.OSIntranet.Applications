using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Security;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Globalization;
using System.Security.Cryptography;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Security;

internal class CaptchaGenerator : ICaptchaGenerator
{
    #region Private constants

    private const int Width = 250;
    private const int Height = 100;
    private const int NoiseLines = 50;
    private const int FontSize = 32;

    #endregion

    #region Methods

    public async Task<byte[]> GenerateAsync(string code, CancellationToken cancellationToken = default)
    {
        FontFamily fontFamily = SystemFonts.Collection.GetByCulture(CultureInfo.InvariantCulture).First();

        using Image<Rgba32> image = CreateImage(Width, Height);
        AddNoise(image, NoiseLines);
        AddCode(image, code, SystemFonts.CreateFont(fontFamily.Name, FontSize, FontStyle.Bold));

        using MemoryStream memoryStream = new MemoryStream();
        await image.SaveAsPngAsync(memoryStream, cancellationToken);

        return memoryStream.ToArray();
    }

    private static Image<Rgba32> CreateImage(int width, int height)
    {
        Image<Rgba32> image = new Image<Rgba32>(width, height);
        image.Mutate(context => context.Fill(Color.LightSkyBlue));
        return image;
    }

    private static void AddNoise(Image<Rgba32> image, int noiseLines)
    {
        image.Mutate(context =>
        {
            for (int i = 0; i < noiseLines; i++)
            {
                Point startingPoint = new Point(RandomNumberGenerator.GetInt32(0, image.Width + 1), RandomNumberGenerator.GetInt32(0, image.Height + 1));
                Point endingPoint = new Point(RandomNumberGenerator.GetInt32(startingPoint.X, image.Width + 1), RandomNumberGenerator.GetInt32(startingPoint.Y, image.Height + 1));
                context.DrawLine(Color.WhiteSmoke.WithAlpha(0.5f), 2, startingPoint, endingPoint);
            }
        });
    }

    private static void AddCode(Image<Rgba32> image, string code, Font font)
    {
        TextMeasurer.TryMeasureCharacterSizes(code, new TextOptions(font), out ReadOnlySpan<GlyphBounds> characterSizes);
        float maxWidth = characterSizes.ToArray().Max(c => c.Bounds.Width) + 10;
        float maxHeight = characterSizes.ToArray().Max(c => c.Bounds.Height) + 20;

        float minX = Math.Max(0, (image.Width - (maxWidth * code.Length)) / 2);
        float minY = Math.Max(0, (image.Height - maxHeight) / 2);

        image.Mutate(conntext =>
        {
            for (int i = 0; i < code.Length; i++)
            {
                float x = minX + (i * maxWidth) + RandomNumberGenerator.GetInt32(0, 11);
                float y = minY + RandomNumberGenerator.GetInt32(0, 21);
                conntext.DrawText(code[i].ToString(), font, Color.DarkGray.WithAlpha(0.8f), new PointF(x, y));
            }
        });
    }

    #endregion
}