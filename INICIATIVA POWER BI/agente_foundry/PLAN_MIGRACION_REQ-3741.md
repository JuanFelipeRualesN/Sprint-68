# Plan de migración REQ-3741 — Asistente de datos de Power BI a los repositorios oficiales

Documento del Departamento. Lo ejecutarán agentes (Sonnet, esfuerzo alto) que **no** tienen el contexto de esta sesión, así que cada tarea dice los archivos que toca, cuándo se da por terminada y cómo se prueba.

Fecha: 23-nov-2026. Origen: MVP `INICIATIVA POWER BI/agente_foundry/EyaInsightsApi` (en UAT como `app-eya-msj-uat-eastus-001`) y el panel `repos/gs-msj-private-webapp-teachers/src/components/data-analysis-module/assistant-panel/`.

---

## 0. Acceso y fuentes revisadas

| Fuente | Acceso | Qué se leyó |
|---|---|---|
| `gs-base-api` (Azure DevOps) | **Sí, lectura** (`git ls-remote` y clon por HTTPS) | `master` (e2d9d0e, 25-sep-2025), `main` = `uat` (ac9c9d0, 20-nov-2026), `dev` (3ab4bff, 16-nov-2026) |
| `gs-msj-private-webapp-teachers` | **Sí, lectura** | `main` (5504a3d, 23-nov-2026), `dev` (0a820f1, 16-nov-2026) |
| Wiki `msj-and-private.wiki` | **Sí**, clonando su repositorio git (`_git/msj-and-private.wiki`). La API REST de la wiki devolvió 401 | Páginas de *MSJ (Estudia y Avanza) Projects*, guía de build/deploy de APIs y WebApps, *PowerBI Data Analysis Dashboard Setup*, *AI-Module* |

No se creó ninguna rama ni se hizo push en Azure DevOps.

### ⚠️ Hallazgo bloqueante: `master` de gs-base-api está desactualizada

- `master` es del **25-sep-2025** y **no tiene `AIServicesAPI`**, `ReadingAIAPI`, `PDFGeneratorLibrary`, `PdfConverterProvider` ni los proyectos de pruebas. Tampoco tiene Key Vault ni `AzureOpenAIClient` en `Program.cs`.
- La rama `main` existe (contra lo que dice el encargo) y es igual a `uat`. `dev` es donde se integran los PR (el último commit es «Merged PR 518»).
- Según la wiki (*APIs Build and Deployment Automation Guide*), los pipelines se disparan con `dev`, `uat` y `main`. **`master` no se despliega.**
- Si `req-3741` sale de `master`, la regla 5 (patrón de `AIServicesAPI`) no se puede cumplir y el PR traería 14 meses de diferencias.

**Recomendación:** crear `req-3741` desde **`dev`** y abrir el PR contra `dev`, que es como llegan los PR al repo. Si Pedro prefiere, la base puede ser `main`. **Hay que confirmarlo con Pedro antes de la tarea B0** (pregunta P1). Este plan describe el código de `main`/`dev`, que en estos módulos coinciden. La copia `repos/gs-base-api` de esta rama (sincronizada el 16-nov) corresponde a `dev`.

---

## 1. Lo que hay en los repos destino

### 1.1 gs-base-api (monolito .NET 8)

**Solución y arranque**
- Solución: `GSBaseAPI/GSBaseAPI.sln` (15 proyectos). El host es `GSBaseAPI/GSBaseAPI.csproj`, que referencia como proyectos a `AddressAPI`, `AIServicesAPI`, `DataAnalysisAPI`, `GSWebAPI`, `MessagesAPI`, `ReadingAIAPI` (`ReadingAPI.csproj`), `SchoolsAPI`, `SubscriptionsAPI`, `TeachersAPI` y `UsersAPI`.
- Los módulos de negocio **no se referencian entre sí**: solo usan librerías (`CacheHandlerLibrary`, `PdfConverterProvider`). Cuando un módulo necesita a otro, lo llama por HTTP (carpeta `FeignAPIs`), aunque estén en el mismo proceso. Por ejemplo, `UserFeignAPI` llama a UsersAPI.
- `GSBaseAPI/Program.cs`:
  - carga `appsettings.aiservices.json`;
  - si existe `KeyVault:VaultUrl`, añade Key Vault con `DefaultAzureCredential` (los secretos se nombran `Seccion--Clave`, p. ej. `AzureOpenAI--ApiKey`);
  - CORS `AllowFrontend` con `AllowAnyOrigin`;
  - JSON en **PascalCase** (`PropertyNamingPolicy = null` y `DefaultNamingStrategy` en Newtonsoft);
  - registra cada controlador con `builder.Services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(<Módulo>.Controllers.<X>Controller).Assembly))`;
  - registra en DI los servicios de AIServicesAPI (`QuotaService`, `QuotaConfigService`, `IPromptMonitoringService`, `IRoleNameResolver`, …);
  - crea un **singleton `AzureOpenAIClient(new Uri(AzureOpenAI:Endpoint), new AzureKeyCredential(AzureOpenAI:ApiKey))`** y falla al arrancar si falta alguno de los dos;
  - llama a `<Módulo>.AppConfiguration.Initialize(builder.Configuration)` para cada módulo (configuración estática compartida).
- Configuración por entorno: `appsettings.{msj-dev|msj-uat|msj|private-dev|private-uat|private}.json`. El pipeline fija `ASPNETCORE_ENVIRONMENT` con `projectEnvironment` (wiki).
- Pruebas que ya existen: `GSWebAPI.Tests` y `ReadingAPI.Tests`, con **xUnit 2.9 + Microsoft.NET.Test.Sdk + Testcontainers.MySql 4.1**. `GSWebAPI.Tests/Infrastructure/MySqlDatabaseFixture.cs` y `TestDoubles.cs` sirven de modelo. Ambos proyectos están en la `.sln`.

**AIServicesAPI** (namespace `GSAIServicesAPI`, `Nullable` activado)
- `Controllers/BaseController.cs`: `GetAuthenticatedUserAsync(module)`, **`GetAuthenticatedSessionAsync(permissionName)`**, que devuelve `ValidationResponseModel?` o `null`, y `GetLocalizedMessage(key)`, que lee `Messages:{key}:{es|en}` de `appsettings.aiservices.json`.
- `FeignAPIs/UserFeignAPI.cs`: `GetUserIdRequestOnOtherAPIAsync(token, ip, permissionName, lang)` hace POST a `AppSettings:gus_endpoint_web_service_url` + `AppSettings:gus_endpoint_permission_session_api` con la cabecera `Token`, usa la caché `UsersAPICache`, y con `AppSettings:development_mode_session_off = "1"` devuelve un usuario simulado.
- `FeignAPIs/Models/ValidationResponseModel.cs` trae `SessionValid, UserId, FullName, IsStaff, IsDirector, Token, RoleId, IpAddress, LangCode, Messages`. **No trae `OreId`**, aunque UsersAPI sí lo devuelve (`UsersAPI/Controllers/ProfilesController.cs` lo usa).
- Rutas con el prefijo `aiservicesapi/…` (p. ej. `[Route("aiservicesapi/[controller]")]`, `aiservicesapi/tutor`). Cada controlador tiene un `private const string Module = "AI…"` (`AIGenerate`, `AIDocuments`, `AIAthenas`…).
- `Filters/RequiresAiEnabledAttribute.cs` lee `QuotaSettings.AiEnabled` (EF, `AppDbContext`) y responde 503 con `Messages:AIDisabled`.
- Cuotas (`Services/Quotas/QuotaService.cs`): `ReserveAsync(userId, roleId, token, QuotaResourceType.TextTokens, estimatedTokens, Module)`, luego `CommitAsync(reservation, actualTokens)` o `ReleaseAsync(reservation)`. Si se niega, responde 429 con `QuotaExceeded`, `remaining` y `resetsAt`. `TokenEstimator` estima tokens.
- Monitoreo de prompts (`Services/PromptMonitoring/IPromptMonitoringService`): `CaptureAsync(PromptMonitoringCaptureRequestModel{UserId, Role, Prompt, Response, Subject, ClassificationJson, IpAddress})`. Nunca lanza excepciones y solo clasifica a estudiantes.
- Uso directo de `AzureOpenAIClient`: `Services/Tutor/GenerativeTutorEngine.cs` (`GetChatClient(AppSettings:azure_openai_deployment)`) y `PromptClassifier.cs`. `GenerateController` usa Foundry (`RagProjectClient`) y lee el deployment con un fallback (`AppSettings:AiPlanningGenerationDeploymentName`).
- Paquetes: `Azure.AI.OpenAI 2.1.0`, `OpenAI [2.9.1]` (versión fijada), `Azure.Identity`, EF Core 9 + Pomelo MySQL, MongoDB.Driver, SharpToken. **No tiene Dapper ni NPOI.**

