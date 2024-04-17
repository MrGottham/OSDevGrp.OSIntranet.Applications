using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Security;

namespace OSDevGrp.OSIntranet.Domain.Security
{
    public static class ScopeFactory
    {
        #region Methods

        public static IScopeBuilder Create(string name, string description)
        {
            NullGuard.NotNullOrWhiteSpace(name, nameof(name))
                .NotNullOrWhiteSpace(description, nameof(description));

            return new ScopeBuilder(name, description);
        }

        #endregion
    }
}