namespace GSAIServicesAPI
{
    /// <summary>
    /// To handle the app configuration settings. This class provides a static property to access the application's configuration and a method to initialize it. It is designed to be used throughout the application to retrieve configuration values as needed.
    /// </summary>
    public static class AppConfiguration
    {
        /// <summary>
        /// Gets the application's configuration settings. This property holds the configuration values that can be accessed throughout the application. It is initialized using the Initialize method and should be set before any configuration values are accessed.
        /// </summary>
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Initializes the application's configuration settings. This method should be called at the start of the application to set the Configuration property with the provided IConfiguration instance. It ensures that the configuration values are available for use throughout the application.
        /// </summary>
        /// <param name="configuration">The configuration instance to initialize with.</param>
        public static void Initialize(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