**DataAnalysisAPI** (namespace `DataAnalysisAPI`)
- Capas: `Controllers/`, `DataAnalysisAPI.Domain/Models/…`, `DataAnalysisAPI.Infrastructure/Dapper/*.cs` (clases estáticas con SQL y Dapper, p. ej. `Results.GetResultGenerics`), `DataAnalysisAPI.Services/*.cs`, `DataAnalysisAPI.Services/Utils/ExcelGenerator.cs` (**NPOI**) y `Utils/`.
- Conexión de solo lectura: `Utils/Configurations.GetMySqlConnection()` usa `ConnectionStrings:AnalysisConnectionStringReader` (base `gs_data_analysis`).
- Sesión: `DataAnalysisAPI.Services/FeignAPIs/GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, permissionName)`, que es síncrona y usa `AppSettings:gus_web_service_url` + `gus_endpoint_permission_session_api`. Su `ValidationResponseModel` **sí trae `OreId`**.
- Rutas: `[Route("dataanalysisapi")]` + kebab-case (`embed-powerbi-info`, `results-list`, …). `PowerBIController` usa el permiso `embed-powerbi-info` y aplica el `OreId` de la sesión (RLS `Admin_ORE_Filter`, según la wiki de Power BI).
- Paquetes: Dapper 2.1.66, MySqlConnector 2.4.0, NPOI 2.7.4.

**Seguridad (problema que ya existe):** `GSBaseAPI/appsettings.json` y otros `appsettings.*.json` del repo tienen **credenciales en texto plano** (cadenas MySQL con contraseña, la clave de Azure OpenAI y la de Blob Storage). No se copian aquí. Este ticket **no debe añadir ninguna** (ver R4).

### 1.2 gs-msj-private-webapp-teachers (React 16, CRA expulsado)

- Dashboard de reportes: `src/components/data-analysis-module/dashboard/index.js` (usa `isRoleAllowed` de `src/utils/utils.js` y `sessionDetails.Roles`) y `powerbi-embed/index.js` (llama a `getPowerBIReports` de `src/api/data-warehouse.js`). En `main` **no existe `assistant-panel/`**: solo está en la copia `repos/` de esta rama, que es idéntica a `main` en todo lo demás de `data-analysis-module/`.
- URL de API: `src/custom-classes/settings.js` lee `process.env.REACT_APP_*` y expone `static API_DATAWAREHOUSE_URL` (`REACT_APP_DATAWAREHOUSE_API`, que usa `/dataanalysisapi/...`) y `static API_AI_URL` (`REACT_APP_AI_API`, que usa `/aiservicesapi/...`), además de métodos `static getXxxAPI()` que arman cada URL.
- Llamadas: `src/api/*.js` usa `axios` de `src/api/interceptors.js`, que añade las cabeceras `Token` (`SessionManager.getSession()`) y `Accept-Language`. Así lo hacen todas las llamadas de la plataforma.
- i18n: `i18next` 23 + `react-i18next` 14. Los namespaces están en `src/translations/i18next/{en,es}/*.json` (p. ej. `ai-assistant.json`), se registran en `src/i18n.js` y hay variantes por versión (`translations/{msj,private,…}`, `load-versioned-translations.js`).
- Recharts `^2.15.4` ya es dependencia. Jest 23 (`yarn test` → `scripts/test.js`). **`main` no tiene ningún archivo `*.test.js`**: las 3 suites del panel serían las primeras.
- Variables `.env` por entorno y pipeline (wiki *WebApps … Step 2: Create and configure .env files*).

---

## 2. Dónde va cada pieza y por qué

### Decisión: el asistente va en AIServicesAPI y las consultas en DataAnalysisAPI, unidos por un Feign

| Pieza del MVP | Destino | Motivo |
|---|---|---|
| Consultas MySQL (`ConsultasMySql.cs`), reglas de cada consulta (`Rutas.cs`, `P` de `Program.cs`), modelos de filas (`Modelos.cs`) | **DataAnalysisAPI** (`Infrastructure/Dapper`, `Services`, `Domain/Models/UsageInsights`) | Este módulo ya es dueño de `gs_data_analysis`, tiene Dapper y la conexión de solo lectura `AnalysisConnectionStringReader`, y su sesión trae `OreId`. Las consultas quedan junto a las que ya alimentan Power BI. |
| Alcance por ORE/escuela (`Alcance`, parte de `Auth.cs`) | **DataAnalysisAPI** (`DataScopeResolver`) | El alcance se calcula donde se ejecuta el SQL, con la sesión validada por `GetUserDetails`. Se aplica una sola vez, en el único lugar que toca datos. |
| Exportar a Excel (`Excel.cs`, `Columnas.cs`) y gráficas Recharts (`Graficas.cs`) | **DataAnalysisAPI** (`Services/UsageInsights*`, Excel con **NPOI** sobre `ExcelGenerator`) | Ya exporta Excel con NPOI. Seguir la convención (regla 4) evita sumar ClosedXML. |
| Asistente (`Asistente.cs`: bucle de herramientas, conversaciones, verificación de cifras, registro de descargas y gráficas), `instrucciones.txt`, `openapi.json` | **AIServicesAPI** (`Controllers/DataAssistantController.cs`, `Services/DataAssistant/*`, `Resources/DataAssistant/*`) | Lo indicó Andrés (regla 5). Usa el `AzureOpenAIClient` singleton, `BaseController.GetAuthenticatedSessionAsync`, las cuotas, `RequiresAiEnabled`, el monitoreo y los mensajes localizados. |
| Llamada «herramienta → consulta» (en el MVP, loopback HTTP reenviando el Token) | **AIServicesAPI** `FeignAPIs/DataAnalysisFeignAPI.cs` → `dataanalysisapi/usage-insights/...` | Así se comunican los módulos en el repo (ver `UserFeignAPI`). Al reenviar el Token, DataAnalysisAPI vuelve a validar y aplicar el alcance, y el modelo no puede salirse del de la persona (lo mismo que garantiza hoy el MVP). Sin referencias entre proyectos de negocio. |
| `Auth.cs` (modos local/servicio, `x-api-key`, CORS, vigilancia de loopback), `ConsultasEjemplo.cs`, `Program.cs` del MVP, `CredencialDePrueba`, Foundry/identidad administrada | **Se eliminan** | El monolito ya resuelve sesión, CORS y hosting. `development_mode_session_off` reemplaza el modo local. Solo queda la credencial por API key (patrón de AIServicesAPI). |

Alternativa descartada: que AIServicesAPI referencie el proyecto DataAnalysisAPI y llame al servicio en memoria. Es más rápido (sin un salto HTTP ni una segunda validación de sesión, que está en caché), pero rompe la convención de módulos independientes. Si Pedro la prefiere, solo cambian B8 y B10 (pregunta P5).

### Sesión y conexión: qué se reutiliza

- **Sesión, en los dos módulos:** se usa la de cada uno, sin auth propia. En AIServicesAPI, `GetAuthenticatedSessionAsync(permissionName)`. En DataAnalysisAPI, `GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, permissionName)`. Hay que añadir `public string? OreId { get; set; }` a `GSAIServicesAPI.FeignAPIs.Models.ValidationResponseModel`; es un cambio aditivo porque la deserialización no distingue mayúsculas.
- **Permiso enviado a UsersAPI:** el MVP usa `embed-powerbi-info`, que ya tienen los administradores del tablero. Se propone `private const string Module = "embed-powerbi-info"` en los dos controladores nuevos, salvo que Pedro quiera un permiso nuevo (`AIDataAssistant`), que exigiría un script en la base de UsersAPI (pregunta P3).
- **Conexión:** `DataAnalysisAPI.Utils.Configurations.GetMySqlConnection()`, sin una cadena nueva. La cuenta de esa cadena debe tener solo `SELECT` (pregunta P6: el nombre de usuario de la cadena de UAT no parece de solo lectura).
- **Modelo:** el `AzureOpenAIClient` singleton (`AzureOpenAI:Endpoint` y `AzureOpenAI:ApiKey` en Key Vault). El deployment se lee de `AppSettings:ai_data_assistant_deployment_name` y, si falta, de `AppSettings:azure_openai_deployment` (el mismo fallback que tiene hoy el MVP).
- **API de modelo:** **Chat Completions con tool calling** (`ChatClient` de `AzureOpenAIClient.GetChatClient(deployment)`, igual que `GenerativeTutorEngine`). El MVP usa `/openai/v1/responses` con `previous_response_id`, pero con `Azure.AI.OpenAI 2.1.0` y `OpenAI [2.9.1]` fijados no hay un cliente Responses de Azure soportado. Por eso el historial de la conversación se guarda en el servidor (ver B10). Antes de escribir B10 se hace una prueba corta: si con esas versiones hay un cliente Responses de Azure que funcione, se puede usar, documentándolo en el PR.

### Cuotas, `RequiresAiEnabled` y monitoreo de prompts

| Mecanismo | ¿Aplica? | Justificación |
|---|---|---|
| `[RequiresAiEnabled]` | **Sí**, en `ask` (no en `downloads`/`charts`, que no llaman al modelo) | Es el interruptor global de IA de la plataforma: si se apaga la IA, también se apaga el asistente. |
| Cuotas (`QuotaService`, `TextTokens`) | **Sí**, en `ask` | Controla el costo con la misma configuración por rol. Una pregunta puede hacer hasta 6 llamadas al modelo: se reserva una estimación (instrucciones + herramientas + historial + `TokenEstimator.AssumedOutputTokens` × pasos), se confirma la suma de `Usage.TotalTokenCount` de todas las llamadas y se libera si falla. Si el rol admin no tiene límite, `Enforced=false` y solo queda el registro de uso. |
| Monitoreo (`IPromptMonitoringService.CaptureAsync`) | **Sí**, con `Subject = "data-assistant"`, rol de la sesión y sin clasificación (solo se clasifican estudiantes) | Deja auditoría de quién preguntó qué sobre datos institucionales. El modelo de captura no tiene un campo «módulo», así que `Subject` identifica el origen. Hay que confirmar que estos registros pueden salir en la vista de monitoreo de admins (pregunta P4). |

