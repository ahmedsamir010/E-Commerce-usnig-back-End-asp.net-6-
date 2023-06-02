namespace API_01.Errors
{
    public class ApiResponse
    {
        private static readonly Dictionary<int, string> DefaultMessages = new Dictionary<int, string>
        {
            { 400, "Bad Request" },
            { 401, "Unauthorized" },
            { 404, "Resource Not Found" },
            { 500, "Internal Server Error" }
        };

        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            if (DefaultMessages.TryGetValue(statusCode, out var defaultMessage))
            {
                return defaultMessage;
            }

            return "An error occurred.";
        }
    }
}
