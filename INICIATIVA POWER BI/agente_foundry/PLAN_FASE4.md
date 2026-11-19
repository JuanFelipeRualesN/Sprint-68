# REQ-3741 · Fase 4 — Excel descargable y gráficas desde el asistente

Plan para ejecutar sin contexto previo. Todo vive en `INICIATIVA POWER BI/agente_foundry/` salvo la tarea del panel (repo del front).

## 0. Cómo funciona hoy (lo mínimo que hay que saber)
- `EyaInsightsApi/Program.cs`: 12 rutas `GET` (minimal API) que devuelven un `Respuesta` (`Modelos.cs`): `consulta, fuente, columna_fecha, periodo, filtros, datos_hasta, resultados[], nota, resumen, total_filas, generado_en`, en snake_case. Parámetros validados en la clase `P` (`limite` 1–50, `orden`, `escuela`, fechas). Errores: `ErrorConsulta` → 400, `MySqlException` → 503. Middleware `x-api-key` si existe `EYA_API_KEY`.
- Datos: `IConsultasUso` con dos implementaciones (`ConsultasEjemplo`, `ConsultasMySql` en solo lectura). Las consultas internas ya traen la lista completa (1000 escuelas / 5000 maestros) y el handler corta con `limite`.
- `Asistente.cs` (`POST /asistente/preguntar`, apagado por defecto): bucle contra Foundry (Responses API). Las herramientas se generan **leyendo todos los `paths` de `openapi.json`**; cada herramienta se ejecuta llamando por HTTP a la propia API. El servidor (no el modelo) arma `referencias`, `avisos`, `advertencias` (verificador anti-invención) y `consultas[]` (herramienta + argumentos + estado).
- Panel React (repo `gs-msj-private-webapp-teachers`, rama `REQ-3741-panel-asistente`, sin commit, en la máquina del usuario) muestra esa respuesta.
- Pruebas: `pruebas_mysql/verificar_api_mysql.py <url> <offset> [paridad|dominio|incluir]` recalcula por fuerza bruta contra `pruebas_mysql/esquema_y_datos.sql` (MySQL 8, datos sintéticos). Hoy 116/116 en los tres modos. Solo usa la biblioteca estándar de Python.

## 1. Objetivo y alcance
**Objetivo:** que una respuesta del asistente pueda (a) ofrecer un **.xlsx descargable** con el resultado completo de la consulta y sus referencias, y (b) mostrar una **gráfica** del resultado, ambos derivados de forma determinista de las consultas existentes (sin cifras escritas por el modelo).

**Dentro:**
- Exportación `.xlsx` de las 11 consultas de datos (todas menos `buscar-escuela`).
- Especificación de gráfica (JSON) para las consultas con forma graficable.
- `descargas[]` y `graficas[]` en la respuesta del asistente + ruta de descarga.
- Regla nueva en `instrucciones.txt`; extensión del verificador.
- Cambios del panel (tarea aparte, repo del front).

**Fuera:** regiones (Fase 2) y talleres (Fase 3); datos por hora; consultas nuevas; gráficas dentro del Excel; PNG en el servidor; PDF/CSV; streaming; autenticación de usuario del servicio (sigue siendo requisito previo al despliegue, ver `PANEL_ASISTENTE.md`); cambios al agente de Foundry en la nube (`crear_agente.py`).

