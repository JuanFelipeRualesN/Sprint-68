# REQ-3741 · Fase 4 — Diseño de autenticación y autorización de EyaInsightsApi

Diseño para ejecutar sin contexto previo. Bloquea el despliegue: hoy la API (incluido `GET /exportar/{consulta}`, que baja hasta 5000 maestros con nombre) no autentica a nadie salvo que exista `EYA_API_KEY`, y `correr_api_mysql.sh` no la define.

Rutas relativas a la raíz del repo `Sprint-68`. API = `INICIATIVA POWER BI/agente_foundry/EyaInsightsApi/`. Front = `repos/gs-msj-private-webapp-teachers/` (PR #5). Backend corporativo = `repos/gs-base-api/`.

---

## 1. Hallazgos

### 1.1 El front NO manda tokens de Entra ID a las APIs de la plataforma
- `src/authConfig.js`: MSAL (`PublicClientApplication`, creado en `src/index.js:59`) con `authority` **`https://login.microsoftonline.com/common/`** (multi-inquilino, sin tenant fijo), `clientId` = `Settings.APP_AZURE_CLIENT_ID` (sale de `REACT_APP_APP_AZURE_CLIENT_ID` en `.env.*`), `cacheLocation: sessionStorage`. No define scopes propios ni roles de aplicación.
- Scopes que se piden: solo de Microsoft Graph (`User.Read` en `guest-module/view/loginSso.js:49`; `Calendars.ReadWrite`, `OnlineMeetings.ReadWrite`, `MailboxSettings.Read` en `teacher-module/view/teams/*` y `hooks/useTimeZones.js`). El `Authorization: Bearer ${token.accessToken}` que se ve en el código es **solo hacia Graph** (`create-meeting.js`, `graph.js`). Esos tokens tienen audiencia Graph: ninguna API nuestra puede validarlos.
- MSAL se usa **solo para iniciar sesión con SSO**: `loginSso.js` toma el `idToken` y lo canjea en `POST {USERS_API}/usersapi/login/sso` (`src/api/users.js:800`, `Settings.getLoginSsoAPI()` en `custom-classes/settings.js:2399`). También existe login con usuario y clave (`/usersapi/login/client`), sin MSAL.
- La sesión real es un **token propio de la plataforma**: `SessionManager.getSession()` (`custom-classes/session-manager.js:92`) lo lee de `localStorage['auth'].Token`. Todas las APIs lo reciben en el header **`Token`** (no `Authorization`): `src/api/interceptors.js` (interceptor de axios y `getAuthHeaders()` → `{ Token, "Accept-Language" }`).
- Roles en el front (`settings.js:228-259`): `1 client`, `2 admin`, `3 student`, `4 employee`, `5 teacher`, `6 director`, `7 parent`, `8 admin-ore`. El panel del asistente solo se pinta para `admin` y `admin-ore` (`assistant-panel/config.js`, `REACT_APP_PBI_ASSISTANT_ROLES`), pero eso es solo visual.
- El panel (`assistant-panel/assistant-api.js`) usa `fetch` sin headers de sesión: `preguntar` solo manda `Content-Type`; `descargarArchivo` hace un GET "simple" sin headers a propósito para no disparar preflight de CORS. `leerAdjuntos()` (`assistant-api.js:55`) aísla la lectura de `descargas[]`/`graficas[]`; `exportar-directo.js` llama a `/exportar` directo.

### 1.2 gs-base-api: cómo valida tokens y qué roles/permisos existen
- **Emisión** (`UsersAPI/Controllers/LoginController.cs:54` → `UsersAPI.Services/Accounts/Authentication.cs:324 LoginProcessSSO`): valida el `idToken` de Entra contra el `OpenIdConfigUrl` del cliente SSO (por dominio, `SsoManager.GetClientDetails`), con `ValidateAudience` = clientId del SSO y **`ValidateIssuer = false`** (`Authentication.cs:359`); toma el correo de `emails`/`preferred_username` y hace el login normal. El token de sesión es un **JWT HS256 propio** (`CustomClasses/TokenManager.cs:37 GenerateToken`, claim `Name = userId`, emisor = audiencia = `_issuer`), además registrado como sesión en la base.
- **Validación desde otras APIs** (patrón ya usado por `DataAnalysisAPI`, la API de Power BI): `DataAnalysisAPI/DataAnalysisAPI.Services/FeignAPIs/GetUserDetails.cs` lee el header `Token` y hace `POST {gus_web_service_url}/usersapi/accounts/get_user_by_token` con `{ Token, IpAddress, PermissionName }` (endpoint en `UsersAPI/Controllers/AccountController.cs:225`; claves de config `gus_web_service_url` y `gus_endpoint_permission_session_api` en `GSBaseAPI/appsettings.json:49,374`). UsersAPI (`CustomClasses/ValidateGenericCall.cs:69` → `Authentication.CheckIfUserCanDoOperation`) comprueba sesión vigente **y** que el rol tenga el permiso con ese nombre; responde 401 si no, o 200 con `ValidationResponseModel`: `SessionValid, UserId, RoleId, OreId, IsStaff, IsDirector, Messages`.
- **Permisos por endpoint**: cada acción de DataAnalysisAPI pide uno por nombre: `embed-powerbi-info`, `connected-list`, `connected-document`, `users-list`, `users-document`, `results-list`, `results-document`, `grades-academic`, `school`, `ore`, etc. Los nombres viven en la tabla de permisos de UsersAPI (`PermissionController`, `RolesController`, `Repositories/Roles/*`), no en código.
- **Alcance por rol** (mismo criterio que el tablero): `PowerBIController.cs:43-49` y `ConnectionsController.cs:43-53` filtran por `OreId` si la sesión lo trae (admin-ore) y no filtran si no (admin). `EmbedPowerBIReport(reportId, workspaceId, oreId)` aplica ese ORE al token de inserción de Power BI. **Quien ve todas las escuelas: `admin` (rol 2, sin `OreId`)**. `admin-ore` (rol 8) ve solo su ORE.
- Los datos de EyaInsightsApi ya tienen la llave para filtrar: `school.ore_id` y `user.ore_id` (ver `pruebas_mysql/esquema_y_datos.sql:6-7`). `vw_teacher_activities` solo trae el **nombre** del ORE (`ore`), no el id.

### 1.3 EyaInsightsApi hoy
- `Program.cs:75-85`: middleware que exige `x-api-key` **solo si** existe `EYA_API_KEY` (menos `/salud`).
- `Program.cs:62,66`: CORS `panel` solo si `Asistente:Habilitado`; permite **solo** el header `content-type`, métodos GET/POST, expone `content-disposition`. `/exportar` y las 12 consultas quedan sin CORS si el asistente está apagado.
- `Program.cs:92`: `POST /asistente/preguntar` ejecuta las herramientas del modelo por HTTP loopback con `x-api-key` (`Asistente.cs:102-119 EjecutarAsync`). Si se autentica al usuario y el loopback sigue con la clave, **las consultas del modelo se saltarían el alcance por ORE**.
- `Asistente.cs:128`: hacia Foundry usa `TokenCredential` (`azcli` en local; identidad administrada desplegado) con scope `https://ai.azure.com/.default`. El modelo **no** llama a la API: devuelve llamadas a funciones y el servidor las ejecuta.
- El agente "en la nube" de `crear_agente.py` (otro flujo, fase anterior) sí llama a la API desplegada (`openapi.json` → `servers`) con la conexión de proyecto `eya-api-key` (header `x-api-key`): **identidad de servicio, sin usuario**.
- T6 del `PLAN_FASE4.md` (descargas/gráficas en el asistente + `GET /asistente/descargas/{id}`) está pendiente; ya existe `/exportar` y `Graficas.cs`.

---

## 2. Opciones

| | A. Validar JWT de Entra ID en EyaInsightsApi | B. gs-base-api (DataAnalysisAPI) como proxy | **B′. Validación delegada en UsersAPI (recomendada)** | C. Enlace firmado de vida corta |
|---|---|---|---|---|
| Qué es | Nueva app registration con scope propio (`api://…/Insights.Read`) y app roles; el front pide ese token con MSAL; la API lo valida (`AddJwtBearer`). | El panel llama a rutas nuevas de DataAnalysisAPI que reenvían a EyaInsightsApi (privada). | EyaInsightsApi lee el header `Token` de la plataforma y lo valida con `usersapi/accounts/get_user_by_token` + un nombre de permiso, igual que `GetUserDetails.cs`. Obtiene `UserId/RoleId/OreId` y aplica el alcance. | URL con `exp`+`firma` HMAC que baja un archivo sin cabecera. |
| Encaje con lo existente | Malo: hoy nadie usa tokens de Entra para APIs; los usuarios que entran con usuario/clave no tienen cuenta MSAL; roles y ORE viven en la base de UsersAPI, no en Entra (habría que mapear correo→rol igual). | Bueno en arquitectura (la API queda detrás del backend). | Exacto: mismo token, mismos permisos, mismo `OreId` que el tablero de Power BI. | No es autenticación: es un complemento para descargas. |
| Cambios fuera de este repo | App registration + consentimiento en el tenant + cambios de MSAL en todo login. | Código nuevo en gs-base-api (otro equipo/ciclo), timeouts de 60-90 s del asistente, paso de binarios. | Solo **dar de alta un permiso** en UsersAPI y asignarlo a roles 2 y 8 (dato, no código). Si no, se reusa `embed-powerbi-info`. | Clave de firma en Key Vault. |
| Riesgo | Alto (login de todos). | Medio (dos despliegues acoplados). | Bajo; dependencia en línea de UsersAPI (mitigada con caché corta). | Fuga del enlace por historial/logs mientras viva. |

**Recomendación: B′.** Reusa sin cambios de código en gs-base-api el mecanismo que ya protege los datos de Power BI (sesión + permiso + ORE), sirve a los usuarios con SSO y con clave, y el alcance queda impuesto **en el servidor**. A se descarta (tokens de Graph, login mixto, roles fuera de Entra). B queda como endurecimiento opcional de red (EyaInsightsApi sin ingreso público, solo alcanzable desde la red de la plataforma) si el equipo de infraestructura lo pide; no cambia el contrato de B′. C **no se necesita para el panel**: el front ya baja con `fetch` → `blob`, así que puede mandar el header `Token`; queda como tarea opcional (A7) solo si se quiere abrir la URL fuera del panel.

Detalles de B′ que el ejecutor debe respetar:
- Header de entrada: `Token` (el que manda toda la plataforma). Nunca loguear su valor; para caché/log usar `SHA-256(token)` truncado.
- `IpAddress` hacia UsersAPI: IP del cliente (`X-Forwarded-For` primero, como `IPAddressDetector`); el endpoint la exige no vacía.
- `HttpClient` único (o `IHttpClientFactory`), async, timeout 5 s. Caché en memoria `hash(token)+permiso → Usuario` 60 s (positivos) y 10 s (401). Así el loopback del asistente no multiplica llamadas.
- **Falla cerrada**: UsersAPI caída/timeout → 503 `autenticacion_no_disponible`; 401 de UsersAPI → 401 `no_autorizado`; rol sin acceso → 403 `sin_permiso`.

---

## 3. Autorización

### 3.1 Quién ve qué
| Identidad | Cómo llega | Alcance de datos |
|---|---|---|
| `admin` (RoleId 2, sin OreId) | `Token` válido + permiso | Todas las escuelas. |
| `admin-ore` (RoleId 8, con OreId) | `Token` válido + permiso | Solo escuelas con `school.ore_id = OreId` (y maestros de esas escuelas). |
| Cualquier otro rol (director, teacher…) | `Token` válido | 403 `sin_permiso` (aunque UsersAPI le diera el permiso). Ampliar a director requiere saber su escuela: `get_user_by_token` no devuelve `SchoolId` → pregunta 1. |
| Servicio (`x-api-key` = `EYA_API_KEY`) | Header `x-api-key` sin `Token` | Todas las escuelas. Solo para el agente en la nube (`crear_agente.py`) y scripts; en producción apagado salvo que el usuario diga lo contrario (pregunta 6). |
| Anónimo | — | Solo `/salud`. |

`admin-ore` sin `OreId` en la respuesta → 403 (no "todo").

### 3.2 Contrato interno (fijo, para poder paralelizar)
Archivo nuevo `EyaInsightsApi/Auth.cs`:
```csharp
public sealed record Alcance(int? OreId) { public static readonly Alcance Todo = new(null); public bool EsTodo => OreId is null; }
public sealed record Usuario(string Id, string Rol, Alcance Alcance, string Origen); // Origen: "plataforma" | "servicio" | "local"
// Auth.Usuario(HttpContext) → Usuario (lo deja el middleware en HttpContext.Items["eya.usuario"]); nunca null tras el middleware.
```
Las consultas reciben el `Alcance` (p. ej. `IConsultasUso.Para(Alcance)` que devuelve una vista filtrada, o un parámetro más; decide A2, sin cambiar firmas públicas de rutas).

### 3.3 Reglas de alcance en datos
- Filtro en SQL, nunca después en memoria sobre resultados ya cortados: `sc.ore_id = @eya_ore` en todas las consultas que pasan por `school`; usuarios sin escuela **quedan fuera** cuando hay alcance (hoy "pasan" en `FE()`, `ConsultasMySql.cs:87`).
- `vw_teacher_activities` (uso-docentes): filtrar por las escuelas del ORE (join por nombre de escuela/ORE contra `school`) — verificar en la base real que el nombre del ORE ↔ `ore_id` es 1:1 (pregunta 8).
- `escuela=` fuera del alcance → mismo 400 que una escuela inexistente (no revelar que existe). `buscar-escuela` solo devuelve escuelas del alcance.
- Si hay caché de listas completas en `ConsultasMySql`, la clave incluye `OreId`.
- `Referencias` (JSON y hoja del Excel) añade `alcance: "todas las escuelas" | "ORE <nombre>"`.
- Auditoría: una línea de log por `/exportar` y `/asistente/descargas` con `usuario.Id`, rol, OreId, consulta, filas (sin token, sin datos).

### 3.4 Identidades servicio a servicio
1. **EyaInsightsApi → Foundry** (modelo): identidad administrada del App Service (`Asistente:Credencial` ≠ `azcli`) con rol de usuario de Azure AI en el proyecto. Sin claves. El modelo nunca ve tokens ni la API.
2. **EyaInsightsApi → EyaInsightsApi** (loopback de herramientas y de descargas): reenvía **el mismo `Token` del usuario** (no `x-api-key`). La segunda validación es un acierto de caché y el alcance se aplica igual: el modelo no puede escapar del ORE del usuario.
3. **EyaInsightsApi → UsersAPI**: HTTP con el `Token` del usuario, como hace DataAnalysisAPI. No necesita credencial propia.
4. **Agente en la nube de Foundry → API** (`crear_agente.py`): identidad distinta del usuario (`x-api-key` en la conexión `eya-api-key`), alcance total. No lo usa el panel. Producción: `EYA_API_KEY` vacío (servicio deshabilitado) salvo decisión del usuario.

---

## 4. Tareas

Orden: **A1 ∥ A2 ∥ A5 → A3 ∥ A4 → A6** (A7 opcional, tras A3). Un PR para A1–A4 y A6 en `Sprint-68`; A5 en la rama del PR #5 del front. A3 **reemplaza a T6** de `PLAN_FASE4.md` (T5 sigue igual; T7/T8 tras A3/A5).

### A1 — Middleware de autenticación (`Auth.cs`) · Sonnet/high · ∥ A2, A5
- Archivos: nuevo `EyaInsightsApi/Auth.cs`; `EyaInsightsApi/Program.cs` (reemplaza el middleware de `Program.cs:75-85`); `appsettings.json` (solo claves sin secretos).
- Config: `Auth:Modo` = `local` (por defecto; sin validación, `Usuario("local","admin",Alcance.Todo,"local")`, solo si `ASPNETCORE_URLS` es loopback, si no → no arranca) | `plataforma`. `Auth:UsersApiUrl`, `Auth:Permiso` (por defecto `embed-powerbi-info` hasta tener el nuevo), `Auth:RolesTodo=2`, `Auth:RolesOre=8`, `Auth:CacheSegundos=60`. `EYA_API_KEY` opcional = identidad de servicio.
- Orden en el pipeline: CORS → auth → rutas. `OPTIONS` (preflight) y `/salud` sin auth. Comparación de `x-api-key` en tiempo constante (`CryptographicOperations.FixedTimeEquals`).
- Respuestas: 401 `no_autorizado`, 403 `sin_permiso`, 503 `autenticacion_no_disponible` (JSON como hoy).
- Terminado: con `Auth:Modo=plataforma`, sin `Token` → 401; token que UsersAPI rechaza → 401; rol 5 → 403; rol 2 → 200; UsersAPI apagada → 503; `/salud` → 200 sin token; 2 peticiones seguidas con el mismo token = 1 llamada a UsersAPI.
- Prueba: `dotnet build`; con el stub de A4 (si A4 no está, un stub mínimo propio en el scratchpad); verificador actual en modo `local` sigue 116/116 (o el total vigente) en los tres modos.

### A2 — Alcance por ORE en las consultas · Sonnet/high · ∥ A1, A5
- Archivos: `EyaInsightsApi/ConsultasMySql.cs`, `ConsultasEjemplo.cs`, `Modelos.cs` (interfaz `IConsultasUso`), `Rutas.cs`, `Program.cs` (handler de `/exportar` y loop de `Rutas.Consultas`, `Program.cs:123-129`), `Excel.cs`/`Columnas.cs` (fila `alcance` en Referencias), `pruebas_mysql/esquema_y_datos.sql` (añadir 2-3 escuelas con otro `ore_id` y sus maestros/conexiones **sin cambiar** los números de las escuelas existentes; si cambia algún esperado, ajustar el verificador con el mismo cálculo de fuerza bruta).
- Usa `Alcance` de §3.2 (si A1 no está mergeado, crea solo los records en `Auth.cs` tal cual).
- Reglas: §3.3 completa. `ConsultasEjemplo`: asignar `ore_id` sintético para que el filtro también funcione sin MySQL.
- Terminado: con `Alcance(OreId=X)`, ninguna fila de otra ORE en las 12 consultas ni en `/exportar` (xlsx y gráfica); `escuela` de otra ORE → 400 igual que inexistente; `Alcance.Todo` da exactamente lo de hoy.
- Prueba: verificador en los tres modos sin regresiones; bloque nuevo de A4 para el filtrado.

### A3 — Asistente con identidad del usuario + T6 + CORS · Sonnet/high · tras A1 (y T4 hecho)
- Archivos: `EyaInsightsApi/Asistente.cs`, `EyaInsightsApi/Program.cs`.
- Loopback: `EjecutarAsync` deja de recibir `clave`; recibe los headers a reenviar (`Token` del usuario, o `x-api-key` si el usuario es de servicio, `Accept-Language`). Nada de `x-api-key` cuando hay usuario.
- T6 tal como en `PLAN_FASE4.md` (`descargas[]`, `graficas[]` al final de `RespuestaAsistente`, dedupe, `destacar`) **más**: la entrada de caché de cada descarga guarda `usuario.Id` y `OreId`; `GET /asistente/descargas/{id}` exige auth y responde 404 `descarga_expirada` si el id no existe, expiró **o es de otro usuario** (mismo 404, no revelar). Reconstruye por loopback a `/exportar/...` con el `Token` del que pide.
- Conversaciones (`previous_response_id`, 2 h): ligar `conversacion_id` al `usuario.Id`; otro usuario → empezar conversación nueva.
- CORS (en `Program.cs:62`, ahora **siempre** que `Cors:Origenes` tenga valor, no solo con asistente): `WithOrigins(origenes)` (dominios del front por ambiente, sin `*`), `WithHeaders("content-type","token","accept-language")`, `WithMethods("GET","POST")`, `WithExposedHeaders("content-disposition")`, `SetPreflightMaxAge(10 min)`, sin `AllowCredentials` (no hay cookies). Aplica a `/exportar`, `/asistente/*` y las consultas.
- Terminado: pregunta real devuelve `descargas`/`graficas` y el enlace baja el `.xlsx` con el `Token`; un id inventado o de otro usuario → 404; un admin-ore que pregunta por otra ORE no recibe sus datos; preflight desde el origen del front → 204 con `access-control-allow-headers` que incluye `token` y `access-control-expose-headers: content-disposition`; desde otro origen → sin cabeceras CORS.
- Prueba: `dotnet build`; verificador verde; `curl -i -X OPTIONS` con `Origin` y `Access-Control-Request-Headers: token`; manual con Foundry (`az login`) o marcar **no probado** en el PR (R6).

### A4 — Pruebas de autenticación y alcance · Sonnet/high · tras A1 y A2; ∥ A3
- Archivos: nuevo `pruebas_mysql/usersapi_falso.py` (solo stdlib, `http.server`: `POST /usersapi/accounts/get_user_by_token` responde según una tabla fija `token → {SessionValid, UserId, RoleId, OreId}` o 401; cuenta llamadas en `GET /llamadas`); `pruebas_mysql/verificar_api_mysql.py` (bloque `[auth]` opcional con argumento `auth`).
- Casos (≈15): sin token 401; token malo 401; rol teacher 403; admin 200 = resultados de hoy; admin-ore: 0 filas de otra ORE en `conexiones-por-escuela`, `conexiones-por-maestro`, `participacion-escuelas`, `uso-docentes`, `buscar-escuela` y en el `.xlsx` de `/exportar`; `escuela` de otra ORE → 400; `x-api-key` correcta sin token → 200 (si `EYA_API_KEY`); incorrecta → 401; UsersAPI apagada → 503; caché: 5 peticiones = 1 llamada; `OPTIONS` sin token → no 401.
- Terminado: todos ✓ en los tres modos con `Auth:Modo=plataforma` apuntando al stub; los casos de siempre siguen ✓ en `local`.

### A5 — Front: mandar la sesión · Sonnet/medium · ∥ A1, A2 (contrato fijo) — **repo del front, rama del PR #5**
- Archivos: `src/components/data-analysis-module/assistant-panel/assistant-api.js`, `exportar-directo.js` (si llama a `fetch` propio), `textos.js`, `__tests__/*`.
- Cambio: `preguntar`, `descargarArchivo` y la exportación directa usan `getAuthHeaders()` de `src/api/interceptors.js` (`Token` + `Accept-Language`), sin `x-api-key`. Seguir con `fetch` → `blob` (no `<a href>`); quitar el comentario de "sin headers propios". 401 → texto "Tu sesión expiró, vuelve a iniciar sesión"; 403 → "No tienes permiso para estos datos"; 503 `autenticacion_no_disponible` → "No se pudo validar tu sesión, intenta de nuevo". `leerAdjuntos()` no cambia.
- Terminado: en la red del navegador cada llamada al servicio lleva `Token`; las pruebas unitarias del panel pasan y cubren 401/403.
- Prueba: tests del panel (`__tests__`), build con esbuild; manual con la API en `plataforma` contra el stub o UsersAPI de dev. **Puede requerir la máquina del usuario** para la prueba en navegador.

### A6 — Configuración de despliegue y documentación · Sonnet/medium (docs: Haiku/low) · al final
- Archivos: `correr_api_mysql.sh` (exporta `Auth__Modo=local` explícito y `Cors__Origenes=http://localhost:3000`; comentario de que producción usa `plataforma`), `EyaInsightsApi/Program.cs` (al arrancar con `ASPNETCORE_ENVIRONMENT=Production`: si `Auth:Modo` ≠ `plataforma` o falta `Auth:UsersApiUrl` → no arranca con mensaje claro), `EXPORTAR_API.md` (sección Seguridad: header `Token`, alcance, errores 401/403/503, CORS), `PANEL_ASISTENTE.md` (punto 2 de pendientes → hecho), `PLAN_FASE4.md` (T6 → "ver A3 en DISENO_AUTH.md"; R4 actualizado).
- Lista de variables de App Service (sin valores secretos): `Auth__Modo`, `Auth__UsersApiUrl`, `Auth__Permiso`, `Cors__Origenes`, `Asistente__Credencial=managed`, `EYA_API_KEY` (vacío salvo pregunta 6), cadena MySQL desde Key Vault.
- Terminado: arranque en Production sin auth falla; documentación sin secretos (nada de `.env.local` ni `.env.*`).

### A7 — (Opcional) Enlace firmado de vida corta · Sonnet/high · tras A3, solo si el usuario lo pide (pregunta 7)
- `POST /exportar/{consulta}/enlace` (autenticado, mismos parámetros) → `{url, expira_en}` con `url = /exportar/{consulta}?<params>&exp=<unix>&sub=<userId>&ore=<OreId|todo>&firma=<HMAC-SHA256 base64url>`; vida 5 min; clave `EYA_FIRMA_DESCARGAS` (Key Vault, ≥32 bytes). `GET /exportar` acepta `firma` válida y no vencida **en lugar** del `Token`, recalculando el alcance desde `ore` firmado; parámetros alterados → 403. `Cache-Control: no-store`, `Referrer-Policy: no-referrer`.
- Terminado: la URL baja el Excel sin cabecera hasta `exp`; cambiar cualquier parámetro o pasar `exp` → 403.

### Tarea del usuario (no de agente)
- Dar de alta el permiso (p. ej. `data-assistant`) en UsersAPI y asignarlo a los roles 2 y 8 en cada ambiente; luego `Auth__Permiso=data-assistant`.

---

## 5. Preguntas para el usuario
1. **Roles en producción:** ¿el asistente y `/exportar` son solo para `admin` (todas las escuelas) y `admin-ore` (su ORE)? ¿Algún otro rol (p. ej. `director` con alcance a su escuela)? Si sí, ¿de dónde sale su escuela, dado que `get_user_by_token` no devuelve `SchoolId`?
2. **Permiso en UsersAPI:** ¿creamos un permiso nuevo (`data-assistant`) asignado a los roles 2 y 8, o reusamos `embed-powerbi-info` (el que ya protege el tablero)? ¿Quién lo inserta en cada ambiente (dev, uat, prod)?
3. **UsersAPI por ambiente:** ¿la URL base que debe usar EyaInsightsApi es la misma de `REACT_APP_USERS_API` en `repos/gs-msj-private-webapp-teachers/.env.msj*` (y `gus_web_service_url` de `gs-base-api/GSBaseAPI/appsettings.json`)? ¿Es alcanzable desde donde se despliegue EyaInsightsApi?
4. **Despliegue y CORS:** ¿dónde corre EyaInsightsApi (App Service en la misma red que el private endpoint de MySQL)? ¿Cuáles son los dominios del front por ambiente para `Cors__Origenes`?
5. **Entra ID / Foundry:** ¿se puede asignar identidad administrada al App Service con rol de usuario de Azure AI en el proyecto de Foundry? (Tenant, app registration y scopes **no** hacen falta con la opción recomendada; solo si eligen la opción A: tenant ID, app registration de la API con scope expuesto y app roles, y confirmar que todos los admin entran por SSO.)
6. **Agente en la nube (`crear_agente.py`, conexión `eya-api-key`):** ¿se usará en producción? Si no, se deja `EYA_API_KEY` vacío y la API no acepta identidad de servicio.
7. **Descarga fuera del panel:** ¿hace falta abrir/pegar la URL de exportación en el navegador, en Excel ("Desde la web") o en un correo, sin la sesión del panel? Si sí, se hace A7 (enlace firmado de 5 min).
8. **Datos:** ¿el nombre del ORE (`ore`) y su `ore_id` en `gs_data_analysis.school` son 1:1, y ese `ore_id` es el mismo `OreId` que devuelve UsersAPI para un admin-ore?

---

## 6. Hallazgo fuera de alcance (informar, no corregir aquí)
`LoginProcessSSO` usa `ValidateIssuer = false` con una app multi-inquilino (`authority …/common/`) y toma el correo de `preferred_username`: un usuario de **otro** tenant con ese UPN podría iniciar sesión como un usuario existente si el cliente SSO de su dominio lo permite. Conviene validar el emisor (`tid` del tenant del cliente SSO) en gs-base-api. No bloquea este diseño, pero B′ hereda la confianza en esa sesión.

---

## 7. Implementación (A1–A6) · decisiones del usuario, configuración y supuestos

### 7.1 Decisiones del usuario sobre las preguntas de §5 y cómo se aplicaron
| # | Respuesta | En el código |
|---|---|---|
| 1 | Roles con acceso: `admin`, `admin-ore` y `director`. | `Auth:RolesTodo=2`, `Auth:RolesOre=8`, `Auth:RolesEscuela=6`. Director = **solo su escuela** (el alcance más restrictivo; el diseño no lo definía). Su escuela sale de `gs_data_analysis.user.school_id` (UsersAPI no devuelve `SchoolId`); si no se encuentra → 403. |
| 2 | Reusar `embed-powerbi-info`. | `Auth:Permiso` por defecto `embed-powerbi-info`. No hay que dar de alta ningún permiso. |
| 3 | URL de UsersAPI por ambiente: no se sabe. | `Auth__UsersApiUrl` (obligatoria en `plataforma`; en Production la API no arranca si falta). Ejemplo: `https://users-api.ejemplo.invalid` (no real). |
| 4 | Despliegue y orígenes CORS: no se sabe. | `Cors__Origenes` (lista separada por comas). En Production no se permite `*` ni valores que no sean un origen http(s). |
| 5 | Identidad administrada hacia Foundry: sí. | `Asistente__Credencial=managed` (por defecto): `DefaultAzureCredential` con la identidad administrada del App Service; `Asistente__ClientId` si es asignada por el usuario. Sin claves. |
| 6 | Agente en la nube y panel van a producción. | El panel usa `Token`. El agente en la nube (`crear_agente.py`, conexión `eya-api-key`) usa la identidad de servicio: **`EYA_API_KEY` debe tener valor en producción** (desde Key Vault) y ese agente ve todas las escuelas. |
| 7 | Descarga fuera del panel: no. | No se hizo A7 (enlace firmado). |
| 8 | ORE ↔ `ore_id` 1:1: no se sabe. | No se pudo verificar (no hay acceso a la base real ni el esquema de UsersAPI lo dice). Ver supuestos. |

### 7.2 Variables de entorno (App Service). Ningún valor secreto va en el repositorio
| Variable | Valor | Notas |
|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Activa las comprobaciones de arranque. |
| `Auth__Modo` | `plataforma` | Obligatorio en Production (`local` no arranca). |
| `Auth__UsersApiUrl` | URL base de UsersAPI del ambiente | **Pendiente del usuario.** Debe ser alcanzable desde el App Service; se prefiere https. |
| `Auth__Permiso` | `embed-powerbi-info` (por defecto) | Cambia a `data-assistant` solo si algún día se crea un permiso propio. |
| `Auth__RolesTodo` / `Auth__RolesOre` / `Auth__RolesEscuela` | `2` / `8` / `6` (por defecto) | Ids de rol de UsersAPI. |
| `Auth__CacheSegundos` | `60` (por defecto) | Caché de sesión válida; las inválidas, 10 s; los fallos de UsersAPI no se cachean. |
| `Cors__Origenes` | `https://<front dev>,https://<front uat>,…` | **Pendiente del usuario.** Sin `*`. |
| `Asistente__Habilitado` | `true` | Activa `/asistente/*`. |
| `Asistente__Credencial` | `managed` **o** `apikey` | `managed`: identidad administrada hacia Foundry (rol «Azure AI User» sobre el proyecto). `apikey`: clave de Foundry (cuando no se puede asignar ese rol; ver abajo). `azcli` solo en desarrollo y `prueba` solo fuera de Production. |
| `AzureOpenAI__ApiKey` | secreto en Key Vault | Solo con `Asistente__Credencial=apikey` (obligatoria: sin ella la API no arranca y dice por qué). Mismo nombre de configuración que la API de IA de gs-base-api (`AzureOpenAI:ApiKey`). En App Service: `@Microsoft.KeyVault(SecretUri=https://kv-eya-msj-uat.vault.azure.net/secrets/FOUNDRY-API-KEY/)`. Nunca en archivos, logs, respuestas ni mensajes de error. |
| `AzureOpenAI__Endpoint` | `https://chatbotaipowerbi.openai.azure.com` (por defecto) | Solo con `apikey`: endpoint del **recurso** (`AzureOpenAI:Endpoint` de gs-base-api), no el del proyecto. Si ese host no acepta la clave, probar `https://chatbotaipowerbi.services.ai.azure.com`. En Production debe ser https. |
| `Foundry__Modelo` / `AppSettings__azure_openai_deployment` | `gpt-4.1-mini` (por defecto) | Nombre del **deployment**. `Foundry__Modelo` manda; con `apikey`, si falta, se usa `AppSettings:azure_openai_deployment` (el que usa gs-base-api). |
| `KeyVault__VaultUrl` | opcional | Alternativa a la referencia `@Microsoft.KeyVault(...)`: carga el vault como configuración (`AddAzureKeyVault`, `DefaultAzureCredential`), como GSBaseAPI; los secretos se llaman `AzureOpenAI--ApiKey`, etc. Ambas formas dan el mismo valor; no hace falta usar las dos. |
| `Foundry__Endpoint`, `Foundry__Modelo` | del proyecto de Foundry | Ya tienen valor por defecto; revisar por ambiente. `Foundry__Endpoint` (proyecto) lo usan `managed`/`azcli`; con `apikey` no se usa (se usa `AzureOpenAI__Endpoint`). |
| `EYA_API_KEY` | secreto en Key Vault | Identidad de servicio del agente en la nube. Vacío = no existe esa identidad. |
| `Datos__Origen=mysql`, `EYA_MYSQL` (o `EYA_MYSQL_HOST/_USER/_PASSWORD`) | cadena desde Key Vault | Cuenta de solo lectura. |

Pendiente del usuario en Azure: (a) URL de UsersAPI por ambiente; (b) dominios del front por ambiente; (c) habilitar la identidad administrada del App Service y asignarle el rol «Azure AI User» en el proyecto de Foundry; (d) revisar que UsersAPI sea alcanzable desde el App Service.

### 7.3 Supuestos que quedan explícitos en el código
1. **`OreId` de UsersAPI = `gs_data_analysis.school.ore_id`** (`ConsultasMySql.FA`). Si no coincidieran, un admin-ore vería otras escuelas o ninguna. Verificar con un admin-ore real antes de abrir el acceso. Dato a favor (revisado en el código): en UsersAPI `upd_user_ore` se inserta y actualiza por `user_id` (`Users.cs` ~L1001 y ~L1024), o sea un ORE por usuario; no demuestra que ese id sea el de `school.ore_id`.
2. **`UserId` de UsersAPI = `gs_data_analysis.user.user_id`** para ubicar la escuela de un director (`ConsultasMySql.EscuelaDeUsuario`). Si no coincide, el director recibe 403 (falla cerrada) o, en el peor caso, la escuela de otro usuario: verificar con un director real.
3. **Nombre de ORE ↔ `ore_id` 1:1** para `uso-docentes`: la vista `vw_teacher_activities` trae solo nombres, así que se exige que la escuela del alcance coincida en nombre de escuela **y** de ORE (el criterio más restrictivo). Si hay dos escuelas con el mismo nombre en el mismo ORE, ambas cuentan.
4. `get_user_by_token` responde 401 también cuando el rol no tiene el permiso: eso se ve como 401 (no 403). El 403 es por rol no autorizado en esta API.
5. Los usuarios sin escuela quedan fuera cuando hay alcance (admin-ore y director); el admin los sigue viendo como hoy.

### 7.4 Qué se probó y qué no
- **Probado** (MySQL 8 local con datos sintéticos, UsersAPI y Foundry **falsos**): `pruebas_mysql/correr_pruebas_auth.sh` → local **190/190** y plataforma **239/239** en `paridad`, `dominio` e `incluir`; pruebas del panel (jest) 20/20; 6 comprobaciones de arranque (loopback / Production) en el script.
- **No probado:** UsersAPI real (el formato de la respuesta se tomó del código de gs-base-api), Foundry real (identidad administrada, modelo, instrucciones), el despliegue en Azure, el navegador con la sesión real y la prueba de los roles reales (OreId / director).
- **Hallazgo de §6 sin cambios:** `LoginProcessSSO` con `ValidateIssuer = false`; fuera de alcance.

### Foundry con API key (`Asistente__Credencial=apikey`)
- **Por qué:** en Azure no se pudo asignar «Azure AI User» a la identidad administrada del App Service; la administración de Azure pidió seguir el patrón de la API de IA existente (`repos/gs-base-api/AIServicesAPI`): `AzureOpenAIClient` con `AzureOpenAI:Endpoint` + `AzureOpenAI:ApiKey` (`AzureKeyCredential`), deployment en `AppSettings:azure_openai_deployment`, secretos desde Key Vault. Solo el agente de Foundry (`GenerateController`) usa `AIProjectClient` + `DefaultAzureCredential`; ese no es el patrón copiado.
- **Qué se tomó y qué no:** mismas claves de configuración (`AzureOpenAI__Endpoint`, `AzureOpenAI__ApiKey`, nombre del deployment) y Key Vault por referencia de App Service **o** `KeyVault__VaultUrl`. **No se migró al SDK `Azure.AI.OpenAI`/Chat Completions:** el bucle del asistente usa la Responses API (`/openai/v1/responses`, historial en el servidor con `previous_response_id`, `function_call`/`function_call_output`); pasarlo a Chat Completions obligaba a guardar y reenviar el historial y a reescribir el bucle de herramientas, un riesgo alto sin poder probarlo contra el servicio real. Se conserva la llamada HTTP actual y solo cambia la autenticación.
- **Qué cambia:** la API llama a `{AzureOpenAI__Endpoint}/openai/v1/responses` con la cabecera `api-key: <clave>` y **sin** `Authorization` ni token Entra. El cuerpo es el mismo (`model` = deployment, `instructions`, `input`, herramientas). Con `managed`/`azcli` nada cambia (Bearer al endpoint del proyecto).
- **Arranque:** con `apikey` y sin `AzureOpenAI__ApiKey` la API no arranca («exige la clave en AzureOpenAI:ApiKey»); un `AzureOpenAI__Endpoint` que no sea https tampoco arranca en Production. La clave no se escribe en ningún log ni respuesta; si Foundry responde 401/403 el cliente recibe `503 sin_acceso_al_modelo` sin el cuerpo de Foundry.
- **No traído a propósito** (el módulo de gs-base-api los tiene): cuotas, `RequiresAiEnabled` y monitoreo de prompts. Conviene evaluarlos después, sobre todo cuotas por usuario, porque con una clave compartida cualquier uso cuenta contra el mismo recurso.
- **NO probado contra el servicio real** (sin acceso desde el entorno de desarrollo): solo con el Foundry falso (cabecera `api-key` presente, `Authorization` ausente, ruta `/openai/v1/responses`, deployment, la clave no sale en log ni respuestas). Pendiente en UAT: que el host acepte la clave en `/openai/v1/responses` (si no, cambiar `AzureOpenAI__Endpoint` entre `*.openai.azure.com` y `*.services.ai.azure.com`), que el deployment exista en ese recurso y que `KeyVault__VaultUrl` cargue los secretos (solo se compiló, no se ejecutó contra un vault).
