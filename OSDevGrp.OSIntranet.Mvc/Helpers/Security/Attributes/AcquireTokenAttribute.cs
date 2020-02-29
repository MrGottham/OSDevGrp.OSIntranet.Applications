using System;
using OSDevGrp.OSIntranet.Mvc.Helpers.Security.Enums;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AcquireTokenAttribute : Attribute
    {
        #region Constructor

        public AcquireTokenAttribute(TokenType tokenType)
        {
            TokenType = tokenType;
        }

        #endregion

        #region Properties

        public TokenType TokenType { get; }

        #endregion
    }
}