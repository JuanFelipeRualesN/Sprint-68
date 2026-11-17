# REQ-3741 · Pruebas con datos reales y panel junto al Power BI

Estado a 12-nov-2026. Todo lo marcado ✅ se ejecutó y verificó hoy con **datos de ejemplo**; lo marcado ⏳ depende de la base real o de una decisión.

## Actualización 12-nov (tarde): esquema real descubierto y `ConsultasMySql` listo

**Lo que mostró la base real** (`gs_data_analysis`, MySQL 8.0.46, servidor en UTC):

| Hallazgo | Consecuencia |
|---|---|
| `connected_date` = **una fila por usuario y día** (23.993 filas, 2.982 usuarios; coincide filas = usuario-día). Llega hasta **11-nov-2026** | «Conexión» queda definida así; sin ambigüedad fila vs. usuario-día |
| `result_generic` llega solo hasta **19-oct-2026**, y solo **611 de 12.273** pruebas están completadas | Las preguntas «hasta el 10-nov» sobre pruebas serán parciales: la API ya avisa con dos fechas `datos_hasta` distintas (pruebas / conexiones) |
| Roles: student = 3, **teacher = 5**, director = 6, admin = 2, admin-ore = 8, client = 1 | «Maestro» = role_id 5 |
| 65 escuelas (63 con usuarios), sin nombres repetidos, 186 usuarios sin escuela | El nombre sirve como identificador; «por escuela» usa las 63 con usuarios |
| Catálogo de ~70 materias, solo ~10 con pruebas (EN, MAT-SP, SP, A1-SP, PA-SP, GEO-SP, TR-SP, A2, FPC-SP, MAA-SP) | «Área» = materia con pruebas asignadas alguna vez; hay una materia de catálogo llamada «BORRAR» que no se usa |
| Las columnas son `TIMESTAMP` en UTC | **Decisión (12-nov): el «día» se lee en hora de Puerto Rico (UTC-4)**; es el valor por defecto de la API (`Datos__OffsetUtcMinutos=-240`; con `0` vuelve a UTC) y `02_verdad_terreno.sql` usa `@off_horas = 4`. Las referencias lo citan: «día en hora de Puerto Rico (UTC-4)». Falta confirmar con las páginas de Power BI que cuadra |

**Hecho y verificado con una base MySQL 8 de prueba en Docker** (esquema real, datos sintéticos, usuario de solo `SELECT`):
- `EyaInsightsApi/ConsultasMySql.cs`: las 6 consultas, parametrizadas, sesión `READ ONLY`, sin SQL libre, error 503 genérico si cae la base.
- `pruebas_mysql/verificar_api_mysql.py`: recalcula todo por fuerza bruta en Python y lo compara con la API. **28/28 coinciden** en UTC y en UTC-4; con la zona equivocada fallan 7 (el verificador sí detecta diferencias). Incluye fronteras de medianoche, escuela sin usuarios, escuela con cero conexiones, docentes sin escuela, filas de pruebas de no-estudiantes (ignoradas), comodín `%` y un intento de inyección SQL (rechazado).
- Agente de Foundry contra esa API: las 7 del ticket y las 9 trampas (**9/9 sin observaciones**), con las dos fechas «datos hasta» y la definición de conexión citadas.
- `sql/02_verdad_terreno.sql` reescrito con las definiciones reales y validado en esa misma base (se corrigieron 2 errores: `NULL` en vez de 0 y un `ONLY_FULL_GROUP_BY`).

**Aún sin probar con datos reales:** solo se probó con datos sintéticos. Falta correr `correr_api_mysql.sh` con la conexión real y comparar con `02_verdad_terreno.sql` y con Power BI.

