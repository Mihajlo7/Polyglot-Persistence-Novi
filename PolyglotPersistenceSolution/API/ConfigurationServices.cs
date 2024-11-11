using API.JsonConverter;
using API.Services;
using RelationDataAccess.Implementation;
using IDataAccess;
using HybridDataAccess.Implementation;

namespace API
{
    public static class ConfigurationServices
    {
        public static IServiceCollection ConfigureHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["GeneratorAPI:Url"];
            var headerKey = configuration["GeneratorAPI:HeaderKey"];
            var headerValue = configuration["GeneratorAPI:HeaderValue"];

            services.AddHttpClient("GeneratorAPI", config =>
            {
                config.BaseAddress = new Uri(baseUrl);
                config.DefaultRequestHeaders.Add(headerKey, headerValue);

                config.Timeout=new TimeSpan(0,30,0);
            });

            services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();

            return services;
        }

        public static IServiceCollection ConfigureRepository(this IServiceCollection services)
        {
            /*
            services.AddScoped<IConsumerRepository, ConsumerRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            */
            services.AddScoped<IDataAccess.IConsumerRepository>(provider => new SqlConsumerRepository("small_db"));
            return services;
        }

        public static IServiceCollection ConfigureConverters(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new ProductDetailsConverter());
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });
            return services;
        }
    }
}