---

## 3. Tabla de renombres al inglés

Convenciones: clases, interfaces, records y métodos en PascalCase; variables y parámetros en camelCase; rutas en kebab-case con el prefijo del módulo; parámetros de query en camelCase (como `reportId`, `subjectCode` en el front); JSON en PascalCase, que es lo que sale por defecto del monolito; claves de `AppSettings` en snake_case (`gus_endpoint_…`, `azure_openai_deployment`). Los nombres de herramientas del modelo van en snake_case inglés. Ningún identificador, comentario ni texto interno queda en español. Los textos que ve el usuario pasan a i18n (front) o a `Messages:{key}:{es|en}` (backend).

### 3.1 Archivos (backend)

| Actual (MVP) | Nuevo |
|---|---|
| `EyaInsightsApi/Modelos.cs` | `DataAnalysisAPI/DataAnalysisAPI.Domain/Models/UsageInsights/*.cs` (un archivo por record) |
| `EyaInsightsApi/ConsultasMySql.cs` | `DataAnalysisAPI/DataAnalysisAPI.Infrastructure/Dapper/UsageInsights.cs` |
| `EyaInsightsApi/Rutas.cs` + clase `P` de `Program.cs` | `DataAnalysisAPI/DataAnalysisAPI.Services/UsageInsights.cs` + `DataAnalysisAPI.Services/Utils/UsageInsightsQueryParser.cs` |
| `EyaInsightsApi/Auth.cs` (alcance) | `DataAnalysisAPI/DataAnalysisAPI.Services/Utils/DataScopeResolver.cs` (el resto se elimina) |
| `EyaInsightsApi/Excel.cs` | `DataAnalysisAPI/DataAnalysisAPI.Services/Utils/UsageInsightsExcelExporter.cs` (NPOI) |
| `EyaInsightsApi/Columnas.cs` | `DataAnalysisAPI/DataAnalysisAPI.Services/Utils/UsageInsightsColumnFormatter.cs` |
| `EyaInsightsApi/Graficas.cs` | `DataAnalysisAPI/DataAnalysisAPI.Services/Utils/ChartSpecificationBuilder.cs` |
| (rutas en `Program.cs`) | `DataAnalysisAPI/Controllers/UsageInsightsController.cs` |
| `EyaInsightsApi/Asistente.cs` | `AIServicesAPI/Services/DataAssistant/DataAssistantService.cs`, `DataAssistantToolCatalog.cs`, `DataAssistantConversationStore.cs`, `DataAssistantDownloadRegistry.cs`, `AnswerFigureVerifier.cs` |
| (rutas `/asistente/*` en `Program.cs`) | `AIServicesAPI/Controllers/DataAssistantController.cs` |
| (loopback en `Program.cs`) | `AIServicesAPI/FeignAPIs/DataAnalysisFeignAPI.cs` |
| `EyaInsightsApi/instrucciones.txt` | `AIServicesAPI/Resources/DataAssistant/system-instructions.txt` (traducido, ver R5) |
| `agente_foundry/openapi.json` | `AIServicesAPI/Resources/DataAssistant/tool-definitions.json` |
| `ConsultasEjemplo.cs`, `Program.cs`, `EyaInsightsApi.csproj`, `appsettings.json` del MVP | Se eliminan (no se migran) |
| `pruebas_mysql/esquema_y_datos.sql` | `DataAnalysisAPI.Tests/TestData/usage-insights-schema-and-data.sql` (comentarios en inglés) |

### 3.2 Clases, records y tipos

| Actual | Nuevo |
|---|---|
| `Alcance(OreId, EscuelaId)` · `Todo` · `Nada` · `EsTodo` | `DataScope(OreId, SchoolId)` · `All` · `None` · `IsAll` |
| `Usuario`, `AuthOpciones`, `ValidadorSesion`, `Auth`, `EstadoAuth` | Se eliminan (se usan `ValidationResponseModel` + `DataScopeResolver`) |
| `Respuesta` | `UsageInsightResponse` |
| `ErrorConsulta` | `InvalidQueryParameterException` |
| `IConsultasUso` / `ConsultasMySql` / `ConsultasEjemplo` | `UsageInsights` (Dapper, estática, recibe `DataScope`) / — / eliminada |
| `ExclusionCuentas` | `TestAccountExclusion` |
| `FilaArea` | `SubjectAreaUsageRow` |
| `FilaEscuela` | `SchoolConnectionsRow` |
| `FilaMaestro` | `TeacherConnectionsRow` |
| `FilaComparacion` | `SubjectAreaComparisonRow` |
| `FilaTendencia` | `ConnectionsTrendRow` |
| `FilaComparacionEscuela` | `SchoolComparisonRow` |
| `FilaCatalogoEscuela` | `SchoolCatalogRow` |
| `FilaPatron` | `ConnectionPatternRow` |
| `FilaParticipacion` | `SchoolParticipationRow` |
| `FilaAcumulada` | `CumulativeConnectionsRow` |
| `ConteoActivos` | `ActiveUsersCount` |
| `FilaUsoDocentes` | `TeacherActivityRow` |
| `FilaPromedioQuiz` | `FullQuizAverageRow` |
| `FilaSuscripcion` / `FilaSuscripcionVista` / `FilaEstadoSuscripcion` / `ConteoSuscripcion` | `SubscriptionStatusRow` / `SubscriptionStatusViewRow` / `SubscriptionStatusTotalRow` / `SubscriptionStatusCount` |
| `Rutas` | `UsageInsights` (Services) + `UsageInsightsController` |
| `P` | `UsageInsightsQueryParser` |
| `Columnas` · `Excel` · `Graficas` | `UsageInsightsColumnFormatter` · `UsageInsightsExcelExporter` · `ChartSpecificationBuilder` |
| `OpcionesGrafica` | `ChartOptions` |
| `AsistenteFoundry` | `DataAssistantService` |
| `ContextoTablero` | `DashboardContextDto` |
| `PreguntaAsistente` | `DataAssistantAskRequest` |
| `RespuestaAsistente` | `DataAssistantAskResponse` |
| `ResultadoHerramienta` | `ToolExecutionResult` |
| `EntradaDescarga` | `DownloadRegistration` |
| `ErrorFoundry` | `ModelInvocationException` |
| `CredencialDePrueba` | Se elimina |

### 3.3 Métodos públicos

| Actual | Nuevo |
|---|---|
| `UsoPorArea` | `GetSubjectAreaUsage` |
| `BuscarEscuelas` | `SearchSchools` |
| `ConexionesPorEscuela` | `GetConnectionsBySchool` |
| `ConexionesPorMaestro` | `GetConnectionsByTeacher` |
| `CompararAreas` | `CompareSubjectAreas` |
| (comparar-escuelas, en `Rutas`) | `CompareSchools` |
| `Tendencia` | `GetConnectionsTrend` |
| `ConexionesYNuevosPorDia` | `GetDailyConnectionsAndNewUsers` |
| (conexiones-acumuladas, en `Rutas`) | `GetCumulativeConnections` |
| `PromedioFullQuiz` | `GetFullQuizAverage` |
| `UsuariosActivos` | `GetActiveUsersCount` |
| `UsoDocentes` | `GetTeacherActivity` |
| `PatronConexiones` | `GetConnectionPatterns` |
| `ParticipacionEscuelas` | `GetSchoolParticipation` |
| `UsuariosPorSuscripcion` | `GetUsersBySubscriptionStatus` |
| `EscuelaExiste` · `EscuelaDeUsuario` · `Para(alcance)` | `SchoolExists` · `GetUserSchoolId` · (el `DataScope` va como parámetro) |
| Propiedades `Fuente`, `NotaArea`, `ColumnaFechaPruebas`, `ColumnaFechaConexiones`, `DatosHastaPruebas`, `DatosHastaConexiones`, `NotaConexiones`, `ExcluyeCuentasDePrueba`, `NotaPromedioQuiz`, `NotaActivos`, `NotaDocentes`, `NotaParticipacion`, `NotaSuscripcion`, `DescripcionAlcance` | `Source`, `SubjectAreaNote`, `TestsDateColumn`, `ConnectionsDateColumn`, `TestsDataUntil`, `ConnectionsDataUntil`, `ConnectionsNote`, `ExcludesTestAccounts`, `FullQuizAverageNote`, `ActiveUsersNote`, `TeacherActivityNote`, `ParticipationNote`, `SubscriptionNote`, `ScopeDescription` (las notas que ve el usuario salen de `Messages`, ver B4) |
| `Rutas.Intentar` / `Ejecutar` / `RegistrarFallo` / `Con` | `try/catch` en el controlador + `UsageInsights.LogQueryFailure` + `UsageInsights.Execute(queryName, parameters, scope)` |
| `P.Pct` · `Max` · `Unir` · `Nota` · `Periodo` · `FechaOpcional` · `MaxLimite` · `OpcionesDeGrafica` · `Escuela` | `Percentage` · `MaxDate` · `JoinNotes` · `BuildDataUntilNote` · `BuildPeriod` · `ParseOptionalDate` · `GetMaxLimit` · `ParseChartOptions` · `ResolveSchool` |
| `Columnas.Titulo` · `EsPorcentaje` · `Aplanar` · `Texto` · `Numero` · `MensajeSinResultados` · `HayMasFilas` · `NombreArchivo` · `ANodo` | `GetColumnTitle` · `IsPercentage` · `Flatten` · `ToText` · `ToNumber` · `BuildNoResultsMessage` · `HasMoreRows` · `BuildFileName` · `ToJsonNode` |
| `Excel.Libro` | `UsageInsightsExcelExporter.BuildWorkbook` |
| `Graficas.Graficable` · `Desde` | `ChartSpecificationBuilder.IsChartable` · `Build` |
| `AsistenteFoundry.PreguntarAsync` · `EjecutarAsync` · `BuscarDescarga` · `QueryDeGrafica` · `QueryDeGraficaAmpliada` · `QueryDeExcel` · `AnioAcademico` · `Verificar` | `DataAssistantService.AskAsync` · `ExecuteToolAsync` · `DataAssistantDownloadRegistry.Find` · `BuildChartQuery` · `BuildExpandedChartQuery` · `BuildExcelQuery` · `GetAcademicYear` · `AnswerFigureVerifier.Verify` |