## 2. Decisiones de diseño
| # | Decisión | Justificación |
|---|---|---|
| D1 | **Formato `.xlsx` con ClosedXML** (`ClosedXML` 0.104.x, licencia MIT). | EPPlus ≥5 exige licencia comercial; el OpenXML SDK puro es muy verboso; ClosedXML es MIT, sin dependencias nativas (corre igual en Linux/App Service). CSV descartado: tildes/Excel regional y sin hoja de referencias. |
| D2 | **Dos hojas:** `Datos` (encabezados legibles, una fila por resultado, números como número, `%` con formato `0.0`, autofiltro, fila de encabezado fija) y `Referencias` (consulta, fuente, columna de fecha, período(s), cada filtro, cuentas de prueba, datos hasta, filas exportadas, generado en, nota completa con AVISO/EMPATE, y el `resumen` clave por clave). | Las referencias son la garantía del proyecto; el Excel sale del equipo sin el panel, debe llevarlas consigo. |
| D3 | **Gráficas = especificación JSON generada en el servidor; dibuja el cliente.** | Evita dependencias nativas (SkiaSharp/fuentes) en el servidor, sale en el tema y el idioma del panel, es verificable con el mismo verificador (se comparan números, no píxeles) y conserva el principio "el modelo no escribe cifras". |
| D4 | **Una sola ruta nueva genérica:** `GET /exportar/{consulta}?formato=xlsx|grafica&<mismos parámetros>`, donde `{consulta}` es la ruta existente sin `/` (p. ej. `conexiones-por-maestro`). | Reutiliza validaciones y cálculos sin duplicar 11 handlers. Para lograrlo, cada handler actual pasa a ser una función con nombre registrada en un diccionario (`ruta → Func<HttpRequest, IConsultasUso, Respuesta>`); `MapGet` y `/exportar` usan la misma función. |
| D5 | **En exportación, `limite` sube a 5000** (por defecto = máximo). Se marca con `HttpContext.Items["exportar"]=true` y `P.OrdenLimite` lo respeta. Si se corta, la hoja `Referencias` lo dice. La gráfica usa el `limite` normal (≤50) para no dibujar 5000 barras. | Quien pide «en Excel» quiere la lista completa, no las 10 filas del chat. |
| D6 | **`/exportar` NO va en `openapi.json`.** | `Asistente.CargarHerramientas` convierte cada `path` en herramienta del modelo: el modelo no debe poder pedir archivos ni ver binarios. |
| D7 | **El agente no "llama" a una herramienta de Excel.** El servidor añade a cada consulta exitosa del turno una entrada en `descargas[]` (`{id, nombre, consulta, url:"/asistente/descargas/{id}", filas_estimadas}`) y, si la consulta es graficable, una en `graficas[]` (la especificación ya calculada, con `limite` del turno). `destacar=true` en ambas si la pregunta pide Excel/descarga o gráfica (regex en el servidor: `excel|xlsx|descarg|hoja de c[aá]lculo|spreadsheet|download` / `gr[aá]fic|chart|graph|plot`). | Determinista, sin riesgo de enlaces inventados, cero cambios al bucle de herramientas. El modelo solo sabe (por instrucciones) que el botón existe. |
| D8 | **Descarga por id opaco:** `id` = GUID de 128 bits; caché en memoria `id → (ruta, query sanitizada, expira)` con la misma vida que la conversación (2 h) y tope de 1000 entradas (se descartan las más viejas). `GET /asistente/descargas/{id}` reconstruye el archivo llamando a `/exportar` por loopback (igual que `EjecutarAsync`). Sujeta al mismo middleware `x-api-key` que `/asistente/preguntar`; CORS `panel` pasa a `GET, POST` y expone `content-disposition`. Id desconocido/expirado → 404 JSON `{error:"descarga_expirada"}`. | No expone parámetros arbitrarios al navegador, no guarda datos en disco ni en memoria (solo la consulta), y el archivo se genera al hacer clic. |
| D9 | **Mapa consulta → gráfica** (fijo en código, archivo nuevo `Graficas.cs`): | |

| Consulta | Tipo | Eje X | Series |
|---|---|---|---|
| `tendencia-conexiones` | `lineas` | `periodo` | `conexiones` |
| `conexiones-acumuladas` | `lineas` | `periodo` | `conexiones_acumuladas`, `usuarios_acumulados` |
| `patrones-conexion` | `barras` | `nombre` (7 días o roles, en el orden devuelto) | `conexiones` |
| `conexiones-por-escuela` / `-por-maestro` | `barras_horizontales` | `nombre` | `conexiones` |
| `participacion-escuelas` | `barras_horizontales` (unidad `%`) | `nombre` | `porcentaje_participacion` |
| `uso-por-area` | `barras_agrupadas` | `nombre` | `pruebas_asignadas`, `pruebas_completadas` |
| `comparar-areas` | `barras_agrupadas` | `nombre` | `pruebas_completadas_a`, `pruebas_completadas_b` |
| `comparar-escuelas` | `barras_agrupadas` | `nombre` | `conexiones_a`, `conexiones_b` |
| `promedio-full-quiz` | `barras_horizontales` (`%`) | escuela/materia (o "escuela · materia") | `promedio_solo_realizadas` |
| `uso-docentes` | `barras_agrupadas` | `escuela` | `maestros`, `con_conexiones`, `con_asignaciones` |
| `buscar-escuela` | — (sin gráfica) | | |

