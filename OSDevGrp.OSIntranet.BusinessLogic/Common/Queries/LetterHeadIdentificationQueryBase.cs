using OSDevGrp.OSIntranet.BusinessLogic.Interfaces.Common.Queries;

namespace OSDevGrp.OSIntranet.BusinessLogic.Common.Queries
{
    public abstract class LetterHeadIdentificationQueryBase : ILetterHeadIdentificationQuery
    {
        #region Properties

        public int Number { get; set; }

        #endregion
    }
}