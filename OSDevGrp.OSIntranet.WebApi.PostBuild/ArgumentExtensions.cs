namespace OSDevGrp.OSIntranet.WebApi.PostBuild;

public static class ArgumentExtensions
{
    #region Methods

    public static bool IsPostBuild(this IReadOnlyCollection<string> arguments)
    {
        return arguments.Count > 0 && arguments.ElementAt(0) == "postbuild";
    }

    #endregion
}