**Cómo correr la API contra la base real (decisión de Juan, 12-nov: con lo que ya tenemos, sin pedir más a Andrés).** Con la VPN activa y desde una terminal tuya:
```bash
"/Users/juanfelipe/Desktop/REQUIRE SPRINT 1/INICIATIVA POWER BI/agente_foundry/correr_api_mysql.sh"
```
Pide host, puerto, usuario y clave (la clave no se muestra ni se guarda) y deja la API en `http://127.0.0.1:5099`. Es una cuenta de **producción con permisos de admin**, así que la API se protege sola: abre cada sesión en `READ ONLY` y **se niega a arrancar** si no lo logra (probado: con una cuenta con todos los permisos, un `INSERT` falla con el error 1792); además no existe ninguna ruta que escriba ni acepte SQL libre. Con la API arriba, en otra terminal: `EYA_BASE_URL=http://127.0.0.1:5099 python3 probar_agente_local.py` (y `--preguntas preguntas_trampa.txt`).

**Primera prueba con la base real (12-nov, 20:55):** la API conectó en modo solo lectura y los **totales cuadran exactamente** con el descubrimiento: 23.993 conexiones (2022: 5.409 · 2023: 7.347 · 2024: 1.861 · 2025: 6.924 · 2026: 2.452), 12.273 pruebas asignadas, 611 completadas y las 10 materias con sus cifras idénticas. Las 7 preguntas del ticket respondieron con referencias y fechas «datos hasta» correctas. La revisión humana encontró tres problemas que el verificador automático no veía, y se corrigieron o están en curso:
1. **Incoherencia en pruebas:** «English: 10 asignadas, 14 completadas, 90 %». Causa: completadas y asignadas usaban fechas distintas. **Corregido:** una sola base (cohorte por fecha de asignación; completadas = de las asignadas en el período las ya realizadas, nunca > asignadas); verificado 29/29 con casos que cruzan de mes.
2. **Listas truncadas:** la API devolvió 10 filas y el agente mostró 5 sin avisar. **Corregido:** regla en las instrucciones + verificador que detecta filas omitidas.
3. **Cuentas y escuelas internas/de prueba** ("Escuela Calidad Grupo Require", "Escuela DEMO", docentes internos) dominan los rankings. **Pendiente:** correr `sql/04_diagnostico_cuentas_de_prueba.sql` y decidir qué se excluye (por escuela o por dominio de correo).
Dato a vigilar: **2024 tiene solo 1.861 conexiones** frente a ~7.000 en 2023 y 6.924 en 2025 (¿hueco del ETL o menor uso?).

**Segunda prueba con la base real (12-nov, 22:10, API reiniciada con el código corregido):** la incoherencia quedó resuelta («English: 10 asignadas, 9 completadas, 90 %», completadas ≤ asignadas), las listas salen completas (10 de 10) y las 9 preguntas trampa se comportan bien (pide período, rechaza calificaciones/estudiantes, detecta escuelas inexistentes, Azure bloquea el jailbreak). Los 2 «REVISAR» restantes son falsos positivos del verificador (una fecha deducida de `datos_hasta`; una pregunta sobre una sola materia). **Lo que sigue abierto es de negocio, no de código:** los rankings reales los dominan escuelas y docentes internos o de prueba (Escuela Calidad Grupo Require, Calidad IT Require Puerto Rico, Equipo Require, Demostraciones, School Subject Matter Experts, Pruebas Rezago; 10 escuelas con 0 conexiones desde septiembre, varias demo). Además, en sep–oct 2026 solo hay pruebas asignadas en English (10); en 2026 los trimestres bajan 1.458 → 555 → 237 → 202 conexiones (suman 2.452, cuadra con el total).

**Diagnóstico de cuentas de prueba (12-nov, 17:20) y exclusión implementada.** Las 65 escuelas muestran que **casi todas las de prueba/internas están en el ORE `DEMOSTRACION`** (Demostraciones, Training Require, Equipo Require, DEMO, TRAINING SCHOOOL, Calidad Grupo Require, 12.1.2023a, schoolTEsting01, Escalera González, Pruebas Rezago, Calidad IT Require PR, SMEs…), más «Escuela Demo T3 School» (1 usuario, ORE de San Juan). Por correo, **`requiretechnology.com` = 285 usuarios y 807 de las 2.452 conexiones de 2026 (33 %)**. Decisión: la API **excluye por defecto** (1) escuelas del ORE `DEMOSTRACION`, (2) la escuela «Escuela Demo T3 School» y (3) usuarios con correo `@requiretechnology.com`; cada respuesta lo declara (`filtros.cuentas_de_prueba`, nota y referencias) y `datos_hasta` se calcula sin esas cuentas. Configurable: `Datos__ExcluirCuentasDePrueba=false` (para comparar con Power BI, que probablemente las incluye), `Datos__OresExcluidos`, `Datos__DominiosExcluidos`, `Datos__EscuelasExcluidas` (listas separadas por coma). «Escuela Facilitadores MSJ» (11 usuarios, 8 docentes, sin conexiones en 2026) **no** se excluyó: decisión de Juan. También se agregó el **aviso `EMPATE`** cuando el valor de la última fila mostrada se repite fuera del límite (muchas escuelas con 0 conexiones: la lista no es exhaustiva).