Contrato de la especificación (snake_case), **formato Recharts** (decisión del usuario, sección 5 · 3): `data` es un arreglo de objetos planos `{categoria, <serie1>, <serie2>…}`; `eje_x.clave` (siempre `"categoria"`) y cada `series[].clave` son las claves de esos objetos; `tipo` = `"line"` | `"bar"` (el front admite también `"pie"`, esta API no lo genera); `layout` (solo en `bar`) = `"horizontal"` (columnas) | `"vertical"` (barras horizontales), con la semántica del prop `layout` de `BarChart`. `barras` / `barras_agrupadas` del mapa de arriba son `bar` + `layout:"horizontal"` (varias series = agrupadas); `barras_horizontales` es `bar` + `layout:"vertical"`; `lineas` es `line`.
```json
{ "consulta": "tendencia_conexiones", "tipo": "line", "titulo": "Conexiones por mes",
  "eje_x": { "clave": "categoria", "titulo": "Período" },
  "eje_y": { "titulo": "Conexiones", "unidad": "conexiones" },
  "series": [ { "clave": "conexiones", "nombre": "Conexiones" } ],
  "data": [ { "categoria": "2026-01", "conexiones": 12 }, { "categoria": "2026-02", "conexiones": 30 } ],
  "periodo": {...}, "filtros": {...}, "datos_hasta": "2026-11-17", "nota": "…", "filas": 2, "hay_mas_filas": false }
```
`data` copia tal cual los números de `resultados` (los `null` quedan `null`). Sin gráfica si hay < 2 filas (`/exportar?formato=grafica` responde 422 `{error:"sin_grafica", mensaje}`).

## 3. Tareas
Leyenda: **M/E** = modelo / esfuerzo sugeridos. **∥** = puede correr en paralelo con las indicadas.

### T0 — Entorno de pruebas (M/E: Haiku/low · ∥ con T1)
- Archivos: ninguno del repo (solo entorno).
- Cambio: instalar .NET 8 SDK (`dotnet-install.sh --channel 8.0`) y MySQL 8; cargar `pruebas_mysql/esquema_y_datos.sql`; arrancar la API con `Datos__Origen=mysql`, `EYA_MYSQL_HOST=127.0.0.1`, `EYA_MYSQL_SSL=None`, `Datos__OffsetUtcMinutos=0`, `ASPNETCORE_URLS=http://127.0.0.1:5101`.
- Terminado: `python3 pruebas_mysql/verificar_api_mysql.py http://127.0.0.1:5101 0` → **116/116** en `paridad`, `dominio` e `incluir` (reiniciando la API con las variables de cada modo, ver cabecera del verificador). Anotar los comandos exactos en el PR.

### T1 — Refactor: handlers con nombre (M/E: Sonnet/high · ∥ con T0)
- Archivos: `EyaInsightsApi/Program.cs` (opcional: mover handlers a `EyaInsightsApi/Rutas.cs`).
- Cambio: cada lambda de `MapGet` pasa a `static Respuesta Xxx(HttpRequest r, IConsultasUso c)`; `static readonly Dictionary<string, Func<HttpRequest, IConsultasUso, Respuesta>> Rutas` con las 12 rutas; `foreach` que hace `app.MapGet("/"+k, (HttpRequest r, IConsultasUso c) => Ejecutar(() => f(r, c)))`. `P.OrdenLimite` acepta límite máximo 5000 si `r.HttpContext.Items["exportar"] is true` (sin `limite` → 5000); en modo normal todo igual (1–50, defecto 10).
- Terminado: comportamiento idéntico (sin cambios en JSON ni mensajes de error).
- Prueba: verificador 116/116 en los tres modos; `dotnet build` sin advertencias nuevas.

