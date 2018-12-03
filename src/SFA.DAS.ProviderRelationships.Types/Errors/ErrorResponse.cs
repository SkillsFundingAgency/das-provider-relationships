namespace SFA.DAS.ProviderRelationships.Types.Errors
{
    public class ErrorResponse
    {
        public string ErrorCode { get; }
        public string Message { get; }
        
        public ErrorResponse(string errorCode = null, string message = null)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}