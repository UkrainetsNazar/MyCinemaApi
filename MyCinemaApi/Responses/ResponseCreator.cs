public static class ResponseCreator
{
    public static ApiResponse<T> Success<T>(T data, int statusCode = 200, double? processingTimeMs = null)
    {
        return new ApiResponse<T>(statusCode, data, null, processingTimeMs);
    }

    public static ApiResponse<T> Error<T>(string errorMessage, int statusCode = 400, double? processingTimeMs = null)
    {
        return new ApiResponse<T>(statusCode, default, errorMessage, processingTimeMs);
    }
}