**Error de MySQL 8 encontrado por el verificador independiente.** Una subconsulta `IN (… LEFT JOIN …)` dentro del `ON` de un `LEFT JOIN` devolvió resultados **equivocados** (contaba pruebas de docentes como de estudiantes: 38 en vez de 35). Se reescribió con una tabla derivada y `JOIN` explícitos (API y `02_verdad_terreno.sql`); verificado 36/36 con la exclusión activada y apagada. Lección: toda consulta nueva debe pasar por `pruebas_mysql/verificar_api_mysql.py`.

**Tercera prueba con la base real (12-nov, 22:31, exclusión activada, API reiniciada):** 7/7 y 9/9 sin observaciones. Los rankings ahora muestran escuelas reales (Vimenti 364, The School of San Juan 343, Leap STEAM 262, The United School of San Juan 195, Escuela del Deporte 182, Corretjer 75, Esc. Especializada en Matemáticas 63, Albert Einstein 30…) y **coinciden exactamente con la consulta independiente `04` que ejecutó Juan en Workbench**. El aviso de empates funciona («hay 28 escuelas más con cero conexiones»). Cada respuesta declara «cuentas de prueba = excluidas».

**Hallazgos de negocio (no son errores de código):**
1. **Sin las cuentas de prueba, la última actividad real de pruebas (`result_generic`) es del 23-abr-2026** (con ellas era el 19-oct). Las preguntas del ticket sobre «área más utilizada entre el 1-sep y el 10-nov» **no se pueden responder con datos reales**: el agente lo dice («no hay datos…, llegan hasta el 23-abr»). Hay que confirmar si es uso real (sin pruebas desde abril) o un ETL de `result_generic` detenido, y revisarlo en la página de pruebas de Power BI.
2. **Las conexiones reales se desploman tras el primer trimestre de 2026:** T1 = 1.110, T2 = 331, T3 = 30, T4 (hasta 11-nov) = 90 (sin cuentas de prueba; con ellas 1.458 / 555 / 237 / 202). Tampoco hay que olvidar que 2024 tuvo solo 1.861 conexiones.
3. Las preguntas sobre pruebas dependen de cuentas de prueba para tener datos recientes: cualquier demo con esas preguntas mostrará ceros a menos que se apague la exclusión.

**Pendiente de decisión (cambia las cifras):** definición de «área» (¿materias agrupadas, p. ej. todas las de matemáticas?) y qué mide «uso». Además, ejecutar en Workbench `SELECT MAX(id), MAX(etl_last_date) FROM etl_last_date;` (la captura solo mostraba filas de 2024) para saber cuándo corrió el ETL por última vez.

---

## 0. Qué quedó hecho hoy

