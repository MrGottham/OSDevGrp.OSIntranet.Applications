namespace OSDevGrp.OSIntranet.WebApi.Models.Core
{
    public class ErrorModel
    {
        public int ErrorCode { get; set; }

        public string ErrorType { get; set; }

        public string ErrorMessage { get; set; }

        public string Method { get; set; }
    }
}
