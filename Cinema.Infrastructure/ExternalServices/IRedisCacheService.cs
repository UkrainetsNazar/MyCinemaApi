namespace Cinema.Infrastructure.ExternalServices
{
    public interface IRedisCacheService
    {
        public T? Data<T>(string key);
        public void SetData<T>(string key, T data);
    }
}
