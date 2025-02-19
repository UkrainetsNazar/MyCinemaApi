namespace Cinema.Infrastructure.ExternalServices
{
    public interface IRedisCacheService
    {
        T? Data<T>(string key);
        void SetData<T>(string key, T data, int durationTime);
        Task ClearDataByPatternAsync(string pattern);
    }
}
