namespace Library.API.Extensions
{
    public static class OptionsExtensions
    {
        public static void ConfigureOptions<TOptions>(this IServiceCollection services, IConfiguration configuration) where TOptions : class
        {
            services.Configure<TOptions>(configuration.GetSection(typeof(TOptions).Name));
        }
    }
}
