namespace API.Services
{
    public interface IHttpClientFactoryService
    {
        public Task<List<T>> ExecuteAsync<T>(string endpoint,int loop=1);
    }
}
