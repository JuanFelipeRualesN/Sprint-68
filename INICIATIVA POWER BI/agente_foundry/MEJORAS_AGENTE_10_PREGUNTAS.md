# Cómo hacer que el agente Foundry responda las 10 preguntas sin Power BI

Base: `../MSJ_Utilization_modelo_y_10_preguntas.md` (lectura del TMDL) y la arquitectura actual del agente: un agente de Azure AI Foundry con una herramienta OpenAPI (`openapi.json`) que llama a `EyaInsightsApi` (C#), y esta consulta MySQL `gs_data_analysis`.

## 1. Qué responde hoy y qué falta

| # | Pregunta | ¿La responde hoy? | Qué falta |
|---|---|---|---|
| P1 | Total de usuarios y con suscripción activa | ❌ (las instrucciones dicen «sin suscripciones») | Endpoint de usuarios y suscripciones |
| P2 | % conectados por escuela (las 10 más bajas) | ⚠️ `conexiones_por_escuela` da conexiones, no % contra usuarios activos | Denominador: usuarios activos por escuela |
| P3 | Activos no conectados por rol | ❌ | Endpoint por rol, con la definición corregida |
| P4 | Evolución de conexiones en el último año académico | ✅ `tendencia_conexiones` | Parámetro de año académico (ago–jul) y granularidad mensual |
| P5 | Escuelas visitadas y no visitadas | ❌ (el dato está solo en el Excel) | Cargar el Excel en Azure y un endpoint |
| P6 | Canceladas y sin suscripción | ❌ | El mismo endpoint de P1 |
| P7 | % Full Quiz por escuela y materia | ✅ `promedio_full_quiz` | Leer `vw_academics_details` para coincidir con el tablero (excluir Posprueba, año académico) |
| P8 | Quizzes tomados y no tomados por período | ⚠️ `uso_por_area` agrupa por materia, no por período | Agrupar por año académico y período (P1–P4) |
| P9 | Conectados por ciudad y ORE | ⚠️ solo por escuela | Agrupar por city y ore |
| P10 | Suscripciones por año académico | ❌ | Endpoint del historial de suscripciones |

## 2. Cambio de fondo: leer lo mismo que Power BI, sin pasar por Power BI

Hoy la API consulta las tablas base y reconstruye la lógica a mano. Por eso las pruebas asignadas no cuadraban (7.117 frente a 1.029, ver `CRUCE_POWER_BI.md`). El tablero lee las **vistas `vw_*`** y después les aplica reglas en Power Query.

**Propuesta: una capa de vistas del agente en MySQL**, por ejemplo un esquema `agente` o vistas `vw_agente_*`, que tome las `vw_*` y aplique en SQL las mismas reglas del M:

- Exclusiones: `role <> 'client'`, «Escuela Facilitadores MSJ» y las escuelas de prueba. Que la comparación no distinga mayúsculas (`UPPER(TRIM(...))`), para no depender de cómo venga el texto, que es el fallo latente del M.
- `UPPER(TRIM(school_name))` y la corrección de «LUIS RODRÍGUEZ CABRERO».
- Sistema Educativo: pasar los `school_identifier` SEMI/Alianza a una **tabla** (`agente_sistema_educativo`), no dejarlos fijos en el código.
- **Año académico correcto, de agosto a julio**, para conexiones, suscripciones y pruebas. Se calcula una sola vez: `CASE WHEN MONTH(f) >= 8 THEN CONCAT(YEAR(f),'-',YEAR(f)+1) ELSE CONCAT(YEAR(f)-1,'-',YEAR(f)) END`.
- Un **calendario continuo** (`agente_calendario`), para que «diferencia con el día anterior» no dé vacío los días sin conexiones.

Ventajas:

- Las cifras salen idénticas al tablero sin consumir capacidad de Power BI ni Copilot (la F2 de `capturas_prueba_F2`).
- La regla vive en un solo lugar. La API solo filtra por fecha y escuela y agrega.
- Si no se pueden crear vistas en producción, la misma lógica va como SQL parametrizado en `ConsultasMySql.cs`, leyendo directamente las `vw_*`.

Requisito previo: el SQL de las `vw_*` (`sql/08_leer_vistas_de_power_bi.sql`). Sin él, las reglas que viven en MySQL (qué es una «conexión», cómo se calcula `total_connected`) quedan sin ver.

## 3. Endpoints nuevos (OpenAPI)

Cada respuesta devuelve las cifras ya calculadas (el agente no hace cálculos, según la regla 1 de `instrucciones.txt`), el bloque `resumen`, la `nota` con la definición y un parámetro **`definicion=tablero|corregida`** cuando hay diferencia, para dar las dos cifras y decir en qué se diferencian.

| Operación | Responde | Parámetros | Devuelve |
|---|---|---|---|
| `resumen_usuarios` | P1, P6 | `escuela?`, `rol?` | total, activos, cancelados (todos y «solo cancelados»), sin suscripción, solapados (activo + cancelado) |
| `conexion_por_escuela_pct` | P2 | `desde`, `hasta`, `orden`, `limite`, `definicion` | por escuela: conectados, activos, % ya calculado |
| `no_conectados_por_rol` | P3 | `desde?`, `hasta?`, `definicion` | por rol: activos, conectados, no conectados. `tablero` = 4.556; `corregida` (solo conectados activos) = 6.262 |
| `escuelas_visitadas` | P5 | `estado=visitada\|no_visitada\|todas` | escuela, fechas de visita inicial y última, sistema educativo, alertas de calidad (por ejemplo, última anterior a inicial) |
| `quizzes_por_periodo` | P8 | `anio_academico?`, `periodo?`, `excluir_posprueba=true` | por año y período: tomados, no tomados, asignados |
| `conexiones_por_ubicacion` | P9 | `desde`, `hasta`, `agrupar=ciudad\|ore` | usuarios distintos y conexiones |
| `suscripciones_por_anio` | P10 | `anio_academico?`, `criterio=activacion\|fin` | suscripciones, usuarios y filas por año |
| (ampliar) `tendencia_conexiones` | P4 | `anio_academico`, `granularidad=mes` | serie mensual de conexiones y usuarios distintos |

Pon una descripción precisa en cada `operationId`: el modelo elige la herramienta por la descripción, así que hay que evitar que dos se solapen.

## 4. Excel de escuelas visitadas en Azure

Hoy el Excel está en el **OneDrive personal** de una persona («Microsoft Teams Chat Files»). Si esa cuenta se da de baja, se pierde el origen del tablero, y el agente no tendría de dónde leer. Primer paso, valga la opción que valga: **moverlo a un sitio de SharePoint del equipo o a Azure**.

| Opción | Cómo | A favor | En contra |
|---|---|---|---|
| **A (recomendada). Cargarlo en MySQL** | Un pipeline de Azure Data Factory o una Logic App copia `Table1` a una tabla `agente_escuelas_visitadas` (con tipos: fechas DATE, school_identifier como clave) cada día o cuando cambia el archivo | Una sola fuente para el agente. Se cruza en SQL con usuarios y conexiones. Se valida al cargar (fechas, duplicados, LEAP STEAM) | Requiere permiso de escritura en un esquema aparte y un pipeline |
| B. Azure Blob Storage | El archivo se sube a un contenedor y la API lo lee con `Azure.Storage.Blobs` + `ClosedXML`, con identidad administrada y caché por ETag | Simple y sin tocar MySQL | Dos fuentes. El cruce por escuela se hace en C# |
| C. Leer SharePoint con Microsoft Graph | Una app registrada con `Sites.Selected` (solo ese sitio); la API descarga el archivo | El equipo sigue editando el Excel donde ya lo hace | Configurar permisos en Entra ID. Depende de la disponibilidad de Graph |
| ❌ Subirlo al agente (Code Interpreter o File Search) | Archivo adjunto al agente | Rápido para una demo | Es una copia estática que no se refresca. File Search es semántico y no sirve para conteos exactos. Code Interpreter da resultados que no se pueden reproducir |

Calidad al cargar: convertir las fechas de texto (M/D/AAAA) a DATE, marcar las filas con la última visita anterior a la inicial y recalcular el «Período» con la regla de agosto a julio en vez de fiarse del texto escrito a mano.

## 5. Red, seguridad y alojamiento

- La vista de linaje muestra que MySQL y el Excel llegan a Power BI por un **gateway on-premises con VPN**, así que la base no es pública. El agente no se conecta a MySQL directamente: lo hace la API.
- Para alojar la API en Azure: **App Service o Container Apps con integración de VNet** (o un endpoint privado hacia MySQL Flexible Server). El usuario de MySQL debe ser **solo de lectura** y tener acceso solo a las `vw_*` y vistas `agente`.
- Foundry llama a la API por HTTPS. Para la herramienta OpenAPI hay que autenticarse con **identidad administrada** (o una conexión del proyecto con clave), nunca dejar la API anónima. Los secretos van en Key Vault.
- Sin datos personales en las respuestas: los endpoints devuelven solo agregados. La lista de no conectados por nombre, que está en el tablero, queda fuera del agente.

## 6. Limitaciones del agente Foundry y cómo cubrirlas

| Limitación | Mitigación |
|---|---|
| El modelo se equivoca con la aritmética y los redondeos | La API devuelve todo calculado (% y totales). Ya es la regla 1; hay que mantenerla en los endpoints nuevos |
| Respuestas grandes de la herramienta (tokens, latencia) | `limite` y `orden` en todos los endpoints, más un `resumen` con los totales de TODAS las filas |
| Elige mal la herramienta cuando hay muchas parecidas | Unas 10–15 operaciones con descripciones que no se solapen. Mejor un parámetro `agrupar` que operaciones casi iguales |
| Ambigüedad de período y año académico | Regla en las instrucciones: «año académico» = agosto a julio. P1, P5 y P6 son fotos del momento y **no exigen período** (hay que ajustar la regla 2) |
| Definiciones distintas del tablero | `definicion=tablero\|corregida` y la `nota` explican la diferencia (P3: 4.556 frente a 6.262) |
| Frescura de los datos | `/salud` y cada respuesta llevan `datos_hasta`. El tablero refresca una vez al día (00:15); el agente lee en vivo y puede ir por delante |
| No hay un conector nativo de MySQL en Foundry | Herramienta OpenAPI sobre la API propia (lo actual). Más adelante se puede exponer la misma API como **servidor MCP** para reutilizarla desde otros clientes |

**Oportunidades:**

- **Evaluaciones de Foundry:** usar las 10 preguntas, con sus cifras de referencia del documento del TMDL, como conjunto de evaluación automático (en `probar_agente_local.py` y en la matriz `05_matriz-validacion.xlsx`).
- Trazas y monitoreo del agente (Application Insights) para ver qué herramienta elige en cada pregunta.
- Respuestas bilingües: ya está la regla 5a.

## 7. Cambios en `instrucciones.txt`

1. ALCANCE: agregar usuarios, suscripciones (agregadas), visitas a escuelas, quizzes por período y conexiones por ciudad y ORE. Quitar «NO tienes … suscripciones».
2. Regla 2: P1, P5 y P6 (fotos del momento) no piden período. «Año académico» = 1 ago–31 jul. «El último año académico» = el más reciente que esté completo, y se menciona también el que está en curso.
3. Nueva regla: si la respuesta trae `definicion`, decir cuál se usó y, si difiere del tablero, dar las dos cifras.
4. Referencias: añadir `definicion` y, en P5, `fuente=excel escuelas visitadas (cargado el <fecha>)`.

## 8. Orden sugerido

1. Leer el SQL de las `vw_*` (`sql/08`) y decidir: vistas `agente` en MySQL o SQL dentro de la API.
2. Sacar el Excel del OneDrive personal y cargarlo en MySQL (opción A) o en Blob (opción B).
3. Implementar los 7 endpoints nuevos y ampliar `tendencia_conexiones`. Actualizar `openapi.json`, `ConsultasMySql.cs`, `ConsultasEjemplo.cs` y `Program.cs`.
4. Actualizar `instrucciones.txt` y volver a crear el agente (`crear_agente.py`).
5. Pruebas con las cifras de referencia, teniendo en cuenta que esas cifras son del corte del 2026-11-05 (TMDL local) y la API lee en vivo: comparar con el mismo corte o aceptar la diferencia de los días posteriores.
6. Desplegar la API en Azure con integración de VNet e identidad administrada.
