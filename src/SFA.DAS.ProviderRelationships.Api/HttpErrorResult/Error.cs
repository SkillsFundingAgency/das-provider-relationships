namespace SFA.DAS.ProviderRelationships.Api.HttpErrorResult
{
    public class Error
    {
        public string ErrorCode { get; }
        public string Message { get; }
        
        public Error(string errorCode = null, string message = null)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}