### 3.4 Rutas

| Actual (MVP) | Nuevo |
|---|---|
| `GET /uso-por-area` | `GET dataanalysisapi/usage-insights/subject-area-usage` |
| `GET /buscar-escuela` | `GET dataanalysisapi/usage-insights/search-schools` |
| `GET /conexiones-por-escuela` | `GET dataanalysisapi/usage-insights/connections-by-school` |
| `GET /conexiones-por-maestro` | `GET dataanalysisapi/usage-insights/connections-by-teacher` |
| `GET /comparar-areas` | `GET dataanalysisapi/usage-insights/compare-subject-areas` |
| `GET /comparar-escuelas` | `GET dataanalysisapi/usage-insights/compare-schools` |
| `GET /promedio-full-quiz` | `GET dataanalysisapi/usage-insights/full-quiz-average` |
| `GET /tendencia-conexiones` | `GET dataanalysisapi/usage-insights/connections-trend` |
| `GET /conexiones-acumuladas` | `GET dataanalysisapi/usage-insights/cumulative-connections` |
| `GET /uso-docentes` | `GET dataanalysisapi/usage-insights/teacher-activity` |
| `GET /patrones-conexion` | `GET dataanalysisapi/usage-insights/connection-patterns` |
| `GET /participacion-escuelas` | `GET dataanalysisapi/usage-insights/school-participation` |
| `GET /usuarios-por-suscripcion` | `GET dataanalysisapi/usage-insights/users-by-subscription` |
| `GET /exportar/{consulta}?formato=xlsx\|grafica` | `GET dataanalysisapi/usage-insights/{query}/export?format=xlsx\|chart` |
| `GET /salud` | `GET dataanalysisapi/usage-insights/data-status` (pide sesión; devuelve `TestsDataUntil`, `ConnectionsDataUntil`) |
| `POST /asistente/preguntar` | `POST aiservicesapi/data-assistant/ask` |
| `GET /asistente/descargas/{id}` | `GET aiservicesapi/data-assistant/downloads/{id}` |
| `GET /asistente/graficas/{id}` | `GET aiservicesapi/data-assistant/charts/{id}` |

Herramientas del modelo (`operationId`): `uso_por_area` → `get_subject_area_usage`, `buscar_escuela` → `search_schools`, `conexiones_por_escuela` → `get_connections_by_school`, `conexiones_por_maestro` → `get_connections_by_teacher`, `comparar_areas` → `compare_subject_areas`, `comparar_escuelas` → `compare_schools`, `promedio_full_quiz` → `get_full_quiz_average`, `tendencia_conexiones` → `get_connections_trend`, `conexiones_acumuladas` → `get_cumulative_connections`, `uso_docentes` → `get_teacher_activity`, `patrones_conexion` → `get_connection_patterns`, `participacion_escuelas` → `get_school_participation`, `usuarios_por_suscripcion` → `get_users_by_subscription`.

### 3.5 Parámetros de query y sus valores

| Actual | Nuevo |
|---|---|
| `desde`, `hasta` | `from`, `to` |
| `desde_a`, `hasta_a`, `desde_b`, `hasta_b` | `fromA`, `toA`, `fromB`, `toB` |
| `orden` (`asc`/`desc`) | `order` (`asc`/`desc`) |
| `limite` | `limit` |
| `escuela` | `school` |
| `min_conexiones`, `max_conexiones` | `minConnections`, `maxConnections` |
| `granularidad` = `dia`/`mes`/`trimestre`/`anio` | `granularity` = `day`/`month`/`quarter`/`year` |
| `agrupar` = `escuela`/`materia`/`escuela_materia`/`dia_semana`/`rol`/`total` | `groupBy` = `school`/`subject`/`school_subject`/`weekday`/`role`/`total` |
| `texto` | `text` |
| `fecha`, `periodo_desde` | `date`, `periodFrom` |
| `formato` = `xlsx`/`grafica` | `format` = `xlsx`/`chart` |
| `todas`, `incluir_ceros` (gráfica ampliada) | `all`, `includeZeros` |

### 3.6 Campos JSON del contrato con el front

| Actual (snake_case, ES) | Nuevo (PascalCase, EN) |
|---|---|
| Petición `pregunta`, `conversacion_id`, `contexto{reporte, pagina}`, `idioma` | `Question`, `ConversationId`, `Context{Report, Page}`, `Language` |
| Respuesta `respuesta`, `conversacion_id`, `referencias[]`, `avisos[]`, `advertencias[]`, `consultas[]`, `error`, `descargas[]`, `graficas[]` | `Answer`, `ConversationId`, `References[]`, `Notices[]`, `Warnings[]`, `ExecutedQueries[]`, `ErrorCode`, `Downloads[]`, `Charts[]` |
| `consultas[i]` = `{herramienta, argumentos, estado}` | `{Tool, Arguments, Status}` |
| `descargas[i]` = `{id, nombre, consulta, url, destacar}` | `{Id, FileName, Query, Url, Highlighted}` |
| Envoltura de consulta y referencia: `consulta, fuente, columna_fecha, periodo{desde,hasta}, filtros, datos_hasta, resultados, nota, resumen, alcance, total_filas, generado_en` | `Query, Source, DateColumn, Period{From,To}, Filters, DataUntil, Results, Note, Summary, Scope, TotalRows, GeneratedAt` + **`HasMoreRows`** (nuevo, bool) |
| Gráfica: `id, url, consulta, tipo, titulo, eje_x, eje_y, unidad, series[{clave,nombre}], data, total_categorias, categorias_mostradas, categorias_en_cero, hay_mas_filas, serie_temporal, total_valor, variante, layout, nota, periodo, filtros, datos_hasta` | `Id, Url, Query, Type, Title, XAxisLabel, YAxisLabel, Unit, Series[{Key,Name}], Data, TotalCategories, ShownCategories, ZeroCategories, HasMoreRows, IsTimeSeries, TotalValue, Variant, Layout, Note, Period, Filters, DataUntil` |
| Filas de resultados (`nombre`, `conexiones`, `usuarios_conectados`, `porcentaje_del_total`, `pruebas_asignadas`…) | Las propiedades de los records de 3.2 tal cual (`Name`, `Connections`, `ConnectedUsers`, `PercentageOfTotal`, `AssignedTests`, …). La lista completa sale de los records |
| Códigos de error `pregunta_vacia`, `parametros_invalidos`, `base_no_disponible`, `consulta_desconocida`, `descarga_expirada`, `sin_grafica`, `modelo_no_disponible`, `sin_acceso_al_modelo`, `contenido_bloqueado` | `empty_question`, `invalid_parameters`, `database_unavailable`, `unknown_query`, `download_expired`, `chart_unavailable`, `model_unavailable`, `model_access_denied`, `content_blocked`. El texto del error sale de `Messages:<Key>:{es,en}` |
| Marcadores en texto `AVISO:`, `EMPATE:`, `HAY MÁS FILAS:`, que se detectan con regex | Campos estructurados: `Notices[]` (texto localizado), `HasMoreRows` (bool) y `Ties` (bool en `Summary`). **Nada se detecta por texto** |

### 3.7 Configuración

