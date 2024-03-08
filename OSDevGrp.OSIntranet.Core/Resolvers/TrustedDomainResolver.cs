using Microsoft.Extensions.Options;
using OSDevGrp.OSIntranet.Core.Interfaces.Resolvers;
using OSDevGrp.OSIntranet.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSDevGrp.OSIntranet.Core.Resolvers
{
    internal class TrustedDomainResolver : ITrustedDomainResolver
    {
        #region Private variables

        private readonly IOptions<TrustedDomainOptions> _trustedDomainOptions;

        #endregion

        #region Constructor

        public TrustedDomainResolver(IOptions<TrustedDomainOptions> trustedDomainOptions)
        {
            NullGuard.NotNull(trustedDomainOptions, nameof(trustedDomainOptions));

            _trustedDomainOptions = trustedDomainOptions;
        }

        #endregion

        #region Methods

        public bool IsTrustedDomain(Uri uri)
        {
            NullGuard.NotNull(uri, nameof(uri));

            return GetTrustDomainCollection().Contains(uri.Host);
        }

        private IEnumerable<string> GetTrustDomainCollection()
        {
            string trustedDomainCollection = _trustedDomainOptions.Value.TrustedDomainCollection;
            return string.IsNullOrWhiteSpace(trustedDomainCollection) == false
                ? trustedDomainCollection.Split(';').Select(trustedDomain => trustedDomain).ToArray()
                : Array.Empty<string>();
        }

        #endregion
    }
}