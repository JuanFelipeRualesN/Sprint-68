# Semántica de Reportes de la plataforma (REQ-3741)

Objetivo: que el agente responda **lo mismo que el tablero** y poder probarlo **sin abrir Power BI**.
Fuentes: `repos/gs-msj-private-webapp-teachers` (frontend), `repos/gs-base-api/DataAnalysisAPI` (backend), modelo `Reports_Semantic_Model` (ver `CRUCE_POWER_BI.md`).
La wiki `msj-and-private.wiki` (Azure DevOps) pide credenciales y no se pudo leer desde la sesión en la nube. Si contradice algo de aquí, manda la wiki.

## 1. Cómo llega el reporte al usuario
| Capa | Qué hace | Dónde |
|---|---|---|
| Menú de reportes | 5 categorías y quién las ve: **Usuarios** (admin, admin-ORE, director, employee), **Conexiones** (admin, admin-ORE, director; el docente ve «actividad de estudiantes»), **Académico** (admin, admin-ORE para la página PBI), **Administrativo** (solo admin) | `src/reportConfig.js` |
| Embed de Power BI | El front solo envía `reportId` + `workspaceId` a la DataWarehouse API (`getPowerBIReportsAPI`), que devuelve `embedToken`. Panel de filtros oculto, sin navegación entre páginas | `data-analysis-module/powerbi-embed/index.js` |
| Descargas Excel | Componentes `REPORT_USER / CONNECTIONS / ACADEMICS / STUDENT_ACTIVITIES_TEACHER` → DataAnalysisAPI (SQL directo a tablas, no a vistas) | `DataAnalysisAPI/*Infrastructure/Dapper` |
| Asistente | Panel junto al tablero → `EyaInsightsApi /asistente/preguntar` | `assistant-panel/` |

⚠️ El backend que genera el `embedToken` (y que decidiría si hay RLS por ORE o escuela) **no está en estos repos**. Hay que confirmarlo en la wiki o con el equipo antes de dar cifras por rol.

## 2. Tres fuentes y sus criterios
| Criterio | Power BI (vistas `vw_*`) | Excel DataAnalysisAPI (tablas) | API del agente (modo paridad) |
|---|---|---|---|
| Escuelas de prueba (ORE `DEMOSTRACION`) | fuera | **dentro** | fuera |
| «Escuela Facilitadores MSJ» | fuera (filtro de página de conexiones) | dentro | fuera |
| Usuarios sin escuela | fuera (`NOT(NULL)`) | dentro (`LEFT JOIN`) | fuera |
| Rol client en conexiones | fuera (`role_id <> 1`) | dentro | fuera |
| Pruebas | solo estudiantes (`role_id = 3`) | solo estudiantes | solo estudiantes |
| Filtro de fecha en pruebas | año académico por **mes de inicio ≥ 8** + `period_no` | `period_from` entre fechas, o `academic_year_id` | igual que Power BI |
| «Conectados» | `DISTINCTCOUNT(email)` | usuarios distintos (`UserId`) | `usuarios_conectados` |
| Alcance por rol | ¿RLS? (sin confirmar) | ORE → escuelas del ORE (`AutoFilters.GetOreSchoolIds`); docente → `user_relation` | no aplica alcance por rol (vista de administrador) |

Consecuencia: **el Excel descargado NO es una referencia válida** para la paridad (incluye cuentas de prueba y usuarios sin escuela). La referencia es el tablero, y el tablero es reproducible con SQL sobre las vistas.

## 3. Glosario (definición operativa = la del tablero)
- **Conexión**: fila de `connected_date` (1 por usuario y día) de un usuario con escuela, rol ≠ client, fuera de ORE DEMOSTRACION. Día en hora de Puerto Rico (UTC-4).
- **Usuario conectado**: correo distinto con al menos una conexión en el período.
- **Escuela conectada**: escuela distinta con al menos una conexión. **Universo de escuelas**: 47 (lista mantenida en `Escuelas_EA_Visitadas.xlsx`; la API la aproxima: escuelas con estudiantes o con conexiones).
- **Prueba asignada**: fila de `vw_academics_details` (`result_generic` de estudiantes). **Completada**: `quiz_taken_date` no nulo.
- **Año académico «2025-2026»**: `academic_year_start` entre el 1-ago-2025 y el 31-jul-2026. **Período**: `P` + `period_no`.
- **% Full Quiz**: `AVG(COALESCE(total_percent_full_quiz,0))` por estudiante/materia/período (`vw_academics_average`).

## 4. Probar sin Power BI
1. `sql/09_oraculo_power_bi.sql` en Workbench (solo lectura): reproduce las medidas del tablero sobre las vistas. Valores de control: 113 / 14 / 84 conexiones (2026 T3+T4); 6.106 / 217 / 5.889 / 146 pruebas (2025-2026 P1–P3); 10.628 filas históricas.
2. Levantar la API con `correr_api_mysql.sh` y comparar sus respuestas con el oráculo para el mismo período.
3. `pruebas_mysql/verificar_api_mysql.py`: regresión offline (Docker, datos sintéticos) que debe seguir en 37/37 tras cada cambio de SQL.
4. Power BI solo se abre para casos que el oráculo no cubre (pendiente: tarjetas de la página demo 1.029 / 206 / 823 / 135).

## 5. Pendientes que cambian cifras
- Confirmar si el embed aplica **RLS** por ORE o escuela. Si lo hace, el agente tiene que recibir el alcance del usuario de sesión (hoy responde como administrador global).
- El filtro oculto de las tarjetas de la página «Reporte Académico (end user-demo)».
- Confirmar con el equipo de datos la fecha del último ETL de `result_generic` (la actividad real de pruebas termina el 23-abr-2026).