| Actual (MVP) | Nuevo / reutilizada |
|---|---|
| `EYA_MYSQL`, `EYA_MYSQL_HOST/PORT/USER/PASSWORD/SSL` | **Reutilizada:** `ConnectionStrings:AnalysisConnectionStringReader` (Key Vault `ConnectionStrings--AnalysisConnectionStringReader`) |
| `Auth:Modo`, `Auth:UsersApiUrl`, `Auth:CacheSegundos` | **Reutilizadas:** `AppSettings:gus_web_service_url` / `gus_endpoint_web_service_url`, `gus_endpoint_permission_session_api`, `cache_on_off`, `cache_token_expiration`, `development_mode_session_off` |
| `Auth:Permiso` | Constante `Module = "embed-powerbi-info"` (pregunta P3) |
| `Auth:RolesTodo` / `RolesOre` / `RolesEscuela` | **Nuevas:** `AppSettings:usage_insights_all_schools_role_ids` (`"2"`), `usage_insights_ore_role_ids` (`"8"`), `usage_insights_school_role_ids` (`"6"`) |
| `Datos:Origen` | Se elimina (siempre MySQL) |
| `Datos:ExcluirCuentasDePrueba`, `OresExcluidos`, `DominiosExcluidos`, `EscuelasExcluidas` | **Nuevas:** `AppSettings:usage_insights_exclude_test_accounts`, `usage_insights_excluded_ores`, `usage_insights_excluded_email_domains`, `usage_insights_excluded_schools` |
| `Datos:ColEstadoSuscripcion`, `ColCancelacionSuscripcion` | **Nuevas:** `AppSettings:usage_insights_subscription_status_column`, `usage_insights_subscription_cancellation_column` (o constantes, si en producción son fijas) |
| `Datos:OffsetUtcMinutos`, `CommandTimeoutSegundos` | **Nuevas:** `AppSettings:usage_insights_utc_offset_minutes`, `usage_insights_command_timeout_seconds` |
| `AzureOpenAI:Endpoint`, `AzureOpenAI:ApiKey` | **Reutilizadas** (Key Vault `AzureOpenAI--ApiKey`) |
| `Foundry:Modelo` | **Nueva:** `AppSettings:ai_data_assistant_deployment_name`, con fallback a `AppSettings:azure_openai_deployment` |
| (loopback) | **Nueva:** `AppSettings:data_analysis_api_url` (base URL del propio monolito para `DataAnalysisFeignAPI`) |
| `Asistente:Habilitado`, `Asistente:Credencial`, `Asistente:ClientId`, `Foundry:Endpoint`, `EYA_API_KEY`, `Cors:Origenes`, `Asistente:Origenes`, `KeyVault:VaultUrl` (del MVP) | Se eliminan (el monolito ya trae Key Vault y CORS) |
| Front `REACT_APP_PBI_ASSISTANT_AVAILABLE` | `REACT_APP_DATA_ASSISTANT_AVAILABLE` |
| Front `REACT_APP_PBI_ASSISTANT_URL` | Se elimina: `Settings.API_AI_URL` y `Settings.API_DATAWAREHOUSE_URL` |
| Front `REACT_APP_PBI_ASSISTANT_ROLES` | Se elimina: constante `DATA_ASSISTANT_ALLOWED_ROLES = ["admin", "admin-ore"]` (el backend también lo verifica) |
| Mensajes nuevos en `AIServicesAPI/appsettings.aiservices.json` → `Messages` | `DataAssistantEmptyQuestion`, `DataAssistantModelUnavailable`, `DataAssistantContentBlocked`, `DataAssistantDownloadExpired`, `DataAssistantChartUnavailable`, `UsageInsightsInvalidParameters`, `UsageInsightsDatabaseUnavailable`, `UsageInsightsUnknownQuery` y notas de datos (`UsageInsightsNote…`), cada uno con `es`/`en` |

### 3.8 Front: archivos, componentes y funciones

Carpeta: `assistant-panel/` → `src/components/data-analysis-module/data-assistant-panel/`.

| Actual | Nuevo |
|---|---|
| `index.js` (`AssistantPanel`) | `index.js` (`DataAssistantPanel`) |
| `assistant-api.js` (`fetch` propio) | `src/api/data-assistant.js` (axios de `interceptors.js`) |
| `config.js` (`ASSISTANT_AVAILABLE`, `ASSISTANT_URL`, `ASSISTANT_ROLES`) | `config.js` (`DATA_ASSISTANT_AVAILABLE`, `DATA_ASSISTANT_ALLOWED_ROLES`) |
| `textos.js` (`textosPara`) | `src/translations/i18next/{es,en}/data-assistant.json` + `useTranslation("data-assistant")` |
| `errores.js` (`mensajeDeError`) | `error-messages.js` (`getErrorMessage`) |
| `exportar-directo.js` (`ExportarDirecto`, `CONSULTAS`) | `direct-export.js` (`DirectExport`, `EXPORTABLE_QUERIES`) |
| `grafica.js` (`Grafica`) | `chart.js` (`Chart`) |
| `grafica-base.js` | `chart-utils.js` |
| `grafica-ampliada.js` (`GraficaAmpliada`, `anchoDeEje`, `MAX_EJE`, `FOCUSABLES`) | `expanded-chart.js` (`ExpandedChart`, `getAxisWidth`, `MAX_AXIS_WIDTH`, `FOCUSABLE_SELECTOR`) |
| `imagen.js` (`svgDe`, `svgACanvas`, `descargarImagen`, `nombreSeguro`, `FUENTE`) | `chart-image.js` (`getSvgElement`, `svgToCanvas`, `downloadChartImage`, `toSafeFileName`, `CHART_FONT`) |
| `style.css` (clases `asst-*` con sufijos en español, p. ej. `asst-exportar`) | `style.css` (clases `data-assistant-*` en inglés, p. ej. `data-assistant-export`) |
| `__tests__/fase4.test.js`, `grafica-ampliada.test.js`, `grafica-tipos.test.js` | `__tests__/data-assistant-panel.test.js`, `expanded-chart.test.js`, `chart-types.test.js` |
| `preguntar`, `leerAdjuntos`, `descargarArchivo`, `descargarExcel`, `descargarExportacion`, `pedirGrafica`, `destinoSeguro`, `origenDe`, `nombreDeContentDisposition` | `askDataAssistant`, `readAttachments`, `downloadFile`, `downloadExcel`, `downloadUsageInsightExport`, `fetchExpandedChart`, `resolveAllowedUrl`, `getOrigin`, `getFileNameFromContentDisposition` |
| `COLORES`, `TIPOS`, `ALTO_VISTA_PREVIA`, `MAX_VISTA_PREVIA`, `MAX_DONA`, `recortar`, `esBarras`, `colorDeSerie`, `colorDeCategoria`, `datosVistaPrevia`, `totalesDe`, `tipoPorDefecto`, `tiposPermitidos`, `tickNombre`, `tooltipGrafica`, `construirGrafica` | `CHART_COLORS`, `CHART_TYPES`, `PREVIEW_HEIGHT`, `PREVIEW_MAX_CATEGORIES`, `DONUT_MAX_SLICES`, `truncateText`, `isBarChart`, `getSeriesColor`, `getCategoryColor`, `getPreviewData`, `getTotals`, `getDefaultChartType`, `getAllowedChartTypes`, `formatCategoryTick`, `renderChartTooltip`, `buildChart` |
| Componentes internos `Negrita`, `TextoRespuesta`, `Referencias`, `BotonExcel`, `Mensaje`; funciones `fecha`, `textoPeriodo`, `chipsDe`, `descargaDeGrafica` | `BoldText`, `AnswerText`, `References`, `ExcelDownloadButton`, `ChatMessage`; `formatDate`, `formatPeriod`, `buildReferenceChips`, `findChartDownload` |
| Prop `onReportReady` (en `powerbi-embed`) | Se mantiene (ya está en inglés) |

---

## 4. Contrato API final

Todas las rutas exigen la cabecera `Token` (sesión de la plataforma) y aceptan `Accept-Language` (`es`/`en`). Respuestas en JSON PascalCase. Errores: `{ "error": "<mensaje localizado>", "ErrorCode": "<código>" }`, con 400, 401, 403, 404, 429, 502 o 503 según el caso.

### 4.1 DataAnalysisAPI — `dataanalysisapi/usage-insights`

`GET dataanalysisapi/usage-insights/{query}?from=YYYY-MM-DD&to=YYYY-MM-DD&order=desc&limit=10&school=...`
- 200 → `UsageInsightResponse`:
  ```json
  { "Query": "connections-by-school", "Source": "gs_data_analysis", "DateColumn": "connection.connected_at",
    "Period": { "From": "2026-01-01", "To": "2026-11-22" }, "Filters": { "Order": "desc", "Limit": 10 },
    "DataUntil": "2026-11-22", "Scope": "ORE San Juan", "Results": [ { "Name": "...", "Connections": 120, "ConnectedUsers": 40, "PercentageOfTotal": 12.5 } ],
    "Note": "...", "Summary": { "Ties": false }, "HasMoreRows": false, "TotalRows": 10, "GeneratedAt": "2026-11-23T15:00:00Z" }
  ```
- Alcance: sale de la sesión (`RoleId` → rol de todas las escuelas, de ORE o de escuela; `OreId`; escuela del usuario) y va **en el SQL**. Un rol fuera de las tres listas recibe 403. Si se pide una escuela fuera del alcance, la respuesta es la misma que para una escuela que no existe.
- `GET .../{query}/export?format=xlsx&<mismos parámetros>` → archivo `.xlsx` (hojas *Data* y *References*) con `Content-Disposition`. No acepta `search-schools`.
- `GET .../{query}/export?format=chart&all=false&order=desc&includeZeros=false&limit=12` → especificación de gráfica (campos de 3.6).
- `GET .../data-status` → `{ "TestsDataUntil": "...", "ConnectionsDataUntil": "...", "Source": "..." }`.