### T2 — Generador de gráficas (M/E: Sonnet/medium · tras T1; ∥ con T3)
- Archivos: nuevo `EyaInsightsApi/Graficas.cs`.
- Cambio: `public static JsonObject? Desde(JsonNode respuesta)` que aplica la tabla D9 sobre el JSON de una `Respuesta` (mismo JSON que ve el asistente; serializar `Respuesta` con las opciones snake_case de la app). Títulos/ejes en español; diccionario de nombres de columna compartido con T3 (`Columnas.Titulo(clave)`, fallback: `_`→espacio y mayúscula inicial). `hay_mas_filas` = la nota contiene `HAY MÁS FILAS` o `resumen` indica más filas que `filas`.
- Terminado: devuelve la especificación para las 10 consultas graficables y `null` para `buscar_escuela` o < 2 filas.
- Prueba: cubierta en T5 (vía `/exportar?formato=grafica`).

### T3 — Generador de Excel (M/E: Sonnet/high · tras T1; ∥ con T2)
- Archivos: `EyaInsightsApi/EyaInsightsApi.csproj` (+ `ClosedXML`), nuevo `EyaInsightsApi/Excel.cs`, nuevo `EyaInsightsApi/Columnas.cs` (si no lo creó T2; coordinar: el primero en terminar lo crea).
- Cambio: `public static byte[] Libro(JsonNode respuesta)` → hojas `Datos` y `Referencias` según D2. Columnas en el orden del JSON; anidados (`periodo` a/b, `filtros`, `resumen`) aplanados como `clave.subclave`. Celdas de texto con `SetValue(string)` (nunca fórmulas). Nombre de archivo `Columnas.NombreArchivo(resp)` = `{consulta}_{desde}_{hasta}.xlsx` (o `_{fecha}` / `_a_…_b_…`), solo `[a-z0-9_-]`.
- Terminado: abre en Excel/LibreOffice sin aviso de reparación; números como número; tildes correctas.
- Prueba: T5 + abrir manualmente uno de cada consulta con `libreoffice --headless --convert-to csv` (sin errores).

### T4 — Ruta `/exportar/{consulta}` (M/E: Sonnet/medium · tras T2 y T3)
- Archivos: `EyaInsightsApi/Program.cs`.
- Cambio: `app.MapGet("/exportar/{consulta}", …)`: busca en `Rutas` (404 JSON `consulta_desconocida` si no está o es `buscar-escuela` con `xlsx`); `formato` = `xlsx` (defecto) | `grafica` (otro valor → 400). `xlsx`: marca `Items["exportar"]=true`, ejecuta, `Results.File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombre)`. `grafica`: límite normal, devuelve la especificación o 422. Errores 400/503 iguales a los de `Ejecutar`. **No** tocar `openapi.json` (D6).
- Terminado: `curl -o x.xlsx ".../exportar/conexiones-por-maestro?desde=2026-01-01&hasta=2026-11-10"` descarga un libro válido; parámetros inválidos dan el mismo 400 que la ruta JSON.
- Prueba: T5.

### T5 — Extender el verificador (M/E: Sonnet/high · tras T4; ∥ con T6)
- Archivos: nuevo `pruebas_mysql/leer_xlsx.py` (solo stdlib: `zipfile` + `xml.etree`, resuelve `sharedStrings`, `inlineStr` y números), `pruebas_mysql/verificar_api_mysql.py` (bloque nuevo `[fase 4: exportar]` antes del resumen).
- Comprobaciones mínimas (≈20):
  1. Para cada una de las 11 consultas: `xlsx` → 200, `content-type` xlsx, `content-disposition` con el nombre esperado.
  2. Filas de `Datos` == `resultados` del JSON con `limite=50` y == función `esperado_*` de fuerza bruta (con límite grande) para `conexiones-por-maestro` (con y sin `min/max_conexiones=1`), `conexiones-por-escuela`, `participacion-escuelas`, `tendencia-conexiones`.
  3. Exportación sin `limite` trae **todas** las filas (> 50 cuando los datos sintéticos lo permiten; si no, == total del `resumen`).
  4. `Referencias` contiene período, `cuentas_de_prueba` del modo, `datos_hasta` y el `AVISO` cuando el período pasa de los datos.
  5. Celdas numéricas son número (no texto); una escuela con tilde se lee igual.
  6. `grafica`: `data[i].categoria` == nombre/período de `resultados[i]` en el mismo orden; cada serie == la columna correspondiente; `tipo`/`layout` según D9 (formato Recharts); suma de `conexiones` de `tendencia` == `resumen.total_conexiones`.
  7. `buscar-escuela` → 404; `formato=pdf` → 400; `limite=0` → 400; escuela inexistente → 400; 1 sola fila → 422.
