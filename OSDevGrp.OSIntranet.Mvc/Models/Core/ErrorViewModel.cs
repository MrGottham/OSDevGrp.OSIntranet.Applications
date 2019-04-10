namespace OSDevGrp.OSIntranet.Mvc.Models.Core
{
    public class ErrorViewModel
    {
        #region Properties

        public string RequestId { get; set; }

        public bool ShowRequestId => string.IsNullOrWhiteSpace(RequestId) == false;

        public int? ErrorCode { get; set; }

        public bool ShowErrorCode => ErrorCode.HasValue;

        public string ErrorMesssage { get; set; }

        public bool ShowErrorMessage => string.IsNullOrWhiteSpace(ErrorMesssage) == false;

        #endregion
    }
}