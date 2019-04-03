using System;
using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public class Token : IToken
    {
        #region Constructors

        public Token(string value, DateTime expires)
        {
            NullGuard.NotNullOrWhiteSpace(value, nameof(value));

            Value = value;
            Expires = expires;
        }

        #endregion

        #region Properties

        public string Value { get; }

        public DateTime Expires { get; }

        #endregion
    }
}
