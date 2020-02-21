using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.Mvc.Helpers.Security
{
    public class TrustedDomainHelper : ITrustedDomainHelper
    {
        #region Private variables

        private readonly string[] _trustedDomainCollection;

        #endregion

        #region Constructor

        public TrustedDomainHelper(IConfiguration configuration)
        {
            NullGuard.NotNull(configuration, nameof(configuration));

            string trustedDomainCollection = configuration["Security:TrustedDomainCollection"];
            _trustedDomainCollection = string.IsNullOrWhiteSpace(trustedDomainCollection) ? new string[0] : trustedDomainCollection.Split(';');
        }

        #endregion

        #region Methods

        public bool IsTrustedDomain(Uri uri)
        {
            NullGuard.NotNull(uri, nameof(uri));

            return _trustedDomainCollection.Contains(uri.Host);
        }

        #endregion
    }
}