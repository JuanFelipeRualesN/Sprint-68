using Microsoft.Extensions.Configuration;

namespace CacheHandlerLibrary
{
    public static class AppConfiguration
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
