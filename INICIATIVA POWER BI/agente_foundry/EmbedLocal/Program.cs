// Sustituto LOCAL y mínimo de GET /dataanalysisapi/embed-powerbi-info (rama uat de gs-base-api: PowerBIController + PowerBI.cs).
// SOLO para desarrollo en esta máquina, mientras UAT no tenga las credenciales de Power BI (solo existen en PROD).
//  · Mismo flujo que el backend: token de aplicación (client_credentials) -> datasetId del informe -> GenerateToken con identidad RLS.
//  · Las credenciales vienen de variables de entorno del proceso (las pide correr_embed_local.sh); no se guardan en archivos.
//  · Escucha solo en 127.0.0.1 y acepta solo el origen del front local. No valida la sesión de la plataforma (no la necesita: no entrega datos, solo un token de lectura del informe).
//  · Formato de respuesta idéntico al backend: { Success, Data:{ embedToken, reportId, embedUrl }, ErrorMessage }.
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var origen = Environment.GetEnvironmentVariable("EMBED_ORIGEN") ?? "http://localhost:3000";
builder.Services.AddCors(o => o.AddPolicy("front", p => p.WithOrigins(origen).WithMethods("GET").AllowAnyHeader()));
// El backend real responde en PascalCase (Success, Data, ErrorMessage, embedToken…); el front lee response.data.Data.
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.PropertyNamingPolicy = null);
var app = builder.Build();
app.UseCors("front");

string Cfg(string n) => Environment.GetEnvironmentVariable(n) ?? "";
var tenant = Cfg("POWERBI_TENANT_ID");
var cliente = Cfg("POWERBI_CLIENT_ID");
var secreto = Cfg("POWERBI_CLIENT_SECRET");
var rol = Cfg("EMBED_RLS_ROLE") is { Length: > 0 } r ? r : "ViewAll";
var usuarioRls = Cfg("EMBED_RLS_USER") is { Length: > 0 } u ? u : "AdminUser";

app.MapGet("/salud", () => new { estado = "ok", configurado = tenant != "" && cliente != "" && secreto != "", rol_rls = rol });

app.MapGet("/dataanalysisapi/embed-powerbi-info", async (string? reportId, string? workspaceId) =>
{
    if (string.IsNullOrWhiteSpace(reportId) || string.IsNullOrWhiteSpace(workspaceId))
        return Results.BadRequest("Missing reportId or workspaceId query parameter.");
    // El front llama con barra final (/embed-powerbi-info/); ASP.NET la acepta igual.
    return await Generar(reportId, workspaceId);
});
async Task<IResult> Generar(string reportId, string workspaceId)
{
    // Respuesta de error con el mismo formato que el backend, pero con código 400 (el backend de UAT responde 200 y esconde el fallo).
    IResult Falla(string m) => Results.Json(new { Success = false, Data = (object?)null, ErrorMessage = m }, statusCode: 400);
    try
    {
        if (tenant == "" || cliente == "" || secreto == "") return Falla("Power BI configuration is missing.");
        using var http = new HttpClient();
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var auth = await http.PostAsync($"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials", ["client_id"] = cliente, ["client_secret"] = secreto,
            ["scope"] = "https://analysis.windows.net/powerbi/api/.default",
        }));
        var authTxt = await auth.Content.ReadAsStringAsync();
        if (!auth.IsSuccessStatusCode) return Falla($"Power BI Authentication Error: {authTxt}");
        var accessToken = JsonDocument.Parse(authTxt).RootElement.GetProperty("access_token").GetString();

        var det = new HttpRequestMessage(HttpMethod.Get, $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports/{reportId}");
        det.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var detResp = await http.SendAsync(det);
        var detTxt = await detResp.Content.ReadAsStringAsync();
        if (!detResp.IsSuccessStatusCode) return Falla($"Failed to get Report Details. Status: {detResp.StatusCode}. Error: {detTxt}");
        if (!JsonDocument.Parse(detTxt).RootElement.TryGetProperty("datasetId", out var ds) || string.IsNullOrEmpty(ds.GetString()))
            return Falla($"The report {reportId} does not have a valid 'datasetId'.");

        var identidad = new { username = usuarioRls, roles = new[] { rol }, datasets = new[] { ds.GetString() } };
        var gen = new HttpRequestMessage(HttpMethod.Post, $"https://api.powerbi.com/v1.0/myorg/groups/{workspaceId}/reports/{reportId}/GenerateToken")
        {
            Content = new StringContent(JsonSerializer.Serialize(new { accessLevel = "View", allowSaveAs = false, identities = new[] { identidad } }), Encoding.UTF8, "application/json"),
        };
        gen.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var genResp = await http.SendAsync(gen);
        var genTxt = await genResp.Content.ReadAsStringAsync();
        if (!genResp.IsSuccessStatusCode)
            return Falla(genResp.StatusCode == System.Net.HttpStatusCode.Forbidden
                ? "Power BI Token Error: 403 Forbidden (la cuenta de servicio no tiene acceso al workspace)."
                : $"Power BI Token Error: {genTxt}");

        var token = JsonDocument.Parse(genTxt).RootElement.GetProperty("token").GetString();
        if (string.IsNullOrEmpty(token)) return Falla("Failed to retrieve embed token from Power BI response.");
        return Results.Json(new
        {
            Success = true,
            Data = new { embedToken = token, reportId, embedUrl = $"https://app.powerbi.com/reportEmbed?reportId={reportId}&groupId={workspaceId}" },
            ErrorMessage = (string?)null,
        });
    }
    catch (Exception ex) { return Falla($"An error occurred: {ex.Message}"); }
}

app.Run();
