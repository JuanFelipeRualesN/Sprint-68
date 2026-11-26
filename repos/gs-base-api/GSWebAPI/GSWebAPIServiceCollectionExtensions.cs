using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Interceptors;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.Community;
using GSWebAPI.GSWebAPI.Services.CustomPlans;
using GSWebAPI.GSWebAPI.Services.CustomPlans.Assistant;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GSWebAPI
{
    /// <summary>Dependency-injection registration of the EF Core modules (Custom Plans and Community).</summary>
    public static class GSWebAPIServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the EF Core data layer (GSWebDbContext on ConnectionStrings:GSWebDBConnection, the same database
        /// and setting the existing DAOs use) and the Custom Plans and Community services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">
        /// App configuration: role ids (AppSettings:ur_role_client, default "1"; AppSettings:ur_role_client_child, default "3")
        /// and optional extra blocked words (AppSettings:community_blocked_words). The connection string is read from the
        /// container's <see cref="IConfiguration"/> each time a context is created.
        /// </param>
        /// <returns>The same <paramref name="services"/>, for chaining.</returns>
        public static IServiceCollection AddGSWebData(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton(TimeProvider.System);
            services.AddSingleton<AuditAndSoftDeleteInterceptor>();
            services.AddSingleton(new GSWebModuleSettings
            {
                ClientRoleId = configuration["AppSettings:ur_role_client"] ?? "1",
                StudentRoleId = configuration["AppSettings:ur_role_client_child"] ?? "3",
            });

            services.AddDbContext<GSWebDbContext>((serviceProvider, options) =>
            {
                // Read on every context creation, so Key Vault reloads apply like they do for the DAOs.
                var connectionString = serviceProvider.GetRequiredService<IConfiguration>()["ConnectionStrings:GSWebDBConnection"]
                    ?? throw new InvalidOperationException("ConnectionStrings:GSWebDBConnection is not configured.");
                GSWebDbContextOptions.Configure(options, connectionString);
                options.AddInterceptors(serviceProvider.GetRequiredService<AuditAndSoftDeleteInterceptor>());
            });

            services.AddHttpClient<IUserDirectoryClient, UserDirectoryClient>();
            services.AddMemoryCache();
            // Timeouts are per call inside the client (15 s, 60 s for the slow standards query).
            services.AddHttpClient<IAthenasCatalogClient, AthenasCatalogClient>();
            services.AddSingleton<IBlobFileStorage, AzureBlobFileStorage>();
            // Built-in English and Spanish lists. Optional extra words: AppSettings:community_blocked_words, comma separated.
            services.AddSingleton<IProfanityFilter>(_ => new ProfanityFilter(
                (configuration["AppSettings:community_blocked_words"] ?? string.Empty)
                    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)));

            services.AddScoped<IStandardFrameworkValidator, StandardFrameworkValidator>();
            services.AddScoped<ICustomPlanService, CustomPlanService>();
            services.AddScoped<ICustomPlanActivityService, CustomPlanActivityService>();
            services.AddScoped<ICustomPlanQuizEvaluator, CustomPlanQuizEvaluator>();
            services.AddScoped<ICustomPlanOverviewService, CustomPlanOverviewService>();
            services.AddScoped<ICustomPlanEvidenceService, CustomPlanEvidenceService>();
            // AI study-plan assistant: the agent of GenerateController (AIProjectClient registered in GSBaseAPI), in-memory
            // conversations (single instance), section CustomPlanAssistant of the settings.
            services.AddSingleton(CustomPlanAssistantSettings.FromConfiguration(configuration));
            services.AddSingleton<IAssistantConversationStore, AssistantConversationStore>();
            services.AddSingleton<IAssistantLanguageModel, FoundryAssistantLanguageModel>();
            services.AddHttpClient<IAthenasLessonSearchClient, AthenasLessonSearchClient>();
            services.AddHttpClient<IClientSubscriptionsClient, ClientSubscriptionsClient>();
            services.AddScoped<ICustomPlanAssistantService, CustomPlanAssistantService>();
            services.AddScoped<ICommunityGroupService, CommunityGroupService>();
            services.AddScoped<ICommunityEventService, CommunityEventService>();
            services.AddScoped<ICommunityResourceService, CommunityResourceService>();
            services.AddScoped<ICommunityForumService, CommunityForumService>();

            return services;
        }
    }
}