| | Cambio | Dónde |
|---|---|---|
| ✅ | Cada respuesta de la API trae **referencias**: `consulta`, `fuente`, `columna_fecha` (qué fecha se filtró), `periodo`, `filtros` (escuela/orden/límite), `datos_hasta`, `total_filas`, `generado_en` | `EyaInsightsApi/Modelos.cs`, `Program.cs` |
| ✅ | **Aviso automático** si el período pedido pasa de `datos_hasta` (el agente lo repite en su respuesta) | `Program.cs` → `P.Nota` |
| ✅ | Consulta nueva `buscar_escuela`: el agente resuelve el nombre exacto y detecta escuelas que no existen | `/buscar-escuela`, `openapi.json` |
| ✅ | El **% de completadas lo calcula la API**, no el modelo (el modelo redondeó mal 652/2723 → 23,95 %; real 23,94 %) | `FilaArea.PorcentajeCompletado` |
| ✅ | Instrucciones del agente en un solo archivo, con alcance, período obligatorio, prohibición de calcular y bloque «Referencias:» | `instrucciones.py` |
| ✅ | **Verificador anti-invención**: toda cifra de la respuesta debe existir en lo que devolvió la API; el bloque Referencias debe citar el período real | `probar_agente_local.py` |
| ✅ | 9 preguntas «trampa» (sin período, escuela inexistente, calificaciones, estudiantes, presión para inventar, fechas futuras/relativas) | `preguntas_trampa.txt` |
| ✅ | Scripts para MySQL Workbench: descubrir esquema/frescura y respuesta correcta de las 7 preguntas | `sql/01_…sql`, `sql/02_…sql` |

Resultado de las pruebas de hoy (modelo gpt-4.1-mini en Foundry, API .NET local, datos inventados): las 7 preguntas del ticket respondieron con referencias y sin cifras inventadas; de las 9 trampas, el agente **se negó o pidió el período correctamente** en calificaciones, estudiantes, escuela inexistente y pregunta sin fechas. Azure bloqueó por sí mismo el intento «ignora tus reglas» (filtro anti-jailbreak, error 400).

Límites que se vieron y siguen abiertos: (a) con una pregunta de diferencia entre dos áreas el modelo restó por su cuenta (acertó, pero contra la regla); (b) las referencias escritas por el modelo pueden variar de formato (espacios, `orden/limite` vs `orden=…; límite=…`). Mitigación en §3.

## 1. Cómo enlazar MySQL Workbench y el agente

La base está detrás de un *private endpoint*: ni Foundry ni tu terminal la ven desde internet. Hay dos caminos de acceso y **hay que no mezclarlos**:

```
 Workbench ──VPN──► MySQL (gs_data_analysis)          ← para VER y validar datos (tú)
 API .NET local ─VPN─► MySQL                            ← para probar el agente con datos reales (hoy)
 Modelo en Foundry ◄── herramientas (consultas) ── API  ← el modelo solo ve el JSON que devuelve la API
```

**Paso 1 · Acceso de solo lectura (pedir a Andrés Guevara).** Un usuario MySQL `eya_ro` con `SELECT` únicamente sobre `gs_data_analysis`. No usar la cadena de conexión del `appsettings.json` del repo (trae una cadena de conexión con clave) ni pegarla en chats ni en el repo.

**Paso 2 · Conectar Workbench.** Azure VPN Client (ya instalado) conectado → Workbench → nueva conexión: host = el del *private endpoint*, puerto 3306, usuario `eya_ro`, SSL requerido. Probar con `SELECT 1;`.

**Paso 3 · Descubrir.** Ejecutar `sql/01_descubrir_esquema.sql` por secciones y pasarme los resultados de A, B, C, D, E y G (columnas reales, hasta qué fecha hay datos, si «conexión» es fila o usuario-día, qué `role_id` es docente, zona horaria). Con eso escribo `ConsultasMySql.cs` (implementa la misma interfaz `IConsultasUso`; no cambia el agente ni el contrato).

**Paso 4 · Verdad de terreno.** Ejecutar `sql/02_verdad_terreno.sql` y guardar las cifras de las 7 preguntas. Es la hoja contra la que se compara al agente.

**Paso 5 · Probar el agente con datos reales, sin exponer la base.** La cadena de conexión va en una variable de entorno (`EYA_MYSQL`) o `dotnet user-secrets`, nunca en archivos del repo:

