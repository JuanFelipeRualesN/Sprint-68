# Revisión REQ-3741 — asistente de datos Power BI (backend + front)

Revisión previa al envío de los PR al líder técnico. Referencia: `PLAN_MIGRACION_REQ-3741.md` y el MVP `EyaInsightsApi`.

| Repo | Rama | Último commit | Diff revisado | Destino PR |
|---|---|---|---|---|
| `gs-base-api` | `req-3741` | `ea7d293` | `main...req-3741` (19 commits, 86 archivos, +10 231/−1) | `uat` |
| `gs-msj-private-webapp-teachers` | `req-3741` | `4e109162` | `main...req-3741` (6 commits, 34 archivos, +3 028/−4) | `uat` |

Front: `git diff origin/uat origin/main` está vacío (los 3 commits de diferencia son merges con el mismo árbol), y `git diff uat...req-3741` da los mismos 34 archivos que `main...req-3741`. El PR a `uat` solo lleva cambios del ticket.

## Resumen

| Severidad | Cantidad |
|---|---|
| Alta | 1 |
| Media | 5 |
| Baja | 6 |
| Desajustes de contrato front ↔ backend | 4 (1 media, 3 baja) |

**Build y pruebas (resultados reales de esta revisión):**

- Backend: `dotnet build GSBaseAPI/GSBaseAPI.sln` con el SDK 8.0.134 falla en `AIServicesAPI/Services/Plannings/PlanPromptBuilder.cs:88` (CS0121, `Split([...])` ambiguo). El error ya está en `main` y no es del ticket: con un SDK 9 o posterior compila. Corregí esa línea solo en mi copia local (`new[] { ' ', ',', ';' }`) para poder seguir. Después de eso compila sin errores.
- `DataAnalysisAPI.Tests`: **168/168 OK**. `AIServicesAPI.Tests`: **93/93 OK**. Ambos usan MySQL en Docker (Testcontainers). Sin el daemon de Docker fallan 129 y 25 pruebas respectivamente (ver M5).
- Front: el `npm test` del repo no arranca, porque faltan `scripts/test.js` y `config/jest/*` (problema que ya existía, ver B6). Ejecuté jest 23.6.0, la versión fijada en `package.json`, con transformaciones CRA equivalentes: **98/100 OK, 6/7 suites**. Las 2 pruebas que fallan son las de `config.test.js` y son reales (ver M4). El build de producción del front no se pudo verificar en este entorno por la misma falta de `config/`.

**¿Listos tras los arreglos?** Backend: sí, una vez cerrados A1, M1 y M2 (M3 y M5 se pueden documentar en el PR). Front: sí, una vez cerrado M4. El panel sale con el flag en `false` en todos los entornos, así que no cambia nada visible hasta que se active.

---

## Contrato front ↔ backend

El backend serializa en PascalCase (`PropertyNamingPolicy = null` en `DataAssistantController.cs:32-36` y en `UsageInsightsJson.cs:17-22`). El mapper del front (`src/api/data-assistant-contract.js:26-31`) lee PascalCase y acepta camelCase como respaldo, así que el *casing* es compatible.

### Desajustes

