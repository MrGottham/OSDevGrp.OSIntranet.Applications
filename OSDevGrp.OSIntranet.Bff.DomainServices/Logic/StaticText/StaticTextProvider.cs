using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;
using System.Collections.Concurrent;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

internal class StaticTextProvider : IStaticTextProvider
{
    #region Private variables

    private readonly IReadOnlyDictionary<StaticTextKey, string> _staticTexts = GenerateStaticTexts();

    #endregion

    #region Methods

    public Task<string> GetStaticTextAsync(StaticTextKey staticTextKey, IEnumerable<object> arguments, IFormatProvider formatProvider, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(formatProvider);

        cancellationToken.ThrowIfCancellationRequested();

        if (_staticTexts.TryGetValue(staticTextKey, out string? staticText) == false)
        {
            throw new ArgumentOutOfRangeException(nameof(staticTextKey), staticTextKey, $"The static text key {staticTextKey} is not supported.");
        }

        return Task.FromResult(string.Format(formatProvider, staticText, arguments.ToArray()));
    }

    private static IReadOnlyDictionary<StaticTextKey, string> GenerateStaticTexts()
    {
        IDictionary<StaticTextKey, string> staticTexts = new ConcurrentDictionary<StaticTextKey, string>();
        staticTexts.Add(StaticTextKey.MrGotthamsHomepage, "Mr. Gottham's Homepage");
        staticTexts.Add(StaticTextKey.OSDevelopmentGroup, "OS Development Group");
        staticTexts.Add(StaticTextKey.Copyright, "OS Development Group © {0}");
        staticTexts.Add(StaticTextKey.BuildInfo, "Build {0:yyyyMMddHHmm}");
        staticTexts.Add(StaticTextKey.Start, "Start");
        staticTexts.Add(StaticTextKey.Login, "Log ind");
        staticTexts.Add(StaticTextKey.Logout, "Log ud");
        return staticTexts.AsReadOnly();
    }

    #endregion
}