- Terminado: 116 + nuevas, todas ✓ en los tres modos.

### T6 — Asistente: `descargas[]` y `graficas[]` (M/E: Sonnet/high · tras T4; ∥ con T5)
- Archivos: `EyaInsightsApi/Asistente.cs`, `EyaInsightsApi/Program.cs`.
- Cambio:
  - `RespuestaAsistente` + `List<JsonNode>? Descargas = null, List<JsonNode>? Graficas = null` **al final** (no romper la construcción posicional de `Program.cs:104`).
  - En `PreguntarAsync`, por cada consulta con estado 200 y `consulta != buscar_escuela`: registrar descarga (D8, mapear `operationId` → ruta con `_rutas`, query = argumentos ya saneados) y `Graficas.Desde(salida)` si no es null. Deduplicar consultas idénticas del turno. `destacar` según regex D7 sobre la pregunta.
  - `GET /asistente/descargas/{id}` (solo con asistente habilitado): loopback a `/exportar/...` con `x-api-key`; propaga `content-type` y `content-disposition`; 404 `descarga_expirada`.
  - CORS `panel`: `.WithMethods("GET","POST").WithExposedHeaders("content-disposition")`.
- Terminado: una pregunta real devuelve `descargas`/`graficas` y el enlace baja el `.xlsx`; un id inventado da 404.
- Prueba: `dotnet build`; verificador sigue verde; prueba manual con Foundry (`az login`) usando `preguntas_fase4.txt` (T7). Si no hay acceso a Foundry, dejarlo explícito en el PR como **no probado**.

### T7 — Instrucciones y preguntas de prueba (M/E: Sonnet/medium · tras T6)
- Archivos: `EyaInsightsApi/instrucciones.txt`, nuevo `preguntas_fase4.txt`, `probar_agente_local.py` (solo si hace falta imprimir `descargas`/`graficas`).
- Cambio en instrucciones (regla 7, breve): si piden Excel/descarga, ejecuta la consulta habitual (`limite=50` si piden «todos») y di que el botón **Descargar Excel** bajo la respuesta trae la lista completa con sus referencias; si piden gráfica, ejecuta la consulta y di que la gráfica se muestra debajo; nunca escribas enlaces, rutas ni nombres de archivo; no describas tendencias con cifras que no estén en la consulta. Mismo texto en inglés implícito por la regla 5a.
- `preguntas_fase4.txt` (≈12, mismo formato que `preguntas_fase1.txt`): «Descárgame en Excel los maestros que se conectaron una sola vez en 2026», «Hazme una gráfica de las conexiones por mes de este año», «Chart of connections by weekday last month», etc., cada bloque con la consulta y parámetros esperados.
- Terminado: con Foundry, las respuestas no contienen URLs y traen `destacar=true` en lo pedido; `advertencias` vacías.

