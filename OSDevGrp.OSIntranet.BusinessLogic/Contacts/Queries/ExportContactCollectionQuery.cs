using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Contacts.Queries;
using OSDevGrp.OSIntranet.Core.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Contacts.Queries
{
    public class ExportContactCollectionQuery : RefreshableTokenBasedQuery, IExportContactCollectionQuery
    {
        #region Methods

        public IGetContactCollectionQuery ToGetContactCollectionQuery()
        {
            return new GetContactCollectionQuery
            {
                TokenType = TokenType,
                AccessToken = AccessToken,
                RefreshToken = RefreshToken,
                Expires = Expires
            };
        }

        #endregion
    }
}