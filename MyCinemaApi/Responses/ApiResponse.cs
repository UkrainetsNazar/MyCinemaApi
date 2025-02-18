public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
    public double? ProcessingTimeMs { get; set; }

    public ApiResponse(int statusCode, T? data = default, string? errorMessage = null, double? processingTimeMs = null)
    {
        StatusCode = statusCode;
        Data = data;
        ErrorMessage = errorMessage;
        ProcessingTimeMs = processingTimeMs;
    }
}
