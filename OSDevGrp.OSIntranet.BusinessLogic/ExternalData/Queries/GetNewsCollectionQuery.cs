using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.ExternalData.Queries;
using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Validation;
using OSDevGrp.OSIntranet.Core;

namespace OSDevGrp.OSIntranet.BusinessLogic.ExternalData.Queries
{
    public class GetNewsCollectionQuery : IGetNewsCollectionQuery
    {
        #region Constructor

        public GetNewsCollectionQuery(bool fromExternalDashboard, int numberOfNews)
        {
            FromExternalDashboard = fromExternalDashboard;
            NumberOfNews = numberOfNews;
        }

        #endregion

        #region Properties

        public bool FromExternalDashboard { get; }

        public int NumberOfNews { get; }

        #endregion

        #region Methods

        public IValidator Validate(IValidator validator)
        {
            NullGuard.NotNull(validator, nameof(validator));

            return validator.Integer.ShouldBeBetween(NumberOfNews, 0, 250, GetType(), nameof(NumberOfNews));
        }

        #endregion
    }
}