| # | Campo / código | Front (lo que espera) | Backend (lo que envía) | Efecto | Arreglo | Sev. |
|---|---|---|---|---|---|---|
| C1 | `References[].HasMoreRows`, `References[].Summary` (`Ties`), `References[].Note` | `data-assistant-contract.js:76-78` los lee; `data-assistant-panel/index.js:95-96` muestra los chips «más filas» y «empates» | `AIServicesAPI/Services/DataAssistant/DataAssistantService.cs:379`: `BuildReference` solo copia `Query, Source, DateColumn, Period, Filters, Scope, DataUntil, TotalRows, GeneratedAt` | Los chips «hay más filas» y «empates» nunca aparecen. La lista recortada y los empates solo se ven si el texto de `Notices` los menciona. El plan (§4, F4) dice que deben ir como campos estructurados | Añadir `"HasMoreRows", "Note", "Summary"` al arreglo de `BuildReference` y una prueba en `DataAssistantServiceTests` | Media |
| C2 | IA apagada → `ErrorCode = "ai_disabled"` | `data-assistant-panel/error-messages.js:16` | `AIServicesAPI/Filters/RequiresAiEnabledAttribute.cs:33` devuelve 503 `{ error }` sin `ErrorCode` | El front cae en `http_503` y muestra «modelo no disponible» (`error-messages.js:11`) en vez de «IA desactivada» | Opción 1 (preferida): que el front muestre `error` (ya viene localizado) cuando no haya `ErrorCode`. Opción 2: añadir `ErrorCode = "ai_disabled"` al filtro. Es un filtro compartido: revisar a los otros consumidores, aunque es un campo aditivo | Baja |
| C3 | Cuota agotada → `ErrorCode = "quota_exceeded"` | `error-messages.js:15` | `DataAssistantController.cs:109-115` devuelve 429 `{ error, remaining, resetsAt }` sin `ErrorCode` (igual que `GenerateController`, plan §4.3) | Funciona gracias al respaldo `http_429`. El caso `quota_exceeded` nunca ocurre | Quitar `quota_exceeded` del front, o añadir `ErrorCode = "quota_exceeded"` al 429 (aditivo) | Baja |
| C4 | `Language` del cuerpo de `ask` | `data-assistant-contract.js:22` lo envía y lo documenta como idioma del panel | `DataAssistantController.cs:118`/`291-292`: el idioma del llamador sale de `Accept-Language`. `request.Language` solo se usa como pista en el contexto (`DataAssistantService.cs` `BuildContextText`) | Sin fallo: el front también envía `Accept-Language` (`interceptors.js:9`). Si alguno de los dos cambiara, divergirían | Documentar en el JSDoc que `Accept-Language` manda y `Language` solo orienta | Baja |

### Verificado sin desajuste

| Elemento | Front | Backend |
|---|---|---|
| `POST aiservicesapi/data-assistant/ask` | `settings.js` `getDataAssistantAskAPI` | `DataAssistantController.cs:24,85` |
| `GET aiservicesapi/data-assistant/downloads/{id}` y `charts/{id}` (prefijo con lista blanca) | `settings.js` `getDataAssistantResourceAPI`, `data-assistant.js:274-285` | `DataAssistantController.cs:188,228`; URLs en `DataAssistantService.cs:288,298` |
| `GET dataanalysisapi/usage-insights/{query}/export?format=xlsx&from&to` | `data-assistant.js:329-331`, `direct-export.js:11-14,37` (8 consultas) | `UsageInsightsController.cs:79-110`. Las 8 consultas existen en `UsageInsights.cs:35-47` |
| Request `Question, ConversationId, Context{Report,Page}, Language` | `data-assistant-contract.js:18-23` | `DataAssistantModels.cs:317,326` |
| Response `Answer, ConversationId, References, Notices, Warnings, ExecutedQueries{Tool,Arguments,Status}, ErrorCode, Downloads, Charts` | `data-assistant-contract.js:144-154` | `DataAssistantModels.cs:340-341`, `DataAssistantService.cs:151` |
| `Downloads[] {Id, FileName, Query, Url, Highlighted}` | `contract.js:82-88` | `DataAssistantService.cs:283-290` |
| Gráfica: clave de fila `Category`, `Series[{Key,Name}]`, `Extra{Key,Name,Unit}`, `Grouping`, `Order`, `IncludeZeros`, `All`, `Layout`, `Variant`, `TotalValue`, `TotalCategories`, `ShownCategories`, `ZeroCategories`, `HasMoreRows`, `IsTimeSeries`, `XAxisLabel`, `YAxisLabel`, `Unit`, `Note`, `Period`, `Filters`, `DataUntil` | `contract.js:103-137` | `ChartSpecificationBuilder.cs:27,172-253` (`Order`/`All` solo en barras; `IncludeZeros`/`ZeroCategories` solo cuando se ordena: el front lo tolera) |
| `Charts[].Id/Url/Highlighted` | `contract.js:106-107,131` | `DataAssistantService.cs:297-299`. En el gráfico ampliado, `Id`/`Url` vienen de `DataAssistantController.cs:257-258`; sin `Highlighted` (→ `false`, correcto) |
| `Period.A/B {From,To}` (comparativas) | `contract.js:53-60` | `UsageInsights.cs:365`; nombres de serie en `ChartSpecificationBuilder.cs:268-273` |
| `Filters.School` / `Filters.TestAccounts` (`all`, `excluded`/`included`) | `index.js:91-92` | `UsageInsights.cs:190,259…651` |
| Opciones del gráfico ampliado `all, order, includeZeros, limit` | `data-assistant.js:340-349` | `DataAssistantService.cs:36,360-369`, `UsageInsightsQueryParser.cs:160-185` |
| Errores `{ error, ErrorCode }`: `unauthorized`, `forbidden`, `empty_question`, `model_unavailable`, `model_access_denied`, `content_blocked`, `download_expired`, `data_unavailable`, `invalid_parameters`, `chart_unavailable`, `database_unavailable`, `unknown_query` | `data-assistant.js:211-218`, `error-messages.js:9-20` | `DataAssistantController.cs:92-148,199,211,294-295`; `UsageInsightsController.cs:59,108,123-144,150-156`. (`data_unavailable` y `unknown_query` caen en el texto genérico del front, lo cual es aceptable) |