### T8 — Panel: botón Excel y gráficas (M/E: Sonnet/high · tras T6; ∥ con T7) — **repo del front**
- Archivos (rama `REQ-3741-panel-asistente`, `src/components/data-analysis-module/assistant-panel/`): `index.js`, `assistant-api.js`, `textos.js`, `style.css`, nuevo `grafica.js`.
- Cambio: por cada `descargas[i]`, botón «Descargar Excel» que hace `fetch` (mismos headers que `preguntar`) → `blob` → descarga con el nombre de `content-disposition`; 404 → «El enlace expiró, vuelve a preguntar». Por cada `graficas[i]`, componente **Recharts** (el front ya trae `recharts ^2.15.4` con React `^16.13.1`; ver `src/components/homeschooler-module/charts/*.js`) que consume `data`, `eje_x.clave`, `series[].clave`, `tipo` y `layout` directo (sin SVG propio), con leyenda, ejes, unidad `%`, tooltip/título accesible, y aviso si `hay_mas_filas`. Visible si `destacar`; si no, plegado bajo «Ver gráfica». ES/EN en `textos.js`.
- Terminado y prueba: igual que en `PANEL_ASISTENTE.md` (navegador, ES/EN, escritorio y móvil); compila con esbuild. **Requiere la máquina del usuario** (el repo del front no está en este entorno).

### T9 — Documentación (M/E: Haiku/low · al final)
- Archivos: `SEMANTICA_REPORTES.md` (sección 11 «Fase 4»), `PANEL_ASISTENTE.md` (contrato con `descargas`/`graficas`, ruta de descarga).
- Terminado: contrato y resultados del verificador (n/n en los tres modos) documentados; sin secretos (no copiar nada de `.env.local`).

**Orden:** T0 ∥ T1 → T2 ∥ T3 → T4 → T5 ∥ T6 → T7 ∥ T8 → T9. Un solo PR para T1–T7 y T9; T8 en el repo del front.

## 4. Riesgos
- **R1 Refactor de `Program.cs` (T1):** puede cambiar sin querer un mensaje o un orden. Mitigación: el verificador completo antes y después; no tocar la lógica de cada handler, solo moverla.
- **R2 Tamaño:** 5000 maestros × ~4 columnas es pequeño (< 1 MB); ClosedXML es lento con libros grandes, aquí no aplica. Mantener el tope 5000.
- **R3 Datos que cambian entre la respuesta y el clic:** la descarga se recalcula al hacer clic (refresco diario de la base). `generado_en` del Excel lo deja claro; aceptable.
- **R4 Seguridad:** las descargas exponen los mismos datos que el chat y pasan por el mismo `x-api-key`; el servicio sigue sin autenticar usuarios ni imponer alcance por rol → **solo local** hasta que esté detrás del backend de la plataforma.
- **R5 Herramientas del modelo:** si alguien añade `/exportar` a `openapi.json`, el modelo lo verá como herramienta. Comentario en `CargarHerramientas` y en T4.
- **R6 Sin Foundry en la nube:** T6–T7 no se pueden probar de punta a punta sin `az login`; quedan como prueba manual del usuario.
- **R7 ClosedXML:** fijar versión exacta en el `.csproj`; si su licencia o dependencias cambian, alternativa = `DocumentFormat.OpenXml` (MIT) con más código.

## 5. Decisiones del usuario (antes «preguntas abiertas»)
1. **Filas del Excel:** lista **completa**, hasta 5000 filas. ✔ implementado (D5).
2. **Gráfica dentro del Excel:** **no**; solo datos + referencias. ✔
3. **Librería de gráficas en el front:** `gs-msj-private-webapp-teachers` ya usa **Recharts ^2.15.4** (React ^16.13.1). La especificación de T2 se ajustó para consumirla directo: `data` (objetos planos), claves de eje y serie, y `tipo` bar/line (pie admitido por el front, no generado). ✔ implementado en `Graficas.cs`; T8 usa Recharts, no SVG propio.
4. **Botón Excel y gráfica:** se muestran **SIEMPRE** que la consulta lo admita (no solo cuando la persona los pide ni plegados). En D7 y T8, `destacar` deja de ser necesario para mostrar; si se conserva, solo ordena/resalta.
5. **Exportación directa fuera del asistente:** **sí**. `/exportar/{consulta}` es una ruta pública de la API con la misma protección que el resto (`x-api-key` solo si existe `EYA_API_KEY`). Documentada en `EXPORTAR_API.md`; sigue fuera de `openapi.json` (D6, para que el modelo no la vea como herramienta). **Riesgo:** hoy esa protección es opcional y no hay autenticación de usuario ni alcance por rol (ver R4 y `EXPORTAR_API.md`).
