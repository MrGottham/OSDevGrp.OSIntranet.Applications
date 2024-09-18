using OSDevGrp.OSIntranet.Core;
using OSDevGrp.OSIntranet.Core.Interfaces.Enums;

namespace OSDevGrp.OSIntranet.WebApi.Helpers.Resolvers
{
    internal class ErrorDescriptionResolver
    {
        #region Methods

        internal static string Resolve(ErrorCode errorCode, params object[] argumentCollection)
        {
            NullGuard.NotNull(argumentCollection, nameof(argumentCollection));

            return new IntranetExceptionBuilder(errorCode, argumentCollection).Build().Message;
        }

        #endregion
    }
}