---

## Hallazgos — severidad alta

### A1. `data_analysis_api_url` vacío en todos los entornos: el asistente no puede consultar datos
- **Dónde:** `GSBaseAPI/appsettings.json:33`, `appsettings.msj-uat.json:37`, `appsettings.private-uat.json:30` (y `msj`, `msj-dev`, `private`, `private-dev`, `med-rd`, `med-quindio`), `appsettings.json.template`.
- **Por qué importa:** `DataAnalysisFeignAPI` (`AIServicesAPI/FeignAPIs/DataAnalysisFeignAPI.cs`, constructor y `SendAsync`) lanza `InvalidOperationException` si la URL falta. `ExecuteToolAsync` la convierte en `data_unavailable` (`DataAssistantService.cs:242-247`), así que en UAT **todas** las preguntas acaban en «no pude consultar los datos». Las descargas y gráficas de `downloads/{id}` y `charts/{id}` devuelven 502. Además, `ai_data_assistant_deployment_name` está vacío y cae en `azure_openai_deployment` (`gpt-4o-mini` en UAT, `AzureOpenAiDataAssistantChatClient.cs:37-39`). Hay que confirmar que ese modelo soporta tool calling con la calidad esperada.
- **Arreglo:** poner por entorno la URL base del propio monolito (en UAT, la misma que `REACT_APP_DATAWAREHOUSE_API` del front, p. ej. `https://estudiayavanza-baseapi-uat…`) o guardarla en Key Vault si la política lo exige. Decidir el despliegue del asistente por entorno. Añadir al PR una lista de verificación de configuración por entorno.

## Hallazgos — severidad media

### M1. Las referencias pierden `HasMoreRows`, `Summary` y `Note` (= C1)
- **Dónde:** `AIServicesAPI/Services/DataAssistant/DataAssistantService.cs:379`; front en `src/components/data-analysis-module/data-assistant-panel/index.js:95-96`.
- **Arreglo:** ver C1.

### M2. Si falla la exportación de la gráfica, se pierde toda la respuesta
- **Dónde:** `DataAssistantService.cs:294` (`AttachDownloadAndChartAsync` llama a `ExportUsageInsightAsync` sin `try/catch`).
- **Por qué importa:** si la consulta del chat salió bien pero la llamada `format=chart` sufre un timeout (`TaskCanceledException`) o un `HttpRequestException`, la excepción sube hasta `DataAssistantController.cs:144`. La persona recibe 502 `model_unavailable`, aunque ya había datos y la cuota se libera. Con 6 pasos y una llamada de gráfica por consulta, la probabilidad crece. `JsonNode.Parse` sobre un cuerpo 200 que no sea JSON también rompería.
- **Arreglo:** envolver la exportación de la gráfica en `try/catch (HttpRequestException or TaskCanceledException or InvalidOperationException or JsonException)`, registrar solo el tipo de excepción y seguir sin gráfica (la descarga queda registrada). Añadir una prueba con el doble de `IDataAnalysisFeignAPI` lanzando una excepción.

### M3. El estado en memoria no sirve con varias instancias
- **Dónde:** `GSBaseAPI/Program.cs` (registro `AddSingleton` de `DataAssistantConversationStore` / `DataAssistantDownloadRegistry`), `DataAssistantDownloadRegistry.cs:39-46`, `DataAssistantConversationStore.cs:44-54`.
- **Por qué importa:** si el App Service de UAT o de producción escala a más de una instancia sin afinidad ARR, `ConversationId` y los ids de descarga y gráfica viven en una sola instancia. Las preguntas de seguimiento pierden el contexto y los botones Excel/ampliar devuelven `download_expired` al azar. Un reinicio o despliegue también los borra (TTL 2 h). Las interfaces ya permiten cambiar de almacén.
- **Arreglo:** confirmar con infraestructura el número de instancias y la afinidad. Si hay más de una, usar `IDistributedCache` (Redis o la caché que ya usa `CacheHandlerLibrary`) detrás de las interfaces existentes. Dejarlo explícito en la descripción del PR.