### 4.2 AIServicesAPI — `aiservicesapi/data-assistant`

`POST aiservicesapi/data-assistant/ask` (`[RequiresAiEnabled]`, cuota `TextTokens`)
```json
{ "Question": "¿Qué escuelas tuvieron más conexiones en 2026?", "ConversationId": null,
  "Context": { "Report": "Usage", "Page": "Connections" }, "Language": "es" }
```
→ 200
```json
{ "Answer": "...", "ConversationId": "c_...", "References": [ { "Query": "...", "Period": {...}, "DataUntil": "...", "Scope": "...", "TotalRows": 10, "GeneratedAt": "..." } ],
  "Notices": ["..."], "Warnings": [], "ExecutedQueries": [ { "Tool": "get_connections_by_school", "Arguments": {...}, "Status": 200 } ],
  "ErrorCode": null, "Downloads": [ { "Id": "d_...", "FileName": "connections-by-school.xlsx", "Query": "connections-by-school", "Url": "/aiservicesapi/data-assistant/downloads/d_...", "Highlighted": true } ],
  "Charts": [ { "Id": "d_...", "Url": "/aiservicesapi/data-assistant/charts/d_...", "Type": "bar", "...": "..." } ] }
```
- Errores: 401 (sesión), 403 (rol), 429 (cuota: `error`, `remaining`, `resetsAt`, igual que `GenerateController`), 503 (IA apagada o modelo sin acceso), 502 (modelo no disponible), 200 con `ErrorCode = "content_blocked"` (filtro de contenido de Azure, igual que el MVP).
- `ConversationId` queda ligado al `UserId`: un id de otra persona inicia una conversación nueva.

`GET aiservicesapi/data-assistant/downloads/{id}` devuelve el `.xlsx`, reconstruido con el Token de quien lo pide. Si el id es de otra persona, venció o no existe, la respuesta es siempre 404 `download_expired`.

`GET aiservicesapi/data-assistant/charts/{id}?all=&order=&includeZeros=&limit=` devuelve la gráfica ampliada (mismas reglas de dueño).

### 4.3 Cambios en el panel del front

- `settings.js`: añadir
  - `static getDataAssistantAskAPI = () => \`${this.API_AI_URL}/aiservicesapi/data-assistant/ask\``;
  - `static getDataAssistantResourceAPI = (relativeUrl) => \`${this.API_AI_URL}${relativeUrl}\`` (para `Url` de `Downloads`/`Charts`, aceptando solo rutas que empiecen por `/aiservicesapi/data-assistant/`);
  - `static getUsageInsightsExportAPI = (query, params) => \`${this.API_DATAWAREHOUSE_URL}/dataanalysisapi/usage-insights/${query}/export?...\``.
- `src/api/data-assistant.js`: `askDataAssistant` (`axios.post`), `downloadFile` (`axios.get` con `responseType: "blob"`) y `fetchExpandedChart`. El axios de `interceptors.js` pone `Token` y `Accept-Language`, como en el resto de la plataforma. Se elimina el `fetch` propio y se mantiene la lista blanca de destinos (`resolveAllowedUrl`), que ahora acepta solo los dos orígenes de `settings.js`.
- Campos: el panel lee los nombres PascalCase de 3.6. Los avisos, la bandera de «hay más filas» y los empates se leen de los campos estructurados, no del texto.
- Textos: `useTranslation("data-assistant")`. Las claves en inglés están en `es/` y `en/`, y se registran en `src/i18n.js` como los demás namespaces.
- Montaje (reconstruido de `PANEL_ASISTENTE.md`):
  - `dashboard/index.js`: importar `DataAssistantPanel`; guardar en el estado el objeto `report` de Power BI; pasar `onReportReady` a `PowerBIEmbed`; añadir la clase `assistant-open` al layout; renderizar el panel **solo si** `DATA_ASSISTANT_AVAILABLE && isRoleAllowed(DATA_ASSISTANT_ALLOWED_ROLES, sessionDetails.Roles)`.
  - `powerbi-embed/index.js`: prop opcional `onReportReady(report)`, que se llama cuando el informe queda cargado.
  - El panel lee la página activa (`report.getActivePage()` y el evento `pageChanged`) y la manda en `Context`.
  - No se tocan el chatbot de planeación, `REACT_APP_AI_BOT_AVAILABLE` ni `REACT_APP_AI_DASHBOARD_AVAILABLE`.

---

## 5. Pruebas

### 5.1 Backend (gs-base-api)

Línea base del MVP: `pruebas_mysql/correr_pruebas_auth.sh` da **274/275 en modo local y 350/351 en plataforma**; la falla conocida de cada modo debe quedar documentada. Se conserva así:

1. **`DataAnalysisAPI.Tests`** (nuevo proyecto xUnit, en la `.sln`, con los mismos paquetes que `GSWebAPI.Tests`). Un `MySqlDatabaseFixture` con Testcontainers carga `TestData/usage-insights-schema-and-data.sql`. Si Docker no está disponible, lee la cadena de una variable de entorno (`USAGE_INSIGHTS_TEST_CONNECTION_STRING`) y apunta a un MySQL 8 local. Cubre:
   - una prueba por consulta: cifras, orden, límites, empates, `HasMoreRows`, notas de «datos hasta», períodos que se salen de los datos y parámetros inválidos (400);
   - alcance: todas las escuelas, ORE y escuela, incluida la escuela fuera del alcance que se comporta como si no existiera;
   - `DataScopeResolver` (prueba unitaria pura por rol);
   - Excel (abrir el `.xlsx` con NPOI y revisar las hojas *Data* y *References*) y especificación de gráfica (tipos, `TotalCategories`, `ZeroCategories`).
2. **`AIServicesAPI.Tests`** (nuevo proyecto xUnit). `DataAssistantService` recibe dos interfaces, `IDataAssistantChatClient` (adaptador sobre `ChatClient`) y `IDataAnalysisFeignAPI`, para poder usar dobles de prueba. Cubre:
   - el bucle de herramientas (máximo 6 pasos);
   - que se descarten argumentos que la herramienta no declara;
   - que el modelo **no** pueda cambiar el alcance, porque el Token se reenvía y el alcance no es un argumento;
   - el registro de descargas y gráficas ligado a su dueño (404 para un id ajeno);
   - la verificación de cifras (`Warnings`), la conversación ligada a su usuario y que caduque a las 2 h;
   - la cuota (429 al negar y `Commit` con la suma de tokens), la IA apagada (503) y `content_blocked`;
   - la captura en el monitoreo con `Subject = "data-assistant"`.
3. **Verificador de caja negra adaptado** (fuera de los repos oficiales, en esta rama `Sprint-68`): copiar `pruebas_mysql/` a `pruebas_monolito/`, cambiar rutas, parámetros y campos según la sección 3, y correrlo contra el monolito local. El monolito se configura con `AppSettings:gus_web_service_url` apuntando a `usersapi_falso.py`, `AzureOpenAI:Endpoint` apuntando a una versión de `foundry_falso.py` que hable el protocolo de Chat Completions, y `AnalysisConnectionStringReader` apuntando al MySQL de pruebas. Objetivo: el **mismo número de verificaciones aprobadas** que el MVP, menos las que ya no aplican (modo `x-api-key`, CORS propio, `/salud` sin sesión), cada una listada con su motivo.

### 5.2 Front (gs-msj-private-webapp-teachers)

- Mover y renombrar las 3 suites del panel (3.8) y adaptarlas al contrato nuevo.
- Nueva suite `src/api/__tests__/data-assistant.test.js`: URLs armadas desde `Settings`, cabeceras puestas por el interceptor (simulando axios), rechazo de `Url` fuera de la lista blanca y nombre de archivo tomado de `Content-Disposition`.
- Nueva suite de montaje: el panel no se renderiza con la bandera apagada ni con un rol distinto de admin/admin-ore, y sí con admin.
- Comando: `CI=true yarn test --watchAll=false` (Jest 23 vía `scripts/test.js`). Todo debe pasar y `yarn build` debe compilar.

---

## 6. Tareas

Leyenda de modelo/esfuerzo: **S-alto** = Sonnet, esfuerzo alto; **S-medio** = Sonnet, esfuerzo medio (tareas mecánicas). «Paralelo con» indica qué tareas pueden correr a la vez en ramas o worktrees distintos que se integran en `req-3741`.

Reglas comunes a todas las tareas:
- Todo el código, los comentarios y los nombres van en inglés.
- Documentación XML `///` en todo lo público, como en `AIServicesAPI`, y JSDoc en el front.
- Ningún secreto en archivos: en los `appsettings.*.json` solo se añaden claves con valores vacíos o no sensibles.
- Commits pequeños en `req-3741`.

### Repo gs-base-api

