using System;
using System.Globalization;

namespace OSDevGrp.OSIntranet.BusinessLogic.Providers
{
    internal static class DefaultFormatProvider
    {
        #region Methods

        internal static IFormatProvider Create()
        {
            return new CultureInfo("da-DK");
        }

        #endregion
    }
}