### M4. `config.test.js` usa `jest.isolateModules`, que no existe en jest 23.6.0
- **Dónde:** `src/components/data-analysis-module/data-assistant-panel/__tests__/config.test.js:8`; `package.json` fija `"jest": "23.6.0"` (`isolateModules` llegó en Jest 24).
- **Por qué importa:** con la versión del repo, 2 de las 100 pruebas fallan (`TypeError: jest.isolateModules is not a function`). El commit F6 dice que «All suites pass», lo que indica que se ejecutó con otra versión de jest.
- **Arreglo:** usar `jest.resetModules()` seguido de `require("../config")` dentro de `loadConfig`, que funciona en jest 23.

### M5. Las pruebas .NET nuevas necesitan Docker y entraron en la solución
- **Dónde:** `GSBaseAPI/GSBaseAPI.sln` (se añaden `DataAnalysisAPI.Tests` y `AIServicesAPI.Tests`), `DataAnalysisAPI.Tests/Infrastructure/UsageInsightsDatabaseFixture.cs`, `AIServicesAPI.Tests/Infrastructure/AssistantDatabaseFixture.cs`.
- **Por qué importa:** si un pipeline o un desarrollador ejecuta `dotnet test` sobre la solución sin un daemon de Docker, fallan 154 pruebas. Lo comprobé aquí: 129 + 25 fallos sin Docker y 0 fallos con Docker.
- **Arreglo:** documentar en el README y en el PR el requisito (Docker, o las variables `USAGE_INSIGHTS_TEST_CONNECTION_STRING` / `AI_SERVICES_TEST_CONNECTION_STRING`). Marcar las pruebas de BD con `[Trait("Category","Database")]` para que el pipeline pueda filtrarlas, o confirmar que el agente de build tiene Docker.

## Hallazgos — severidad baja

### B1. Códigos de error sin `ErrorCode` (= C2 y C3)
- `RequiresAiEnabledAttribute.cs:33` y `DataAssistantController.cs:109-115`. Ver C2 y C3.

### B2. Nombres de columna configurables que se interpolan en SQL sin validar
- **Dónde:** `DataAnalysisAPI/Utils/UsageInsightsSettings.cs:56-57` (`usage_insights_subscription_status_column`, `usage_insights_subscription_cancellation_column`), que se usan como `sph.{statusColumn}` en `DataAnalysisAPI.Infrastructure/Dapper/UsageInsights.cs:509` y siguientes.
- **Por qué importa:** viene de configuración y no del usuario, así que el riesgo es bajo. Aun así, una entrada errónea o maliciosa en appsettings o Key Vault se convierte en SQL. El resto de valores sí van como parámetros (`@scope_ore`, `@scope_school`, `@excl_domain{i}`…) o son enteros (`UtcOffsetMinutes`).
- **Arreglo:** validar con `^[A-Za-z_][A-Za-z0-9_]{0,63}$` al leer, y usar el valor por defecto si no cumple.

### B3. El log de fallos de MySQL incluye el mensaje del motor
- **Dónde:** `DataAnalysisAPI/DataAnalysisAPI.Services/UsageInsights.cs:104-105` (mensaje de hasta 300 caracteres y 80 del SQL).
- **Por qué importa:** en un SELECT rara vez incluye datos, pero algunos mensajes de MySQL citan valores. No registra token ni parámetros, lo cual está bien.
- **Arreglo:** registrar solo `Number` y `SqlState`, o mantener el mensaje solo en el nivel `Debug`.

### B4. Identificador en español en una prueba
- **Dónde:** `AIServicesAPI.Tests/DataAssistantServiceTests.cs:306,310` (`var descargar`).
- **Arreglo:** renombrarlo a `downloadAnswer` o similar. El resto de texto en español en las pruebas son aserciones sobre mensajes localizados o entradas de usuario, y está permitido.

