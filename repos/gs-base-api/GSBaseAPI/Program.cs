using Azure;
using Azure.AI.OpenAI;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Storage.Blobs;
using GSBaseAPI.GPTModule.Services;
using GSBaseAPI.GPTModule.Services.ExternalAPIs;
using GSBaseAPI.GPTModule.Services.Helpers;
using GSWebAPI.AccessLogs.Infrastructure;
using GSWebAPI.AccessLogs.Infrastructure.Interfaces;
using GSWebAPI.AccessLogs.Services;
using GSWebAPI.CustomClasses.CacheHandler;
using Microsoft.Extensions.FileProviders;
using MessagesAPI.CustomClasses.CacheHandler;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using ReadingAPI.ReadingAPI.Services.Metrics;
using ReadingAPI.ReadingAPI.Services.Metrics.ProgressAndDomain;
using ReadingAPI.ReadingAPI.Services.Metrics.ReadingAndContent;
using ReadingAPI.ReadingAPI.Services.Metrics.UsageAndEngagement;
using ReadingAPI.ReadingAPI.Services.Story;
using SubscriptionsAPI.CustomClasses.CacheHandler;
using System.Globalization;
using TeachersAPI.CustomClasses.CacheHandler;
using UsersAPI.CustomClasses.CacheHandler;

var builder = WebApplication.CreateBuilder(args);

// Add AI services error messages configuration file
builder.Configuration.AddJsonFile("appsettings.aiservices.json", optional: true, reloadOnChange: true);

// Add data analysis (usage insights) localized messages configuration file. It is read from the application folder (where the build copies it):
// "dotnet run" uses the project folder as content root, so a plain relative path would not find it and the messages would show as keys.
builder.Configuration.AddJsonFile(new PhysicalFileProvider(AppContext.BaseDirectory), "appsettings.dataanalysis.json", optional: true, reloadOnChange: true);

// Use invariant culture so date/number formatting is consistent across machines.
// This is needed when running locally on computers with different regional settings (e.g. dd/MM vs MM/dd).
// IMPORTANT: keep this commented out for production environments; enable it only for local runs.
//CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
//CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

// ===== Azure Key Vault Configuration =====
var env = builder.Environment.EnvironmentName;

var keyVaultUrl = builder.Configuration["KeyVault:VaultUrl"];
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    try
    {
        var credential = new DefaultAzureCredential();

        var reloadInterval = builder.Configuration.GetValue<TimeSpan?>("KeyVault:ReloadInterval")
            ?? TimeSpan.FromMinutes(10);

        builder.Configuration.AddAzureKeyVault(
            new Uri(keyVaultUrl),
            credential,
            new AzureKeyVaultConfigurationOptions
            {
                ReloadInterval = reloadInterval
            });

        Console.WriteLine($"✓ Azure Key Vault configured: {keyVaultUrl} (reload every {reloadInterval})");

        // Sanity check: confirm a known secret actually loaded, so a naming
        // mismatch or partial failure surfaces here instead of in a DB call later.
        var testKey = builder.Configuration["ConnectionStrings:GSWebDBConnection"];
        if (string.IsNullOrEmpty(testKey))
        {
            Console.WriteLine("⚠ WARNING: ConnectionStrings:GSWebDBConnection is empty after Key Vault load. " +
                               "Check that the secret 'ConnectionStrings--GSWebDBConnection' exists in the vault.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠ Azure Key Vault access failed: {ex.Message}");

        if (!env.Equals("Development", StringComparison.OrdinalIgnoreCase))
        {
            throw;
        }
    }
}
else
{
    Console.WriteLine("⚠ KeyVault:VaultUrl not configured. Secrets will be loaded from appsettings only.");
}
// ===== End Azure Key Vault Configuration =====
// ===== End Azure Key Vault Configuration =====

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // maintain PascalCase
    }).AddNewtonsoftJson(opts =>
    {
        // keep whatever settings you like here
        opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        // If you need camelCase output:
        // opts.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        // preserve C# property names (PascalCase)
        opts.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new DefaultNamingStrategy()
        };

        opts.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
        
    });

builder.Services.Configure<BackgroundQueueOptions>(
    builder.Configuration.GetSection("BackgroundQueue"));

builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<QueuedHostedService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSAIServicesAPI.Controllers.GenerateController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ReadingAPI.Controllers.ReadingDiagnosticsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ReadingAPI.Controllers.StoryController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AddressAPI.Controllers.CountriesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AddressAPI.Controllers.StatesController).Assembly));

builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.ConnectionsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.GenericsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.ResultsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.TestController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.UsersController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.PowerBIController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.UsageInsightsController).Assembly));

builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.AddressesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.AvatarController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.CompetitionController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.LanguageController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.LessonController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.MiniGameController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.PlatformSettingController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.PrePostController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.QuizController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.ReportsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.RewardsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.VersionGameController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GSWebAPI.Controllers.AccessLogs.AccessLogsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(MessagesAPI.Controllers.FilesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(MessagesAPI.Controllers.MessageController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SchoolsAPI.Controllers.AcademicDetailsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SchoolsAPI.Controllers.RegionController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SchoolsAPI.Controllers.SchoolsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SchoolsAPI.Controllers.SubscriptionSchoolController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SchoolsAPI.Controllers.TermsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.AcademicsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.BillingIntervalController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.CancellationReasonsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.CouponsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.CurrencyController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.CustomerController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.ItemProductsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.PlansController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.ProductsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.StripeController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.SubscriptionsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubscriptionsAPI.Controllers.TaxesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.AccommodationController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.AssignmentsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.CacheController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.CodesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.CustomLessonController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.EvaluationController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.FilesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.ForumController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.GradeController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.HomeworkController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.InitiativeProjectInnovationController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.LearningGapController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.LessonsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.MaterialController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.OtherSubjectIntegrationController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.PlanningController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.PlanningControllerV2).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.PraxisReflectionController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.PrePostController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.PreviewPdfController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.QuestionTypeController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.QuizTypeController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.ReportController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.StrategyController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.SubjectController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.TeacherGroupController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(TeachersAPI.Controllers.TimeZoneController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.AccountController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.ActivityController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.AddressesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.LoginController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.PermissionController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.ProfilesConnectionController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.ProfilesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.RegisterController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.RolesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.StatusAccountsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.UsersSchoolController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.UsersController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UsersAPI.Controllers.UsersMetricsController).Assembly));



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Use a unique, stable ID for each .NET type
    c.CustomSchemaIds(t =>
    {
        // FullName handles namespaces; replace '+' for nested types
        var id = (t.FullName ?? t.Name).Replace("+", ".");
        // If you *still* could collide across assemblies with identical namespaces,
        // uncomment the next line to include the assembly too:
        // id = $"{id}_{t.Assembly.GetName().Name}";
        return id;
    });
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GS API", Version = "v1" });
});

builder.Services.AddSingleton<IQuizJobQueue, QuizJobQueue>();
builder.Services.AddHostedService<QuizJobWorker>();

/** To add memory cache for version caching **/
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ITutorConversationStore, MemoryTutorConversationStore>();
builder.Services.AddScoped<VersionCache>();
builder.Services.AddScoped<TeachersAPI.CustomClasses.CacheHandler.AthenasAPICache>();
builder.Services.AddScoped<GSWebAPI.CustomClasses.CacheHandler.UsersAPICache>();
builder.Services.AddScoped<SubscriptionsAPI.CustomClasses.CacheHandler.UsersAPICache>();
builder.Services.AddScoped<TeachersAPI.CustomClasses.CacheHandler.UsersAPICache>();
builder.Services.AddScoped<SubscriptionAPICache>();
builder.Services.AddScoped<CoursesCache>();
builder.Services.AddScoped<AddressAPICache>();
builder.Services.AddScoped<TeachersAPI.CustomClasses.CacheHandler.FilesCache>();
builder.Services.AddScoped<MessagesAPI.CustomClasses.CacheHandler.FilesCache>();
builder.Services.AddScoped<GSWebAPI.CustomClasses.CacheHandler.AthenasAPICache>();
builder.Services.AddScoped<QuestionTypesCache>();
builder.Services.AddScoped<QuizTypeCache>();
builder.Services.AddScoped<SubscriptionAPICache>();
builder.Services.AddScoped<AuthenticationCache>();
builder.Services.AddScoped<CacheHandlerLibrary.ExternalAPIsCache>();
// OpenAI client
builder.Services.AddHttpClient<IOpenAiClient, OpenAiClient>();

// Teachers Homework API client
builder.Services.AddHttpClient<ITeachersHomeworkApi, TeachersHomeworkApi>();

// Depr tutor service
builder.Services.AddScoped<DeprTutorService>();