| # | Tarea | Archivos | Terminado cuando | Prueba | Modelo | Paralelo |
|---|---|---|---|---|---|---|
| **B0** | Crear `req-3741` desde la base que confirme Pedro (P1; recomendada `dev`) y registrar la línea base | — | La rama existe en el remoto; `dotnet build GSBaseAPI/GSBaseAPI.sln` y `dotnet test` de los proyectos actuales pasan y su resultado queda anotado en la descripción del PR | build + test | S-medio | — (primera) |
| **B1** | Modelos de dominio en inglés: records de filas, `UsageInsightResponse` (con `HasMoreRows`), `DataScope`, `TestAccountExclusion`, `ChartOptions`, `InvalidQueryParameterException` | `DataAnalysisAPI/DataAnalysisAPI.Domain/Models/UsageInsights/*.cs` | Compila; nombres según 3.2 y 3.6; `///` en todo | build | S-medio | Con B8 y B9 |
| **B2** | `DataScopeResolver`: sesión (`RoleId`, `OreId`) + escuela del usuario → `DataScope`, con las listas de roles de config (3.7). Falla cerrada | `DataAnalysisAPI.Services/Utils/DataScopeResolver.cs`, `GSBaseAPI/appsettings*.json` (solo claves nuevas, sin secretos) | Admin → `All`; admin-ore → `OreId`; director → `SchoolId`; otro rol o `OreId` vacío para admin-ore → `None` (403) | Pruebas unitarias en B7 | S-alto | Tras B1 |
| **B3a** | Pasar a Dapper las consultas de **conexiones** (`ConexionesPorEscuela`, `ConexionesPorMaestro`, `Tendencia`, `ConexionesYNuevosPorDia`, `PatronConexiones`, `ParticipacionEscuelas`, `UsuariosActivos`, `BuscarEscuelas`, `EscuelaExiste`, `EscuelaDeUsuario`) con filtro de `DataScope` y exclusión de cuentas de prueba **en el SQL** | `DataAnalysisAPI.Infrastructure/Dapper/UsageInsights.cs` (parte 1) | SQL igual al MVP (mismas columnas, `DataScope` como parámetros, nunca concatenado); usa `Configurations.GetMySqlConnection()` y el timeout de config | Pruebas en B7 | S-alto | Con B3b |
| **B3b** | Pasar a Dapper las consultas de **pruebas, docentes y suscripción** (`UsoPorArea`, `CompararAreas`, `PromedioFullQuiz`, `UsoDocentes`, `UsuariosPorSuscripcion`) | `DataAnalysisAPI.Infrastructure/Dapper/UsageInsights.cs` (parte 2) | Igual que B3a | Pruebas en B7 | S-alto | Con B3a |
| **B4** | Lógica de las 13 consultas (de `Rutas.cs` y `P`): validar parámetros (3.5), topes (50; 5000 al exportar), totales, %, empates, `HasMoreRows`, notas de «datos hasta», comparar escuelas y conexiones acumuladas. Notas y errores salen de `Messages:{key}:{es,en}` según `Accept-Language` | `DataAnalysisAPI.Services/UsageInsights.cs`, `DataAnalysisAPI.Services/Utils/UsageInsightsQueryParser.cs`, mensajes en `GSBaseAPI/appsettings.json` → `Messages` (o en el archivo de mensajes que use DataAnalysisAPI) | Sin marcadores en texto; mismas cifras que el MVP | B7 | S-alto | Tras B1–B3 |
| **B5** | `UsageInsightsController` (`[Route("dataanalysisapi/usage-insights")]`): 13 GET + `data-status`; sesión con `GetUserDetails`, `DataScopeResolver`, 400/401/403/503 y log `LogQueryFailure` sin parámetros, datos ni Token. Registrar el `AssemblyPart` en `Program.cs` | `DataAnalysisAPI/Controllers/UsageInsightsController.cs`, `GSBaseAPI/Program.cs` | Las rutas responden según 4.1 con un Token válido (UsersAPI falso) | B7 + verificador (B13) | S-alto | Tras B4 |
| **B6** | Exportar: Excel con NPOI (hojas *Data* y *References*, nombres de archivo de `BuildFileName`) y especificación de gráfica (`ChartSpecificationBuilder`, `UsageInsightsColumnFormatter`). Ruta `{query}/export?format=xlsx\|chart` | `DataAnalysisAPI.Services/Utils/UsageInsightsExcelExporter.cs`, `ChartSpecificationBuilder.cs`, `UsageInsightsColumnFormatter.cs`, `UsageInsightsController.cs` | Mismo contenido que el MVP (comparar con `leer_xlsx.py`); gráficas con los campos de 3.6 | B7 | S-alto | Con B5 (tras B4) |
| **B7** | Proyecto `DataAnalysisAPI.Tests` (xUnit + Testcontainers.MySql, con fallback a variable de entorno) y pruebas de 5.1-1 | `DataAnalysisAPI.Tests/**`, `GSBaseAPI/GSBaseAPI.sln` | `dotnet test DataAnalysisAPI.Tests` en verde; cubre cada consulta, el alcance, Excel y gráfica | `dotnet test` | S-alto | Empieza tras B1; se completa con B3–B6 |
| **B8** | `OreId` en el `ValidationResponseModel` de AIServicesAPI. `IDataAnalysisFeignAPI` / `DataAnalysisFeignAPI`: GET JSON, export xlsx (bytes + `Content-Disposition`) y chart, **reenviando** `Token` y `Accept-Language`, con base `AppSettings:data_analysis_api_url` y timeout. Registrar con `AddHttpClient` | `AIServicesAPI/FeignAPIs/Models/ValidationResponseModel.cs`, `AIServicesAPI/FeignAPIs/DataAnalysisFeignAPI.cs`, `GSBaseAPI/Program.cs`, `appsettings*.json` (clave vacía) | Compila; nunca registra el Token en logs | B12 | S-alto | Con B1 |
| **B9** | Recursos del asistente: `tool-definitions.json` (13 herramientas con nombres y parámetros de 3.4 y 3.5, descripciones en inglés) e instrucciones de sistema traducidas al inglés (R5), con la orden de responder en el idioma del usuario. `DataAssistantToolCatalog` las carga (recurso embebido o `CopyToOutputDirectory`) | `AIServicesAPI/Resources/DataAssistant/*`, `AIServicesAPI/Services/DataAssistant/DataAssistantToolCatalog.cs`, `AIServicesAPI.csproj` | El catálogo produce las `ChatTool` y relaciona cada herramienta con su ruta | B12 | S-alto | Con B1 y B8 |
| **B10** | `DataAssistantService` con `ChatClient` y tool calling (antes, la prueba corta de §2 sobre Responses). Incluye: máximo 6 pasos; descarte de argumentos no declarados; historial en `IMemoryCache` por `ConversationId` + `UserId` con TTL de 2 h y tope de mensajes; `References` copiadas de las respuestas de la API, no escritas por el modelo; `Notices`; `AnswerFigureVerifier`; `DataAssistantDownloadRegistry` (id opaco → consulta + query + `UserId`, TTL, tope de 1000); conteo de tokens de todas las llamadas | `AIServicesAPI/Services/DataAssistant/*.cs`, `IDataAssistantChatClient` + adaptador | Mismo comportamiento que el MVP con los dobles de B12 | B12 | S-alto | Tras B8 y B9 |
| **B11** | `DataAssistantController : BaseController` (`[Route("aiservicesapi/data-assistant")]`): `ask` (`[RequiresAiEnabled]`, `GetAuthenticatedSessionAsync(Module)`, verificación de rol, `ReserveAsync`, `CommitAsync` o `ReleaseAsync`, `CaptureAsync`), `downloads/{id}`, `charts/{id}`; mensajes en `appsettings.aiservices.json`; DI en `Program.cs` | `AIServicesAPI/Controllers/DataAssistantController.cs`, `AIServicesAPI/appsettings.aiservices.json`, `GSBaseAPI/Program.cs` | Contrato 4.2 completo; mapea los errores de 4.2 | B12 + B13 | S-alto | Tras B10 |
| **B12** | Proyecto `AIServicesAPI.Tests` (xUnit) con las pruebas de 5.1-2 | `AIServicesAPI.Tests/**`, `.sln` | `dotnet test AIServicesAPI.Tests` en verde | `dotnet test` | S-alto | Empieza con B8; se completa tras B11 |
| **B13** | Verificador de caja negra adaptado (en `Sprint-68`, no en el repo oficial): `pruebas_monolito/` con un Foundry falso que hable Chat Completions y un script que arranca el monolito con UsersAPI falso y MySQL de pruebas | `INICIATIVA POWER BI/agente_foundry/pruebas_monolito/**` (este repo) | Informe con aprobadas y fallidas comparado con 274/275 y 350/351; cada diferencia justificada | Correr el script | S-alto | Tras B5, B6 y B11 |
| **B14** | Configuración y documentación: claves nuevas (vacías) en cada `appsettings.{env}.json`; lista de secretos de Key Vault que debe crear Andrés (ninguno nuevo si se reutilizan `AzureOpenAI--ApiKey` y `ConnectionStrings--AnalysisConnectionStringReader`); sección en `GSBaseAPI/README.md` (inglés) con rutas, configuración y cómo probar | `GSBaseAPI/appsettings.*.json`, `GSBaseAPI/README.md` | Ningún valor sensible añadido; README actualizado | Revisión del diff | S-medio | Con B13 |

### Repo gs-msj-private-webapp-teachers

