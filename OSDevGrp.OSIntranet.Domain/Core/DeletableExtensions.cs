using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Domain.Interfaces.Core;

namespace OSDevGrp.OSIntranet.Domain.Core
{
    public static class DeletableExtensions
    {
        public static void SetDeletable(this IDeletable deletable, bool canDelete)
        {
            NullGuard.NotNull(deletable, nameof(deletable));

            if (canDelete)
            {
                deletable.AllowDeletion();
                return;
            }

            deletable.DisallowDeletion();
        }
    }
}