```bash
Datos__Origen=mysql EYA_MYSQL='Server=…;Database=gs_data_analysis;User ID=eya_ro;Password=…;SslMode=Required' \
ASPNETCORE_URLS=http://127.0.0.1:5099 DOTNET_ROLL_FORWARD=Major dotnet run     # terminal 1 (VPN activa)
EYA_BASE_URL=http://127.0.0.1:5099 python3 probar_agente_local.py              # terminal 2: 7 preguntas
EYA_BASE_URL=http://127.0.0.1:5099 python3 probar_agente_local.py --preguntas preguntas_trampa.txt
```
El modelo corre en Foundry y las herramientas se ejecutan en tu máquina: los datos reales nunca quedan en una URL pública. Lo que sí viaja a Azure OpenAI (dentro de tu tenant) son las filas agregadas que la API devuelve para responder; **no devolver emails ni datos de estudiantes** (la API ya solo da agregados).

**Paso 6 · Despliegue (decisión pendiente, §5).** Cómo llega Foundry a datos privados:
- **Opción A — herramienta OpenAPI de Foundry** (lo que ya probamos con datos de ejemplo): Foundry llama a la API desde Azure, así que la API debe ser alcanzable públicamente (clave `x-api-key`) y a la vez tener *VNet integration* hacia el MySQL privado. Expone agregados reales a cualquiera que tenga la clave.
- **Opción B — el bucle lo ejecuta nuestro servicio** (lo que hace `probar_agente_local.py`): el servicio llama al modelo de Foundry con identidad administrada y ejecuta las consultas dentro de la VNet. Sin endpoint público de datos, filtro por usuario/rol en código y referencias construidas por el servidor. **Recomendada** para el panel (ver §4).

## 2. Protocolo de pruebas con datos reales

Cuatro capas que deben coincidir: **SQL (verdad) = API = respuesta del agente = tablero de Power BI** (misma página y mismo período).

| Criterio de aprobación | Cómo se mide |
|---|---|
| 0 cifras inventadas | `probar_agente_local.py`: verificador sin observaciones en las 7 + trampas |
| Cifras exactas | Cada número del agente = el de `02_verdad_terreno.sql` (comparación manual, tabla de 7 filas) |
| Referencias siempre | 100 % de respuestas con período, columna de fecha, escuela, `datos_hasta` |
| Fechas bien resueltas | «lo corrido de 2026», «último trimestre» → fechas exactas escritas; sin período → pregunta |
| Honestidad | escuela inexistente, estudiantes, calificaciones, presión para inventar → se niega o pregunta |
| Cuadra con Power BI | Q3/Q6/Q7 contra las páginas de conexiones; si difiere, se documenta por qué (definición de «conexión», zona horaria, filtros del tablero) |
| Estable | Repetir cada pregunta 3 veces: misma cifra (el modelo no es determinista) |

Pendientes de definición (afectan los números, no solo el código): qué es «área» (hoy = materia), qué es «uso» (hoy = pruebas completadas) y qué cuenta como «conexión». Cada decisión queda citada en `columna_fecha`/`nota` para que la respuesta sea auditable.

## 3. Referencias en cada respuesta

Lo de «filtros de fechas y escuelas» se cubre en dos niveles:
1. **Texto del agente:** cierra con `Referencias: consulta=…; fuente=…; fecha filtrada=…; período=… a … (inclusive); escuela=…; orden/límite=…; datos hasta=…; filas=…; generado=…`.
2. **Estructurado (lo que muestra la interfaz):** el servicio extrae `consulta/columna_fecha/periodo/filtros/datos_hasta` **directamente de lo que devolvió la API** y los muestra como *chips* bajo la respuesta («Período 01-sep → 10-nov-2026 · Escuela: todas · Fecha: quiz_taken_date · Datos hasta 10-nov»). Así lo que ve el usuario no depende de que el modelo lo escriba bien. Si el modelo calculó una cifra que no está en las filas, el servicio puede marcarla («cifra calculada por el asistente») con el mismo verificador.

## 4. Interacción en el apartado del Power BI

Dónde vive hoy: `Dashboard` (`dashboard/index.js`) → `.dash-main-content` → `PowerBIEmbedComponent`, que embebe con *embed token* y oculta paneles de filtros y navegación.