| # | Tarea | Archivos | Terminado cuando | Prueba | Modelo | Paralelo |
|---|---|---|---|---|---|---|
| **F0** | Crear `req-3741` desde `main` (confirmar si el PR va a `main` o a `dev`, P2); `yarn install`; línea base de `yarn build` y `CI=true yarn test --watchAll=false` | — | Rama en el remoto; línea base anotada | build + test | S-medio | Con B0 |
| **F1** | Copiar el panel de `repos/…/assistant-panel/` a `data-assistant-panel/` con los renombres de archivos, componentes, funciones, constantes y clases CSS de 3.8 (sin cambiar todavía el comportamiento) | `src/components/data-analysis-module/data-assistant-panel/**` | Sin identificadores, comentarios ni clases CSS en español; los tests del MVP pasan con los nombres nuevos | jest | S-medio | Con B1–B4 (no depende del backend) |
| **F2** | i18n: `textos.js` → `src/translations/i18next/{es,en}/data-assistant.json` (claves en inglés, textos ES/EN), registrados en `src/i18n.js` (y en las variantes por versión si el repo lo exige); componentes con `useTranslation("data-assistant")` | `src/translations/i18next/{es,en}/data-assistant.json`, `src/i18n.js`, componentes del panel | No queda `textos.js`; cambiar el idioma de la plataforma cambia el panel | jest (render en `es` y `en`) | S-medio | Tras F1 |
| **F3** | Capa de API: métodos de `settings.js` (4.3) y `src/api/data-assistant.js` con axios e interceptores; quitar `fetch`, `ASSISTANT_URL` y `REACT_APP_PBI_ASSISTANT_URL`; lista blanca de destinos | `src/custom-classes/settings.js`, `src/api/data-assistant.js`, `data-assistant-panel/config.js` | Ninguna URL ni secreto hardcodeado; el Token lo pone solo el interceptor | `src/api/__tests__/data-assistant.test.js` | S-alto | Tras F1 (contrato de 4.x congelado) |
| **F4** | Adaptar el panel al contrato PascalCase y a los campos estructurados (`Notices`, `HasMoreRows`, `Summary.Ties`, `Downloads`, `Charts`, `ErrorCode`); exportación directa a `dataanalysisapi/usage-insights/{query}/export` con los parámetros nuevos | Componentes del panel, `direct-export.js`, `chart-utils.js`, `expanded-chart.js` | Funciona con respuestas de ejemplo del contrato 4.2 | jest actualizado | S-alto | Tras F3 |
| **F5** | Montaje: `dashboard/index.js` y `powerbi-embed/index.js` según 4.3; bandera `REACT_APP_DATA_ASSISTANT_AVAILABLE`; roles `DATA_ASSISTANT_ALLOWED_ROLES` con `isRoleAllowed`. Documentar la variable en los `.env` de ejemplo o en el README (sin valores sensibles) | `src/components/data-analysis-module/dashboard/index.js`, `powerbi-embed/index.js`, estilos del layout, `README.md` | Con la bandera apagada no hay cambios visibles; con admin y la bandera encendida se ve el panel | Suite de montaje | S-alto | Con F4 |
| **F6** | Pruebas jest de 5.2 completas y prueba manual contra el monolito local (B13): preguntar, descargar Excel, ampliar gráfica, exportación directa | `__tests__/**` | `CI=true yarn test --watchAll=false` y `yarn build` en verde; capturas en el PR | jest + manual | S-alto | Tras F5 y B11 |

### Tarea final (los dos repos)

| # | Tarea | Terminado cuando | Modelo |
|---|---|---|---|
| **R1** | **Revisión previa a los PR.** (a) Seguridad: ningún secreto, endpoint privado ni SQL en el front; ningún secreto nuevo en `appsettings`; Token nunca en logs; alcance siempre en el SQL; SQL siempre parametrizado; dueño verificado en `downloads`/`charts`; `[RequiresAiEnabled]` y cuota en `ask`. (b) Inglés: `grep` de palabras y tildes en español (`[áéíóúñ]`, `consulta`, `escuela`, `fecha`, `desde`, `hasta`…) en los archivos tocados, excepto los JSON de i18n `es`. (c) Convenciones de 3. (d) `dotnet build`/`dotnet test` y `yarn test`/`yarn build` en verde. (e) Descripciones de PR con resumen, decisiones de §2, configuración nueva y resultado de pruebas | Informe de revisión sin hallazgos abiertos | S-alto (o Opus si se quiere una segunda mirada) |
| **R2** | Abrir **un PR por repo**: `req-3741` → base confirmada (P1/P2), con **Pedro como revisor**. Solo con autorización explícita del usuario | Los dos PR abiertos y enlazados al ticket REQ-3741 | S-medio |

Total: **gs-base-api 15 tareas (B0–B14, con B3 dividida en B3a y B3b)**, **front 7 tareas (F0–F6)** y **2 finales (R1, R2)**.

Orden sugerido: B0/F0 → (B1, B8, B9, F1) → (B2, B3a, B3b, F2) → B4 → (B5, B6, B10, F3) → (B7, B11, F4, F5) → (B12, B13, B14, F6) → R1 → R2.

---

## 7. Riesgos y preguntas

### Riesgos

| # | Riesgo | Mitigación |
|---|---|---|
| R1 | **Rama base `master` desactualizada** (sin `AIServicesAPI`) | Confirmar con Pedro (P1) antes de B0. Recomendado: `dev` |
| R2 | Conversaciones y descargas en memoria: si el App Service corre con varias instancias, un `ConversationId` o un id de descarga puede caer en otra instancia y dar 404 | V1 con `IMemoryCache` y afinidad de sesión (ARR affinity). Si hay escalado horizontal (P7), guardar en el MySQL de AIServicesAPI (EF) o en Mongo, que ya existen |
| R3 | Chat Completions en lugar de Responses: cambia cómo se encadenan los pasos y el historial pasa a guardarse en el servidor | Prueba corta al inicio de B10; pruebas de B12; batería de preguntas reales (`preguntas_fase1.txt`, `preguntas_trampa.txt`) en UAT antes del PR |
| R4 | El repo ya tiene **secretos en `appsettings*.json`** | No añadir ninguno; que R1 lo verifique. Recomendar a Andrés y Pedro rotar esas credenciales y moverlas a Key Vault (fuera del alcance de este ticket) |
| R5 | Traducir las instrucciones del modelo al inglés y renombrar las herramientas puede cambiar la calidad de las respuestas en español | Pedir al modelo que responda en el idioma de la pregunta; repetir en UAT la batería de preguntas reales y comparar con el MVP |
| R6 | Feign interno: un salto HTTP más y una validación de sesión más (en caché) por herramienta, y depende de que `data_analysis_api_url` esté bien configurada en cada entorno | Timeout corto, prueba en B13 y documentación en B14. Alternativa: referencia entre proyectos (P5) |
| R7 | El permiso `embed-powerbi-info` también da acceso al asistente | Aceptable para la v1 (mismos usuarios que el tablero). Si se quiere separar, permiso nuevo (P3) |
| R8 | Testcontainers necesita Docker en CI o en local | Fallback con variable de entorno hacia un MySQL 8 de pruebas (B7) |
| R9 | Las notas que el MVP generaba en español dentro del texto pasan a mensajes localizados y campos estructurados: el verificador debe dejar de buscar texto | Lo adapta B13; R1 revisa que no quede ninguna detección por regex |
| R10 | El front `main` no tiene pruebas: las suites del panel serían las primeras y podrían chocar con la configuración de Jest 23 | Correr la línea base en F0; si Jest falla por configuración, documentarlo en el PR sin cambiar la configuración global sin permiso |
| R11 | La cuenta de `AnalysisConnectionStringReader` en UAT parece tener un usuario de escritura | Pedir a Andrés un usuario con solo `SELECT` sobre `gs_data_analysis` (P6) |

### Preguntas para el usuario (o para Pedro y Andrés)

1. **P1 (bloqueante):** `master` de gs-base-api es de sep-2025 y no tiene `AIServicesAPI`. ¿Creamos `req-3741` desde `dev` (recomendado, donde se integran los PR) o desde `main`? ¿Contra cuál va el PR?
2. **P2:** En el front, ¿el PR va a `main` como indicó Pedro, o a `dev` como el flujo dev → uat → main del pipeline?
3. **P3:** ¿Reutilizamos el permiso `embed-powerbi-info` de UsersAPI o creamos `AIDataAssistant`, que necesita un script en la base de UsersAPI?
4. **P4:** ¿Las preguntas de admins al asistente deben quedar en el monitoreo de prompts, visibles en la vista de monitoreo, o solo en un log de auditoría?
5. **P5:** ¿Aceptan que AIServicesAPI llame a DataAnalysisAPI por HTTP interno (Feign, patrón actual), o prefieren una referencia directa entre proyectos (más rápida, pero rompe la independencia de los módulos)?
6. **P6:** ¿La cadena `AnalysisConnectionStringReader` de cada entorno usa una cuenta de solo lectura? Si no, ¿puede Andrés crear una?
7. **P7:** ¿El App Service del monolito escala a varias instancias? Define si la v1 puede guardar conversaciones y descargas en memoria.
8. **P8:** ¿El panel se muestra también a directores (rol 6, alcance de una escuela, que ya soporta el backend) o solo a admin y admin-ore como en el MVP?
9. **P9:** ¿Qué pasa con el App Service del MVP (`app-eya-msj-uat-eastus-001`) cuando se integre el PR? ¿Se apaga?