// Reading AI services
builder.Services.AddScoped<SessionTrackerService>();
builder.Services.AddScoped<SessionMetricsService>();
builder.Services.AddScoped<ModuleUsageService>();
builder.Services.AddScoped<PerformanceMetricsService>();
builder.Services.AddScoped<ContentMetricsService>();
builder.Services.AddScoped<StudentMetricsService>();
builder.Services.AddScoped<StoryModifierService>();
builder.Services.AddScoped<StoryReaderService>();
builder.Services.AddScoped<UserStoryResultsService>();
builder.Services.AddScoped<ReadingAPI.ReadingAPI.Infrastructure.ReadingLessonsInfrastructure>();

var readingAPIConnString = builder.Configuration.GetConnectionString("ReadingAIAPIDBConnection");
builder.Services.AddDbContext<ReadingAPI.ReadingAPI.Infrastructure.ReadingAIDbContext>(options =>
    options.UseMySql(readingAPIConnString, ServerVersion.AutoDetect(readingAPIConnString)));

// Azure Speech options + service
builder.Services.Configure<AzureSpeechOptions>(builder.Configuration.GetSection("AzureSpeech"));
builder.Services.AddSingleton<SpeechService>();

var aiServicesConnString = builder.Configuration.GetConnectionString("AIServicesAPIDBConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(aiServicesConnString, ServerVersion.AutoDetect(aiServicesConnString)));

// GS Web EF Core data layer (Custom Plans and Community modules) on ConnectionStrings:GSWebDBConnection.
GSWebAPI.GSWebAPIServiceCollectionExtensions.AddGSWebData(builder.Services, builder.Configuration);

builder.Services.AddHttpClient<GSAIServicesAPI.FeignAPIs.AthenasFeignAPI>();
builder.Services.AddHttpClient<GSAIServicesAPI.FeignAPIs.ImageGenerationFeignAPI>();
builder.Services.AddHttpClient<GSAIServicesAPI.FeignAPIs.IDataAnalysisFeignAPI, GSAIServicesAPI.FeignAPIs.DataAnalysisFeignAPI>();
// Image generation providers; the order they are tried in comes from ImageGeneration:ProviderOrder.
builder.Services.AddHttpClient<GSAIServicesAPI.Services.Images.Providers.IImageGenerationProvider,
                               GSAIServicesAPI.Services.Images.Providers.FoundryImageProvider>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Images.Providers.IImageGenerationProvider,
                           GSAIServicesAPI.Services.Images.Providers.StableDiffusionImageProvider>();

builder.Services.AddScoped<GSAIServicesAPI.Services.CodesService>();
builder.Services.AddScoped<GSAIServicesAPI.Services.LessonsService>();
builder.Services.AddHttpClient<GSAIServicesAPI.Services.Plannings.PlanningsSearchClient>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Plannings.PlanningsService>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Plannings.AthenasLessonsService>();
builder.Services.AddSingleton<GSAIServicesAPI.Services.Foundry.RagProjectClient>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Quotas.QuotaConfigService>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Quotas.QuotaService>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Tutor.GenerativeTutorEngine>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Tutor.RagTutorEngine>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Tutor.AthenasLessonClient>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Tutor.SubjectLanguageProvider>();

builder.Services.Configure<GSAIServicesAPI.Infrastructure.Configuration.MongoDbSettings>(
    builder.Configuration.GetSection(GSAIServicesAPI.Infrastructure.Configuration.MongoDbSettings.SectionName));
builder.Services.AddSingleton<GSAIServicesAPI.Infrastructure.Context.MongoDbContext>();
builder.Services.AddScoped(typeof(GSAIServicesAPI.Infrastructure.Interfaces.IMongoRepository<>),
                           typeof(GSAIServicesAPI.Infrastructure.Repositories.MongoRepository<>));
builder.Services.AddScoped<GSAIServicesAPI.Services.PromptMonitoring.IPromptMonitoringService,
                           GSAIServicesAPI.Services.PromptMonitoring.PromptMonitoringService>();
builder.Services.AddScoped<GSAIServicesAPI.Services.PromptMonitoring.IPromptClassifier,
                           GSAIServicesAPI.Services.PromptMonitoring.PromptClassifier>();
builder.Services.AddSingleton<GSAIServicesAPI.Services.IRoleNameResolver,
                              GSAIServicesAPI.Services.RoleNameResolver>();
builder.Services.AddSingleton<GSAIServicesAPI.Services.PromptMonitoring.IViewerStudentsProvider,
                              GSAIServicesAPI.Services.PromptMonitoring.UsersApiViewerStudentsProvider>();
builder.Services.AddScoped<GSAIServicesAPI.Services.Images.IImageGenerationService,
                           GSAIServicesAPI.Services.Images.ImageGenerationService>();
builder.Services.AddSingleton<GSAIServicesAPI.Services.Images.IBlobUrlSigner,
                              GSAIServicesAPI.Services.Images.BlobUrlSigner>();
builder.Services.AddSingleton<GSAIServicesAPI.Services.Documents.IDocumentContentRewriter,
                              GSAIServicesAPI.Services.Documents.DocumentContentRewriter>();
builder.Services.AddSingleton<GSAIServicesAPI.Services.Images.IImageBlobStorage,
                              GSAIServicesAPI.Services.Images.AzureImageBlobStorage>();

builder.Services.AddSingleton<GSAIServicesAPI.Services.Tutor.TutorSettingsService>();

// Data assistant (usage insights): the model client, the tools and the in-memory state of the conversations and downloads.
// The state sits behind interfaces so it can move to a store shared by several instances of the monolith.
builder.Services.AddSingleton<GSAIServicesAPI.Services.DataAssistant.DataAssistantToolCatalog>();
builder.Services.AddSingleton<GSAIServicesAPI.Services.DataAssistant.IDataAssistantConversationStore, GSAIServicesAPI.Services.DataAssistant.DataAssistantConversationStore>();
builder.Services.AddSingleton<GSAIServicesAPI.Services.DataAssistant.IDataAssistantDownloadRegistry, GSAIServicesAPI.Services.DataAssistant.DataAssistantDownloadRegistry>();
builder.Services.AddScoped<GSAIServicesAPI.Services.DataAssistant.IDataAssistantChatClient, GSAIServicesAPI.Services.DataAssistant.AzureOpenAiDataAssistantChatClient>();
builder.Services.AddScoped<GSAIServicesAPI.Services.DataAssistant.DataAssistantService>();

var azureOpenAiEndpoint = builder.Configuration["AzureOpenAI:Endpoint"];
var azureOpenAiApiKey = builder.Configuration["AzureOpenAI:ApiKey"];

/*
if (string.IsNullOrWhiteSpace(azureOpenAiEndpoint) || string.IsNullOrWhiteSpace(azureOpenAiApiKey))
{
    throw new InvalidOperationException(
        "AzureOpenAI:Endpoint and AzureOpenAI:ApiKey must be configured for the AI services module.");
}
*/

builder.Services.AddSingleton(new AzureOpenAIClient(
    new Uri(azureOpenAiEndpoint),
    new AzureKeyCredential(azureOpenAiApiKey)));

builder.Services.AddOptions<GSAIServicesAPI.Models.Images.BlobStorageOptions>()
    .Bind(builder.Configuration.GetSection("BlobStorage"))
    .Validate(o => !string.IsNullOrWhiteSpace(o.ConnectionString),
        "BlobStorage:ConnectionString is required.")
    .Validate(o => !string.IsNullOrWhiteSpace(o.ContainerName),
        "BlobStorage:ContainerName is required.")
    .ValidateOnStart();

builder.Services.AddSingleton(sp =>
{
    var opts = sp.GetRequiredService<IOptions<GSAIServicesAPI.Models.Images.BlobStorageOptions>>().Value;
    return new BlobServiceClient(opts.ConnectionString);
});

var foundryProjectEndpoint = builder.Configuration["Foundry:ProjectEndpoint"]
    ?? throw new InvalidOperationException("Foundry:ProjectEndpoint must be configured.");

builder.Services.AddSingleton(new Azure.AI.Projects.AIProjectClient(
    new Uri(foundryProjectEndpoint),
    new DefaultAzureCredential()));

var app = builder.Build();

// Use this when encountering date format issues caused by the server's regional settings
//var defaultCulture = new CultureInfo("en-US");
//CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
//CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

MessagesAPI.AppConfiguration.Initialize(builder.Configuration);
AddressAPI.AppConfiguration.Initialize(builder.Configuration);
DataAnalysisAPI.AppConfiguration.Initialize(builder.Configuration);
GSWebAPI.AppConfiguration.Initialize(builder.Configuration);
SchoolsAPI.AppConfiguration.Initialize(builder.Configuration);
SubscriptionsAPI.AppConfiguration.Initialize(builder.Configuration);
TeachersAPI.AppConfiguration.Initialize(builder.Configuration);
UsersAPI.AppConfiguration.Initialize(builder.Configuration);
GSBaseAPI.AppConfiguration.Initialize(builder.Configuration);
CacheHandlerLibrary.AppConfiguration.Initialize(builder.Configuration);
ReadingAPI.AppConfiguration.Initialize(builder.Configuration);
GSAIServicesAPI.AppConfiguration.Initialize(builder.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "GS API v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