### B5. Cambios ajenos al ticket que el PR debe explicar
- **Backend:**
  - `GSBaseAPI/GSBaseAPI.csproj`: `AWSSDK.Core 4.0.0.26` (commit B0 `e000a88`, arreglo de restore NU1107).
  - `AIServicesAPI.Tests`: alineación de EF Relational (`cb747bd`).
  - `GSBaseAPI/README.md`: se borra la línea 7 de `main`, «- Forcing deployment 2.». Puede ser un disparador de despliegue; confirmar o restaurar.
  - `GSBaseAPI.sln`: se añaden los 2 proyectos de prueba.
- **Front:** `.env.*` (8 archivos, flag `REACT_APP_DATA_ASSISTANT_AVAILABLE=false`) y `README.md`.
- **Arreglo:** dedicar una sección «Cambios no funcionales» a esto en la descripción de cada PR.

### B6. La infraestructura de pruebas del front no está en el repo (ya existía)
- **Dónde:** `package.json` (`"test": "node scripts/test.js"` y `jest.transform` → `config/jest/*.js`). Ni `scripts/test.js` ni `config/` están versionados, y se resuelve `babel-core 6` en lugar del puente de Babel 7.
- **Por qué importa:** nadie puede ejecutar las suites nuevas con `npm test` tal como está el repo. No lo introduce este ticket, pero limita el valor de las pruebas F1–F6.
- **Arreglo:** abrir otro ticket para versionar `config/jest` y `scripts/test.js` (o pasar a `react-scripts test`) y añadir `babel-core@7.0.0-bridge.0`. En este PR basta con indicar cómo se ejecutaron las pruebas.

---

## Seguridad (regla 2) — sin hallazgos bloqueantes

- **Secretos:** no hay claves, cadenas de conexión ni tokens reales en appsettings, launchSettings, pruebas ni documentación. Las pruebas usan `test-password` (contenedor efímero) y `the-azure-key` (doble falso). Las claves nuevas de `AppSettings` no son secretas (`UsageInsightsSettings.cs` lo documenta). En el front no hay URL ni clave nueva: usa `REACT_APP_AI_API` / `REACT_APP_DATAWAREHOUSE_API`, que ya existían.
- **Autenticación:** todas las rutas nuevas validan la sesión con el patrón existente. `UsageInsightsController.cs:120` usa `GetUserDetails.GetUserIdRequestOnOtherAPI`, como `PowerBIController`. `DataAssistantController.cs:89,191,231` usa `GetAuthenticatedSessionAsync` más la lista de roles. Las descargas y gráficas están ligadas a `UserId` y se regeneran con el token de quien pide, así que el alcance se vuelve a aplicar.
- **Alcance ORE/escuela:** `DataScopeResolver.cs:25-51` sale de la sesión. Se aplica en SQL con parámetros (`UsageInsights.cs` (Dapper) `:134-150`). Una escuela fuera del alcance recibe la misma respuesta que una inexistente (hay prueba).
- **Inyección SQL:** valores de usuario parametrizados. Solo se interpolan enteros y los nombres de columna de configuración (B2).
- **Excel:** NPOI `SetCellValue(string)` escribe celdas de texto, no fórmulas (`UsageInsightsExcelExporter.cs:98`), así que no hay inyección de fórmulas al abrir el archivo.
- **Logs:** no se registran token, pregunta ni datos (`DataAssistantController.cs:140,146,178,210,214,250`; `DataAssistantService.cs:244`). Ver B3.
- **Front:** el token solo va a URLs de una lista blanca: prefijo `/aiservicesapi/data-assistant/` normalizado contra `../`, o el origen de data warehouse más `/dataanalysisapi/usage-insights/` (`settings.js`, `data-assistant.js:274-285`). Se usa axios con `responseType: blob`, no `<a href>` con token.

## Inglés y nombres (regla 3)

Los identificadores, comentarios XML/JSDoc, rutas, campos JSON, claves de configuración, logs, códigos de error y nombres de prueba están en inglés en ambos repos. Las instrucciones de sistema y `tool-definitions.json` también están en inglés. El español que queda está en sitios permitidos: textos i18n del front, `Messages:*:es` de appsettings, patrones de detección «descarg|gráfic» (`appsettings.aiservices.json`, que deben reconocer preguntas en español) y aserciones de pruebas sobre mensajes localizados. La única excepción es B4.

