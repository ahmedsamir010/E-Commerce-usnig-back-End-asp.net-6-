namespace API_01.Errors
{
    public class ApiExceptionResponse:ApiResponse
    {
        public string? Destails { get; set; }

        public ApiExceptionResponse(int statusCode ,string? message=null , string? details=null):base(statusCode,message)
        {
            Destails = details;
        }

    }
}
