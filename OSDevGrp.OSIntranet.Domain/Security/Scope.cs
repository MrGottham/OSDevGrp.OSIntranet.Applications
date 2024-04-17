using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;
using System.Collections.Generic;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    internal class Scope : IScope
    {
        #region Constructor

        public Scope(string name, string description, IEnumerable<string> relatedClaims)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name))
                .NotNullOrWhiteSpace(description, nameof(description))
                .NotNull(relatedClaims, nameof(relatedClaims));

            Name = name.Trim();
            Description = description.Trim();
            RelatedClaims = new HashSet<string>(relatedClaims);
        }

        #endregion

        #region Properties

        public string Name { get; }

        public string Description { get; }

        public IEnumerable<string> RelatedClaims { get; }

        #endregion
    }
}