## Convenciones (regla 4)

- **Backend:** controladores como los vecinos (`BaseController` y `GetLocalizedMessage` en AIServicesAPI; `ControllerBase` y `GetUserDetails` en DataAnalysisAPI). El cliente Feign tipado se registra con `AddHttpClient<I, T>`. Servicios y DI en `Program.cs` junto al resto de AIServicesAPI. Mensajes en `appsettings.aiservices.json` y en el nuevo `appsettings.dataanalysis.json`, con el mismo patrón. Ciclos de vida coherentes: `DataAssistantService` y el cliente de chat son *scoped* sobre singletons (`AzureOpenAIClient`, catálogo, almacenes) y no capturan servicios *scoped* en singletons.
- **Workaround de OpenAI 2.9.1:** se llama al método de protocolo `CompleteChatAsync(BinaryContent, …)` (`AzureOpenAiDataAssistantChatClient.cs:54-55`), que evita el `MissingMethodException` de los métodos tipados. Lo cubre `AzureOpenAiAdapterTests` (deployment, `api-version`, cabecera `api-key`, parseo de `tool_calls`). Correcto. Conviene quitarlo cuando se suba el paquete.
- **Front:** la estructura `data-assistant-panel/` (index, componentes, `style.css`, `__tests__`) sigue la de `data-analysis-module`. La capa API está en `src/api` y las URLs en `Settings`. i18next usa un namespace propio. Usar axios plano en lugar de la instancia de `interceptors.js` está justificado en `data-assistant.js:165-167` (el panel muestra los errores dentro del chat, sin modal ni cierre de sesión).

## Borradores de descripción de PR (rama `claude/req-3741-pr-drafts`)

Ambos están en inglés y la estructura es correcta. Correcciones de exactitud:

### `PR_DESCRIPTION_REQ-3741_gs-base-api.md`
- «20 commits» → son **19** (`git rev-list --count main..req-3741`). La base `ac9c9d0` es correcta.
- Añadir a «Changes»: los 2 proyectos de prueba entran en `GSBaseAPI.sln`; la alineación de EF Relational en `AIServicesAPI.Tests` (`cb747bd`); y que se borra la línea «- Forcing deployment 2.» de `GSBaseAPI/README.md` (B5). Confirmar si esto último es intencional.
- Tests: decir explícitamente que las suites nuevas necesitan Docker o las variables `*_TEST_CONNECTION_STRING` (M5). Sin eso, `dotnet test` sobre la solución da 154 fallos.
- Añadir a «Open items» lo que salió en esta revisión: M1/C1 (referencias sin `HasMoreRows`/`Summary`/`Note`) y M2 (un fallo de la gráfica tumba la respuesta), salvo que se arreglen antes de abrir el PR. B2, B3 y la falta de `ErrorCode` en 503/429 (C2/C3) pueden ir como seguimiento.
- Verificado y correcto: tope de 40 mensajes (`DataAssistantService.cs:27`), sesión `READ ONLY` en UTC, 168/93 pruebas (las reproduje), error CS0121 con el SDK 8.0.134, configuración por entorno y P7 (estado en memoria).

### `PR_DESCRIPTION_REQ-3741_webapp-teachers.md`
- «Jest: 7 suites, 100 tests pass» es cierto **con Jest 27**. Con el `jest 23.6.0` fijado en `package.json` fallan 2 pruebas (`jest.isolateModules`, M4). Arreglar M4 o indicar la versión exacta usada.
- «Open items → Contract fields … must be confirmed»: ya están confirmados. `Category`, `Grouping`, `Extra`, `Order`, `IncludeZeros`, `All`, `Highlighted`, `Period.A/B`, `Filters.School` y `Filters.TestAccounts` existen en el backend con esos nombres (ver la tabla del contrato). Cambiar el punto por los desajustes reales: C1 (`References[].HasMoreRows`/`Summary`/`Note` no llegan) y C2/C3 (`ai_disabled` y `quota_exceeded` no se envían; el front cae en `http_503`/`http_429`).
- La nota de rama (`main` 3 commits por delante de `uat` con árboles idénticos) es correcta: lo verifiqué.
- «react-scripts build: compiled successfully» y lint: no pude reproducirlos aquí porque falta `config/` (B6). Conviene indicar cómo se ejecutó (configuración local fuera del repo), como ya se hace con las pruebas.
