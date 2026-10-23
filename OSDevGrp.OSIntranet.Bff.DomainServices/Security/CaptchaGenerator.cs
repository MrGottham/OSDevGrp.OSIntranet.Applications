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
        image.Mutate(context => context.Paint(canvas => canvas.Fill(Brushes.Solid(Color.LightSkyBlue))));
        return image;
    }

    private static void AddNoise(Image<Rgba32> image, int noiseLines)
    {
        image.Mutate(context =>
        {
            context.Paint(canvas =>
            {
                for (int i = 0; i < noiseLines; i++)
                {
                    Point startingPoint = new Point(RandomNumberGenerator.GetInt32(0, image.Width + 1), RandomNumberGenerator.GetInt32(0, image.Height + 1));
                    Point endingPoint = new Point(RandomNumberGenerator.GetInt32(startingPoint.X, image.Width + 1), RandomNumberGenerator.GetInt32(startingPoint.Y, image.Height + 1));
                    canvas.DrawLine(Pens.Solid(Color.WhiteSmoke.WithAlpha(0.5f), 2), startingPoint, endingPoint);
                }
            });
        });
    }

    private static void AddCode(Image<Rgba32> image, string code, Font font)
    {
        TextOptions textOptions = new TextOptions(font);
        FontRectangle[] characterBounds = code.Select(character => TextMeasurer.MeasureBounds(character.ToString(), textOptions)).ToArray();
        float maxWidth = characterBounds.Max(bounds => bounds.Width) + 10;
        float maxHeight = characterBounds.Max(bounds => bounds.Height) + 20;

        float minX = Math.Max(0, (image.Width - (maxWidth * code.Length)) / 2);
        float minY = Math.Max(0, (image.Height - maxHeight) / 2);

        image.Mutate(context => context.Paint(canvas =>
        {
            for (int i = 0; i < code.Length; i++)
            {
                float x = minX + (i * maxWidth) + RandomNumberGenerator.GetInt32(0, 11);
                float y = minY + RandomNumberGenerator.GetInt32(0, 21);
                RichTextOptions richTextOptions = new RichTextOptions(font)
                {
                    Origin = new PointF(x, y)
                };
                canvas.DrawText(richTextOptions, code.AsSpan(i, 1), Brushes.Solid(Color.DarkGray.WithAlpha(0.8f)), null);
            }
        }));
    }

    #endregion
}