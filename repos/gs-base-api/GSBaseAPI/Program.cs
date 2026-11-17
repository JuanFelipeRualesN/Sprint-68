using GSWebAPI.CustomClasses.CacheHandler;
using MessagesAPI.CustomClasses.CacheHandler;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using SubscriptionsAPI.CustomClasses.CacheHandler;
using TeachersAPI.CustomClasses.CacheHandler;
using UsersAPI.CustomClasses.CacheHandler;
using AthenasAPICache = TeachersAPI.CustomClasses.CacheHandler.AthenasAPICache;

var builder = WebApplication.CreateBuilder(args);

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AddressAPI.Controllers.CountriesController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(AddressAPI.Controllers.StatesController).Assembly));

builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.ConnectionsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.GenericsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.ResultsController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.TestController).Assembly));
builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(DataAnalysisAPI.Controllers.UsersController).Assembly));

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

/** To add memory cache for version caching **/
builder.Services.AddMemoryCache();
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

var app = builder.Build();

MessagesAPI.AppConfiguration.Initialize(builder.Configuration);
AddressAPI.AppConfiguration.Initialize(builder.Configuration);
DataAnalysisAPI.AppConfiguration.Initialize(builder.Configuration);
GSWebAPI.AppConfiguration.Initialize(builder.Configuration);
SchoolsAPI.AppConfiguration.Initialize(builder.Configuration);
SubscriptionsAPI.AppConfiguration.Initialize(builder.Configuration);
TeachersAPI.AppConfiguration.Initialize(builder.Configuration);
UsersAPI.AppConfiguration.Initialize(builder.Configuration);
CacheHandlerLibrary.AppConfiguration.Initialize(builder.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "GS API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
