using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class ScopeBuilder : IScopeBuilder
    {
        #region Private variables

        private readonly string _name;
        private readonly string _description;
        private readonly HashSet<string> _relatedScopes;

        #endregion

        #region Constructor

        public ScopeBuilder(string name, string description)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name))
                .NotNullOrWhiteSpace(description, nameof(description));

            _name = name;
            _description = description;
            _relatedScopes = new HashSet<string>();
        }

        #endregion

        #region Methods

        public IScopeBuilder WithRelatedClaim(string claimType)
        {
            NullGuard.NotNullOrWhiteSpace(claimType, nameof(claimType));

            _relatedScopes.Add(claimType);

            return this;
        }

        public IScope Build()
        {
            return new Scope(_name, _description, _relatedScopes);
        }

        #endregion
    }
}