**Diseño propuesto**
- **Botón «Asistente»** en el encabezado del área del informe; abre un **panel lateral derecho** (≈380 px) que no tapa el tablero; en móvil, hoja a pantalla completa. Componente **nuevo** (`assistant-panel/`), tras una bandera **nueva** (`REACT_APP_PBI_ASSISTANT_AVAILABLE`); no se reutilizan `AI_BOT`/`AI_DASHBOARD` ni se toca el chatbot de planeación.
- **Contexto del tablero:** una cabecera del panel muestra «Estás viendo: Reporte de conexiones · página X». El informe embebido ya vive en `reportRef`; se expone con un callback (`onReportReady`) y se lee `report.getActivePage()`, los filtros/segmentadores de la página y el evento de cambio de página. Ese contexto se envía como **sugerencia** (p. ej. período del tablero) y el agente **no lo trata como dato**: sigue exigiendo/citando el período explícito.
- **Sugerencias por página:** chips con preguntas que sí puede responder (las del ticket), cambiando según la página.
- **Respuesta:** texto breve + chips de referencias (§3) + aviso «datos hasta» + botón «copiar». Una conversación por usuario (el servidor guarda `previous_response_id`); botón «nueva conversación».
- **Errores amables:** filtro de contenido de Azure (400), timeout, sin datos, escuela inexistente.
- **Fase 3 (opcional):** botón «Ver en el tablero» que cambia de página (`setPage`) o aplica el filtro de escuela con la API de `powerbi-client` para que la persona vea el mismo dato en el gráfico.

**Backend nuevo** (no toca `gs-base-api`): `POST /asistente/preguntar { pregunta, conversacionId?, contexto }` → `{ respuesta, referencias[], conversacionId }`. Autenticación con la misma sesión de la plataforma.

**Seguridad (requisito, no mejora):** el alcance lo impone el **servicio** según el rol de la sesión (admin / admin ORE / director / docente), forzando el filtro de ORE/escuela en la consulta, nunca el modelo. Fase 1 solo para roles de administración, como los informes `report-users`/`report-admin`. Un director no debe poder preguntar por otra escuela.

**Por validar en la prueba (no comprobado aún):** que los eventos y lecturas de `powerbi-client` (`getActivePage`, filtros, evento de página) funcionan con *embed token* en este informe; cuánto tarda cada respuesta con la base real (hoy 4–6 s con datos de ejemplo); idioma ES/EN (en Copilot el idioma se «pegaba» en inglés).

## 5. Decisiones y dependencias

| Decisión / dependencia | Quién | Impacto |
|---|---|---|
| ~~Usuario MySQL `eya_ro`~~ — **no se pide**: se usa la conexión actual de Workbench con la protección `READ ONLY` de la API (recomendable a futuro, no bloquea) | Juan | Ya no bloquea |
| Cada cuánto se carga `gs_data_analysis` (lo da `MAX(etl_last_date)`) y si es la misma fuente que el modelo `MSJ_Utilization` | Juan (se mira en Workbench / Power BI) | Define `datos_hasta` y si Power BI y agente deben coincidir |
| Opción A (OpenAPI de Foundry) vs B (bucle en nuestro servicio) | Pedro / Andrés | Seguridad y despliegue; recomendación: B |
| Definición de «área», «uso» y «conexión» | Pedro / Juan | Cambia las cifras |
| Tocar `Dashboard`/`PowerBIEmbedComponent` (≈3 líneas: import, render y `onReportReady`) en rama aparte | Juan | Memoria dice no modificar repos existentes; esto es lo mínimo para el panel |
| Eliminar App Service `rg-foundry-eya` (API de ejemplo) y confirmar F2 pausada | Juan | Costos y limpieza |

## 6. Próximos pasos

1. ✅ Workbench conectado y esquema descubierto; ✅ `ConsultasMySql.cs` escrito y verificado con datos sintéticos; ✅ `correr_api_mysql.sh` probado (clave con símbolos, cuenta admin, clave incorrecta). Juan: correr el script con la conexión real.
2. Con la API real arriba: ejecutar ejecutar `sql/02_verdad_terreno.sql` en Workbench y comparar con las 7 preguntas + trampas, y con las páginas del tablero de Power BI.
3. Decidir opción A/B y alcance por rol; luego construir el servicio `/asistente/preguntar` y el panel (fase 1), en rama aparte.
