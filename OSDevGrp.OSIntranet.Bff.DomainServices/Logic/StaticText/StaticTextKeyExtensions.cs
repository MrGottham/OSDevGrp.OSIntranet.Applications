using OSDevGrp.OSIntranet.Bff.DomainServices.Interfaces.Logic.StaticText;

namespace OSDevGrp.OSIntranet.Bff.DomainServices.Logic.StaticText;

internal static class StaticTextKeyExtensions
{
    #region Private variables

    private static readonly IEnumerable<object> NoArguments = [];

    #endregion

    #region Methods

    internal static IEnumerable<object> DefaultArguments(this StaticTextKey staticTextKey)
    {
        switch (staticTextKey)
        {
            case StaticTextKey.BalanceBelowZero:
                return [0];
        }   

        return NoArguments;
    }

    #endregion
}